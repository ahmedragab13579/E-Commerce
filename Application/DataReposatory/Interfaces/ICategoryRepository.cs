using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataReposatory.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> IsCategoryExist(string name);
        Task<bool> HasProducts(int categoryId);
    }
}
