using System.ComponentModel.DataAnnotations;

namespace CoddingGurrus.web.Helper
{
    public class GetRoleRequest
    {
        [Required]
        public int Id { get; set; }
    } 
    public class GetMenuRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
