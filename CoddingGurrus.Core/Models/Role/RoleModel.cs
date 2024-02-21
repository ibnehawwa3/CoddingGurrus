using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Models.Role
{
    public class RoleModel
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int ConcurrencyStamp { get; set; }
        public int NormalizedName { get; set; }
    }
}
