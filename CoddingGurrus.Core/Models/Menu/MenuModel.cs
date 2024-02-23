using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Models.Menu
{
    public class MenuModel
    {
        public int? Id { get; set; } = 0;
        public string Name { get; set; }
        public string Url { get; set; }
        public int ParentId { get; set; } = 0;
        public int MenuOrder { get; set; } = 0;
        public string MenuImage { get; set; }
        public bool Archived { get; set; } = false;
        public bool IsShow { get; set; } = false;

        public int? TotalRecords { get; set; }
    }
}
