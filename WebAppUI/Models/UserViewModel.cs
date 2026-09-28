using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAppUI.Models
{
    public class UserViewModel
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
    }

    public class CreateUserViewModel
    {
        [Required][EmailAddress] public string Email { get; set; } = null!;
        [Required][DataType(DataType.Password)][MinLength(6)] public string Password { get; set; } = null!;
        [DataType(DataType.Password)][Compare("Password")] public string ConfirmPassword { get; set; } = null!;
        public List<string> SelectedRoles { get; set; } = new();
    }

    public class EditUserViewModel
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> AvailableRoles { get; set; } = new();
        public List<string> SelectedRoles { get; set; } = new();
    }
}