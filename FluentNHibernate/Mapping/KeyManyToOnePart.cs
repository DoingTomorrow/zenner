// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.KeyManyToOnePart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Identity;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class KeyManyToOnePart
  {
    private readonly KeyManyToOneMapping mapping;
    private bool nextBool = true;

    public KeyManyToOnePart(KeyManyToOneMapping mapping)
    {
      this.mapping = mapping;
      this.Access = new AccessStrategyBuilder<KeyManyToOnePart>(this, (Action<string>) (value => mapping.Set<string>((Expression<Func<KeyManyToOneMapping, string>>) (x => x.Access), 2, value)));
      this.NotFound = new NotFoundExpression<KeyManyToOnePart>(this, (Action<string>) (value => mapping.Set<string>((Expression<Func<KeyManyToOneMapping, string>>) (x => x.NotFound), 2, value)));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyManyToOnePart Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    public KeyManyToOnePart ForeignKey(string foreignKey)
    {
      this.mapping.Set<string>((Expression<Func<KeyManyToOneMapping, string>>) (x => x.ForeignKey), 2, foreignKey);
      return this;
    }

    public AccessStrategyBuilder<KeyManyToOnePart> Access { get; private set; }

    public NotFoundExpression<KeyManyToOnePart> NotFound { get; private set; }

    public KeyManyToOnePart Lazy()
    {
      this.mapping.Set<bool>((Expression<Func<KeyManyToOneMapping, bool>>) (x => x.Lazy), 2, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
      return this;
    }

    public KeyManyToOnePart Name(string name)
    {
      this.mapping.Set<string>((Expression<Func<KeyManyToOneMapping, string>>) (x => x.Name), 2, name);
      return this;
    }

    public KeyManyToOnePart EntityName(string entityName)
    {
      this.mapping.Set<string>((Expression<Func<KeyManyToOneMapping, string>>) (x => x.EntityName), 2, entityName);
      return this;
    }
  }
}
