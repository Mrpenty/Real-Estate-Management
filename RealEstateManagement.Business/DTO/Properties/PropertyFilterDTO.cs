using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Properties
{
    public class PropertyFilterDTO
    {
        //public string ScopePrice { get; set; }
        public string? Type { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        //public string ScopeArea { get; set; }
        public decimal? MinArea { get; set; }
        public decimal? MaxArea { get; set; }
        public string? Location { get; set; }
        public List<string>? AmenityName { get; set; }
        public string? PropertyType { get; set; }
        public bool? IsVerified { get; set; }
        public string? SearchKeyword { get; set; }
        public int UserId { get; set; }
        public List<int> Provinces { get; set; }
        public List<int> Wards { get; set; }
        public List<int> Streets { get; set; }
    }

}
