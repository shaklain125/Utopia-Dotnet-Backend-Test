namespace UtopiaBackendChallenge.Lib
{
    public class TopSalesPeopleDTO
    {
        public string? SalePerson { get; set; }
        public decimal TotalEarned { get; set; } = 0;
        public int TotalItems { get; set; } = 0;
        public List<TopProduct> TopProducts { get; set; }

        private Dictionary<int, (int Index, int Count)> productsDict;

        public TopSalesPeopleDTO()
        {
            TopProducts = new List<TopProduct>();
            productsDict = new Dictionary<int, (int Index, int Count)>();
        }

        public void addProduct(int ProductId)
        {
            if (!productsDict.ContainsKey(ProductId))
            {
                productsDict.Add(ProductId, (TopProducts.Count(), 0));
                TopProduct product = new TopProduct();
                product.ProductId = ProductId;
                TopProducts.Add(product);
            }
            productsDict[ProductId] = (productsDict[ProductId].Index, productsDict[ProductId].Count + 1);
            TopProducts[productsDict[ProductId].Index].Sold = productsDict[ProductId].Count;
        }

        public void sortProducts()
        {
            TopProducts.Sort((a, b) =>
            {
                var n = b.Sold - a.Sold;
                if (n != 0) return n;
                return a.ProductId - b.ProductId;
            });
        }

        public class TopProduct
        {
            public int ProductId { get; set; }
            public int Sold { get; set; } = 0;
        }
    }
}