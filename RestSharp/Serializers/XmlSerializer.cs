// Decompiled with JetBrains decompiler
// Type: RestSharp.Serializers.XmlSerializer
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

#nullable disable
namespace RestSharp.Serializers
{
  public class XmlSerializer : ISerializer
  {
    public XmlSerializer() => this.ContentType = "text/xml";

    public XmlSerializer(string @namespace)
    {
      this.Namespace = @namespace;
      this.ContentType = "text/xml";
    }

    public string Serialize(object obj)
    {
      XDocument xdocument = new XDocument();
      Type type1 = obj.GetType();
      string name1 = type1.Name;
      SerializeAsAttribute attribute1 = ReflectionExtensions.GetAttribute<SerializeAsAttribute>(type1);
      if (attribute1 != null)
        name1 = attribute1.TransformName(attribute1.Name ?? name1);
      XElement xelement1 = new XElement(name1.AsNamespaced(this.Namespace));
      if (obj is IList)
      {
        string name2 = "";
        foreach (object obj1 in (IEnumerable) obj)
        {
          Type type2 = obj1.GetType();
          SerializeAsAttribute attribute2 = ReflectionExtensions.GetAttribute<SerializeAsAttribute>(type2);
          if (attribute2 != null)
            name2 = attribute2.TransformName(attribute2.Name ?? name1);
          if (name2 == "")
            name2 = type2.Name;
          XElement xelement2 = new XElement((XName) name2);
          this.Map(xelement2, obj1);
          xelement1.Add((object) xelement2);
        }
      }
      else
        this.Map(xelement1, obj);
      if (this.RootElement.HasValue())
      {
        XElement content = new XElement(this.RootElement.AsNamespaced(this.Namespace), (object) xelement1);
        xdocument.Add((object) content);
      }
      else
        xdocument.Add((object) xelement1);
      return xdocument.ToString();
    }

    private void Map(XElement root, object obj)
    {
      Type type = obj.GetType();
      IEnumerable<PropertyInfo> propertyInfos = ((IEnumerable<PropertyInfo>) type.GetProperties()).Select(p => new
      {
        p = p,
        indexAttribute = p.GetAttribute<SerializeAsAttribute>()
      }).Where(_param0 => _param0.p.CanRead && _param0.p.CanWrite).OrderBy(_param0 => _param0.indexAttribute != null ? _param0.indexAttribute.Index : int.MaxValue).Select(_param0 => _param0.p);
      SerializeAsAttribute attribute1 = ReflectionExtensions.GetAttribute<SerializeAsAttribute>(type);
      foreach (PropertyInfo prop in propertyInfos)
      {
        string str = prop.Name;
        object obj1 = prop.GetValue(obj, (object[]) null);
        if (obj1 != null)
        {
          string serializedValue = this.GetSerializedValue(obj1);
          Type propertyType = prop.PropertyType;
          bool flag = false;
          SerializeAsAttribute attribute2 = prop.GetAttribute<SerializeAsAttribute>();
          if (attribute2 != null)
          {
            str = attribute2.Name.HasValue() ? attribute2.Name : str;
            flag = attribute2.Attribute;
          }
          SerializeAsAttribute attribute3 = prop.GetAttribute<SerializeAsAttribute>();
          if (attribute3 != null)
            str = attribute3.TransformName(str);
          else if (attribute1 != null)
            str = attribute1.TransformName(str);
          XElement xelement1 = new XElement(str.AsNamespaced(this.Namespace));
          if (propertyType.IsPrimitive || propertyType.IsValueType || propertyType == typeof (string))
          {
            if (flag)
            {
              root.Add((object) new XAttribute((XName) str, (object) serializedValue));
              continue;
            }
            xelement1.Value = serializedValue;
          }
          else if (obj1 is IList)
          {
            string name = "";
            foreach (object obj2 in (IEnumerable) obj1)
            {
              if (name == "")
                name = obj2.GetType().Name;
              XElement xelement2 = new XElement((XName) name);
              this.Map(xelement2, obj2);
              xelement1.Add((object) xelement2);
            }
          }
          else
            this.Map(xelement1, obj1);
          root.Add((object) xelement1);
        }
      }
    }

    private string GetSerializedValue(object obj)
    {
      object obj1 = obj;
      if (obj is DateTime && this.DateFormat.HasValue())
        obj1 = (object) ((DateTime) obj).ToString(this.DateFormat);
      if (obj is bool)
        obj1 = (object) obj.ToString().ToLower();
      return obj1.ToString();
    }

    public string RootElement { get; set; }

    public string Namespace { get; set; }

    public string DateFormat { get; set; }

    public string ContentType { get; set; }
  }
}
