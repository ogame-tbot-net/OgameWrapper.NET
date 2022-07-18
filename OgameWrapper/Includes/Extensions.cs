using JsonFlatFileDataStore;
using OgameWrapper.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace OgameWrapper.Includes
{
    internal static class Extensions
    {
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			Random rnd = new();
			return source.OrderBy((item) => rnd.Next());
		}

		public static string FirstCharToUpper(this string input)
		{
			return input.First().ToString().ToUpper() + input[1..];
		}

		public static bool Has(this List<Celestial> celestials, Celestial celestial)
		{
			foreach (Celestial cel in celestials)
			{
				if (cel.HasCoords(celestial.Coordinate))
				{
					return true;
				}
			}
			return false;
		}

		public static IEnumerable<Celestial> Unique(this IEnumerable<Celestial> source)
		{
			return source.Distinct(new CelestialComparer()).ToList();
		}

		internal class CelestialComparer : IEqualityComparer<Celestial>
		{
			public bool Equals(Celestial x, Celestial y)
			{
				return x.ID == y.ID;
			}

			public int GetHashCode([DisallowNull] Celestial obj)
			{
				return obj.ID;
			}
		}

		public static bool TryGetItem<T>(this DataStore ds, string key, out T item)
        {
			try
            {
				item = ds.GetItem<T>(key);
				if (item != null)
					return true;
				else
					return false;
            }
			catch
            {
				item = default(T);
				return false;
            }
        }

		public static List<int> AllIndexesOf(this string str, string value)
		{
			if (String.IsNullOrEmpty(value))
				throw new ArgumentException("the string to find may not be empty", "value");
			List<int> indexes = new List<int>();
			for (int index = 0; ; index += value.Length)
			{
				index = str.IndexOf(value, index);
				if (index == -1)
					return indexes;
				indexes.Add(index);
			}
		}
	}
}
