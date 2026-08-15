using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SupplyForge.App.Interfaces;

namespace SupplyForge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.RegisterAsync(registerDto);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Register");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Paso 1: Valida credenciales y retorna memberships disponibles + PreAuthToken
        /// PreAuthToken es válido por 5 minutos y se usa para acceder a /select-company
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.LoginAsync(loginDto);

                if (!result.Success)
                {
                    return Unauthorized(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Login");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Paso 2: Selecciona compañía y genera JWT completo (Access Token)
        /// [Authorize] valida que el cliente tenga un PreAuthToken válido en el header Authorization
        /// Si no tiene token o está expirado → 401 Unauthorized
        /// </summary>
        [HttpPost("select-company")]
        [Authorize] // ← Valida que haya un JWT válido en el header
        public async Task<IActionResult> SelectCompany([FromBody] SelectCompanyDTO selectCompanyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.SelectCompanyAsync(selectCompanyDto);

                if (!result.Success)
                {
                    return Unauthorized(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en SelectCompany");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}
