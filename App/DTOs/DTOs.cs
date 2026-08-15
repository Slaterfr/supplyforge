
using System.ComponentModel.DataAnnotations;

public class RegisterDTO
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "La confirmación de contraseña es requerida.")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class LoginDTO
{
    [Required(ErrorMessage = "El email es requerido.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida.")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? PreAuthToken { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public List<MembershipDTO> Memberships { get; set; } = new();
}

public class SelectCompanyDTO
{
    [Required(ErrorMessage = "El email es requerido.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ID de compañía es requerido.")]
    public Guid CompanyId { get; set; }
}

public class SelectCompanyResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public UserDTO? User { get; set; }
    public CompanyInfoDTO? Company { get; set; }
}

public class MembershipDTO
{
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

public class CompanyInfoDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

public class UserDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class AuthResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public UserDTO? User { get; set; }
}

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

public class ClientDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public string ContactInfo { get; set; } = string.Empty;
}

public class OrderDTO
{
    [Required]
    public Guid ClientId { get; set; }
}

public class ShipmentItemDTO
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Unit quantity must be a positive integer.")]
    public int UnitQuantity { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Unit weight cannot be negative.")]
    public decimal UnitWeight { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Unit price cannot be negative.")]
    public decimal UnitPrice { get; set; }
}

public class VehicleDTO
{
    [Required]
    public string PlateNumber { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Max load must be greater than zero.")]
    public decimal MaxLoad { get; set; }
}