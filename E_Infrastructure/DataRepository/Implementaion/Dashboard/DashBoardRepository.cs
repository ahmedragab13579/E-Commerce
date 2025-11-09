using Application.DataReposatory.Interfaces.Dashboard;
using E_Domain.Enums;
using E_Domain.Models;
using E_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.DataRepository.Implementaion.Dashboard
{
    public class DashBoardRepository: IDashBoardRepository
    {
     
        private readonly E_ApplicationDbContext _context;

     
        public DashBoardRepository(E_ApplicationDbContext Context)
        {
            this._context= Context;
        
        }

        public   async Task<int> GetTotalOrdersCountAsyncPerDayAsync()
        {
            return (  await _context.Orders.CountAsync(m => m.OrderDate == DateTime.UtcNow.Date)) ;
        }
        public  Task<int> GetTotalOrdersCountAsyncPerMonthAsync()
        {
            return (_context.Orders.CountAsync(m => m.OrderDate.Month == DateTime.UtcNow.Month&& m.OrderDate.Year == DateTime.UtcNow.Year));
        }
        public  Task<int> GetTotalOrdersCountAsyncPerYearAsync()
        {
            return (_context.Orders.CountAsync(m => m.OrderDate.Year == DateTime.UtcNow.Year));
        }


        public Task<int> GetOrdersCountByStatusAsync(OrderStatus status)
        {
            return _context.Orders.CountAsync(m => m.Status == status);
        }

        public async Task<int> GetNewUsersCountThisMonthAsync(TablesOperation operation)
        {
            var UserEntityType = _context.Model.FindEntityType(typeof(User));

            if (UserEntityType == null)
            {
                return 0;
            }

            var UserTableName = UserEntityType.GetTableName();

            var now = DateTime.UtcNow;

            return await _context.AuditLogs.CountAsync(log =>
                log.TableName == UserTableName &&
                log.OperationType == operation.ToString() &&
                log.ChangeDate.Year == now.Year &&
                log.ChangeDate.Month == now.Month
            );
        }
        public Task<int> GetTotalClosedToFinishProductsCountAsyncAsync()
        {
            return (_context.Products.CountAsync(m => m.Stock <= 50));
        }

    }
}
