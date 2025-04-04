// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.Proxy`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Helpers.Prebuilt;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers
{
  public class Proxy<TProxyType>
  {
    public static ProxyConvention UsedForType<TPersistentType>()
    {
      return (ProxyConvention) new BuiltProxyConvention(typeof (TProxyType), typeof (TPersistentType));
    }
  }
}
