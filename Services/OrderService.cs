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
                //TotalOrder = ,
                //ReferenceLivraison
                NomClient = orderDto.Clt_LName,
                PrenomClient = orderDto.Clt_Name,
                AdresseClient = orderDto.Clt_Adress,
                TelephoneClient = orderDto.Clt_Tele,
                VilleClient = orderDto.Clt_City,
                PaysClient = orderDto.Clt_Country,
            };

            _contextDB.Orders.Add(order);
            await _contextDB.SaveChangesAsync();

            decimal total = 0;

            foreach (var ligneDto in orderDto.Lignes)
            {
                if (!prixProduits.TryGetValue(ligneDto.Product_ID, out var prixUnitaire))
                    throw new ArgumentException($"Produit introuvable : {ligneDto.Product_ID}");

                var ligne = new LigneOrder
                {
                    IdOrder = ligneDto.Product_ID,
                    Quantite = ligneDto.Quantity,
                    PrixUnitaire = ligneDto.Single_Price,
                    IdProduit = ligneDto.Product_ID,
                    IdCouleur = ligneDto.Color_ID,
                    IdTaille = ligneDto.Size_ID,
                };

                _contextDB.LigneOrders.Add(ligne);
                total += prixUnitaire * ligneDto.Quantity;
            }

            order.TotalOrder = total;
            await _contextDB.SaveChangesAsync();
        }

    }
}
