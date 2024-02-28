using CoddingGurrus.Core.Models.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Dtos
{
    public class TopicsDto : TopicsModel
    {
        public int? TotalRecords { get; set; }
    }
}
