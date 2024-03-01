using CoddingGurrus.Core.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Dtos
{
    public class RoleDto 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? NormalizedName { get; set; }
        public int? TotalRecords { get; set; } = 10;
    }
}
