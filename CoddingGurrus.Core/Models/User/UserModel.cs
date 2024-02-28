using System.ComponentModel.DataAnnotations;

namespace CoddingGurrus.Core.Models.User
{
    public class UserModel
    {
        public string FirstName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public int Id { get; set; }
        public string? MobileNumber { get; set; }
        public string? LastName { get; set; }
        public string? StreetNumber { get; set; }
        public string? ZipCode { get; set; }
        public string? Town { get; set; }
        public string? Country { get; set; }
        public string? CountryCode { get; set; }
        public string? EmailAddress { get; set; }
        public int TotalRecords { get; set; }
        public string? UserId { get; set; }
        public DateTime DateRegistration { get; set; }
    }
}
