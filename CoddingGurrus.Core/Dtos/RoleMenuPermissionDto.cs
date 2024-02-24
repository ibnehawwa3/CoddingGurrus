using CoddingGurrus.Core.Models.RoleMenuPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Dtos
{
    public class RoleMenuPermissionDto :RoleMenuPermissionModel
    {
        public int? TotalRecords { get; set; }
    }
}
