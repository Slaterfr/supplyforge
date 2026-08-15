namespace SupplyForge.App.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto);
        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto);
        Task<SelectCompanyResponseDTO> SelectCompanyAsync(SelectCompanyDTO selectCompanyDto);
    }
}
