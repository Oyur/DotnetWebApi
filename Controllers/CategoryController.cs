using System.Linq;
using DotnetWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")] //https://localhost:5001/api/Category
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public CategoryController(ApplicationDBContext context)
        {
            _context = context;
        }
        
        // Read ALLCategory
        [HttpGet]
        public ActionResult<Category> GetAll()
        {
            var allcategory = _context.Category.ToList();
            return Ok(allcategory);
        }

        //Read byID
        [HttpGet("{Id}")] //get แบบมีparameter
        public ActionResult<Category> GetById(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var category = _context.Category.Where(c => c.CategoryId == Id);
            return Ok(category);
        }

        //Add Category
        [HttpPost]
        public ActionResult Create(Category category)
        {
            _context.Category.Add(category);
            _context.SaveChanges();
            return Ok("Add Complete");
        }

        //Update searchbyId
        [HttpPut("{Id}")]
        public ActionResult UpdateById(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var UpdateSearch = _context.Product.Find(Id);
            if(UpdateSearch == null)
            {
                return NotFound();
            }
            return Ok("Go to Update Page");
        }
        //Update
        [HttpPut]
        public ActionResult Update(Category category)
        {
            if(category == null)
            {
                return NotFound();
            }
            _context.Category.Update(category);
            _context.SaveChanges();
            return Ok("Update Complete");
        }

        //delete
        [HttpDelete("{Id}")]
        public IActionResult Delete(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var DeleteCategory = _context.Category.Where(c => c.CategoryId == Id).First();
            if(DeleteCategory == null)
            {
                return NotFound();
            }
            _context.Category.Remove(DeleteCategory);
            _context.SaveChanges();
            return Ok("Delete Complete");
        }
    }
}