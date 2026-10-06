using ChemiseLab.Dto.Colors;

namespace ChemiseLab.Dto.Products
{
    public class Get_DetailsProduct_Dto
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Product_Desc { get; set; }
        public string Product_Ref { get; set; }
        public decimal Product_Price { get; set; }
        public int Product_Category { get; set; }
        public List<Get_SizesProduct_Dto> Product_Sizes { get; set; }
        public List<Get_ColorsProduct_Dto> Product_Colors { get; set; }
        public List<ProduitImages_Dto> Product_Images { get; set; }
    }
}
