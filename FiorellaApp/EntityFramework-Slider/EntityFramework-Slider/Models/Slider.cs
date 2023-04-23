using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework_Slider.Models
{
    public class Slider:BaseEntity
    {
        public string Image { get; set; }


        [Required(ErrorMessage ="Don't be empty")]
        [NotMapped]  // hansisa propertyni bazaya salmamaq uhcun istifade edilir.// IFormFile sadece UI-da inputdan file sechende file olaraq goture bilek deye 
        public IFormFile Photo { get; set; } // fayllarnan iwleyende bu propertini qoyuruq// IFormFile sirf fayllarnan iwlemek uchundur sistemin ozunden gelir
    }
}
