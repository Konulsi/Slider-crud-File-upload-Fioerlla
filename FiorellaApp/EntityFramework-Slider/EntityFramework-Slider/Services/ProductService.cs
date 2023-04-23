using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAll() => await _context.Products.Include(m => m.Images).ToListAsync();
        public async Task<Product> GetById(int id) => await _context.Products.FindAsync(id);
        public async Task<Product> GetFullDataById(int id) => await _context.Products.Include(m => m.Images).Include(m => m.Category)?.FirstOrDefaultAsync(m => m.Id == id); // yoxlayiriq databazadaki productun idisinnen cookiedeki productun(yeni basketdeki productun) idsi eynidirse

        
    }
}
