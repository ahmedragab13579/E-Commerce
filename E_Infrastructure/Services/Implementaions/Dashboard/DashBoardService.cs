using Application.DataReposatory.Interfaces.Dashboard;
using Application.Dtos.DashBoard;
using Application.Results;
using Application.Services.InterFaces.Dashboard;
using E_Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.Services.Implementaions.Dashboard
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IDashBoardRepository _dashBoardRepository;
        public DashBoardService(IDashBoardRepository _dashBoardRepository)
        {
            this._dashBoardRepository = _dashBoardRepository;

        }
        public async Task<Result<DashboardDto>> GetTotalDashBoardInformation()
        {
            var newUsersTask = _dashBoardRepository.GetNewUsersCountThisMonthAsync(TablesOperation.Add);
            var pendingOrdersTask = _dashBoardRepository.GetOrdersCountByStatusAsync(OrderStatus.Pending);
            var shippedOrdersTask = _dashBoardRepository.GetOrdersCountByStatusAsync(OrderStatus.Shipped);
            var lowStockTask = _dashBoardRepository.GetTotalClosedToFinishProductsCountAsyncAsync();
            var ordersTodayTask = _dashBoardRepository.GetTotalOrdersCountAsyncPerDayAsync();
            var ordersMonthTask = _dashBoardRepository.GetTotalOrdersCountAsyncPerMonthAsync();
            var ordersYearTask = _dashBoardRepository.GetTotalOrdersCountAsyncPerYearAsync();

            await Task.WhenAll(
                newUsersTask, pendingOrdersTask, shippedOrdersTask, lowStockTask,
                ordersTodayTask, ordersMonthTask, ordersYearTask
            );

           
            var dashboardDto = new DashboardDto
            {
                NewUsersThisMonth = newUsersTask.Result,
                PendingOrders = pendingOrdersTask.Result, 
                ShippedOrders = shippedOrdersTask.Result,
                ProductsLowInStock = lowStockTask.Result,
                OrdersToday = ordersTodayTask.Result,
                OrdersThisMonth = ordersMonthTask.Result,
                OrdersThisYear = ordersYearTask.Result
            };

            return Result<DashboardDto>.Ok(dashboardDto, "All Dashboard Information");
        }
    }
}
