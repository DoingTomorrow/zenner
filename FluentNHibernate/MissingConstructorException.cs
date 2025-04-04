// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MissingConstructorException
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace FluentNHibernate
{
  [Serializable]
  public class MissingConstructorException : Exception
  {
    public MissingConstructorException(Type type)
      : base("'" + type.AssemblyQualifiedName + "' is missing a parameterless constructor.")
    {
    }

    protected MissingConstructorException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
