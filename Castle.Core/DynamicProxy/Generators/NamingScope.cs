// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.NamingScope
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Collections.Generic;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class NamingScope : INamingScope
  {
    private readonly IDictionary<string, int> names = (IDictionary<string, int>) new Dictionary<string, int>();
    private readonly INamingScope parentScope;

    public NamingScope()
    {
    }

    private NamingScope(INamingScope parent) => this.parentScope = parent;

    public string GetUniqueName(string suggestedName)
    {
      int num1;
      if (!this.names.TryGetValue(suggestedName, out num1))
      {
        this.names.Add(suggestedName, 0);
        return suggestedName;
      }
      int num2 = num1 + 1;
      this.names[suggestedName] = num2;
      return suggestedName + "_" + num2.ToString();
    }

    public INamingScope SafeSubScope() => (INamingScope) new NamingScope((INamingScope) this);

    public INamingScope ParentScope => this.parentScope;
  }
}
