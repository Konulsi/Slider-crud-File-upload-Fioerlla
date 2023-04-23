using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;
        public CategoryController(ICategoryService categoryService, AppDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _categoryService.GetAll());
        }


        [HttpGet]
        public IActionResult Create()     /*async-elemirik cunku data gelmir databazadan*/ //sadece indexe gedir hansiki inputa data elavve edib category yaradacaq
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)     // bazaya neyise save edirik deye asinxron olmalidi
        {
            try
            {
                if (!ModelState.IsValid)    // create eden zaman input null olarsa, yeni isvalid deyilse(isvalid olduqda data daxil edilir) view a qayit
                {
                    return View();
                }

                var existData = await _context.Categories.FirstOrDefaultAsync(m => m.Name.Trim().ToLower() == category.Name.Trim().ToLower());
                // yoxlayiriq bize gelen yeni inputa yazilan name databazada varsa  error chixartmaq uchun

                if (existData is not null) /*gelen data databazamizda varsa yeni null deyilse*/
                {
                    ModelState.AddModelError("Name", "This data already exist!"); // bu inputa  daxil olan name bazada vaxrsa error chixartsin. Name propertisinin altinda. buradaki Name hemin input-un adidir.

                    return View();
                }

                //int num = 1;
                //int num2 = 0;
                //int result = num / num2;  // her hansi error olduqda 

                await _context.Categories.AddAsync(category);  //bazadaki categorie tablesine category ni add edir liste
                await _context.SaveChangesAsync();    //save edir bazaya 
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return RedirectToAction("Error", new { msj = ex.Message}); // bashqa actiona arqument kimi data gonderirik.
            }
           
        }


        public IActionResult Error(string msj)
        {
            ViewBag.error = msj;   // bawqa actiondan gelen argumenti parametr kimi qebul edirik viewbag vasitesile;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)   //bu halda dele etdikde her yerden silinir. 
        {
            if (id is null) return BadRequest();

            Category category = await _context.Categories.FindAsync(id);   //databazada id li elementi tapiriq

            if(category is null) return NotFound();

            _context.Categories.Remove(category);    //gelen categoryni databazadan silirik

            await _context.SaveChangesAsync();   // deyiwikliyi databazaya save edirik

            return RedirectToAction(nameof(Index));
            
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(int? id)  // bele ancaq softdeleti deyiwir
        {
            if (id is null) return BadRequest();

            Category category = await _context.Categories.FindAsync(id);   //databazada id li elementi tapiriq

            if (category is null) return NotFound();

            category.SoftDelete = true;   // bazadan silmir  softdaleteni deyiwib true edir.

            await _context.SaveChangesAsync();   // deyiwikliyi databazaya save edirik

            return RedirectToAction(nameof(Index));

        }




        [HttpGet]
        public async Task<IActionResult> Edit(int? id)   // 1ci addimburada gelen id li categoryni bazadan tapib view a gonderib gostetririk sadece(yeni sehife avhilmalidi edit edeceyimiz categroy orada gorsenmelidir)
        {
            if (id is null) return BadRequest();

            Category category = await _context.Categories.FindAsync(id);   //databazada id li elementi tapiriq

            if (category is null) return NotFound();


            return View(category);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Category category)   // 2ci addim ise hemin sehifede olan categoryni tezede bazadan tapib, onun nameni deyiowmeliyik gelen teze name ye(yeni teze inputa daxil olana)
        {
            try
            {
                if (!ModelState.IsValid)    // create eden zaman input null olarsa, yeni isvalid deyilse(isvalid olduqda data daxil edilir) view a qayit
                {
                    return View();
                }


                if (id is null) return BadRequest();

                Category dbCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);   // databazani mewgul etmemek uchun(bir nov databazaya muraciet etdikde elaqe achiq qalmasin deye) AsNoTracking yaziriq

                if (dbCategory is null) return NotFound();

                if (dbCategory.Name.Trim().ToLower() == category.Name.Trim().ToLower())  // eyni adi tekrar yazib update elemek istedikde bura girir ve index e qayidir. 
                {
                    return RedirectToAction(nameof(Index));
                }

                //dbCategory.Name= category.Name;   // tek bir datasi varsa modelin bele edirik. bir neche propertisi varsa update olunaasi bu zaman awagidaki kimi edirik. her defe her propertini ayriliqda beraberlewdirib yoxlamamaq uchun

                _context.Categories.Update(category);  // modelin bir neche propertisi olduqda update elemek uchun bele yazilir

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { msj = ex.Message}); // bashqa actiona arqument kimi data gonderirik.
            }

        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id) 
        {
            if (id is null) return BadRequest();

            Category category = await _context.Categories.FindAsync(id);   //databazada id li elementi tapiriq

            if (category is null) return NotFound();



            return View(category);
        }

    }
}
