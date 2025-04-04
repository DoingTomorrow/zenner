// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.UnmappedPropertyException
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  [Serializable]
  public class UnmappedPropertyException : Exception
  {
    public UnmappedPropertyException(Type type, string name)
      : base("Unmapped property '" + name + "' on type '" + type.Name + "'")
    {
    }

    protected UnmappedPropertyException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
