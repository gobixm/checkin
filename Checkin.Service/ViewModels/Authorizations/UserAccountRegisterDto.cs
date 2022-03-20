using System.ComponentModel.DataAnnotations;

namespace Checkin.Service.ViewModels.Authorizations;

public record UserAccountRegisterDto
(
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    string Email,

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {6} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    string Password
);
