// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Infrastructure.ResolveException
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace FluentNHibernate.Infrastructure
{
  [Serializable]
  public class ResolveException : Exception
  {
    public ResolveException(Type type)
      : base("Unable to resolve dependency: '" + type.FullName + "'")
    {
    }

    protected ResolveException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
