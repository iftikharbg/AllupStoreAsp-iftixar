using AllupStore.Data;
using AllupStore.Models;
using AllupStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AllupStore.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _dbContext;
        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.Where(c=>!c.IsDeleted && c.IsMain).ToListAsync();
            var homeVm = new HomeViewModel
            {
                Categories = categories
            };
            return View(homeVm);
        }

      
    }
}
