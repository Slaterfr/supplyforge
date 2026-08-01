
using System.ComponentModel.DataAnnotations;

public class ProductDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required]
    public decimal Price { get; set; }
    [Required]
    public decimal Weight { get; set; }
}

public class ShipmentDTO
{
    [Required]
    public string Origin { get; set; } = string.Empty;
    [Required]
    public string Destination { get; set; } = string.Empty;
    [Required]
    public DateTime ShipmentDate { get; set; }
    //[Required]
    // public List<ShipmentItemDTO> Items { get; set; } = new List<ShipmentItemDTO>();
}