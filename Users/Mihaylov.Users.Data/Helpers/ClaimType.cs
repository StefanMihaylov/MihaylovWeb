using System;

namespace Mihaylov.Users.Data.Helpers
{
    [Flags]
    public enum ClaimType
    {
        Username = 1,
        Email = 2,
        FullName = 4,
        FirstName = 8,
        LastName = 16,
    }
}
