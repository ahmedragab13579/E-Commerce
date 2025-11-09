using E_Domain.Enums;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataReposatory.Interfaces.Dashboard
{
    public interface IDashBoardRepository
    {
        public Task<int> GetTotalOrdersCountAsyncPerDayAsync();

        public Task<int> GetTotalOrdersCountAsyncPerMonthAsync();

        public Task<int> GetTotalOrdersCountAsyncPerYearAsync();



        public Task<int> GetOrdersCountByStatusAsync(OrderStatus status);


        public Task<int> GetNewUsersCountThisMonthAsync(TablesOperation operation);

        public Task<int> GetTotalClosedToFinishProductsCountAsyncAsync();
        
    }
}
