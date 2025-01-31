using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models
{
    public class DeviceModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } // "PC" or "Display"

        [NotMapped]
        public int AssignedWorkStationsCount { get; set; }
    }
}
