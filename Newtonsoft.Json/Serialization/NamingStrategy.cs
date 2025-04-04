// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.NamingStrategy
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  public abstract class NamingStrategy
  {
    public bool ProcessDictionaryKeys { get; set; }

    public bool OverrideSpecifiedNames { get; set; }

    public virtual string GetPropertyName(string name, bool hasSpecifiedName)
    {
      return hasSpecifiedName && !this.OverrideSpecifiedNames ? name : this.ResolvePropertyName(name);
    }

    public virtual string GetDictionaryKey(string key)
    {
      return !this.ProcessDictionaryKeys ? key : this.ResolvePropertyName(key);
    }

    protected abstract string ResolvePropertyName(string name);
  }
}
