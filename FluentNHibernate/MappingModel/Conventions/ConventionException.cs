// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Conventions.ConventionException
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace FluentNHibernate.MappingModel.Conventions
{
  [Serializable]
  public class ConventionException : Exception
  {
    private readonly object conventionTarget;

    public ConventionException(string message, object conventionTarget)
      : base(message)
    {
      this.conventionTarget = conventionTarget;
    }

    protected ConventionException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public object ConventionTarget => this.conventionTarget;
  }
}
