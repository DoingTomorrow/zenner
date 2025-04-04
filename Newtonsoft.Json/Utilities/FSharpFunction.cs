// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.FSharpFunction
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Utilities
{
  internal class FSharpFunction
  {
    private readonly object _instance;
    private readonly MethodCall<object, object> _invoker;

    public FSharpFunction(object instance, MethodCall<object, object> invoker)
    {
      this._instance = instance;
      this._invoker = invoker;
    }

    public object Invoke(params object[] args) => this._invoker(this._instance, args);
  }
}
