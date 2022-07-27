using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels;

public class LoginViewModel
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required] 
    public string ReturnUrl { get; set; }
}