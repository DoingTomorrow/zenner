// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.OpCodesMap
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Reflection.Emit;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  public static class OpCodesMap
  {
    private static readonly Dictionary<Type, OpCode> LdindMap = new Dictionary<Type, OpCode>()
    {
      {
        typeof (bool),
        OpCodes.Ldind_I1
      },
      {
        typeof (sbyte),
        OpCodes.Ldind_I1
      },
      {
        typeof (byte),
        OpCodes.Ldind_U1
      },
      {
        typeof (char),
        OpCodes.Ldind_I2
      },
      {
        typeof (short),
        OpCodes.Ldind_I2
      },
      {
        typeof (int),
        OpCodes.Ldind_I4
      },
      {
        typeof (long),
        OpCodes.Ldind_I8
      },
      {
        typeof (ushort),
        OpCodes.Ldind_U2
      },
      {
        typeof (uint),
        OpCodes.Ldind_U4
      },
      {
        typeof (ulong),
        OpCodes.Ldind_I8
      },
      {
        typeof (float),
        OpCodes.Ldind_R4
      },
      {
        typeof (double),
        OpCodes.Ldind_R8
      }
    };
    private static readonly Dictionary<Type, OpCode> StindMap = new Dictionary<Type, OpCode>()
    {
      {
        typeof (bool),
        OpCodes.Stind_I1
      },
      {
        typeof (sbyte),
        OpCodes.Stind_I1
      },
      {
        typeof (byte),
        OpCodes.Stind_I1
      },
      {
        typeof (char),
        OpCodes.Stind_I2
      },
      {
        typeof (short),
        OpCodes.Stind_I2
      },
      {
        typeof (int),
        OpCodes.Stind_I4
      },
      {
        typeof (long),
        OpCodes.Stind_I8
      },
      {
        typeof (ushort),
        OpCodes.Stind_I2
      },
      {
        typeof (uint),
        OpCodes.Stind_I4
      },
      {
        typeof (ulong),
        OpCodes.Stind_I8
      },
      {
        typeof (float),
        OpCodes.Stind_R4
      },
      {
        typeof (double),
        OpCodes.Stind_R8
      }
    };

    public static bool TryGetLdindOpCode(Type valueType, out OpCode opCode)
    {
      return OpCodesMap.LdindMap.TryGetValue(valueType, out opCode);
    }

    public static bool TryGetStindOpCode(Type valueType, out OpCode opCode)
    {
      return OpCodesMap.StindMap.TryGetValue(valueType, out opCode);
    }
  }
}
