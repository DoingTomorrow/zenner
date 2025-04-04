// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.DynamicLoggerEntry
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System.Collections.Generic;
using System.Dynamic;

#nullable disable
namespace S4_Handler.Functions
{
  internal class DynamicLoggerEntry : DynamicObject
  {
    public Dictionary<string, object> DynamicProperties = new Dictionary<string, object>();

    public int Count => this.DynamicProperties.Count;

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      return this.DynamicProperties.TryGetValue(binder.Name, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
      this.DynamicProperties[binder.Name] = value;
      return true;
    }
  }
}
