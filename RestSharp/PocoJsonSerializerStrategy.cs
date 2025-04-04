// Decompiled with JetBrains decompiler
// Type: RestSharp.PocoJsonSerializerStrategy
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace RestSharp
{
  internal class PocoJsonSerializerStrategy : IJsonSerializerStrategy
  {
    internal CacheResolver CacheResolver;
    private static readonly string[] Iso8601Format = new string[3]
    {
      "yyyy-MM-dd\\THH:mm:ss.FFFFFFF\\Z",
      "yyyy-MM-dd\\THH:mm:ss\\Z",
      "yyyy-MM-dd\\THH:mm:ssK"
    };

    public PocoJsonSerializerStrategy()
    {
      this.CacheResolver = new CacheResolver(new MemberMapLoader(this.BuildMap));
    }

    protected virtual void BuildMap(
      Type type,
      SafeDictionary<string, CacheResolver.MemberMap> memberMaps)
    {
      foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        memberMaps.Add(property.Name, new CacheResolver.MemberMap(property));
      foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
        memberMaps.Add(field.Name, new CacheResolver.MemberMap(field));
    }

    public virtual bool SerializeNonPrimitiveObject(object input, out object output)
    {
      return this.TrySerializeKnownTypes(input, out output) || this.TrySerializeUnknownTypes(input, out output);
    }

    public virtual object DeserializeObject(object value, Type type)
    {
      object source = (object) null;
      object obj1;
      switch (value)
      {
        case string _:
          string s = value as string;
          obj1 = string.IsNullOrEmpty(s) || !(type == typeof (DateTime)) && (!ReflectionUtils.IsNullableType(type) || !(Nullable.GetUnderlyingType(type) == typeof (DateTime))) ? (object) s : (object) DateTime.ParseExact(s, PocoJsonSerializerStrategy.Iso8601Format, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
          break;
        case bool _:
          obj1 = value;
          break;
        case null:
          obj1 = (object) null;
          break;
        case long _ when type == typeof (long):
        case double _ when type == typeof (double):
          obj1 = value;
          break;
        case double _ when type != typeof (double):
        case long _ when type != typeof (long):
          obj1 = typeof (IConvertible).IsAssignableFrom(type) ? Convert.ChangeType(value, type, (IFormatProvider) CultureInfo.InvariantCulture) : value;
          break;
        case IDictionary<string, object> _:
          IDictionary<string, object> dictionary = (IDictionary<string, object>) value;
          if (ReflectionUtils.IsTypeDictionary(type))
          {
            Type genericArgument1 = type.GetGenericArguments()[0];
            Type genericArgument2 = type.GetGenericArguments()[1];
            IDictionary newInstance = (IDictionary) CacheResolver.GetNewInstance(typeof (Dictionary<,>).MakeGenericType(genericArgument1, genericArgument2));
            foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) dictionary)
              newInstance.Add((object) keyValuePair.Key, this.DeserializeObject(keyValuePair.Value, genericArgument2));
            source = (object) newInstance;
            goto default;
          }
          else
          {
            source = CacheResolver.GetNewInstance(type);
            SafeDictionary<string, CacheResolver.MemberMap> safeDictionary = this.CacheResolver.LoadMaps(type);
            if (safeDictionary == null)
            {
              source = value;
              goto default;
            }
            else
            {
              using (IEnumerator<KeyValuePair<string, CacheResolver.MemberMap>> enumerator = safeDictionary.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  KeyValuePair<string, CacheResolver.MemberMap> current = enumerator.Current;
                  CacheResolver.MemberMap memberMap = current.Value;
                  if (memberMap.Setter != null)
                  {
                    string key = current.Key;
                    if (dictionary.ContainsKey(key))
                    {
                      object obj2 = this.DeserializeObject(dictionary[key], memberMap.Type);
                      memberMap.Setter(source, obj2);
                    }
                  }
                }
                goto default;
              }
            }
          }
        case IList<object> _:
          IList<object> objectList = (IList<object>) value;
          IList list = (IList) null;
          if (type.IsArray)
          {
            list = (IList) Activator.CreateInstance(type, (object) objectList.Count);
            int num = 0;
            foreach (object obj3 in (IEnumerable<object>) objectList)
              list[num++] = this.DeserializeObject(obj3, type.GetElementType());
          }
          else if (ReflectionUtils.IsTypeGenericeCollectionInterface(type) || typeof (IList).IsAssignableFrom(type))
          {
            Type genericArgument = type.GetGenericArguments()[0];
            list = (IList) CacheResolver.GetNewInstance(typeof (List<>).MakeGenericType(genericArgument));
            foreach (object obj4 in (IEnumerable<object>) objectList)
              list.Add(this.DeserializeObject(obj4, genericArgument));
          }
          source = (object) list;
          goto default;
        default:
          return source;
      }
      return ReflectionUtils.IsNullableType(type) ? ReflectionUtils.ToNullableType(obj1, type) : obj1;
    }

    protected virtual object SerializeEnum(Enum p)
    {
      return (object) Convert.ToDouble((object) p, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    protected virtual bool TrySerializeKnownTypes(object input, out object output)
    {
      bool flag = true;
      if (input is DateTime dateTime)
        output = (object) dateTime.ToUniversalTime().ToString(PocoJsonSerializerStrategy.Iso8601Format[0], (IFormatProvider) CultureInfo.InvariantCulture);
      else if (input is Guid guid)
        output = (object) guid.ToString("D");
      else if ((object) (input as Uri) != null)
        output = (object) input.ToString();
      else if (input is Enum)
      {
        output = this.SerializeEnum((Enum) input);
      }
      else
      {
        flag = false;
        output = (object) null;
      }
      return flag;
    }

    protected virtual bool TrySerializeUnknownTypes(object input, out object output)
    {
      output = (object) null;
      Type type = input.GetType();
      if (type.FullName == null)
        return false;
      IDictionary<string, object> dictionary = (IDictionary<string, object>) new JsonObject();
      foreach (KeyValuePair<string, CacheResolver.MemberMap> loadMap in this.CacheResolver.LoadMaps(type))
      {
        if (loadMap.Value.Getter != null)
          dictionary.Add(loadMap.Key, loadMap.Value.Getter(input));
      }
      output = (object) dictionary;
      return true;
    }
  }
}
