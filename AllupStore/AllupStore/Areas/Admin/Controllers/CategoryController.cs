using AllupStore.Areas.Admin.Constants;
using AllupStore.Areas.Admin.ViewModels;
using AllupStore.Data;
using AllupStore.Models.Entities;
using FlowerStore.Areas.Admin.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AllupStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

   
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
           
            return View(categories);

        }

        public async Task<IActionResult> Detail(int id)
        {
            var category = await _context.Categories.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);
            if (category==null)
            {
                return NotFound();
            }
            return View(category);
        }
    
        
    
        [HttpGet]
    
        public async Task<IActionResult> Create()
        {
            var parents = await _context.Categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();
            ViewBag.Parents = parents;
                return View();
     
       
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            var parents = await _context.Categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();
            ViewBag.Parents = parents;
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (model.IsMain)
            {
                if (model.File == null)
                {
                    ModelState.AddModelError("File", "Select an image");
                    return View();
                }
                if (!model.File.IsImage())
                {
                    ModelState.AddModelError("File", "File is unsupported");
                    return View();
                }
                if (model.File.IsGreaterThanGivenSize(1024))
                {
                    ModelState.AddModelError(nameof(model.File), "File size cannot be greater than 1 mb");
                    return View();
                }

                var imageName = FileUtils.CreateFile(FileConstants.ImagePath, model.File);

                Category category = new Category
                {
                    Name = model.Name,
                    Image = imageName,
                    IsMain = true,
                };
                await _context.Categories.AddAsync(category);
            }
            else
            {
                var parent = await _context.Categories.FirstOrDefaultAsync(c => c.IsMain && !c.IsDeleted && c.Id == model.ParentId);
                if (parent==null)
                {
                    ModelState.AddModelError("ParentId", "Choose valid category");
                    return View();
                }
                Category category = new Category
                {
                    Name = model.Name,
                   
                    IsMain = true,
                    Parent = parent
                };
                await _context.Categories.AddAsync(category);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
     
    }
}
