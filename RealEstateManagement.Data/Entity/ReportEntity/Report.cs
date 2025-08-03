using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Entity.ReportEntity
{
    public class Report
    {
        public int Id { get; set; }

        // Đối tượng bị report
        public int TargetId { get; set; }
        public string TargetType { get; set; } // "PropertyPost", "Review", "User"

        // Người report
        public int ReportedByUserId { get; set; }
        public ApplicationUser ReportedByUser { get; set; }

        public DateTime ReportedAt { get; set; }

        // Nội dung report
        public string Reason { get; set; }
        public string? Description { get; set; }

        // Xử lý
        public string Status { get; set; } = "Pending"; // Pending, Resolved, Ignored
        public int? ResolvedByUserId { get; set; }
        public ApplicationUser? ResolvedByUser { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? AdminNote { get; set; }
    }
}
