//using EXE201.Models;
//using EXE201.Service;
//using EXE201.Service.Interface;
//using Microsoft.AspNetCore.Mvc;
//using Net.payOS;
//using Net.payOS.Types;
//using System.Text.Json;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.IO;

//namespace EXE201.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PayOSController : ControllerBase
//    {
//        private readonly PayOS _payOS;
//        private readonly IBookingService _bookingService;
//        private readonly IPaymentService _paymentService;
//        private readonly ILogger<PayOSController> _logger;
//        private readonly IConfiguration _configuration;

//        public PayOSController(IConfiguration configuration, IBookingService bookingService, IPaymentService paymentService, ILogger<PayOSController> logger)
//        {
//            _configuration = configuration;
//            string clientId = configuration["PayOS:ClientId"];
//            string apiKey = configuration["PayOS:ApiKey"];
//            string checksumKey = configuration["PayOS:ChecksumKey"];

//            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(checksumKey))
//            {
//                throw new ArgumentException("PayOS configuration is missing or invalid.");
//            }

//            _payOS = new PayOS(clientId, apiKey, checksumKey);
//            _bookingService = bookingService;
//            _paymentService = paymentService;
//            _logger = logger;
//        }

//        [HttpPost("create-payment/{bookingId}")]
//        public async Task<IActionResult> CreatePayment(long bookingId)
//        {
//            try
//            {
//                var booking = await _bookingService.GetBookingById(bookingId);
//                if (booking == null)
//                {
//                    _logger.LogWarning("Booking not found for ID: {BookingId}", bookingId);
//                    return NotFound(new { message = "Không tìm thấy đơn đặt." });
//                }

//                if (booking.TotalPrice <= 0)
//                {
//                    _logger.LogWarning("Invalid TotalPrice for booking ID: {BookingId}, TotalPrice: {TotalPrice}", bookingId, booking.TotalPrice);
//                    return BadRequest(new { message = "Giá trị thanh toán phải lớn hơn 0." });
//                }

//                var items = new List<ItemData>
//                {
//                    new ItemData($"Booking {booking.Id} - {booking.Description}", 1, (int)booking.TotalPrice)
//                };

//                var customOrderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
//                var paymentData = new PaymentData(
//                    orderCode: customOrderCode,
//                    amount: (int)booking.TotalPrice,
//                    description: $"Thanh toán Booking {booking.Id}",
//                    items: items,
//                    cancelUrl: _configuration["PayOS:CancelUrl"],
//                    returnUrl: _configuration["PayOS:ReturnUrl"]
//                );

//                var createPayment = await _payOS.createPaymentLink(paymentData);
//                if (createPayment == null || string.IsNullOrEmpty(createPayment.checkoutUrl))
//                {
//                    _logger.LogError("Failed to create payment link for booking ID: {BookingId}", bookingId);
//                    return BadRequest(new { message = "Lỗi khi tạo link thanh toán" });
//                }

//                var payment = new Models.Payment
//                {
//                    BookingId = booking.Id,
//                    OrderCode = customOrderCode,
//                    Amount = (int)booking.TotalPrice,
//                    Description = paymentData.description,
//                    Items = $"Booking {booking.Id} - {booking.Description}",
//                    CancelUrl = paymentData.cancelUrl,
//                    ReturnUrl = paymentData.returnUrl,
//                    PaymentLink = createPayment.checkoutUrl,
//                    Status = "PENDING",
//                    CreatedAt = DateTime.UtcNow
//                };

//                await _paymentService.CreatePaymentAsync(payment);
//                _logger.LogInformation("Payment created successfully for booking ID: {BookingId}, OrderCode: {OrderCode}", bookingId, customOrderCode);

//                return Ok(new
//                {
//                    bookingId = booking.Id,
//                    orderCode = customOrderCode,
//                    checkoutUrl = createPayment.checkoutUrl
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating payment for booking ID: {BookingId}", bookingId);
//                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
//            }
//        }

//        [HttpPost("webhook")]
//        public async Task<IActionResult> Webhook()
//        {
//            try
//            {
//                using var reader = new StreamReader(Request.Body);
//                var body = await reader.ReadToEndAsync();

//                _logger.LogInformation("Webhook received: {Body}", body);

//                var jsonDocument = JsonDocument.Parse(body);
//                var root = jsonDocument.RootElement;

