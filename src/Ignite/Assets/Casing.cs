using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ignite.Assets
{
    public static class Casing
    {
        public static string Default(string templatePath)
        {
            return templatePath;
        }

        public static string Underscore(string templatePath)
        {
            // See comments from: http://stackoverflow.com/a/1098039/200322
            return Regex.Replace(templatePath, @"([A-Z])(?<=[a-z]\1|[A-Za-z]\1(?=[a-z]))", "_$1");
        }

        public static string Lowercase(string templatePath)
        {
            return templatePath.ToLower();
        }

        public static string CamelCase(string templatePath)
        {
            var split = templatePath.ToCharArray();

            bool isWordStart = false;

            for (int i = 0; i < split.Length; i++)
            {
                if ((i == 0 && templatePath[i] != '/'))
                {
                    isWordStart = true;
                }
                else if (templatePath[i] == '/')
                {
                    isWordStart = true;
                    continue;
                }

                if (isWordStart)
                {
                    split[i] = char.ToLower(split[i]);
                    isWordStart = false;
                }
            }

            return new String(split);
        }
    }
}
