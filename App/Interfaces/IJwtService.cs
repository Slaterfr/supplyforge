using SupplyForge.Domain.Entities;

namespace SupplyForge.App.Interfaces
{
    public interface IJwtService
    {
        /// <summary>
        /// Genera JWT temporal (PreAuth) solo con información del usuario
        /// Expira en 5 minutos (desarrollo) o según configuración
        /// Se usa para validar en /select-company
        /// </summary>
        Task<string> GeneratePreAuthTokenAsync(User user);

        /// <summary>
        /// Genera JWT completo (Access) con información del usuario y la compañía
        /// Expira en 15 minutos (desarrollo) o según configuración
        /// Incluye role_id, role_name, tenant_id, company_id
        /// </summary>
        Task<string> GenerateTokenAsync(User user, Guid companyId);
    }
}
