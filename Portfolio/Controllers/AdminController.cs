using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Entity;
using Portfolio.ViewModels;
using AutoMapper;
using Portfolio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Portfolio.Controllers
{
    [Authorize]
    [Route("admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private DataContext _context;
        private readonly IMapper _map;
        private readonly IConfiguration _configuration;

        public AdminController(DataContext context, IMapper map, IConfiguration configuration)
        {
            _context = context;
            _map = map;
            _configuration = configuration;
        }

        [HttpPost("portfolio")]
        public IActionResult createPortfolio(CreatePortfolioViewModel createVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(BadRequest(ModelState));
            }

            try
            {
                if (_context.Portfolio.Any(p => p.Title == createVM.Title)) return Json(StatusCode(409));
                if (!_context.Category.Any(c => c.CategoryId == createVM.CategoryId)) return Json(StatusCode(404, "Category not found"));
                
                //adding to database
                var portfolio = _map.Map<Models.Portfolio>(createVM);
                portfolio.DeveloperInitials = portfolio.DeveloperInitials.ToUpper();
                _context.Add(portfolio);
                _context.SaveChanges();

                if (createVM.Image == null || createVM.Image.Length == 0)
                    return Json(Content("file not selected"));

                var dir = _configuration.GetSection("Directory:Portfolio").Value;
                var category = _context.Category.FirstOrDefault(c => c.CategoryId == createVM.CategoryId);
                
                var path = Path.Combine(dir, category.Name, portfolio.PortfolioId.ToString(), createVM.Image.FileName);

                Directory.CreateDirectory(Path.GetDirectoryName(path));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    createVM.Image.CopyTo(stream);
                }
                
                portfolio.Image = category.Name + "/" + portfolio.PortfolioId.ToString() + "/" + portfolio.Image;
                _context.Update(portfolio);
                _context.SaveChanges();
                
                return Json(StatusCode(201));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //endpoint for editing the info

        //endpoint for editing the photo

        //end point for deleting the portfolio
        
        [HttpPost("category")]
        public IActionResult createCategory([FromBody]CategoryViewModel categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(StatusCode(400));
            }

            try
            {
                if (_context.Category.Any(c => c.Name == categoryVM.Name)) return Json(StatusCode(409));

                var category = _map.Map<Category>(categoryVM);

                _context.Add(category);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(StatusCode(201));
        }
    }
}