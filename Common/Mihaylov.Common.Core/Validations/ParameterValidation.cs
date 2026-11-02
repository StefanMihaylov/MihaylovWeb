using System;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides static helper methods to validate method parameters and throw appropriate exceptions for invalid values.
    /// </summary>
    public class ParameterValidation
    {
        /// <summary>
        /// Validates that the specified parameter is not null.
        /// </summary>
        /// <param name="parameter">The object to validate.</param>
        /// <param name="parameterName">The name of the parameter to include in the exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if parameter is null.</exception>
        public static void IsNotNull(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Validates that the supplied string is not null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="parameter">The string to validate.</param>
        /// <param name="parameterName">The name of the parameter to include in the exception.</param>
        /// <exception cref="ArgumentException">Thrown when parameter is null, empty, or consists only of white-space characters.</exception>
        public static void IsNotEmptyString(string parameter, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentException(parameterName);
            }
        }
    }
}
