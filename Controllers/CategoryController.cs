using ChemiseLab.Dto.Categories;
using ChemiseLab.IServices;
using ChemiseLab.Models;
using ChemiseLab.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChemiseLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categorieService;

    public CategoriesController(ICategoryService categorieService)
    {
        _categorieService = categorieService;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCategories_Dto>>> GetAllCategories()
    {
        try
        {
            var categories = await _categorieService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération des catégories." });
        }
    }

    // GET: api/categories/5/sous-categories
    [HttpGet("{id}/sous-categories")]
    public async Task<ActionResult<IEnumerable<Get_S_Categories_Dto>>> GetAllSousCategories(int id)
    {
        try
        {
            var sousCategories = await _categorieService.GetAll_Sub_Categories_ByIdCat_Async(id);

            if (!sousCategories.Any())
            {
                var nomCategorie = await _categorieService.GetCategoryNameByIdAsync(id);
                return NotFound(new { message = $"Aucune sous-catégorie trouvée pour la catégorie {nomCategorie}" });
            }

            return Ok(sousCategories);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération des sous-catégories." });
        }
    }

    // GET: api/categories/exclude/5
    [HttpGet("excepCategory/{idCategorie}")]
    public async Task<ActionResult<IEnumerable<GetCategories_Dto>>> GetAllCategoriesExcept(int idCategorie)
    {
        try
        {
            var categories = await _categorieService.GetAllCategoriesExceptAsync(idCategorie);

            if (!categories.Any())
                return NotFound(new { message = "Aucune autre catégorie trouvée." });

            return Ok(categories);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération des catégories." });
        }
    }
}