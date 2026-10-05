using System.ComponentModel.DataAnnotations;

namespace ChemiseLab.Dto.Categories
{
    public class AddCategory_Dto
    {
        [Required(ErrorMessage = "Le libellé est obligatoire.")]
        [MaxLength(20)]
        public string Libelle { get; set; } = string.Empty;
    }
}
