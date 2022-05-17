using System.ComponentModel.DataAnnotations;

namespace StoreManager.Application.DTOs.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}