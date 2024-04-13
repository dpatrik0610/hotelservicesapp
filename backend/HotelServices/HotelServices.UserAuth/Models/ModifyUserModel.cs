using System.ComponentModel.DataAnnotations;

namespace Hotelservices.UserAuth.Models
{
    public class ModifyUserModel
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        public string NewUsername { get; set; }

        public string NewEmail { get; set; }

        public string NewPhoneNumber { get; set; }

        public string NewPassword { get; set; }

        public DateTimeOffset? NewLockoutEnd { get; set; }
    }
}
