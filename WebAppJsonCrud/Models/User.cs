using System.ComponentModel.DataAnnotations;

namespace WebAppJsonCrud.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
    }

}
