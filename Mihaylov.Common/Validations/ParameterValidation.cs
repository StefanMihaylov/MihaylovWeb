using System;

namespace Mihaylov.Common.Validations
{
    public class ParameterValidation
    {
        public static void IsNotNull(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void IsNotEmptyString(string parameter, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentException(parameterName);
            }
        }
    }
}
