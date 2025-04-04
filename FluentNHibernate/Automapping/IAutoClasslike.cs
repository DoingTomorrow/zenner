// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.IAutoClasslike
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using System;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public interface IAutoClasslike : IMappingProvider
  {
    void DiscriminateSubClassesOnColumn(string column);

    IAutoClasslike JoinedSubClass(Type type, string keyColumn);

    IAutoClasslike SubClass(Type type, string discriminatorValue);

    void AlterModel(ClassMappingBase mapping);
  }
}
