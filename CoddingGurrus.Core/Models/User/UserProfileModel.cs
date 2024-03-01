
namespace CoddingGurrus.Core.Models.User
{
    public class UserProfileModel
    {
        public int Id { get; set; }
        public string MobileNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetNumber { get; set; }
        public string ZipCode { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string EmailAddress { get; set; }
        public int TotalRecords { get; set; }
        public string? UserId { get; set; }
        public DateTime DateRegistration { get; set; }
    }
}
