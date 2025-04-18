using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<News>>> GetAll()
    {
        var newsList = await _newsService.GetAllAsync();
        return Ok(newsList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<News>> GetById(int id)
    {
        var news = await _newsService.GetByIdAsync(id);
        if (news == null)
        {
            return NotFound();
        }
        return Ok(news);
    }
}
