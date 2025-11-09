using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Domain.Models
{
    public class Role
    {
        public int Id { get;private  set; } 
        public string Name { get;private set; }
        private Role()
        {
            
        }
        public Role(int id, string name)
        {
            Id = id;
            Name = name;
            
        }

    }
}
