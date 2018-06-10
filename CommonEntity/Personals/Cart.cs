namespace Framework.Entities.Personals
{
    public class Cart
    {
        public int OrganizationId { set; get; }

        public int ProductId { set; get; }

        public string Name { set; get; }

        public decimal Price { set; get; }

        public int Quantity { set; get; }
    }
}
