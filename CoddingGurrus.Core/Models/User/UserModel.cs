using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Models.User
{
    public class UserModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public DateTime DateRegistration { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int TotalRecords { get; set; }
    }
}
