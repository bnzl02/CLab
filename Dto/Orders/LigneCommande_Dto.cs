namespace ChemiseLab.Dto.Orders
{
    public class LigneCommande_Dto
    {
        public int Order_ID { get; set; }
        public int Product_ID { get; set; }
        public int Color_ID { get; set; }
        public int Size_ID { get; set; }
        public int Quantity { get; set; }
        public decimal Single_Price { get; set; }
    }
}
