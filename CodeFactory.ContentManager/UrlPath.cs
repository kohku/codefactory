using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.ContentManager
{
    internal class UrlPath
    {
        internal static string AppendTrailingSlash(string path)
        {
            if (path == null)
                return null;

            int length = path.Length;

            if (length != 0 && path[length - 1] != '/')
                path = path + '/';

            return path;
        }
    }
}
