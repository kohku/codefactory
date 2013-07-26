using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Diagnostics;

namespace CodeFactory.Utilities
{
    /// <summary>
    /// Helper class to load resources strings.
    /// </summary>
    public sealed class ResourceStringLoader
    {
        private ResourceStringLoader()
        {
        }

        /// <summary>
        /// Loads a resource name.
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The resource string.</returns>
        public static string GetResourceString(string resourceName)
        {
            string value = null;

            value = LoadAssemblyString(resourceName, Assembly.GetCallingAssembly());

            if (value == null)
                value = LoadAssemblyString(resourceName, Assembly.GetExecutingAssembly());

            if (value == null)
                return null;

            return value;
        }

        /// <summary>
        /// Loads a formatted resource.
        /// </summary>
        /// <param name="resourceName">Resource name.</param>
        /// <param name="arg0">First argument to format.</param>
        /// <returns>The formatted resource string.</returns>
        public static string GetResourceString(string resourceName, object arg0)
        {
            string value = null;

            value = LoadAssemblyString(resourceName, Assembly.GetCallingAssembly());

            if (value == null)
                value = LoadAssemblyString(resourceName, Assembly.GetExecutingAssembly());

            if (value == null)
                return null;

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    value = string.Format(value, arg0);
                }
                catch (ArgumentNullException)
                {
                }
                catch (FormatException)
                {
                }
            }

