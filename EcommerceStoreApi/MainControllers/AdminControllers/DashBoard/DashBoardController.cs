using Application.Dtos.DashBoard;
using Application.Results;
using Application.Services.InterFaces.Dashboard;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreApi.MainControllers.AdminControllers.DashBoard
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Authorize]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardService _dashboardService;
        public DashBoardController(IDashBoardService _dashboardService)
        {
            this._dashboardService = _dashboardService;
            
        }
        /// <summary>
        /// Fetches all DasgBoard Information.
        /// </summary>
        /// <returns>A DasgBoard Ogject For All Data.</returns>
        /// <response code="200">A DasgBoard Ogject For All Data.</response>
        [HttpGet]
        public async Task<IActionResult> GetOrdersCountByStatus()
        {
            Result<DashboardDto> result = await _dashboardService.GetTotalDashBoardInformation();
            return ActionResultStatus.MapResult(result);
        }
       
     

    }
}
