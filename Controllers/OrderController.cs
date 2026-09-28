using ChemiseLab.Dto.Orders;
using ChemiseLab.IServices;
using ChemiseLab.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemiseLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderservice;
        public OrderController(IOrderService orderservice)
        {
            _orderservice = orderservice;
        }

        [HttpPost]
        public async Task<IActionResult> Create_New_Order([FromBody] NewOrder_Dto order)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Données invalides." });

            try
            {
                await _orderservice.Create_New_OrderAsync(order);
                return Ok(new { message = "Votre commande créée avec succès, Notre équipe ChemiseLab va vous contacter." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Une erreur est survenue lors de la création de votre commande." });
            }
        }
    }
}
