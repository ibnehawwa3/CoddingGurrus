using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Models.Topics
{
    public class TopicsModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long CourseId { get; set; }
    }
}
