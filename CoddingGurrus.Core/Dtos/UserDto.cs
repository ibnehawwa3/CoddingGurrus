using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Dtos
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string Id { get; set; }
        public DateTime DateRegistration { get; set; }
        public int TotalRecords { get; set; }
    }
}
