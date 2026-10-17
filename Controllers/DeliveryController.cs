using ChemiseLab.Dto.Delivery;
using ChemiseLab.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemiseLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        // GET: api/delivery/districts
        [HttpGet("districts")]
        public async Task<ActionResult<IEnumerable<DistrictsList_Dto>>> GetDistricts()
        {
            try
            {
                var districts = await _deliveryService.GetDistrictsAsync();
                return Ok(districts);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Impossible de récupérer la liste des villes !!" });
            }
        }
    }
}
