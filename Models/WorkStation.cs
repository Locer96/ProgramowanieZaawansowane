using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models
{
    public class WorkStation
    {
        public int Id { get; set; }

        public AspNetUserWorkStation? UserWorkStation { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Work Station Number")]
        public required string WorkStationNumber { get; set; }

        [MaxLength(100)]
        [Display(Name = "PC Serial Number")]
        public string? PCSerialNumber { get; set; }

        [MaxLength(100)]
        public string? PC { get; set; }

        [MaxLength(100)]
        public string? Display { get; set; }

        public bool Keyboard { get; set; }
        public bool Mouse { get; set; }

        [MaxLength(400)]
        [Display(Name = "Additional Info")]
        public string? AdditionalInfo { get; set; }
    }
}
