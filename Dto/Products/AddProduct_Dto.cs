using System.ComponentModel.DataAnnotations;

namespace ChemiseLab.Dto.Products
{
    public class AddProduct_Dto
    {
        [Required(ErrorMessage = "Le libellé est obligatoire.")]
        [MaxLength(30)]
        public string Product_Libelle { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est obligatoire.")]
        [MaxLength(100)]
        public string? Product_Description { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0.")]
        public decimal Product_Prix { get; set; }

        [Required(ErrorMessage = "La référence est obligatoire.")]
        [MaxLength(10)]
        public string Product_Reference { get; set; } = string.Empty;

        [Required(ErrorMessage = "La sous-catégorie est obligatoire.")]
        public int IdSousCategorie { get; set; }
    }
}
