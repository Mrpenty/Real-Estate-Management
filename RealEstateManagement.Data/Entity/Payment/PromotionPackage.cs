using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.Data.Entity.Payment
{
    public class PromotionPackage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public int Level { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true; // Default to active
        public List<PropertyPromotion> PropertyPromotions { get; set; } = new List<PropertyPromotion>();

    }
}
