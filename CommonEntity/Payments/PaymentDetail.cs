namespace Framework.Entities.Payments
{
    public class PaymentDetail
    {

        public int PaymentDetailId { set; get; }

        public int PaymentId { set; get; }

        public int ProductId { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }
    }
}
