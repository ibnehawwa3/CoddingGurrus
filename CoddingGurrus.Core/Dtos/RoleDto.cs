using CoddingGurrus.Core.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Dtos
{
    public class RoleDto : RoleModel
    {
        public int? TotalRecords { get; set; } = 10;
    }
}
