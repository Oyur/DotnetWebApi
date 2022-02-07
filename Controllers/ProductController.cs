using System.Linq;
using DotnetWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetWebApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ProductController(ApplicationDBContext context)
        {
            _context = context;
        }

        //Get AllProduct
        [HttpGet]
        public ActionResult <Product> GetAll()
        {
            var GetAllProduct = (
                from category in _context.Category
                join product in _context.Product
                on category.CategoryId equals product.CategoryId
                select new
                {
                    product.ProductId,
                    product.ProductName,
                    product.Price,
                    product.Quantity,
                    product.Picture,
                    product.CreatedDate,
                    product.ModifiedDate,
                    category.CategoryName,
                    category.CategoryStatus
                }
            );
            return Ok(GetAllProduct);
        }

        //Get ProductByID
        [HttpGet("{Id}")]
        public ActionResult <Product> GetById(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var GetById = (
                from category in _context.Category
                join product in _context.Product
                on category.CategoryId equals product.CategoryId
                where product.ProductId == Id
                select new
                {
                    product.ProductId,
                    product.ProductName,
                    product.Price,
                    product.Quantity,
                    product.Picture,
                    product.CreatedDate,
                    product.ModifiedDate,
                    category.CategoryName,
                    category.CategoryStatus
                }
            );
            return Ok(GetById);
        }

        //Post 
        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            _context.Product.Add(product);
            _context.SaveChanges();
            return Ok("Add Product Complete");
        }

        //Update searchId
        [HttpPut("{Id}")]
        public ActionResult UpdateProductId(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var CheckProductId = _context.Product.Find(Id);
            if(CheckProductId == null)
            {
                return NotFound();
            }
            return Ok("Go next Update Page");
            
        }
        //Update
        [HttpPut]
        public ActionResult UpdateProduct(Product product)
        {
            _context.Product.Update(product);
            _context.SaveChanges();
            return Ok("Update Complete");
        }

        //Delete Product
        [HttpDelete("{Id}")]
        public ActionResult DeleteProduct(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var CheckDeleteProduct = _context.Product.Find(Id);
            if(CheckDeleteProduct == null)
            {
                return NotFound();
            }
            _context.Product.Remove(CheckDeleteProduct);
            _context.SaveChanges();
            return Ok("Delete Complete");
        }
        
    }
}