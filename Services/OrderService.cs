using ChemiseLab.Data;
using ChemiseLab.Dto.Orders;
using ChemiseLab.IServices;
using ChemiseLab.Models;
using Microsoft.EntityFrameworkCore;

namespace ChemiseLab.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _contextDB;

        public OrderService(ApplicationDbContext context)
        {
            _contextDB = context;
        }

        public async Task Create_New_OrderAsync(NewOrder_Dto orderDto)
        {
            if (orderDto.Lignes == null || !orderDto.Lignes.Any())
                throw new ArgumentException("La commande doit contenir au moins un article.");

            var idsProduits = orderDto.Lignes.Select(l => l.Product_ID).Distinct().ToList();

            var prixProduits = await _contextDB.Produits
                .AsNoTracking()
                .Where(p => idsProduits.Contains(p.IdProduit))
                .ToDictionaryAsync(p => p.IdProduit, p => p.Prix);

            var order = new Order
            {
                DateOrder = DateTime.Now,
                StatutOrder = "CR", // CR ==> VL ==> (LV ; AN ; RF)
                TotalOrder = orderDto.TotalAmount,
                //ReferenceLivraison
                NomClient = orderDto.Clt_LName,
                PrenomClient = orderDto.Clt_Name,
                AdresseClient = orderDto.Clt_Adress,
                TelephoneClient = orderDto.Clt_Tele,
                VilleClient = orderDto.Clt_City,
                PaysClient = orderDto.Clt_Country,
            };

            foreach (var ligneDto in orderDto.Lignes)
            {
                if (!prixProduits.TryGetValue(ligneDto.Product_ID, out var prixUnitaire))
                    throw new ArgumentException($"Produit introuvable : {ligneDto.Product_ID}");

                // Rattachée via la navigation : la commande et ses lignes sont enregistrées ensemble (un seul SaveChanges = une seule transaction).
                order.LigneOrders.Add(new LigneOrder
                {
                    Quantite = ligneDto.Quantity,
                    PrixUnitaire = prixUnitaire,
                    IdProduit = ligneDto.Product_ID,
                    IdCouleur = ligneDto.Color_ID,
                    IdTaille = ligneDto.Size_ID,
                });

                int _stockPrdct = await _contextDB.Stocks
                    .Where(s => s.IdProduit == ligneDto.Product_ID
                             && s.IdTaille == ligneDto.Size_ID
                             && s.IdCouleur == ligneDto.Color_ID)
                    .Select(s => s.Stock1)
                    .FirstOrDefaultAsync();

                _stockPrdct = _stockPrdct - 1;
                _contextDB.Entry(_stockPrdct).State = EntityState.Modified;
            }

            _contextDB.Orders.Add(order);
            await _contextDB.SaveChangesAsync();
        }

    }
}
