// Decompiled with JetBrains decompiler
// Type: NHibernate.PropertyNotFoundException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class PropertyNotFoundException : MappingException
  {
    private readonly Type targetType;
    private readonly string propertyName;
    private readonly string accessorType;

    public PropertyNotFoundException(Type targetType, string propertyName, string accessorType)
      : base(string.Format("Could not find a {0} for property '{1}' in class '{2}'", (object) accessorType, (object) propertyName, (object) targetType))
    {
      this.targetType = targetType;
      this.propertyName = propertyName;
      this.accessorType = accessorType;
    }

    public PropertyNotFoundException(Type targetType, string propertyName)
      : base(string.Format("Could not find property nor field '{0}' in class '{1}'", (object) propertyName, (object) targetType))
    {
      this.targetType = targetType;
      this.propertyName = propertyName;
    }

    public PropertyNotFoundException(string propertyName, string fieldName, Type targetType)
      : base(string.Format("Could not find the property '{0}', associated to the field '{1}', in class '{2}'", (object) propertyName, (object) fieldName, (object) targetType))
    {
      this.targetType = targetType;
      this.propertyName = propertyName;
      this.accessorType = fieldName;
    }

    protected PropertyNotFoundException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public Type TargetType => this.targetType;

    public string PropertyName => this.propertyName;

    public string AccessorType => this.accessorType;
  }
}
