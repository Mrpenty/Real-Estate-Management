using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.ReportDTO
{
    public class ReportCreateDto
    {
        public int TargetId { get; set; }
        public string Reason { get; set; }
        public string? Description { get; set; }
    }

}
