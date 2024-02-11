using System;

namespace Mihaylov.Users.Models.Requests
{
    public class ChangePasswordRequest
    {
        public Guid UserId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
