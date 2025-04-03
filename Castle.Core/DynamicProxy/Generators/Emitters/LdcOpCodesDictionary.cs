// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.LdcOpCodesDictionary
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public sealed class LdcOpCodesDictionary : Dictionary<Type, OpCode>
  {
    private static readonly LdcOpCodesDictionary dict = new LdcOpCodesDictionary();
    private static readonly OpCode emptyOpCode = new OpCode();

    private LdcOpCodesDictionary()
    {
      this.Add(typeof (bool), OpCodes.Ldc_I4);
      this.Add(typeof (char), OpCodes.Ldc_I4);
      this.Add(typeof (sbyte), OpCodes.Ldc_I4);
      this.Add(typeof (short), OpCodes.Ldc_I4);
      this.Add(typeof (int), OpCodes.Ldc_I4);
      this.Add(typeof (long), OpCodes.Ldc_I8);
      this.Add(typeof (float), OpCodes.Ldc_R4);
      this.Add(typeof (double), OpCodes.Ldc_R8);
      this.Add(typeof (byte), OpCodes.Ldc_I4_0);
      this.Add(typeof (ushort), OpCodes.Ldc_I4_0);
      this.Add(typeof (uint), OpCodes.Ldc_I4_0);
      this.Add(typeof (ulong), OpCodes.Ldc_I4_0);
    }

    public new OpCode this[Type type]
    {
      get => this.ContainsKey(type) ? base[type] : LdcOpCodesDictionary.EmptyOpCode;
    }

    public static LdcOpCodesDictionary Instance => LdcOpCodesDictionary.dict;

    public static OpCode EmptyOpCode => LdcOpCodesDictionary.emptyOpCode;
  }
}
