﻿namespace Elect.Core.ObjUtils
{
    public class ObjHelper
    {
        public static string ToJsonString(object obj)
        {
            if (obj is JObject jObject)
            {
                return jObject.ToString(Constants.Formatting.JsonSerializerSettings.Formatting);
            }
            return JsonConvert.SerializeObject(obj, Constants.Formatting.JsonSerializerSettings);
        }
        public static T Clone<T>(T obj)
        {
            if (obj == null)
            {
                return default;
            }
            var json = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            });
            return result;
        }
        public static T ConvertTo<T>(object obj)
        {
            if (obj == null)
            {
                return default;
            }
            if (obj is T variable)
            {
                return variable;
            }
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);
                if (u != null)
                {
                    if (u == typeof(string))
                    {
                        return (T)(object)obj.ToString();
                    }
                    return (T)Convert.ChangeType(obj, u);
                }
                if (t == typeof(string))
                {
                    return (T)((object)obj.ToString());
                }
                if (t.IsPrimitive)
                {
                    return (T)Convert.ChangeType(obj, t);
                }
                return (T)Convert.ChangeType(obj, t);
            }
            catch
            {
                return default;
            }
        }
        public static bool TryConvertTo<T>(object obj, T defaultValue, out T value)
        {
            value = ConvertTo<T>(obj);
            // If obj is null, treat as success (consistent with ConvertTo)
            if (obj == null)
                return true;
            // If conversion failed, value will be default
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                value = defaultValue == null ? default : defaultValue;
                return false;
            }
            return true;
        }
        public static T WithoutRefLoop<T>(T obj)
        {
            if (obj == null)
            {
                return default;
            }
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var result = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            });
            return result;
        }
        public static T WithoutVirtualProp<T>(T obj)
        {
            if (obj == null)
            {
                return default;
            }
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new WithoutVirtualContractResolver(),
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None
            });
            var result = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            });
            return result;
        }
        public static T ReplaceNullOrDefault<T>(T value, T newValue)
        {
            if (value == null)
            {
                value = newValue;
            }
            else
            {
                if (value.Equals(default(T)))
                {
                    value = newValue;
                }
            }
            return value;
        }
    }
}
