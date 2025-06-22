using Nest;
using RealEstateManagement.Business.DTO.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.SearchProperties
{
    public class SearchProRepo : ISearchProRepo
    {
        private readonly RentalDbContext _context;
        private readonly IElasticClient _elasticClient;

        public SearchProRepo(RentalDbContext context, IElasticClient elasticClient)
        {
            _context = context;
            _elasticClient = elasticClient;
        }
        public async Task<bool> IndexPropertyAsync(PropertySearchDTO dto)
        {
            var response = await _elasticClient.IndexDocumentAsync(dto);
            return response.IsValid;
        }
        public async Task BulkIndexPropertiesAsync(IEnumerable<PropertySearchDTO> properties)
        {
            await _elasticClient.BulkAsync(b => b.IndexMany(properties));
        }
        public async Task<List<int>> SearchPropertyIdsAsync(string keyword)
        {
            ISearchResponse<PropertySearchDTO> response;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                response = await _elasticClient.SearchAsync<PropertySearchDTO>(s => s
                    .Sort(ss => ss.Ascending(p => p.PromotionPackageLevel))
                    .Query(q => q.MatchAll())
                );
            }
            else
            {
                response = await _elasticClient.SearchAsync<PropertySearchDTO>(s => s
                    .Query(q => q
                        .Bool(b => b
                            .Should(
                                s => s.MultiMatch(m => m
                                    .Fields(f => f
                                        .Field(p => p.Title)
                                        .Field(p => p.Type)
                                        .Field(p => p.DetailedAddress)
                                        .Field(p => p.Amenities)
                                    )
                                    .Query(keyword)
                                    .Fuzziness(Fuzziness.Auto)
                                ),
                                s => s.Prefix(p => p.Title, keyword.ToLower())  // hỗ trợ từ khóa ngắn
                            )
                        )
                    )

                );
            }

            if (!response.IsValid)
                throw new Exception($"Search failed: {response.OriginalException.Message}");

            return response.Documents.Select(d => d.Id).ToList();
        }
    }
}
