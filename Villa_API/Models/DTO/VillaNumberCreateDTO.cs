using System.ComponentModel.DataAnnotations;

namespace Villa_API.Models.DTO
{
    //DTO lam viec voi API
    public class VillaNumberCreateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
    }
}
