using System;

namespace Mihaylov.Common
{
    /// <summary>
    /// Defines user claim types as bit flags that can be combined to represent one or more identity claims such as
    /// Username, Email, FullName, FirstName, and LastName.
    /// </summary>
    [Flags]
    public enum ClaimType : int
    {
        /// <summary>
        /// User name.
        /// </summary>
        Username = 1,

        /// <summary>
        /// Represents an email contact method.
        /// </summary>
        Email = 2,
        
        /// <summary>
        /// Full name including all components (for example, given, middle, and family names).
        /// </summary>
        FullName = 4,
        
        /// <summary>
        /// First name field of a person.
        /// </summary>
        FirstName = 8,
        
        /// <summary>
        /// Identifies the last name field.
        /// </summary>
        LastName = 16,
    }
}
