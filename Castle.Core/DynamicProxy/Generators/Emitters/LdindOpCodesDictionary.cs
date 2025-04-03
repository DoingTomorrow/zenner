// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.LdindOpCodesDictionary
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public sealed class LdindOpCodesDictionary : Dictionary<Type, OpCode>
  {
    private static readonly LdindOpCodesDictionary _dict = new LdindOpCodesDictionary();
    private static readonly OpCode emptyOpCode = new OpCode();

    private LdindOpCodesDictionary()
    {
      this.Add(typeof (bool), OpCodes.Ldind_I1);
      this.Add(typeof (char), OpCodes.Ldind_I2);
      this.Add(typeof (sbyte), OpCodes.Ldind_I1);
      this.Add(typeof (short), OpCodes.Ldind_I2);
      this.Add(typeof (int), OpCodes.Ldind_I4);
      this.Add(typeof (long), OpCodes.Ldind_I8);
      this.Add(typeof (float), OpCodes.Ldind_R4);
      this.Add(typeof (double), OpCodes.Ldind_R8);
      this.Add(typeof (byte), OpCodes.Ldind_U1);
      this.Add(typeof (ushort), OpCodes.Ldind_U2);
      this.Add(typeof (uint), OpCodes.Ldind_U4);
      this.Add(typeof (ulong), OpCodes.Ldind_I8);
    }

    public new OpCode this[Type type]
    {
      get => this.ContainsKey(type) ? base[type] : LdindOpCodesDictionary.EmptyOpCode;
    }

    public static LdindOpCodesDictionary Instance => LdindOpCodesDictionary._dict;

    public static OpCode EmptyOpCode => LdindOpCodesDictionary.emptyOpCode;
  }
}
