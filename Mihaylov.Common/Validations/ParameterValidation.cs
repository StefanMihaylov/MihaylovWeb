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
    }
}
