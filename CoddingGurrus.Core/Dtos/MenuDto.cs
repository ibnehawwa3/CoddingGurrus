using CoddingGurrus.Core.Models.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Dtos
{
    public class MenuDto : MenuModel
    {
        public int? TotalRecords { get; set; }
    }
}
