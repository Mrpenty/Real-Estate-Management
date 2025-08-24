using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class RentalPaymentResultDto
    {
        public int ContractId { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public int LateDays { get; set; }
    }

}
