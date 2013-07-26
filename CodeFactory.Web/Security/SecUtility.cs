using System;
using System.Collections.Generic;
using System.Text;
using CodeFactory.Utilities;
using System.Globalization;
using System.Collections;
using System.Diagnostics;
using System.Web.Hosting;
using System.Configuration.Provider;
using System.Collections.Specialized;

namespace CodeFactory.Web.Security
{
    internal static class SecUtility
    {
        // Methods
        internal static void CheckArrayParameter(ref string[] param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
        {
            if (param == null)
                throw new ArgumentNullException(paramName);

            if (param.Length < 1)
                throw new ArgumentException(ResourceStringLoader.GetResourceString("Parameter_array_empty", new object[] { paramName }), paramName);

            Hashtable hashtable = new Hashtable(param.Length);

            for (int i = param.Length - 1; i >= 0; i--)
            {
                CheckParameter(ref param[i], checkForNull, checkIfEmpty, checkForCommas, maxSize, paramName + "[ " + i.ToString(CultureInfo.InvariantCulture) + " ]");

                if (hashtable.Contains(param[i]))
                    throw new ArgumentException(ResourceStringLoader.GetResourceString("Parameter_duplicate_array_element", new object[] { paramName }), paramName);

                hashtable.Add(param[i], param[i]);
            }
        }

        internal static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
        {
            if (param == null)
            {
                if (checkForNull)
                    throw new ArgumentNullException(paramName);
            }
            else
            {
                param = param.Trim();
                if (checkIfEmpty && (param.Length < 1))
                    throw new ArgumentException(ResourceStringLoader.GetResourceString("Parameter_can_not_be_empty", new object[] { paramName }), paramName);

                if ((maxSize > 0) && (param.Length > maxSize))
                    throw new ArgumentException(ResourceStringLoader.GetResourceString("Parameter_too_long", new object[] { paramName, maxSize.ToString(CultureInfo.InvariantCulture) }), paramName);

                if (checkForCommas && param.Contains(","))
                    throw new ArgumentException(ResourceStringLoader.GetResourceString("Parameter_can_not_contain_comma", new object[] { paramName }), paramName);
            }
        }

        internal static void CheckPasswordParameter(ref string param, int maxSize, string paramName)
        {
            if (param == null)
                throw new ArgumentNullException(paramName);

            if (param.Length < 1)
                throw new ArgumentException(ResourceStringLoader.GetResourceString("Parameter_can_not_be_empty", new object[] { paramName }), paramName);

            if ((maxSize > 0) && (param.Length > maxSize))
                throw new ArgumentException(ResourceStringLoader.GetResourceString("Parameter_too_long", new object[] { paramName, maxSize.ToString(CultureInfo.InvariantCulture) }), paramName);
        }

        internal static bool GetBooleanValue(NameValueCollection config, string valueName, bool defaultValue)
        {
            bool flag;
            string str = config[valueName];

            if (str == null)
                return defaultValue;

            if (!bool.TryParse(str, out flag))
                throw new ProviderException(ResourceStringLoader.GetResourceString("Value_must_be_boolean", new object[] { valueName }));

            return flag;
        }

        internal static string GetDefaultAppName()
        {
            try
            {
                string applicationVirtualPath = HostingEnvironment.ApplicationVirtualPath;

                if (string.IsNullOrEmpty(applicationVirtualPath))
                {
                    applicationVirtualPath = Process.GetCurrentProcess().MainModule.ModuleName;

                    int index = applicationVirtualPath.IndexOf('.');

                    if (index != -1)
                        applicationVirtualPath = applicationVirtualPath.Remove(index);
                }

                if (string.IsNullOrEmpty(applicationVirtualPath))
                    return "/";

                return applicationVirtualPath;
            }
            catch
            {
                return "/";
            }
        }

        internal static int GetIntValue(NameValueCollection config, string valueName, int defaultValue, bool zeroAllowed, int maxValueAllowed)
        {
            int num;
            string s = config[valueName];

            if (s == null)
                return defaultValue;

            if (!int.TryParse(s, out num))
            {
                if (zeroAllowed)
                    throw new ProviderException(ResourceStringLoader.GetResourceString("Value_must_be_non_negative_integer", new object[] { valueName }));

                throw new ProviderException(ResourceStringLoader.GetResourceString("Value_must_be_positive_integer", new object[] { valueName }));
            }

            if (zeroAllowed && (num < 0))
                throw new ProviderException(ResourceStringLoader.GetResourceString("Value_must_be_non_negative_integer", new object[] { valueName }));

            if (!zeroAllowed && (num <= 0))
                throw new ProviderException(ResourceStringLoader.GetResourceString("Value_must_be_positive_integer", new object[] { valueName }));

            if ((maxValueAllowed > 0) && (num > maxValueAllowed))
                throw new ProviderException(ResourceStringLoader.GetResourceString("Value_too_big", new object[] { valueName, maxValueAllowed.ToString(CultureInfo.InvariantCulture) }));

            return num;
        }

        internal static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize)
        {
            if (param == null)
                return !checkForNull;

            param = param.Trim();

            return (((!checkIfEmpty || (param.Length >= 1)) && ((maxSize <= 0) || (param.Length <= maxSize))) && (!checkForCommas || !param.Contains(",")));
        }

        internal static bool ValidatePasswordParameter(ref string param, int maxSize)
        {
            if (param == null)
                return false;

            if (param.Length < 1)
                return false;

            if (maxSize > 0 && param.Length > maxSize)
                return false;

            return true;
        }
    }
}
