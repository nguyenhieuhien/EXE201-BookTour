using EXE201.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EXE201.Controllers
{
    public class UserAccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SystemAccountService _systemAccountService;

        public UserAccountController(IConfiguration config, SystemAccountService systemAccountService)
        {
            _config = config;
            _systemAccountService = systemAccountService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _systemAccountService.Authenticate(request.UserName, request.Password);

            if (user == null || user.Result == null)
                return Unauthorized();

            var token = GenerateJSONWebToken(user.Result);

            return Ok(token);
        }

        private string GenerateJSONWebToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                    , _config["Jwt:Audience"]
                    , new Claim[]
                    {
                new(ClaimTypes.Name, account.UserName),
                new(ClaimTypes.Role, account.RoleId.ToString()),
                    },
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        [HttpGet("DecodeToken")]
        public IActionResult DecodeToken([FromHeader] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Token is required",
                    errorCode = "TOKEN_MISSING"
                });
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = securityKey
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var claims = principal.Claims.ToDictionary(c => c.Type, c => c.Value);

                return Ok(new
                {
                    success = true,
                    message = "Token is valid",
                    data = claims
                });
            }
            catch (SecurityTokenExpiredException)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Token has expired",
                    errorCode = "TOKEN_EXPIRED"
                });
            }
            catch (SecurityTokenException)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Invalid token",
                    errorCode = "TOKEN_INVALID"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while decoding the token",
                    errorCode = "TOKEN_PROCESSING_ERROR",
                    error = ex.Message
                });
            }
        }

    }

    public sealed record LoginRequest(string UserName, string Password);
    }
