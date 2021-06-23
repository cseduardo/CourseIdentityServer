using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Model
{
    public  class SignIn
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string? ReturnUrl { get; set; }
    }
}