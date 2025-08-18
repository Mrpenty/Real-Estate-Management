using System;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;

namespace RealEstateManagement.Data.Entity
{
    public class RentalContract
    {
        public int Id { get; set; }
        public int PropertyPostId { get; set; }
        public int LandlordId { get; set; }
        public int? RenterId { get; set; }

        public decimal DepositAmount { get; set; } // Tiền cọc
        public decimal MonthlyRent { get; set; } // Số tiền trả mỗi tháng
        public int ContractDurationMonths { get; set; } // thời hạn hợp đồng
        public PaymentCycleType PaymentCycle { get; set; } = PaymentCycleType.Monthly; //thanh toán kỳ hạn

        public int PaymentDayOfMonth { get; set; } = 5; // Mặc định trả trước ngày 5 hàng kỳ

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate => StartDate?.AddMonths(ContractDurationMonths);

        public DateTime? LastPaymentDate { get; set; }

        public string PaymentMethod { get; set; } // Phương thức thanh toán
        public string ContactInfo { get; set; } // Phương thức liên lạc

        public ContractStatus Status { get; set; } = ContractStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ConfirmedAt { get; set; }

        public PropertyPost PropertyPost { get; set; }
        public ApplicationUser Landlord { get; set; }
        public ApplicationUser Renter { get; set; }

        public decimal? ProposedMonthlyRent { get; set; }
        public int? ProposedContractDurationMonths { get; set; }
        public PaymentCycleType? ProposedPaymentCycle { get; set; }
        public int? ProposedPaymentDayOfMonth { get; set; }
        public DateTime? ProposedEndDate { get; set; }

        public DateTime? ProposedAt { get; set; } // Ngày landlord đề xuất
        public bool? RenterApproved { get; set; } // null = chưa phản hồi, true = đồng ý, false = từ chối

        public enum ContractStatus
        {
            Pending = 0, //Tạo hợp đồng lần đầu, chờ renter xác nhận khi thuê
            Confirmed = 1, //Hợp đồng đang cho thuê
            Rejected = 2, //Hợp đồng bị chấm dứt
            Expired = 3, //Hợp đồng hết hạn
            RenewalPending = 4 // landlord đề xuất gia hạn, chờ renter duyệt
        }

        public enum PaymentCycleType
        {
            Monthly = 0,
            Quarterly = 1,
            Yearly = 2
        }
    }
}
