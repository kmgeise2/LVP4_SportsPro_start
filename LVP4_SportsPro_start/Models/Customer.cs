using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LVP4_SportsPro_start.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(50)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(50)]
        public string City { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(50)]
        public string State { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(20)]
        public string PostalCode { get; set; }

        public string CountryID { get; set; }
        public Country Country { get; set; }

        [RegularExpression(@"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$",
            ErrorMessage = "Phone number must be in (999) 999-9999 format.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Remote("CheckEmail", "Validation", AdditionalFields = "CustomerID")]
        public string Email { get; set; }

        // navigation property to linking entity
        // Add the code here

        public string FullName => FirstName + " " + LastName;   // read-only property
    }
}