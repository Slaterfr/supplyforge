using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SupplyForge.App.Interfaces;
using SupplyForge.Database;
using SupplyForge.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SupplyForge.App.Services
{
    public class JwtService : IJwtService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;

        public JwtService(ApplicationDbContext context, IConfiguration configuration, ILogger<JwtService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Genera JWT Temporal (PreAuth Token)
        /// Solo contiene: sub (userId), email, iss, aud, exp
        /// Expira en 5 minutos (por defecto)
        /// Usado para validar en /select-company endpoint
        /// </summary>
        public async Task<string> GeneratePreAuthTokenAsync(User user)
        {
            var jwtSecret = _configuration["Jwt:Secret"]
                ?? throw new InvalidOperationException("JWT Secret no configurado en appsettings.");

            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "SupplyForge";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "SupplyForgeUsers";
            var preAuthExpiryMinutes = int.Parse(_configuration["Jwt:PreAuthTokenExpiryMinutes"] ?? "5");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims mínimos para PreAuth
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Name)
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(preAuthExpiryMinutes),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Genera JWT Completo (Access Token)
        /// Contiene: sub, email, role_id, role_name, tenant_id, company_id
        /// Expira en 15 minutos (por defecto)
        /// Usado para requests posteriores a /select-company
        /// </summary>
        public async Task<string> GenerateTokenAsync(User user, Guid companyId)
        {
            // Buscar el membership del usuario en la compañía especificada
            var membership = await _context.CompanyMembers
                .Include(cm => cm.Role)
                .FirstOrDefaultAsync(cm => cm.UserId == user.Id && cm.CompanyId == companyId);

            if (membership == null)
            {
                _logger.LogWarning($"Usuario {user.Email} no pertenece a la compañía {companyId}");
                throw new InvalidOperationException($"El usuario no pertenece a la compañía especificada.");
            }

            // Obtener configuración JWT
            var jwtSecret = _configuration["Jwt:Secret"]
                ?? throw new InvalidOperationException("JWT Secret no configurado en appsettings.");

            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "SupplyForge";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "SupplyForgeUsers";
            var accessTokenExpiryMinutes = int.Parse(_configuration["Jwt:AccessTokenExpiryMinutes"] ?? "15");

            // Crear clave de firma
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crear claims con información completa (usuario + rol + tenant)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim("role_id", membership.Role.Id.ToString()),
                new Claim("role_name", membership.Role.Name),
                new Claim("tenant_id", companyId.ToString()),
                new Claim("company_id", companyId.ToString())
            };

            // Crear token JWT
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenExpiryMinutes),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
