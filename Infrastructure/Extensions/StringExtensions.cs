using System.Text.RegularExpressions;

namespace ITSUPPORTTICKETSYSTEM.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string Slugify(this string that)
        {
            that = Regex.Replace(that, @"[^a-zA-Z0-9\s]", "");
            that = that.ToLower();
            that = Regex.Replace(that, @"\s", "-");
            return that;
        }
         
    }
}