            return value;
        }

        /// <summary>
        /// Loads a formatted resource.
        /// </summary>
        /// <param name="resourceName">Resource name.</param>
        /// <param name="arg0">First argument to format.</param>
        /// <param name="arg1">Second argument to format.</param>
        /// <returns>The formatted resource string.</returns>
        public static string GetResourceString(string resourceName, object arg0, object arg1)
        {
            string value = null;

            value = LoadAssemblyString(resourceName, Assembly.GetCallingAssembly());

            if (value == null)
                value = LoadAssemblyString(resourceName, Assembly.GetExecutingAssembly());

            if (value == null)
                return null;

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    value = string.Format(value, arg0, arg1);
                }
                catch (ArgumentNullException)
                {
                }
                catch (FormatException)
                {
                }
            }

            return value;
        }

        /// <summary>
        /// Loads a formatted resource.
        /// </summary>
        /// <param name="resourceName">Resource name.</param>
        /// <param name="arg0">First argument to format.</param>
        /// <param name="arg1">Second argument to format.</param>
        /// <param name="arg2">Third argument to format.</param>
        /// <returns>The formatted resource string.</returns>
        public static string GetResourceString(string resourceName, object arg0, object arg1, object arg2)
        {
            string value = null;

            value = LoadAssemblyString(resourceName, Assembly.GetCallingAssembly());

            if (value == null)
                value = LoadAssemblyString(resourceName, Assembly.GetExecutingAssembly());

            if (value == null)
                return null;

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    value = string.Format(value, arg0, arg1, arg2);
                }
                catch (ArgumentNullException)
                {
                }
                catch (FormatException)
                {
                }
            }

            return value;
        }

        /// <summary>
        /// Loads a formatted resource.
        /// </summary>
        /// <param name="resourceName">Resource name.</param>
        /// <param name="args">an array of parámeters.</param>
        /// <returns>The formatted resource string.</returns>
        public static string GetResourceString(string resourceName, object[] args)
        {
            string value = null;

            value = LoadAssemblyString(resourceName, Assembly.GetCallingAssembly());

            if (value == null)
                value = LoadAssemblyString(resourceName, Assembly.GetExecutingAssembly());

            if (value == null)
                return null;

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    value = string.Format(value, args);
                }
                catch (ArgumentNullException)
                {
                }
                catch (FormatException)
                {
                }
            }

            return value;
        }

        /// <summary>
        /// Load a resource string.
        /// </summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The resource string.</returns>
        public static string GetResourceString(string baseName, string resourceName)
        {
            return GetResourceString(baseName, resourceName, Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Load a resource string.
        /// </summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <param name="assembly">The assembly to load the resource from.</param>
        /// <returns>The resource string.</returns>
        public static string GetResourceString(string baseName, string resourceName, Assembly assembly)
        {
            if (string.IsNullOrEmpty(baseName))
                throw new ArgumentNullException("ExceptionStringNullOrEmpty", "baseName");

            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentNullException("ExceptionStringNullOrEmpty", "resourceName");

            string value = null;

            if (assembly != null)
                value = LoadAssemblyString(baseName, resourceName, assembly);

            if (value == null)
                value = LoadAssemblyString(baseName, resourceName, Assembly.GetExecutingAssembly());

            if (value == null)
                value = LoadAssemblyString(resourceName, Assembly.GetCallingAssembly());

            if (value == null)
                value = LoadAssemblyString(resourceName, Assembly.GetExecutingAssembly());

            if (value == null)
                return null;

            return value;
        }

        /// <summary>
        /// Loads a resource name including some parameters (could be constants, dinamyc content).
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <param name="parameters">The parameters array.</param>
        /// <returns>The resource string.</returns>
        public static string LoadStringWithParameters(string resourceName, object[] parameters)
        {
            string value = GetResourceString(resourceName);

            foreach (object parameter in parameters)
                value = string.Format("{0} {1}", value, parameter);

            return value;
        }

        /// <summary>
        /// Retrieve a resource string searching in it from all resources within the assembly.
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <param name="originalAssembly">The assembly to load the resource from.</param>
        /// <returns>The resource string.</returns>
        private static string LoadAssemblyString(string resourceName, Assembly originalAssembly)
        {
            return LoadAssemblyString(resourceName, originalAssembly, CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <param name="originalAssembly">The assembly to load the resource from.</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>The resource string.</returns>
        private static string LoadAssemblyString(string resourceName, Assembly originalAssembly, CultureInfo culture)
        {
            string translatedHelpString = null;

            // Retrieve all assemblies names from all resources in this assembly.
            string[] baseNames = originalAssembly.GetManifestResourceNames();

            for (int i = 0; i < baseNames.Length; i++)
            {
                try
                {
                    int lastDotResourcesString = baseNames[i].LastIndexOf(".resources");

                    if (lastDotResourcesString < 0)
                        continue;

                    string baseName = baseNames[i].Remove(lastDotResourcesString,
                        (baseNames[i].Length - lastDotResourcesString));

                    ResourceManager manager = new ResourceManager(baseName, originalAssembly);
                    translatedHelpString = manager.GetString(resourceName, culture);
                }
                catch (Exception ex)
                {
                    // There is nothing we can do if this doesn't find the resource string. The only harm is that
                    // the help text that is registered will not be as helpful as could otherwise be.
                    Trace.TraceWarning(ex.StackTrace);
                }

                if (!string.IsNullOrEmpty(translatedHelpString))
                    return translatedHelpString;
            }

            return null;
        }

        /// <summary>
        /// Load a resource string.
        /// </summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <param name="assembly">The asselbmly to load the resource from.</param>
        /// <returns>The resource string.</returns>
        private static string LoadAssemblyString(string baseName, string resourceName, Assembly assembly)
        {
            return LoadAssemblyString(baseName, resourceName, assembly, CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Load a resource string.
        /// </summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <param name="assembly">The asselbmly to load the resource from.</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>The resource string.</returns>
        private static string LoadAssemblyString(string baseName, string resourceName, Assembly assembly, CultureInfo culture)
        {
            try
            {
                ResourceManager manager = new ResourceManager(baseName, assembly);
                return manager.GetString(resourceName, culture);
            }
            catch (MissingManifestResourceException ex)
            {
                // There is nothing we can do if this doesn't find the resource string. The only harm is that
                // the help text that is registered will not be as helpful as could otherwise be.
                Trace.TraceWarning(ex.StackTrace);
            }

            return null;
        }
    }
}
