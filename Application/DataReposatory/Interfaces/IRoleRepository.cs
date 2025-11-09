using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataReposatory.Interfaces
{
    public interface IRoleRepository: IGenericRepository<Role>
    {
        Task<bool> IsRoleExist(string Name);
        Task<Role> GetByNameAsync(string Name);

    }
}
