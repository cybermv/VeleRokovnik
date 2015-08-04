using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.Owin.Security.Provider;

namespace VeleRokovnik.Util
{
    public static class Utilities
    {
        public static Random Randomiser = new Random();

        public static String ColorToHex(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static string LoremIpsum(int minWords, int maxWords)
        {
            var words = new[]
            {
                "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
                "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
                "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"
            };

            int numWords = Randomiser.Next(minWords, maxWords + 1);

            StringBuilder result = new StringBuilder();

            for (int w = 0; w < numWords; w++)
            {
                if (w > 0)
                {
                    result.Append(" ");
                }
                result.Append(words[Randomiser.Next(words.Length)]);
            }

            result[0] = char.ToUpper(result[0]);
            result.Append(".");

            return result.ToString();
        }

        public static string Dehrvatiziraj(string text)
        {
            StringBuilder toReturn = new StringBuilder(text.Length);
            toReturn.Append(text.ToLowerInvariant());
            toReturn.Replace('č', 'c');
            toReturn.Replace('ć', 'c');
            toReturn.Replace('đ', 'd');
            toReturn.Replace('ž', 'z');
            toReturn.Replace('š', 's');

            return toReturn.ToString();
        }

        public static string CreateUsername(string name, string surname)
        {
            return Dehrvatiziraj(string.Format("{0}{1}{2}",
                name.Substring(0, 1).ToLowerInvariant(),
                surname.ToLowerInvariant(),
                Randomiser.Next(100, 1000)));
        }
    }
}