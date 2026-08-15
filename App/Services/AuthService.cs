using Microsoft.EntityFrameworkCore;
using SupplyForge.App.Interfaces;
using SupplyForge.Database;
using SupplyForge.Domain.Entities;
using BCrypt.Net;

namespace SupplyForge.App.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext context, IJwtService jwtService, ILogger<AuthService> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto)
        {
            try
            {
                // Validar que el email no exista
                var userExists = await _context.Users.AnyAsync(u => u.Email == registerDto.Email);
                if (userExists)
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "El email ya está registrado."
                    };
                }

                // Validar requisitos de contraseña
                if (!ValidatePassword(registerDto.Password))
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "La contraseña debe contener al menos 8 caracteres, una mayúscula, una minúscula y un número."
                    };
                }

                // Hash de la contraseña
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                // Crear nuevo usuario
                var user = new User(
                    Guid.NewGuid(),
                    registerDto.Name,
                    registerDto.Email,
                    passwordHash
                );

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new AuthResponseDTO
                {
                    Success = true,
                    Message = "Registro exitoso. Por favor inicia sesión con tus credenciales.",
                    User = new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el registro");
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = $"Error durante el registro: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Primer paso del login: Valida credenciales y retorna todas las memberships del usuario
        /// Retorna PreAuthToken (JWT temporal válido por 5 minutos)
        /// </summary>
        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            try
            {
                // Validar entrada
                if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                {
                    return new LoginResponseDTO
                    {
                        Success = false,
                        Message = "Email y contraseña son requeridos."
                    };
                }

                // Buscar usuario por email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user == null)
                {
                    return new LoginResponseDTO
                    {
                        Success = false,
                        Message = "Email o contraseña incorrectos."
                    };
                }

                // Verificar contraseña
                var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
                if (!isPasswordValid)
                {
                    return new LoginResponseDTO
                    {
                        Success = false,
                        Message = "Email o contraseña incorrectos."
                    };
                }

                // Obtener todas las memberships del usuario
                var memberships = await _context.CompanyMembers
                    .Where(cm => cm.UserId == user.Id)
                    .Include(cm => cm.Role)
                    .Select(cm => new MembershipDTO
                    {
                        CompanyId = cm.CompanyId,
                        CompanyName = "Company", // TODO: Incluir Company en la query cuando esté disponible
                        RoleId = cm.Role.Id,
                        RoleName = cm.Role.Name
                    })
                    .ToListAsync();

                if (!memberships.Any())
                {
                    return new LoginResponseDTO
                    {
                        Success = false,
                        Message = "El usuario no pertenece a ninguna compañía."
                    };
                }

                // Generar JWT TEMPORAL (PreAuth Token)
                var preAuthToken = await _jwtService.GeneratePreAuthTokenAsync(user);

                return new LoginResponseDTO
                {
                    Success = true,
                    Message = "Credenciales validadas. Selecciona una compañía.",
                    PreAuthToken = preAuthToken,
                    UserId = user.Id,
                    UserName = user.Name,
                    Memberships = memberships
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el login");
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = $"Error durante el login: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Segundo paso del login: Genera JWT completo (Access Token) para la compañía seleccionada
        /// Este endpoint requiere validación de PreAuthToken vía [Authorize]
        /// </summary>
        public async Task<SelectCompanyResponseDTO> SelectCompanyAsync(SelectCompanyDTO selectCompanyDto)
        {
            try
            {
                // Validar entrada
                if (string.IsNullOrWhiteSpace(selectCompanyDto.Email) || selectCompanyDto.CompanyId == Guid.Empty)
                {
                    return new SelectCompanyResponseDTO
                    {
                        Success = false,
                        Message = "Email y Company ID son requeridos."
                    };
                }

                // Buscar usuario por email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == selectCompanyDto.Email);
                if (user == null)
                {
                    return new SelectCompanyResponseDTO
                    {
                        Success = false,
                        Message = "Usuario no encontrado."
                    };
                }

                // Validar que el usuario pertenezca a la compañía
                var membership = await _context.CompanyMembers
                    .Include(cm => cm.Role)
                    .FirstOrDefaultAsync(cm => cm.UserId == user.Id && cm.CompanyId == selectCompanyDto.CompanyId);

                if (membership == null)
                {
                    _logger.LogWarning($"Usuario {user.Email} intentó acceder a compañía {selectCompanyDto.CompanyId} sin permisos");
                    return new SelectCompanyResponseDTO
                    {
                        Success = false,
                        Message = "El usuario no pertenece a esta compañía."
                    };
                }

                // Generar JWT COMPLETO (Access Token) con información de la compañía y rol
                var token = await _jwtService.GenerateTokenAsync(user, selectCompanyDto.CompanyId);

                return new SelectCompanyResponseDTO
                {
                    Success = true,
                    Message = "Login exitoso.",
                    Token = token,
                    User = new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email
                    },
                    Company = new CompanyInfoDTO
                    {
                        Id = selectCompanyDto.CompanyId,
                        Name = "Company", // TODO: Obtener nombre de la compañía
                        RoleId = membership.Role.Id,
                        RoleName = membership.Role.Name
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al seleccionar compañía");
                return new SelectCompanyResponseDTO
                {
                    Success = false,
                    Message = $"Error al seleccionar compañía: {ex.Message}"
                };
            }
        }

        private bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);

            return hasUpperCase && hasLowerCase && hasDigit;
        }
    }
}
