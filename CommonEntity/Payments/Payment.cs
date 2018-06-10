using System;

namespace Framework.Entities.Payments
{
    public class Payment
    {
        public int PaymentId { set; get; }

        public int AccountId { set; get; }

        public string PaymentCode { set; get; }

        public decimal TotalAmount { set; get; }

        public DateTime? CreatedAtDate { set; get; }
    }
}
