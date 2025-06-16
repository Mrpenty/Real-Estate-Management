using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Properties
{
    public class PropertyFilterDTO
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        public decimal? MinArea { get; set; }
        public decimal? MaxArea { get; set; }
        public string? Location { get; set; }
        public List<int>? AmenityIds { get; set; }
        public string? PropertyType { get; set; }
        public bool? IsVerified { get; set; }
        public string? SearchKeyword { get; set; }
    }

}
