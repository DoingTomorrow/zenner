// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DefaultContractResolverState
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System.Collections.Generic;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  internal class DefaultContractResolverState
  {
    public Dictionary<ResolverContractKey, JsonContract> ContractCache;
    public PropertyNameTable NameTable = new PropertyNameTable();
  }
}
