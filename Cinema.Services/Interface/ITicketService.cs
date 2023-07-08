using Cinema.Domain.DomainModels;
using Cinema.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Services.Interface
{
    public interface ITicketService
    {
        
            List<Ticket> GetAllTickets();
            Ticket GetDetailsForTicket(Guid? id);
            void CreateNewTicket(Ticket p);
            void UpdeteExistingTicket(Ticket p);
            AddToShoppingCardDto GetShoppingCartInfo(Guid? id);
            void DeleteTicket(Guid id);
            bool AddToShoppingCart(AddToShoppingCardDto item, string userID);
        
    }
}