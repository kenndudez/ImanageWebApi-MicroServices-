using System.ComponentModel.DataAnnotations;

namespace Imanage.Shared.ViewModels
{
    public class LocationViewModel
    {
        [Required]
        public string Address { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
