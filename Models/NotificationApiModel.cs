using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingChallenge.Models;

public class NotificationApiModel
{
    [Required]
    public String? FirstName { get; set; }
    [Required]
    public String? LastName { get; set; }
    public Boolean IsEmailSelected { get; set; }
    public Boolean IsPhoneSelected { get; set; }
    [EmailAddress]
    public String? Email { get; set; }
    [Phone]
    [MinLength(7), MaxLength(14)]
    public String? PhoneNumber { get; set; }
    [Required]
    public String? Supervisor { get; set; }
}