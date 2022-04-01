using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;  // Add to project specifications

namespace LVP4_SportsPro_start.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        public string ProductCode { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, 1000000)]
        [Column(TypeName = "decimal(8,2)")]          // Add to project specifications
        public decimal YearlyPrice { get; set; }

        public DateTime ReleaseDate { get; set; } = DateTime.Now;

        // navigation property to linking entity
        // Add code here
        
    }
}
