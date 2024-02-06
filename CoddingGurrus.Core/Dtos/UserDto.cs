﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public DateTime DateRegistration { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Guid Password { get; set; }
        public int TotalRecords { get; set; }
    }
}
