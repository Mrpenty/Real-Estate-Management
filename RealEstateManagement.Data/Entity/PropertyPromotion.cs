using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Entity
{
    public class PropertyPromotion
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int PackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        

        public Property Property { get; set; }
        public PromotionPackage PromotionPackage { get; set; }
    }
}
