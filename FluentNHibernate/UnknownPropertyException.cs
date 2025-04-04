// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.UnknownPropertyException
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Runtime.Serialization;
using System.Security;

#nullable disable
namespace FluentNHibernate
{
  [Serializable]
  public class UnknownPropertyException : Exception
  {
    public UnknownPropertyException(Type classType, string propertyName)
      : base("Could not find property '" + propertyName + "' on '" + classType.FullName + "'")
    {
      this.Type = classType;
      this.Property = propertyName;
    }

    public string Property { get; private set; }

    public Type Type { get; private set; }

    protected UnknownPropertyException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.Type = info.GetValue(nameof (Type), typeof (Type)) as Type;
      this.Property = info.GetString(nameof (Property));
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("Type", (object) this.Type, typeof (Type));
      info.AddValue("Property", (object) this.Property);
    }
  }
}