//                if (!root.TryGetProperty("data", out var dataElement) ||
//                    !dataElement.TryGetProperty("orderCode", out var orderCodeElement) ||
//                    !dataElement.TryGetProperty("status", out var statusElement))
//                {
//                    _logger.LogWarning("Invalid webhook structure: {Body}", body);
//                    return BadRequest(new { message = "Cấu trúc webhook không hợp lệ" });
//                }

//                if (!orderCodeElement.TryGetInt64(out long orderCode) || orderCode == 0)
//                {
//                    _logger.LogWarning("Invalid orderCode in webhook: {Body}", body);
//                    return BadRequest(new { message = "OrderCode không hợp lệ" });
//                }

//                string status = statusElement.GetString()?.ToUpper();
//                _logger.LogInformation("Processing webhook for orderCode: {OrderCode}, status: {Status}", orderCode, status);

//                var payment = await _paymentService.GetByOrderCodeAsync(orderCode);
//                if (payment == null)
//                {
//                    _logger.LogWarning("Payment not found for orderCode: {OrderCode}", orderCode);
//                    return NotFound(new { message = "Không tìm thấy thông tin thanh toán." });
//                }

//                switch (status)
//                {
//                    case "PAID":
//                    case "SUCCESS":
//                        if (payment.Status == "PAID")
//                        {
//                            _logger.LogInformation("Payment already PAID for orderCode: {OrderCode}", orderCode);
//                            return Ok(new { message = "Thanh toán đã được xử lý trước đó" });
//                        }

//                        payment.Status = "PAID";
//                        await _paymentService.UpdatePaymentAsync(payment);
//                        _logger.LogInformation("Payment status updated to PAID for orderCode: {OrderCode}", orderCode);
//                        return Ok(new { message = "Thanh toán thành công" });

//                    case "CANCELLED":
//                    case "FAILED":
//                    case "CANCELED":
//                        if (payment.Status == "CANCELLED")
//                        {
//                            _logger.LogInformation("Payment already CANCELLED for orderCode: {OrderCode}", orderCode);
//                            return Ok(new { message = "Thanh toán đã bị hủy trước đó" });
//                        }

//                        payment.Status = "CANCELLED";
//                        await _paymentService.UpdatePaymentAsync(payment);
//                        _logger.LogInformation("Payment status updated to CANCELLED for orderCode: {OrderCode}", orderCode);
//                        return Ok(new { message = "Thanh toán đã bị hủy" });

//                    default:
//                        _logger.LogWarning("Unhandled payment status: {Status} for orderCode: {OrderCode}", status, orderCode);
//                        return BadRequest(new { message = $"Trạng thái thanh toán không được hỗ trợ: {status}" });
//                }
//            }
//            catch (JsonException ex)
//            {
//                _logger.LogError(ex, "Failed to parse webhook JSON: {Message}", ex.Message);
//                return BadRequest(new { message = "Dữ liệu webhook không đúng định dạng JSON" });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Webhook processing error: {Message}", ex.Message);
//                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
//            }
//        }

//        [HttpGet("handle-redirect")]
//        public async Task<IActionResult> HandleRedirect([FromQuery] string status, [FromQuery] long orderCode, [FromQuery] bool cancel, [FromQuery] string id, [FromQuery] string code)
//        {
//            try
//            {
//                _logger.LogInformation("Handling redirect with orderCode: {OrderCode}, status: {Status}, cancel: {Cancel}, id: {Id}, code: {Code}", orderCode, status, cancel, id, code);

//                if (orderCode == 0)
//                {
//                    _logger.LogWarning("Invalid orderCode in redirect: {OrderCode}", orderCode);
//                    return BadRequest(new { message = "OrderCode không hợp lệ" });
//                }

//                if (string.IsNullOrEmpty(status))
//                {
//                    _logger.LogWarning("Missing status in redirect for orderCode: {OrderCode}", orderCode);
//                    return BadRequest(new { message = "Trạng thái thanh toán không được cung cấp" });
//                }

//                var payment = await _paymentService.GetByOrderCodeAsync(orderCode);
//                if (payment == null)
//                {
//                    _logger.LogWarning("Payment not found for orderCode: {OrderCode}", orderCode);
//                    return NotFound(new { message = "Không tìm thấy thông tin thanh toán." });
//                }

