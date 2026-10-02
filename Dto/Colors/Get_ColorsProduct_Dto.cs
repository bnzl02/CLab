using ChemiseLab.Dto.Products;

namespace ChemiseLab.Dto.Colors
{
    public class Get_ColorsProduct_Dto
    {
        public int ID { get; set; }
        public List<string> HexaCode { get; set; } = new();   // remplace Libelle
        public List<Get_SizesProduct_Dto> Sizes { get; set; } = new();
    }
}
