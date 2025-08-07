using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using NuGet.Protocol.Core.Types;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.DTO.SliderDTO;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.User;
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
                       .AddRouteComponents("api", GetEdmModel()));
            return services;
        }
        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<HomePropertyDTO>("Properties");
            builder.EntityType<HomePropertyDTO>().HasKey(u => u.Id);
            builder.EntitySet<InterestedPropertyDTO>("InterestedProperty");
            builder.EntityType<InterestedPropertyDTO>().HasKey(u => u.Id);

            // Thêm Slider DTO vào OData
            builder.EntitySet<SliderResponseDto>("Sliders");
            builder.EntityType<SliderResponseDto>().HasKey(s => s.Id);

            return builder.GetEdmModel();
        }
    }
}
