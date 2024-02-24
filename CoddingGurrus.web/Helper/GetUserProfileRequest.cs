using System.ComponentModel.DataAnnotations;

namespace CoddingGurrus.web.Helper
{
    public class GetByIdRequest
    {
        [Required]
        public string Id { get; set; }
    }
}
