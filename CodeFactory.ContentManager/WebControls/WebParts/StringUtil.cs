using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Utilities;
using System.Globalization;

namespace CodeFactory.ContentManager.WebControls.WebParts
{
    internal static class StringUtil
    {
        // Methods
        internal static string CheckAndTrimString(string paramValue, string paramName)
        {
            return CheckAndTrimString(paramValue, paramName, true);
        }

        internal static string CheckAndTrimString(string paramValue, string paramName, bool throwIfNull)
        {
            return CheckAndTrimString(paramValue, paramName, throwIfNull, -1);
        }

        internal static string CheckAndTrimString(string paramValue, string paramName, bool throwIfNull, int lengthToCheck)
        {
            if (paramValue == null)
            {
                if (throwIfNull)
                    throw new ArgumentNullException(paramName);

                return null;
            }

            string str = paramValue.Trim();

            if (str.Length == 0)
                throw new ArgumentException(ResourceStringLoader.GetResourceString(
                    "PersonalizationProviderHelper_TrimmedEmptyString", new object[] { paramName }));

            if ((lengthToCheck > -1) && (str.Length > lengthToCheck))
                throw new ArgumentException(ResourceStringLoader.GetResourceString(
                    "StringUtil_Trimmed_String_Exceed_Maximum_Length", new object[] { paramValue, paramName,
                        lengthToCheck.ToString(CultureInfo.InvariantCulture) }));

            return str;
        }
    }
}