//                // Kiểm tra code để xác thực (giả sử code=00 là hợp lệ)
//                if (string.IsNullOrEmpty(code) || code != "00")
//                {
//                    _logger.LogWarning("Invalid code in redirect for orderCode: {OrderCode}, code: {Code}", orderCode, code);
//                    return BadRequest(new { message = "Mã xác thực không hợp lệ" });
//                }

//                string normalizedStatus = status.ToUpper();
//                switch (normalizedStatus)
//                {
//                    case "PAID":
//                    case "SUCCESS":
//                        if (payment.Status == "PAID")
//                        {
//                            _logger.LogInformation("Payment already PAID for orderCode: {OrderCode}", orderCode);
//                            return Ok(new { message = "Thanh toán đã được xử lý trước đó", orderCode, status = payment.Status });
//                        }

//                        payment.Status = "PAID";
//                        await _paymentService.UpdatePaymentAsync(payment);
//                        _logger.LogInformation("Payment status updated to PAID for orderCode: {OrderCode}", orderCode);
//                        // Redirect về frontend với thông báo thành công
//                        return Redirect($"{_configuration["PayOS:FrontendUrl"]}?status=PAID&orderCode={orderCode}");

//                    case "CANCELLED":
//                    case "FAILED":
//                    case "CANCELED":
//                        if (payment.Status == "CANCELLED")
//                        {
//                            _logger.LogInformation("Payment already CANCELLED for orderCode: {OrderCode}", orderCode);
//                            return Ok(new { message = "Thanh toán đã bị hủy trước đó", orderCode, status = payment.Status });
//                        }

//                        payment.Status = "CANCELLED";
//                        await _paymentService.UpdatePaymentAsync(payment);
//                        _logger.LogInformation("Payment status updated to CANCELLED for orderCode: {OrderCode}", orderCode);
//                        // Redirect về frontend với thông báo hủy
//                        return Redirect($"{_configuration["PayOS:FrontendUrl"]}?status=CANCELLED&orderCode={orderCode}");

//                    default:
//                        _logger.LogWarning("Unhandled payment status: {Status} for orderCode: {OrderCode}", normalizedStatus, orderCode);
//                        return BadRequest(new { message = $"Trạng thái thanh toán không được hỗ trợ: {status}" });
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error handling redirect for orderCode: {OrderCode}", orderCode);
//                // Redirect về frontend với thông báo lỗi
//                return Redirect($"{_configuration["PayOS:FrontendUrl"]}?status=ERROR&orderCode={orderCode}&error={Uri.EscapeDataString(ex.Message)}");
//            }
//        }

//        [HttpGet("payment-by-ordercode/{orderCode}")]
//        public async Task<IActionResult> GetPaymentByOrderCode(long orderCode)
//        {
//            try
//            {
//                var payment = await _paymentService.GetByOrderCodeAsync(orderCode);
//                if (payment == null)
//                {
//                    _logger.LogWarning("Payment not found for orderCode: {OrderCode}", orderCode);
//                    return NotFound(new { message = "Không tìm thấy thông tin thanh toán." });
//                }

//                return Ok(payment);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving payment for orderCode: {OrderCode}", orderCode);
//                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
//            }
//        }

//        [HttpPut("update")]
//        public async Task<IActionResult> UpdatePayment([FromBody] Payment payment)
//        {
//            try
//            {
//                if (payment == null || payment.OrderCode == 0)
//                {
//                    _logger.LogWarning("Invalid payment data: {Payment}", JsonSerializer.Serialize(payment));
//                    return BadRequest(new { message = "Dữ liệu thanh toán không hợp lệ." });
//                }

//                var existingPayment = await _paymentService.GetByOrderCodeAsync(payment.OrderCode);
//                if (existingPayment == null)
//                {
//                    _logger.LogWarning("Payment not found for orderCode: {OrderCode}", payment.OrderCode);
//                    return NotFound(new { message = "Không tìm thấy thanh toán để cập nhật." });
//                }

//                if (existingPayment.Status == "PAID" && payment.Status != "PAID")
//                {
//                    _logger.LogWarning("Attempt to change status of PAID payment for orderCode: {OrderCode}", payment.OrderCode);
//                    return BadRequest(new { message = "Không thể thay đổi trạng thái của thanh toán đã hoàn tất." });
//                }

