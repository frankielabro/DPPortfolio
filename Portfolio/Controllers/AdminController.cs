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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;

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
        public IActionResult CreatePortfolio(CreatePortfolioViewModel createVM)
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
        
        [HttpPatch("portfolio/{id}")]
        public IActionResult Patch(int id, [FromBody]JsonPatchDocument<PortfolioPathViewModel> portfolioPatch)
        {

            if (!_context.Portfolio.Any(p => p.PortfolioId == id)) return Json(BadRequest("Id doesn't exist."));

            var portfolio = _context.Portfolio.FirstOrDefault(p => p.PortfolioId == id);

            //Use Automapper to map that to our DTO object.
            PortfolioPathViewModel portfolioDTO = _map.Map<PortfolioPathViewModel>(portfolio);

            portfolioPatch.ApplyTo(portfolioDTO); //Apply the patch to that DTO. 

            _map.Map(portfolioDTO, portfolio); //Use automapper to map the DTO back on top of the database object. 

            _context.Portfolio.Update(portfolio); //Update our portfolio in the database. 
            _context.SaveChanges();

            return Json(portfolioDTO);
        }

        //endpoint for editing the photo
        [HttpPut("portfolio/editPhoto/{id}")]
        public IActionResult ChangeImage(int id, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                return Json(BadRequest(ModelState));
            }

            try
            {
                //adding to database

                if (Image == null || Image.Length == 0)return Json(Content("file not selected"));

                var dir = _configuration.GetSection("Directory:Portfolio").Value;

                var portfolio = _context.Portfolio.FirstOrDefault(p => p.PortfolioId == id);

                var category = _context.Category.FirstOrDefault(c => c.CategoryId == portfolio.CategoryId);

                var path = Path.Combine(dir, category.Name, portfolio.PortfolioId.ToString(), Image.FileName);

                Directory.CreateDirectory(Path.GetDirectoryName(path));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }

                portfolio.Image = category.Name + "/" + portfolio.PortfolioId.ToString() + "/" + Image.FileName;
                _context.Update(portfolio);
                _context.SaveChanges();

                return Json(StatusCode(204));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpDelete("portfolio/{id}")]
        public IActionResult DeletePortfolio(int id)
        {
            if (!_context.Portfolio.Any(p => p.PortfolioId == id)) return Json(StatusCode(404, "PortfolioId not found"));

            var portfolio = _context.Portfolio.FirstOrDefault(p => p.PortfolioId == id);

            _context.Remove(portfolio);
            _context.SaveChanges();


            return Json(Ok());
        }



        //end point for deleting the portfolio

        [HttpPost("category")]
        public IActionResult CreateCategory([FromBody]CategoryViewModel categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(BadRequest(ModelState));
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