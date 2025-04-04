// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.MappingBase
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Visitors;
using System;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  public abstract class MappingBase : IMapping
  {
    public abstract void AcceptVisitor(IMappingModelVisitor visitor);

    public abstract bool IsSpecified(string attribute);

    protected abstract void Set(string attribute, int layer, object value);

    void IMapping.Set(string attribute, int layer, object value)
    {
      this.Set(attribute, layer, value);
    }
  }
}
