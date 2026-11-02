namespace Mihaylov.Common
{
    /// <summary>
    /// Specifies access levels for users within the system.
    /// </summary>
    public enum AccessLevel
    {
        /// <summary>
        /// Indicates that the member is public.
        /// </summary>
        Public = 1,

        /// <summary>
        /// Represents an application user.
        /// </summary>
        User,

        /// <summary>
        /// Represents an administrator user or role.
        /// </summary>
        Admin
    }
}
