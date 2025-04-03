// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.DictionaryDescriptor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class DictionaryDescriptor : PropertyDescriptor
  {
    private List<IDictionaryInitializer> initializers;
    private List<IDictionaryMetaInitializer> metaInitializers;
    private static readonly ICollection<IDictionaryInitializer> NoInitializers = (ICollection<IDictionaryInitializer>) new IDictionaryInitializer[0];
    private static readonly ICollection<IDictionaryMetaInitializer> NoMetaInitializers = (ICollection<IDictionaryMetaInitializer>) new IDictionaryMetaInitializer[0];

    public DictionaryDescriptor()
    {
    }

    public DictionaryDescriptor(PropertyInfo property, object[] behaviors)
      : base(property, behaviors)
    {
    }

    public ICollection<IDictionaryInitializer> Initializers
    {
      get
      {
        return (ICollection<IDictionaryInitializer>) this.initializers ?? DictionaryDescriptor.NoInitializers;
      }
    }

    public ICollection<IDictionaryMetaInitializer> MetaInitializers
    {
      get
      {
        return (ICollection<IDictionaryMetaInitializer>) this.metaInitializers ?? DictionaryDescriptor.NoMetaInitializers;
      }
    }

    public DictionaryDescriptor AddInitializer(params IDictionaryInitializer[] inits)
    {
      return this.AddInitializers((IEnumerable<IDictionaryInitializer>) inits);
    }

    public DictionaryDescriptor AddInitializers(IEnumerable<IDictionaryInitializer> inits)
    {
      if (inits != null)
      {
        if (this.initializers == null)
          this.initializers = new List<IDictionaryInitializer>(inits);
        else
          this.initializers.AddRange(inits);
      }
      return this;
    }

    public DictionaryDescriptor CopyInitializers(DictionaryDescriptor other)
    {
      if (this.initializers != null)
        other.AddInitializers((IEnumerable<IDictionaryInitializer>) this.initializers);
      return this;
    }

    public DictionaryDescriptor CopyInitializers(
      DictionaryDescriptor other,
      Func<IDictionaryInitializer, bool> selector)
    {
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (this.initializers != null)
        other.AddInitializers(this.initializers.Where<IDictionaryInitializer>(selector));
      return this;
    }

    public DictionaryDescriptor AddMetaInitializer(params IDictionaryMetaInitializer[] inits)
    {
      return this.AddMetaInitializers((IEnumerable<IDictionaryMetaInitializer>) inits);
    }

    public DictionaryDescriptor AddMetaInitializers(IEnumerable<IDictionaryMetaInitializer> inits)
    {
      if (inits != null)
      {
        if (this.metaInitializers == null)
          this.metaInitializers = new List<IDictionaryMetaInitializer>(inits);
        else
          this.metaInitializers.AddRange(inits);
      }
      return this;
    }

    public DictionaryDescriptor CopyMetaInitializers(DictionaryDescriptor other)
    {
      if (this.metaInitializers != null)
        other.AddMetaInitializers((IEnumerable<IDictionaryMetaInitializer>) this.metaInitializers);
      return this;
    }

    public DictionaryDescriptor CopyMetaInitializers(
      DictionaryDescriptor other,
      Func<IDictionaryMetaInitializer, bool> selector)
    {
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (this.metaInitializers != null)
        other.AddMetaInitializers(this.metaInitializers.Where<IDictionaryMetaInitializer>(selector));
      return this;
    }

    public override PropertyDescriptor CopyBehaviors(PropertyDescriptor other)
    {
      if (other is DictionaryDescriptor)
      {
        DictionaryDescriptor other1 = (DictionaryDescriptor) other;
        this.CopyMetaInitializers(other1).CopyInitializers(other1);
      }
      return base.CopyBehaviors(other);
    }

    protected override void InternalAddBehavior(IDictionaryBehavior behavior)
    {
      if (behavior is IDictionaryInitializer)
        this.AddInitializer((IDictionaryInitializer) behavior);
      if (behavior is IDictionaryMetaInitializer)
        this.AddMetaInitializer((IDictionaryMetaInitializer) behavior);
      base.InternalAddBehavior(behavior);
    }
  }
}
