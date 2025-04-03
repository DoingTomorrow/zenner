// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.ITypeContributor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public interface ITypeContributor
  {
    void CollectElementsToProxy(IProxyGenerationHook hook, MetaType model);

    void Generate(ClassEmitter @class, ProxyGenerationOptions options);
  }
}
