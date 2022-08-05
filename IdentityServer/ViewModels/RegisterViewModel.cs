using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels;

public class RegisterViewModel
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }
    
    [Required]
    public bool IsRemember { get; set; }
}