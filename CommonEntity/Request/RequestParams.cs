namespace Framework.Entities.Request
{
    public class RequestParams
    {
        public string Conditional { set; get; }

        public int PageSize { set; get; } = 9;

        public int PageIndex { set; get; }

        public int OrganizationId { set; get; }

        public int ProductCategoryId { set; get; }

        public int ProductId { set; get; }

        public int AccountId { set; get; }
    }
}
