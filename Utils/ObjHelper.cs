using System.Reflection;
using System.Text.Json;

namespace UtopiaBackendChallenge.Utils
{
    public class Obj
    {
        public static Dictionary<string, object?> getProps<T>(T obj, Func<(string Name, object? Value), bool>? filter = null)
        {
            Dictionary<string, object?> dict = new Dictionary<string, object?>();
            PropertyInfo[] props = (obj ?? new Object()).GetType().GetProperties().ToArray();
            foreach (var prop in props)
            {
                string name = prop.Name;
                object? value = prop.GetValue(obj);
                var val = filter == null ? true : filter((name, value));
                if (val) dict.Add(name, value);
            }
            return dict;
        }

        public static void setProps<T, U>(T obj, U obj1)
        {
            PropertyInfo[] props = (obj ?? new Object()).GetType().GetProperties().ToArray();
            PropertyInfo[] props1 = (obj1 ?? new Object()).GetType().GetProperties().ToArray();
            foreach (var prop in props)
            {
                foreach (var prop1 in props1)
                {
                    if (prop.Name.Equals(prop1.Name))
                    {
                        prop.SetValue(obj, prop1.GetValue(obj1));
                    }
                }
            }
        }
        public static string serialize(object obj, bool keepCase = false)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.DictionaryKeyPolicy = keepCase ? null : JsonNamingPolicy.CamelCase;
            options.PropertyNamingPolicy = keepCase ? null : JsonNamingPolicy.CamelCase;
            return JsonSerializer.Serialize(obj, options);
        }
    }
}