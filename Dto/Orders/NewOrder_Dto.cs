using System.ComponentModel.DataAnnotations;

namespace ChemiseLab.Dto.Orders
{
    public class NewOrder_Dto
    {
        [Required]
        public Decimal TotalAmount { get; set; }
        [Required, MaxLength(20)]
        public string Clt_Name { get; set; } = string.Empty;
        [Required, MaxLength(20)]
        public string Clt_LName { get; set; } = string.Empty;
        [Required]
        public string Clt_Adress { get; set; } = string.Empty;
        [Required]
        public string Clt_Tele { get; set; } = string.Empty;
        [Required]
        public string Clt_City { get; set; } = string.Empty;
        public string Clt_Country { get; set; } = string.Empty;

        [Required, MinLength(1, ErrorMessage = "La commande doit contenir au moins un article.")]
        public List<LigneCommande_Dto> Lignes { get; set; } = new();
    }
}
