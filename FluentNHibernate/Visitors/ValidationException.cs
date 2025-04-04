// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.ValidationException
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Visitors
{
  [Serializable]
  public class ValidationException : Exception
  {
    public ValidationException(string message, string resolution, Type relatedEntity)
      : base(message + " " + resolution + ".")
    {
      this.Resolution = resolution;
      this.RelatedEntity = relatedEntity;
    }

    public Type RelatedEntity { get; private set; }

    public string Resolution { get; private set; }
  }
}
