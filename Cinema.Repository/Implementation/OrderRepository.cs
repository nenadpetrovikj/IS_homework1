using Cinema.Domain.DomainModels;
using Cinema.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }
        public List<Order> getAllOrders()
        {
            return entities
           .Include(z => z.User)
           .Include(z => z.Tickets)
           .Include("Tickets.SelectedTicket")
           .ToListAsync().Result;
        }

        public Order getOrderDetails(BaseEntity model)
        {
            return entities
           .Include(z => z.User)
           .Include(z => z.Tickets)
           .Include("Tickets.SelectedTicket")
           .SingleOrDefaultAsync(z => z.Id == model.Id).Result;
        }
    }

}