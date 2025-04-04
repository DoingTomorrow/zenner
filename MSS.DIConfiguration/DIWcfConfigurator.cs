// Decompiled with JetBrains decompiler
// Type: MSS.DIConfiguration.DIWcfConfigurator
// Assembly: MSS.DIConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FF318A7F-B5DB-4F93-8026-33B4E3BCEF3D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DIConfiguration.dll

using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.DIConfiguration
{
  public class DIWcfConfigurator
  {
    private static IKernel kernel;

    public static IKernel GetConfigurator()
    {
      if (DIWcfConfigurator.kernel != null)
        return DIWcfConfigurator.kernel;
      DIWcfConfigurator.kernel = (IKernel) new StandardKernel(Array.Empty<INinjectModule>());
      List<INinjectModule> m = new List<INinjectModule>()
      {
        (INinjectModule) new RepositoryModule(),
        (INinjectModule) new WcfNHibernateModule()
      };
      DIWcfConfigurator.kernel.Load((IEnumerable<INinjectModule>) m);
      return DIWcfConfigurator.kernel;
    }
  }
}
