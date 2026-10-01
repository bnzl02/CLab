using ChemiseLab.Dto.Products;

namespace ChemiseLab.Dto.Products
{
    public class Get_Products_Dto
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public decimal Product_Price { get; set; }
        public int Product_Couleur_ID { get; set; }
        public List<ProduitImages_Dto> Product_Images { get; set; } = new();
    }
}
