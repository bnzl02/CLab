using System.ComponentModel.DataAnnotations;

namespace ChemiseLab.Dto.Categories
{
    public class Add_SubCategory_Dto
    {
        [Required(ErrorMessage = "Le libellé est obligatoire.")]
        [MaxLength(20)]
        public string Libelle { get; set; } = string.Empty;

        [Required(ErrorMessage = "La catégorie parente est obligatoire.")]
        public int IdCategorie { get; set; }
    }
}
