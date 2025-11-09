using Application.DataReposatory.Interfaces.Dashboard;
using Application.Dtos.DashBoard;
using Application.Results;
using E_Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces.Dashboard
{
    public interface IDashBoardService
    {
        Task<Result<DashboardDto>> GetTotalDashBoardInformation();


    }
}
