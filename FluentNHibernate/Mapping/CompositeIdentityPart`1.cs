// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.CompositeIdentityPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class CompositeIdentityPart<T> : ICompositeIdMappingProvider
  {
    private readonly Action<Member> onMemberMapped;
    private readonly AccessStrategyBuilder<CompositeIdentityPart<T>> access;
    private readonly AttributeStore attributes = new AttributeStore();
    private readonly IList<ICompositeIdKeyMapping> keys = (IList<ICompositeIdKeyMapping>) new List<ICompositeIdKeyMapping>();
    private bool nextBool = true;

    public CompositeIdentityPart(Action<Member> onMemberMapped)
    {
      this.onMemberMapped = onMemberMapped;
      this.access = new AccessStrategyBuilder<CompositeIdentityPart<T>>(this, (Action<string>) (value => this.attributes.Set(nameof (Access), 2, (object) value)));
    }

    public CompositeIdentityPart(string name, Action<Member> onMemberMapped)
      : this(onMemberMapped)
    {
      this.attributes.Set("Name", 0, (object) name);
    }

    public CompositeIdentityPart<T> KeyProperty(Expression<Func<T, object>> expression)
    {
      Member member = expression.ToMember<T, object>();
      return this.KeyProperty(member, member.Name, (Action<KeyPropertyPart>) null);
    }

    public CompositeIdentityPart<T> KeyProperty(
      Expression<Func<T, object>> expression,
      string columnName)
    {
      return this.KeyProperty(expression.ToMember<T, object>(), columnName, (Action<KeyPropertyPart>) null);
    }

    public CompositeIdentityPart<T> KeyProperty(
      Expression<Func<T, object>> expression,
      Action<KeyPropertyPart> keyPropertyAction)
    {
      return this.KeyProperty(expression.ToMember<T, object>(), string.Empty, keyPropertyAction);
    }

    protected virtual CompositeIdentityPart<T> KeyProperty(
      Member member,
      string columnName,
      Action<KeyPropertyPart> customMapping)
    {
      this.onMemberMapped(member);
      Type type = member.PropertyType;
      if (type.IsEnum)
        type = typeof (GenericEnumMapper<>).MakeGenericType(type);
      KeyPropertyMapping mapping1 = new KeyPropertyMapping()
      {
        ContainingEntityType = typeof (T)
      };
      mapping1.Set<string>((Expression<Func<KeyPropertyMapping, string>>) (x => x.Name), 0, member.Name);
      mapping1.Set<TypeReference>((Expression<Func<KeyPropertyMapping, TypeReference>>) (x => x.Type), 0, new TypeReference(type));
      if (customMapping != null)
      {
        KeyPropertyPart keyPropertyPart = new KeyPropertyPart(mapping1);
        customMapping(keyPropertyPart);
      }
      if (!string.IsNullOrEmpty(columnName))
      {
        ColumnMapping mapping2 = new ColumnMapping();
        mapping2.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, columnName);
        mapping1.AddColumn(mapping2);
      }
      this.keys.Add((ICompositeIdKeyMapping) mapping1);
      return this;
    }

    public CompositeIdentityPart<T> KeyReference(Expression<Func<T, object>> expression)
    {
      Member member = expression.ToMember<T, object>();
      return this.KeyReference(member, (IEnumerable<string>) new string[1]
      {
        member.Name
      }, (Action<KeyManyToOnePart>) null);
    }

    public CompositeIdentityPart<T> KeyReference(
      Expression<Func<T, object>> expression,
      params string[] columnNames)
    {
      return this.KeyReference(expression.ToMember<T, object>(), (IEnumerable<string>) columnNames, (Action<KeyManyToOnePart>) null);
    }

    public CompositeIdentityPart<T> KeyReference(
      Expression<Func<T, object>> expression,
      Action<KeyManyToOnePart> customMapping,
      params string[] columnNames)
    {
      return this.KeyReference(expression.ToMember<T, object>(), (IEnumerable<string>) columnNames, customMapping);
    }

    protected virtual CompositeIdentityPart<T> KeyReference(
      Member member,
      IEnumerable<string> columnNames,
      Action<KeyManyToOnePart> customMapping)
    {
      this.onMemberMapped(member);
      KeyManyToOneMapping mapping1 = new KeyManyToOneMapping()
      {
        ContainingEntityType = typeof (T)
      };
      mapping1.Set<string>((Expression<Func<KeyManyToOneMapping, string>>) (x => x.Name), 0, member.Name);
      mapping1.Set<TypeReference>((Expression<Func<KeyManyToOneMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(member.PropertyType));
      foreach (string columnName in columnNames)
      {
        ColumnMapping mapping2 = new ColumnMapping();
        mapping2.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, columnName);
        mapping1.AddColumn(mapping2);
      }
      KeyManyToOnePart keyManyToOnePart = new KeyManyToOnePart(mapping1);
      if (customMapping != null)
        customMapping(keyManyToOnePart);
      this.keys.Add((ICompositeIdKeyMapping) mapping1);
      return this;
    }

    public virtual CompositeIdentityPart<T> CustomType<CType>()
    {
      KeyPropertyMapping keyPropertyMapping = this.keys.Where<ICompositeIdKeyMapping>((Func<ICompositeIdKeyMapping, bool>) (x => x is KeyPropertyMapping)).Cast<KeyPropertyMapping>().LastOrDefault<KeyPropertyMapping>();
      if (keyPropertyMapping != null)
        keyPropertyMapping.Set<TypeReference>((Expression<Func<KeyPropertyMapping, TypeReference>>) (x => x.Type), 0, new TypeReference(typeof (CType)));
      return this;
    }

    public AccessStrategyBuilder<CompositeIdentityPart<T>> Access => this.access;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public CompositeIdentityPart<T> Not
    {
      get
      {
        this.nextBool = false;
        return this;
      }
    }

    public CompositeIdentityPart<T> Mapped()
    {
      this.attributes.Set(nameof (Mapped), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public CompositeIdentityPart<T> UnsavedValue(string value)
    {
      this.attributes.Set(nameof (UnsavedValue), 2, (object) value);
      return this;
    }

    public CompositeIdentityPart<T> ComponentCompositeIdentifier<TComponentType>(
      Expression<Func<T, TComponentType>> expression)
    {
      this.attributes.Set("Class", 0, (object) new TypeReference(typeof (TComponentType)));
      this.attributes.Set("Name", 0, (object) expression.ToMember<T, TComponentType>().Name);
      return this;
    }

    CompositeIdMapping ICompositeIdMappingProvider.GetCompositeIdMapping()
    {
      CompositeIdMapping compositeIdMapping = new CompositeIdMapping(this.attributes.Clone())
      {
        ContainingEntityType = typeof (T)
      };
      this.keys.Each<ICompositeIdKeyMapping>(new Action<ICompositeIdKeyMapping>(compositeIdMapping.AddKey));
      return compositeIdMapping;
    }
  }
}
