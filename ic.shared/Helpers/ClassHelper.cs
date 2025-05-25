using System.ComponentModel;
using System.Reflection;
using System.Text.Json;

namespace IC.Shared.Helpers
{
    public static class ClassHelper
    {
        public static string GetDisplayName<T>()
        {
            var displayName = typeof(T)
              .GetCustomAttributes(typeof(DisplayNameAttribute), true)
              .FirstOrDefault() as DisplayNameAttribute;

            if (displayName != null)
                return displayName.DisplayName;

            return typeof(T).Name;
        }

		public static string GetDisplayName<T>(string propertyName)
		{
			var prop = typeof(T).GetProperty(propertyName);
			if (prop == null) return propertyName;

			var displayNameAttr = prop.GetCustomAttributes(typeof(DisplayNameAttribute), false)
									  .FirstOrDefault() as DisplayNameAttribute;

			return displayNameAttr?.DisplayName ?? propertyName;
		}

		//Kiểm tra tất cả các thuộc tính string của một object có thuộc tính nào null hoặc empty
		public static bool IsAnyNullOrEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

		public static bool IsAllStringPropertyNullOrEmpty(object myObject)
		{
            if (myObject == null) return true;

			foreach (PropertyInfo pi in myObject.GetType().GetProperties())
			{
				if (pi.PropertyType == typeof(string))
				{
					string value = (string)pi.GetValue(myObject);
					if (!string.IsNullOrEmpty(value))
					{
						return false;
					}
				}
			}
			return true;
		}

		//public static string GetDescription<Tsource, Tdest>()
		//{
		//    var config = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDto>());

		//    var mapper = config.CreateMapper();
		//    // or
		//    var mapper = new Mapper(config);
		//    OrderDto dto = mapper.Map<OrderDto>(order);
		//}

		public static void PropertyCopy<TDest, TSource>(TDest destination, TSource source, Dictionary<string, string> mapProps = null, bool mapPropKeyIsDestination = true)
            where TSource : class
            where TDest : class
        {
            var destProperties = destination.GetType().GetProperties()
                .Where(x => x.CanRead && x.CanWrite && !x.GetGetMethod().IsVirtual);
            var sourceProperties = source.GetType().GetProperties()
                .Where(x => x.CanRead && x.CanWrite && !x.GetGetMethod().IsVirtual);
            var copyProperties = sourceProperties.Join(destProperties, x => x.Name, y => y.Name, (x, y) => x);
            foreach (var sourceProperty in copyProperties)
            {
                var prop = destProperties.FirstOrDefault(x => x.Name == sourceProperty.Name && x.PropertyType == sourceProperty.PropertyType);
                if (prop != null) prop.SetValue(destination, sourceProperty.GetValue(source));
            }

            if(mapProps != null)
            {
                foreach(var keyValue in mapProps)
                {
                    var propDest = destProperties.FirstOrDefault(x => x.Name == (mapPropKeyIsDestination ? keyValue.Key : keyValue.Value));
                    var propSource = sourceProperties.FirstOrDefault(x => x.Name == (mapPropKeyIsDestination ? keyValue.Value : keyValue.Key));

                    if (propDest != null && propSource != null && propDest.PropertyType == propSource.PropertyType) propDest.SetValue(destination, propSource.GetValue(source));
                }
            }
        }

        public static string SwapPropertyName<T>(T destType, string sourceProp, Dictionary<string, string> keyValues, string separateBy)
        {
            var result = "";
            var items = sourceProp.Split(separateBy, StringSplitOptions.TrimEntries);
            var typeProperties = destType.GetType().GetProperties().Where(x => x.CanRead && x.CanWrite && !x.GetGetMethod().IsVirtual);
            foreach (var item in items)
            {
                var replace = item;
                if (keyValues.ContainsValue(item)) replace = keyValues.First(x => x.Value == item).Key;
                if (typeProperties.FirstOrDefault(x => x.Name == replace) == null) continue;

                if (result != "") result += separateBy;
                result += replace;
            }

            return result;
        }

        public static T DeepCopyJSON<T>(T input)
        {
            var jsonString = JsonSerializer.Serialize(input);
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
