using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        [ValidateNever]
        public string UserId { get; set; }

        /// <summary>
        /// When User got their Inventory - to be replaced in 2 years
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }
        public string? PC { get; set; }
        public string? Display { get; set; }
        public string? Keyboard { get; set; }
        public string? Mouse { get; set; }
    }
}
