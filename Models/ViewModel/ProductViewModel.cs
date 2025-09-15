namespace NextUses.Models.ViewModel
{
    public class ProductViewModel
    {
        public Product? Product { get; set; } = new Product();
        public List<Category>? Categories { get; set; } = new List<Category>();
    }
}
