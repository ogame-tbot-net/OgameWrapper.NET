using System.Text.RegularExpressions;

namespace OgameWrapper.Includes
{
    internal static class Extensions
    {
		public static int NthIndexOf(this string target, string value, int n)
		{
			Match m = Regex.Match(target, "((" + Regex.Escape(value) + ").*?){" + n + "}");

			if (m.Success)
            {
                return m.Groups[2].Captures[n - 1].Index;
            }
			
			return -1;
		}
	}
}
