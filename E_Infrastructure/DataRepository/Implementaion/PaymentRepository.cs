using Application.DataReposatory.Interfaces;
using E_Domain.Models;
using E_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.DataRepository.Implementaion
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly DbSet<Payment> _paymentSet;
        public PaymentRepository(E_ApplicationDbContext _context):base(_context)
        {
            _paymentSet = this._context.Payments;
            
        }
    
    


  
        
    }
}
