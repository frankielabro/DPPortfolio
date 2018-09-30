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

        public AdminController(DataContext context, IMapper map)
        {
            _context = context;
            _map = map;
        }

        [HttpPost]
        public IActionResult createPortfolio(CreatePortfolioViewModel createVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(StatusCode(400));
            }

            try
            {
                
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(Ok());
        }

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