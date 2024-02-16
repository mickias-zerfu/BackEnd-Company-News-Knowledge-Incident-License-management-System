using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly StoreContext _context;
        public NewsController(StoreContext context)
        {
            this._context = context;

        }

        [HttpGet]
        public async Task<ActionResult<List<News>>> GetNewses()
        {
            var news =await  _context.News.ToListAsync();
            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
            return await _context.News.FindAsync(id);
        }

    }


}