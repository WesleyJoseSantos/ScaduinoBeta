using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebEditor.StyleModels
{
    public static class CssParser
    {
        public static string CssToString(ICss css)
        {
            string cssText = css.Selector + " {\n";

            PropertyInfo[] properties = css.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single();
                string displayName = attribute.DisplayName;
                if (displayName == "selector") continue;
                string value = property.GetValue(css, null).ToString();
                cssText += $"\t{displayName}: {value};\n";
            }
            cssText += "}";
            return cssText;
        }

        public static string CssDocToString(ICssDoc cssDoc)
        {
            var cssDocText = "";

            PropertyInfo[] properties = cssDoc.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(cssDoc, null) as ICss;
                cssDocText += CssToString(value);
                cssDocText += "\n\n";
            }

            return cssDocText;
        }
    }
}
