namespace ChemiseLab.Dto.Products
{
    public class Get_AllProducts_Suggestion_Dto
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Product_Ref { get; set; }
        public string Product_Catg { get; set; }
        public string Product_S_Catg { get; set; }
        public decimal Product_Price { get; set; }
        public string Product_Status { get; set; }
        public List<ProduitImages_Dto> Product_Images { get; set; } = new();
    }
}
