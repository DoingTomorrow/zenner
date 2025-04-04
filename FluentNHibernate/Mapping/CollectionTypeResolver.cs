// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.CollectionTypeResolver
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public static class CollectionTypeResolver
  {
    public static Collection Resolve(Member member)
    {
      Member backingField;
      if (CollectionTypeResolver.IsEnumerable(member) && member.TryGetBackingField(out backingField))
        return CollectionTypeResolver.Resolve(backingField);
      return CollectionTypeResolver.IsSet(member) ? Collection.Set : Collection.Bag;
    }

    private static bool IsSet(Member member)
    {
      return member.PropertyType.Name.In<string>("ISet", "ISet`1", "HashSet`1");
    }

    private static bool IsEnumerable(Member member)
    {
      return member.PropertyType == typeof (IEnumerable) || member.PropertyType.Closes(typeof (IEnumerable<>));
    }
  }
}
