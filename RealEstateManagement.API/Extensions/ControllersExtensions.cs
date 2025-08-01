using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using NuGet.Protocol.Core.Types;
using RealEstateManagement.Business.DTO.Properties;
using System.Text.Json.Serialization;

namespace RealEstateManagement.API.Extensions
{
    public static class ControllersExtensions
    {
        public static IServiceCollection AddControllersServices(this IServiceCollection services)
        {
            services.AddControllers()
             
             .AddOData(options =>
                options.Select()
                       .Filter()
                       .OrderBy()
                       .Expand()
                       .Count()
                       .SetMaxTop(100)
                       .AddRouteComponents("api/Property", GetEdmModel()));
            return services;
        }
        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<HomePropertyDTO>("Properties");
            builder.EntityType<HomePropertyDTO>().HasKey(u => u.Id);
            
            return builder.GetEdmModel();
        }
    }
}
