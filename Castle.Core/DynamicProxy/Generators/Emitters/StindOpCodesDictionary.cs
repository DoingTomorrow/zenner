// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.StindOpCodesDictionary
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public sealed class StindOpCodesDictionary : Dictionary<Type, OpCode>
  {
    private static readonly StindOpCodesDictionary dict = new StindOpCodesDictionary();
    private static readonly OpCode emptyOpCode = new OpCode();

    private StindOpCodesDictionary()
    {
      this.Add(typeof (bool), OpCodes.Stind_I1);
      this.Add(typeof (char), OpCodes.Stind_I2);
      this.Add(typeof (sbyte), OpCodes.Stind_I1);
      this.Add(typeof (short), OpCodes.Stind_I2);
      this.Add(typeof (int), OpCodes.Stind_I4);
      this.Add(typeof (long), OpCodes.Stind_I8);
      this.Add(typeof (float), OpCodes.Stind_R4);
      this.Add(typeof (double), OpCodes.Stind_R8);
      this.Add(typeof (byte), OpCodes.Stind_I1);
      this.Add(typeof (ushort), OpCodes.Stind_I2);
      this.Add(typeof (uint), OpCodes.Stind_I4);
      this.Add(typeof (ulong), OpCodes.Stind_I8);
    }

    public new OpCode this[Type type]
    {
      get => this.ContainsKey(type) ? base[type] : StindOpCodesDictionary.EmptyOpCode;
    }

    public static StindOpCodesDictionary Instance => StindOpCodesDictionary.dict;

    public static OpCode EmptyOpCode => StindOpCodesDictionary.emptyOpCode;
  }
}
