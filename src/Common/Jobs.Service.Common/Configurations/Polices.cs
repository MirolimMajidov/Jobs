using Jobs.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Jobs.Service.Common.Helpers;

namespace Jobs.Service.Common.Configurations
{
    public static class PolicesExtensions
    {
        public static void AddPolices(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                var roleSuperAdmin = UserRole.SuperAdmin.GetDisplayName();
                var roleAdmin = UserRole.Admin.GetDisplayName();
                var roleEditor = UserRole.Editor.GetDisplayName();
                var roleUser = UserRole.User.GetDisplayName();

                options.AddPolicy("SuperAdministrators", policy => policy.RequireRole(roleSuperAdmin));
                options.AddPolicy("Administrators", policy => policy.RequireRole(roleSuperAdmin, roleAdmin));
                options.AddPolicy("Editors", policy => policy.RequireRole(roleSuperAdmin, roleAdmin, roleEditor));
                options.AddPolicy("AllUsers", policy => policy.RequireRole(roleSuperAdmin, roleAdmin, roleEditor, roleUser));
            });
        }
    }
}
