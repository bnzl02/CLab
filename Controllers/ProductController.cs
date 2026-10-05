using ChemiseLab.Dto.Categories;
using ChemiseLab.Dto.Products;
using ChemiseLab.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ChemiseLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductsService productsService, ICategoryService categoryService)
        {
            _productService = productsService;
            _categoryService = categoryService;
        }

        // GET: api/product/sous-categorie/5
        [HttpGet("sous-categorie/{idSc}")]
        public async Task<ActionResult<IEnumerable<Get_Products_Dto>>> GetAllProductsBySousCategorie(int idSc)
        {
            try
            {
                var products = await _productService.Get_AllProducts_ByIdSubCat_Async(idSc);

                if (!products.Any())
                {
                    var nomSousCategorie = await _categoryService.GetSubCategoryNameByIdAsync(idSc);
                    return NotFound(new { message = $"Aucun produit trouvé pour la sous-catégorie: {nomSousCategorie}" });
                }

                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération des produits." });
            }
        }

        // GET: api/product/categorie/5
        [HttpGet("categorie/{idC}")]
        public async Task<ActionResult<IEnumerable<Get_Products_Dto>>> GetAllProductsByCategorie(int idC)
        {
            try
            {
                var products = await _productService.Get_AllProducts_ByIdCat_Async(idC);

                if (!products.Any())
                {
                    var nomCategorie = await _categoryService.GetCategoryNameByIdAsync(idC);
                    return NotFound(new { message = $"Aucun produit trouvé pour la catégorie: {nomCategorie}" });
                }

                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération des produits." });
            }
        }

        // GET: api/product/5
        [HttpGet("{idProduct}/couleur/{idColor}")]
        public async Task<ActionResult<Get_DetailsProduct_Dto>> GetDetailProductByIdP(int idProduct, int idColor)
        {
            try
            {
                var product = await _productService.Get_DetailsProduct_ByIdAsync(idProduct, idColor);

                if (product == null)
                    return NotFound($"Aucun produit trouvé pour l'id {idProduct}");

                return Ok(product);
            }
            catch (Exception) {
                return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération de détails produit." });
            }

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Get_Products_Dto>>> GetAllProducts()
        {
            var All_A_Products = await _productService.GetAllProductsAsync();
            return Ok(All_A_Products);
        }

        // POST: api/product/by-Listcategories
        [HttpPost("by-Listcategories")]
        public async Task<ActionResult<IEnumerable<Get_Products_Dto>>> GetAllProductsByListIdCat([FromBody] List<int> idCat)
        {
            try
            {
                if (idCat == null || !idCat.Any())
                    return BadRequest(new { message = "La liste d'identifiants ne peut pas être vide." });

                var products = await _productService.Get_AllProducts_ByListIdCat_Async(idCat);

                if (!products.Any())
                    return NotFound(new { message = "Aucun produit trouvé pour ces catégories." });

                return Ok(products);
            }
            catch (Exception) {
                return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération des produits." });
            }
        }

        // GET: api/product/except-categorie/5
        [HttpGet("except-categorie/{idCat}")]
        public async Task<ActionResult<IEnumerable<Get_Products_Dto>>> GetAllProductsExceptIdCat(int idCat)
        {
            try
            {
                var products = await _productService.Get_AllProducts_ExceptIdCat_Async(idCat);

                if (!products.Any())
                    return NotFound(new { message = "Aucun produit trouvé." });

                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération des produits." });
            }
        }

        // POST: api/product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] AddProduct_Dto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var idProduit = await _productService.CreateProductAsync(productDto);
                return CreatedAtAction(nameof(GetDetailProductByIdP), new { idPrdct = idProduit },
                    new { message = "Produit créé avec succès.", id = idProduit });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Une erreur est survenue lors de la création du produit." });
            }
        }
    }
}