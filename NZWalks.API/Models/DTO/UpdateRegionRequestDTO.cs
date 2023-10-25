using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public required string Code { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
