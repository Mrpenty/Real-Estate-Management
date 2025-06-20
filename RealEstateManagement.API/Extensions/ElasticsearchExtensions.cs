using Nest;
using RealEstateManagement.Business.DTO.Properties;

namespace RealEstateManagement.API.Extensions
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration config)
        {
            var url = config["Elasticsearch:Uri"];
            var index = config["Elasticsearch:IndexName"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(index);

            var client = new ElasticClient(settings);

            // Nếu index chưa tồn tại thì tạo mới
            if (!client.Indices.Exists(index).Exists)
            {
                client.Indices.Create(index, c => c
                    .Map<PropertySearchDTO>(m => m
                        .AutoMap()
                        .Properties(p => p
                            .Number(n => n
                                .Name(nm => nm.PackageLevel)
                                .Type(NumberType.Integer)
                            )
                        )
                    )

                );
            }

            services.AddSingleton<IElasticClient>(client);
        }

    }
}
