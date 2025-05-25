using System.Collections;
using System.Reflection;

namespace IC.Shared.Helpers
{
	public static class IEnumerableHelper
	{
		public static bool IsAny<TSource>(this IEnumerable<TSource> source)
		{
			switch (source)
			{
				case null:
					return false;
				case TSource[] array:
					return array.Length != 0;
				case ICollection<TSource> collection:
					return collection.Count != 0;
				case ICollection baseCollection:
					return baseCollection.Count != 0;
			}

			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
					return true;
			}

			return false;
		}

        public static T ToObject<T>(this Dictionary<string, string> dictionary)
        {
            T obj = Activator.CreateInstance<T>();
            foreach (var kvp in dictionary)
            {
                PropertyInfo property = typeof(T).GetProperty(kvp.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(obj, int.Parse(kvp.Value));
                    }
                    else if (property.PropertyType == typeof(byte))
                    {
                        property.SetValue(obj, byte.Parse(kvp.Value));
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(obj, kvp.Value);
                    }
                    else if (property.PropertyType == typeof(List<byte>))
                    {
                        var byteList = kvp.Value.Split(',')
                            .Select(byte.Parse)
                            .ToList();
                        property.SetValue(obj, byteList);
                    }
                    else if (property.PropertyType == typeof(List<short>))
                    {
                        var shortList = kvp.Value.Split(',')
                            .Select(short.Parse)
                            .ToList();
                        property.SetValue(obj, shortList);
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        property.SetValue(obj, DateTime.Parse(kvp.Value));
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        property.SetValue(obj, bool.Parse(kvp.Value));
                    }
                }
            }
            return obj;
        }
    }
}
