namespace CodingChallenge.Models;

public class NotificationApiModel
{
    public String? FirstName { get; set; }
    public String? LastName { get; set; }
    public Boolean IsEmailSelected { get; set; }
    public Boolean IsPhoneSelected { get; set; }
    public String? Email { get; set; }
    public String? PhoneNumber { get; set; }
    public String? Supervisor { get; set; }
}