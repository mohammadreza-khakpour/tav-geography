namespace Geography.Services.Tests.Unit.Products.Add
{
    public class AddProductDto
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public int MinimumInventory { get; set; }
        public int CategoryId { get; set; }
    }
}
