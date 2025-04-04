// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.AttributeMap
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Meta
{
  internal abstract class AttributeMap
  {
    public abstract bool TryGet(string key, bool publicOnly, out object value);

    public bool TryGet(string key, out object value) => this.TryGet(key, true, out value);

    public abstract Type AttributeType { get; }

    public static AttributeMap[] Create(TypeModel model, Type type, bool inherit)
    {
      object[] customAttributes = type.GetCustomAttributes(inherit);
      AttributeMap[] attributeMapArray = new AttributeMap[customAttributes.Length];
      for (int index = 0; index < customAttributes.Length; ++index)
        attributeMapArray[index] = (AttributeMap) new AttributeMap.ReflectionAttributeMap((Attribute) customAttributes[index]);
      return attributeMapArray;
    }

    public static AttributeMap[] Create(TypeModel model, MemberInfo member, bool inherit)
    {
      object[] customAttributes = member.GetCustomAttributes(inherit);
      AttributeMap[] attributeMapArray = new AttributeMap[customAttributes.Length];
      for (int index = 0; index < customAttributes.Length; ++index)
        attributeMapArray[index] = (AttributeMap) new AttributeMap.ReflectionAttributeMap((Attribute) customAttributes[index]);
      return attributeMapArray;
    }

    public static AttributeMap[] Create(TypeModel model, Assembly assembly)
    {
      object[] customAttributes = assembly.GetCustomAttributes(false);
      AttributeMap[] attributeMapArray = new AttributeMap[customAttributes.Length];
      for (int index = 0; index < customAttributes.Length; ++index)
        attributeMapArray[index] = (AttributeMap) new AttributeMap.ReflectionAttributeMap((Attribute) customAttributes[index]);
      return attributeMapArray;
    }

    public abstract object Target { get; }

    private sealed class ReflectionAttributeMap : AttributeMap
    {
      private readonly Attribute attribute;

      public override object Target => (object) this.attribute;

      public override Type AttributeType => this.attribute.GetType();

      public override bool TryGet(string key, bool publicOnly, out object value)
      {
        foreach (MemberInfo fieldsAndProperty in Helpers.GetInstanceFieldsAndProperties(this.attribute.GetType(), publicOnly))
        {
          if (string.Equals(fieldsAndProperty.Name, key, StringComparison.OrdinalIgnoreCase))
          {
            PropertyInfo propertyInfo = fieldsAndProperty as PropertyInfo;
            if (propertyInfo != (PropertyInfo) null)
            {
              value = propertyInfo.GetValue((object) this.attribute, (object[]) null);
              return true;
            }
            FieldInfo fieldInfo = fieldsAndProperty as FieldInfo;
            value = fieldInfo != (FieldInfo) null ? fieldInfo.GetValue((object) this.attribute) : throw new NotSupportedException(fieldsAndProperty.GetType().Name);
            return true;
          }
        }
        value = (object) null;
        return false;
      }

      public ReflectionAttributeMap(Attribute attribute) => this.attribute = attribute;
    }
  }
}
