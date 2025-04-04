// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.Prebuilt.BuiltProxyConvention
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
  public class BuiltProxyConvention : ProxyConvention
  {
    public BuiltProxyConvention(Type proxyType, Type persistentType)
      : this((Func<Type, Type>) (type => type != persistentType ? (Type) null : proxyType), (Func<Type, Type>) (type => type != proxyType ? (Type) null : persistentType))
    {
    }

    protected BuiltProxyConvention(
      Func<Type, Type> mapPersistentTypeToProxyInterfaceType,
      Func<Type, Type> mapProxyInterfaceTypeToPersistentType)
      : base(mapPersistentTypeToProxyInterfaceType, mapProxyInterfaceTypeToPersistentType)
    {
    }
  }
}
