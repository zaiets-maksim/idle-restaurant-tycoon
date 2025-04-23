using System;
using System.Text;
using UnityEngine;

namespace Extensions
{
    public static class Make
    {
        public static string Colored(params object[] args)
        {
            if (args.Length > 0 && args[^1] is false)
                return String.Empty;

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is string text)
                {
                    if (i + 1 < args.Length && args[i + 1] is Color color)
                    {
                        string colorCode = ColorToHex(color);
                        stringBuilder.Append($"<color=#{colorCode}>{text}</color>");
                        i++;
                    }
                    else
                    {
                        stringBuilder.Append(text);
                    }
                }
            }

            return stringBuilder.ToString();
        }
        
        private static string ColorToHex(Color color) => ColorUtility.ToHtmlStringRGB(color);
    }
}