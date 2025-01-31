using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models
{
    public class AspNetUserWorkStation
    {
        [Key]
        public int Id { get; set; }
        [ValidateNever]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int WorkStationId { get; set; }
        [ValidateNever]
        public WorkStation WorkStation { get; set; }
    }
}