using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Models.RoleMenuPermission
{
    public class RoleMenuPermissionModel
    {
        public long Id { get; set; }
        public string RoleId { get; set; }
        public string? MenuName { get; set; }
        public long MenuId { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Access { get; set; }
    }
}
