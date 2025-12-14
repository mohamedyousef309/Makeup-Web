using Domain_Layer.Entites.Authantication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.AuthanticationViewModles.Register
{
    public class RegisterViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = default!;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; } = default!;

        [Required(ErrorMessage = "Picture is required")]
        [Url(ErrorMessage = "Invalid picture URL")]
        public string Picture { get; set; } = default!;

        [Required(ErrorMessage = "User address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string UserAddress { get; set; } = default!;

        public IFormFile? Image = null;

        public string? ErrorMessage { get; set; }


    }
}
