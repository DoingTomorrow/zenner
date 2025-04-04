// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.CompositeIdentityInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class CompositeIdentityInstance : 
    CompositeIdentityInspector,
    ICompositeIdentityInstance,
    ICompositeIdentityInspector,
    IIdentityInspectorBase,
    IInspector
  {
    private readonly CompositeIdMapping mapping;
    private bool nextBool = true;

    public CompositeIdentityInstance(CompositeIdMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public void UnsavedValue(string unsavedValue)
    {
      this.mapping.Set<string>((Expression<Func<CompositeIdMapping, string>>) (x => x.UnsavedValue), 1, unsavedValue);
    }

    public IAccessInstance Access
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<CompositeIdMapping, string>>) (x => x.Access), 1, value)));
      }
    }

    public void Mapped()
    {
      this.mapping.Set<bool>((Expression<Func<CompositeIdMapping, bool>>) (x => x.Mapped), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public ICompositeIdentityInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (ICompositeIdentityInstance) this;
      }
    }

    public IEnumerable<IKeyPropertyInstance> KeyProperties
    {
      get
      {
        return this.mapping.Keys.Where<ICompositeIdKeyMapping>((Func<ICompositeIdKeyMapping, bool>) (x => x is KeyPropertyMapping)).Select<ICompositeIdKeyMapping, KeyPropertyInstance>((Func<ICompositeIdKeyMapping, KeyPropertyInstance>) (x => new KeyPropertyInstance((KeyPropertyMapping) x))).Cast<IKeyPropertyInstance>();
      }
    }

    public IEnumerable<IKeyManyToOneInstance> KeyManyToOnes
    {
      get
      {
        return this.mapping.Keys.Where<ICompositeIdKeyMapping>((Func<ICompositeIdKeyMapping, bool>) (x => x is KeyManyToOneMapping)).Select<ICompositeIdKeyMapping, KeyManyToOneInstance>((Func<ICompositeIdKeyMapping, KeyManyToOneInstance>) (x => new KeyManyToOneInstance((KeyManyToOneMapping) x))).Cast<IKeyManyToOneInstance>();
      }
    }
  }
}
