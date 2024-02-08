using System.ComponentModel.DataAnnotations;

namespace CoddingGurrus.web.Helper
{
    public class GetUserProfileRequest
    {
        [Required]
        public string Id { get; set; }
    }
}
