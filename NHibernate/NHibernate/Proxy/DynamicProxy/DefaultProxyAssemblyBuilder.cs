// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.DefaultProxyAssemblyBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  public class DefaultProxyAssemblyBuilder : IProxyAssemblyBuilder
  {
    public AssemblyBuilder DefineDynamicAssembly(AppDomain appDomain, AssemblyName name)
    {
      AssemblyBuilderAccess access = AssemblyBuilderAccess.Run;
      return appDomain.DefineDynamicAssembly(name, access);
    }

    public ModuleBuilder DefineDynamicModule(AssemblyBuilder assemblyBuilder, string moduleName)
    {
      return assemblyBuilder.DefineDynamicModule(moduleName);
    }

    public void Save(AssemblyBuilder assemblyBuilder)
    {
    }
  }
}
