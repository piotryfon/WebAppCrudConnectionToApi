using System.ComponentModel.DataAnnotations;

namespace WebAppJsonCrud.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "The Email Address is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email Address is not valid")]
        public string EmailAddress { get; set; }
    }

}
