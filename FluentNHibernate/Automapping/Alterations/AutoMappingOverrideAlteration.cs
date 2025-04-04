// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.Alterations.AutoMappingOverrideAlteration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Automapping.Alterations
{
  public class AutoMappingOverrideAlteration : IAutoMappingAlteration
  {
    private readonly Assembly assembly;

    public AutoMappingOverrideAlteration(Assembly overrideAssembly)
    {
      this.assembly = overrideAssembly;
    }

    public void Alter(AutoPersistenceModel model)
    {
      foreach (Type overrideType in ((IEnumerable<Type>) this.assembly.GetExportedTypes()).Where<Type>((Func<Type, bool>) (type => !type.IsAbstract)).Select(type => new
      {
        type = type,
        entity = ((IEnumerable<Type>) type.GetInterfaces()).Where<Type>((Func<Type, bool>) (interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof (IAutoMappingOverride<>))).Select<Type, Type>((Func<Type, Type>) (interfaceType => interfaceType.GetGenericArguments()[0])).FirstOrDefault<Type>()
      }).Where(_param0 => _param0.entity != null).Select(_param0 => _param0.type))
        model.Override(overrideType);
    }
  }
}
