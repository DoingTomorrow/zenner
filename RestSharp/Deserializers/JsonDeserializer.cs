// Decompiled with JetBrains decompiler
// Type: RestSharp.Deserializers.JsonDeserializer
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#nullable disable
namespace RestSharp.Deserializers
{
  public class JsonDeserializer : IDeserializer
  {
    public string RootElement { get; set; }

    public string Namespace { get; set; }

    public string DateFormat { get; set; }

    public CultureInfo Culture { get; set; }

    public JsonDeserializer() => this.Culture = CultureInfo.InvariantCulture;

    public T Deserialize<T>(IRestResponse response)
    {
      T target = Activator.CreateInstance<T>();
      if ((object) target is IList)
      {
        Type type = target.GetType();
        if (this.RootElement.HasValue())
        {
          object root = this.FindRoot(response.Content);
          target = (T) this.BuildList(type, root);
        }
        else
        {
          object parent = SimpleJson.DeserializeObject(response.Content);
          target = (T) this.BuildList(type, parent);
        }
      }
      else if ((object) target is IDictionary)
      {
        object root = this.FindRoot(response.Content);
        target = (T) this.BuildDictionary(target.GetType(), root);
      }
      else
      {
        object root = this.FindRoot(response.Content);
        this.Map((object) target, (IDictionary<string, object>) root);
      }
      return target;
    }

    private object FindRoot(string content)
    {
      IDictionary<string, object> dictionary = (IDictionary<string, object>) SimpleJson.DeserializeObject(content);
      return this.RootElement.HasValue() && dictionary.ContainsKey(this.RootElement) ? dictionary[this.RootElement] : (object) dictionary;
    }

    private void Map(object target, IDictionary<string, object> data)
    {
      foreach (PropertyInfo propertyInfo in ((IEnumerable<PropertyInfo>) target.GetType().GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.CanWrite)).ToList<PropertyInfo>())
      {
        Type type = propertyInfo.PropertyType;
        string key = propertyInfo.Name.GetNameVariants(this.Culture).FirstOrDefault<string>((Func<string, bool>) (n => data.ContainsKey(n)));
        object obj = key != null ? data[key] : (object) null;
        if (obj != null)
        {
          if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
            type = type.GetGenericArguments()[0];
          propertyInfo.SetValue(target, this.ConvertValue(type, obj), (object[]) null);
        }
      }
    }

    private IDictionary BuildDictionary(Type type, object parent)
    {
      IDictionary instance = (IDictionary) Activator.CreateInstance(type);
      Type genericArgument = type.GetGenericArguments()[1];
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) parent)
      {
        string key = keyValuePair.Key;
        object obj = this.ConvertValue(genericArgument, keyValuePair.Value);
        instance.Add((object) key, obj);
      }
      return instance;
    }

    private IList BuildList(Type type, object parent)
    {
      IList instance = (IList) Activator.CreateInstance(type);
      Type genericArgument = ((IEnumerable<Type>) type.GetInterfaces()).First<Type>((Func<Type, bool>) (x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IList<>))).GetGenericArguments()[0];
      if (parent is IList)
      {
        foreach (object obj1 in (IEnumerable) parent)
        {
          if (genericArgument.IsPrimitive)
          {
            string source = obj1.ToString();
            instance.Add(source.ChangeType(genericArgument, this.Culture));
          }
          else if (genericArgument == typeof (string))
          {
            if (obj1 == null)
              instance.Add((object) null);
            else
              instance.Add((object) obj1.ToString());
          }
          else if (obj1 == null)
          {
            instance.Add((object) null);
          }
          else
          {
            object obj2 = this.ConvertValue(genericArgument, obj1);
            instance.Add(obj2);
          }
        }
      }
      else
        instance.Add(this.ConvertValue(genericArgument, parent));
      return instance;
    }

    private object ConvertValue(Type type, object value)
    {
      string str = Convert.ToString(value, (IFormatProvider) this.Culture);
      if (type.IsPrimitive)
        return str.Replace("\"", string.Empty).ChangeType(type, this.Culture);
      if (type.IsEnum)
        return type.FindEnumValue(str, this.Culture);
      if (type == typeof (Uri))
        return (object) new Uri(str, UriKind.RelativeOrAbsolute);
      if (type == typeof (string))
        return (object) str;
      if (type == typeof (DateTime) || type == typeof (DateTimeOffset))
      {
        DateTime dateTime = !this.DateFormat.HasValue() ? str.ParseJsonDate(this.Culture) : DateTime.ParseExact(str, this.DateFormat, (IFormatProvider) this.Culture);
        if (type == typeof (DateTime))
          return (object) dateTime;
        if (type == typeof (DateTimeOffset))
          return (object) (DateTimeOffset) dateTime;
      }
      else
      {
        if (type == typeof (Decimal))
          return (object) Decimal.Parse(str, (IFormatProvider) this.Culture);
        if (type == typeof (Guid))
          return (object) (string.IsNullOrEmpty(str) ? Guid.Empty : new Guid(str));
        if (type == typeof (TimeSpan))
          return (object) TimeSpan.Parse(str);
        if (!type.IsGenericType)
          return this.CreateAndMap(type, value);
        Type genericTypeDefinition = type.GetGenericTypeDefinition();
        if (genericTypeDefinition == typeof (List<>))
          return (object) this.BuildList(type, value);
        if (!(genericTypeDefinition == typeof (Dictionary<,>)))
          return this.CreateAndMap(type, value);
        if (type.GetGenericArguments()[0] == typeof (string))
          return (object) this.BuildDictionary(type, value);
      }
      return (object) null;
    }

    private object CreateAndMap(Type type, object element)
    {
      object instance = Activator.CreateInstance(type);
      this.Map(instance, (IDictionary<string, object>) element);
      return instance;
    }
  }
}
