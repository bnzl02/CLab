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
        var categories = await _categorieService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    // GET: api/categories/5/sous-categories
    [HttpGet("{id}/sous-categories")]
    public async Task<ActionResult<IEnumerable<Get_S_Categories_Dto>>> GetAllSousCategories(int id)
    {
        var sousCategories = await _categorieService.GetAll_Sub_Categories_ByIdCat_Async(id);

        if (!sousCategories.Any())
            return NotFound($"Aucune sous-catégorie trouvée pour la catégorie " +
                $"{_categorieService.GetCategoryNameByIdAsync(id)}");

        return Ok(sousCategories);
    }
}