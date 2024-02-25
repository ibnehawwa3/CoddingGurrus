using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Models.Role
{
    public class RoleModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? NormalizedName { get; set; }
    }
}
