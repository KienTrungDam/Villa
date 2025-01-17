﻿using System.ComponentModel.DataAnnotations;

namespace Villa_Web.Models.DTO
{
    //DTO lam viec voi API
    public class VillaNumberUpdateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }
    }
}