//                await _paymentService.UpdatePaymentAsync(payment);
//                _logger.LogInformation("Payment updated successfully for orderCode: {OrderCode}", payment.OrderCode);
//                return Ok(new { message = "Thanh toán được cập nhật thành công.", payment });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating payment for orderCode: {OrderCode}", payment?.OrderCode);
//                return StatusCode(500, new { message = "Lỗi khi cập nhật thanh toán.", error = ex.Message });
//            }
//        }
//    }
//}
using EXE201.Service;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using System.Text.Json;
using System.Threading.Tasks;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayOSController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IBookingService _bookingService;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PayOSController> _logger;
        private readonly IConfiguration _configuration;

        public PayOSController(IConfiguration configuration, IBookingService bookingService, IPaymentService paymentService, ILogger<PayOSController> logger)
        {
            _configuration = configuration;
            string clientId = configuration["PayOS:ClientId"];
            string apiKey = configuration["PayOS:ApiKey"];
            string checksumKey = configuration["PayOS:ChecksumKey"];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(checksumKey))
            {
                throw new ArgumentException("PayOS configuration is missing or invalid.");
            }

            _payOS = new PayOS(clientId, apiKey, checksumKey);
            _bookingService = bookingService;
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("create-payment/{bookingId}")]
        public async Task<IActionResult> CreatePayment(long bookingId)
        {
            try
            {
                var booking = await _bookingService.GetBookingById(bookingId);
                if (booking == null)
                {
                    _logger.LogWarning("Booking not found for ID: {BookingId}", bookingId);
                    return NotFound(new { message = "Không tìm thấy đơn đặt." });
                }

                if (booking.TotalPrice <= 0)
                {
                    _logger.LogWarning("Invalid TotalPrice for booking ID: {BookingId}, TotalPrice: {TotalPrice}", bookingId, booking.TotalPrice);
                    return BadRequest(new { message = "Giá trị thanh toán phải lớn hơn 0." });
                }

                var items = new List<ItemData>
                        {
                            new ItemData($"Booking {booking.Id} - {booking.Description}", 1, (int)booking.TotalPrice)
                        };

                var customOrderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var paymentData = new PaymentData(
                    orderCode: customOrderCode,
                    amount: (int)booking.TotalPrice,
                    description: $"Thanh toán Booking {booking.Id}",
                    items: items,
                    cancelUrl: _configuration["PayOS:CancelUrl"],
                    returnUrl: _configuration["PayOS:ReturnUrl"]
                );

                var createPayment = await _payOS.createPaymentLink(paymentData);
                if (createPayment == null || string.IsNullOrEmpty(createPayment.checkoutUrl))
                {
                    _logger.LogError("Failed to create payment link for booking ID: {BookingId}", bookingId);
                    return BadRequest(new { message = "Lỗi khi tạo link thanh toán" });
                }

                var payment = new Models.Payment
                {
                    BookingId = booking.Id,
                    OrderCode = customOrderCode,
                    Amount = (int)booking.TotalPrice,
                    Description = paymentData.description,
                    Items = $"Booking {booking.Id} - {booking.Description}",
                    CancelUrl = paymentData.cancelUrl,
                    ReturnUrl = paymentData.returnUrl,
                    PaymentLink = createPayment.checkoutUrl,
                    Status = "PENDING",
                    CreatedAt = DateTime.UtcNow
                };

                await _paymentService.CreatePaymentAsync(payment);
                _logger.LogInformation("Payment created successfully for booking ID: {BookingId}, OrderCode: {OrderCode}", bookingId, customOrderCode);

                return Ok(new
                {
                    bookingId = booking.Id,
                    orderCode = customOrderCode,
                    checkoutUrl = createPayment.checkoutUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment for booking ID: {BookingId}", bookingId);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }



        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                var receivedData = JsonSerializer.Deserialize<WebhookResponse>(body);

                if (receivedData == null || string.IsNullOrEmpty(receivedData.orderCode))
                {
                    return BadRequest("Dữ liệu webhook không hợp lệ");
                }

                // Kiểm tra trạng thái thanh toán
                if (receivedData.status == "PAID")
                {
                    // TODO: Cập nhật trạng thái đơn hàng trong database
                    return Ok(new { message = "Thanh toán thành công" });
                }

                return BadRequest("Thanh toán thất bại hoặc chưa hoàn tất");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }
    }

    public class WebhookResponse
    {
        public string orderCode { get; set; }
        public string status { get; set; }
    }
}