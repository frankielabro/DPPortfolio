using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portfolio.Entity;

namespace Portfolio.Controllers
{
    [AllowAnonymous]
    [Route("public")]
    public class PublicController : Controller
    {
        private DataContext _context;
        private readonly IMapper _map;
        private readonly IConfiguration _configuration;

        public PublicController(DataContext context, IMapper map, IConfiguration configuration)
        {
            _context = context;
            _map = map;
            _configuration = configuration;
        }
        
        [HttpGet("portfolio")]
        public IActionResult GetPortfolioByCategory(int category)
        {
            var portfolios = _context.Portfolio.Include("Category").ToList().OrderBy(p => p.Title);
            return Json(portfolios);
        }
        
        [HttpGet("category")]
        public IActionResult GetAllCategory()
        {
            var categories = _context.Category.ToList();
            return Json(categories);
        }
    }
}