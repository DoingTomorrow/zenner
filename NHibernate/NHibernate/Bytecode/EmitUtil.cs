// Decompiled with JetBrains decompiler
// Type: NHibernate.Bytecode.EmitUtil
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace NHibernate.Bytecode
{
  public class EmitUtil
  {
    private static Dictionary<Type, OpCode> typeToOpcode = new Dictionary<Type, OpCode>(12);

    private EmitUtil()
    {
    }

    public static void EmitFastInt(ILGenerator il, int value)
    {
      switch (value)
      {
        case -1:
          il.Emit(OpCodes.Ldc_I4_M1);
          break;
        case 0:
          il.Emit(OpCodes.Ldc_I4_0);
          break;
        case 1:
          il.Emit(OpCodes.Ldc_I4_1);
          break;
        case 2:
          il.Emit(OpCodes.Ldc_I4_2);
          break;
        case 3:
          il.Emit(OpCodes.Ldc_I4_3);
          break;
        case 4:
          il.Emit(OpCodes.Ldc_I4_4);
          break;
        case 5:
          il.Emit(OpCodes.Ldc_I4_5);
          break;
        case 6:
          il.Emit(OpCodes.Ldc_I4_6);
          break;
        case 7:
          il.Emit(OpCodes.Ldc_I4_7);
          break;
        case 8:
          il.Emit(OpCodes.Ldc_I4_8);
          break;
        default:
          if (value > -129 && value < 128)
          {
            il.Emit(OpCodes.Ldc_I4_S, (sbyte) value);
            break;
          }
          il.Emit(OpCodes.Ldc_I4, value);
          break;
      }
    }

    public static void EmitBoxIfNeeded(ILGenerator il, Type type)
    {
      if (!type.IsValueType)
        return;
      il.Emit(OpCodes.Box, type);
    }

    static EmitUtil()
    {
      EmitUtil.typeToOpcode[typeof (bool)] = OpCodes.Ldind_I1;
      EmitUtil.typeToOpcode[typeof (sbyte)] = OpCodes.Ldind_I1;
      EmitUtil.typeToOpcode[typeof (byte)] = OpCodes.Ldind_U1;
      EmitUtil.typeToOpcode[typeof (char)] = OpCodes.Ldind_U2;
      EmitUtil.typeToOpcode[typeof (short)] = OpCodes.Ldind_I2;
      EmitUtil.typeToOpcode[typeof (ushort)] = OpCodes.Ldind_U2;
      EmitUtil.typeToOpcode[typeof (int)] = OpCodes.Ldind_I4;
      EmitUtil.typeToOpcode[typeof (uint)] = OpCodes.Ldind_U4;
      EmitUtil.typeToOpcode[typeof (long)] = OpCodes.Ldind_I8;
      EmitUtil.typeToOpcode[typeof (ulong)] = OpCodes.Ldind_I8;
      EmitUtil.typeToOpcode[typeof (float)] = OpCodes.Ldind_R4;
      EmitUtil.typeToOpcode[typeof (double)] = OpCodes.Ldind_R8;
    }

    public static void PreparePropertyForSet(ILGenerator il, Type propertyType)
    {
      if (propertyType.IsValueType)
      {
        Label label1 = il.DefineLabel();
        Label label2 = il.DefineLabel();
        LocalBuilder local = il.DeclareLocal(propertyType);
        il.Emit(OpCodes.Dup);
        il.Emit(OpCodes.Brtrue_S, label1);
        il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ldloca, local);
        il.Emit(OpCodes.Initobj, propertyType);
        il.Emit(OpCodes.Ldloc, local);
        il.Emit(OpCodes.Br_S, label2);
        il.MarkLabel(label1);
        il.Emit(OpCodes.Unbox, propertyType);
        OpCode opcode;
        if (EmitUtil.typeToOpcode.TryGetValue(propertyType, out opcode))
          il.Emit(opcode);
        else
          il.Emit(OpCodes.Ldobj, propertyType);
        il.MarkLabel(label2);
      }
      else
      {
        if (propertyType == typeof (object))
          return;
        il.Emit(OpCodes.Castclass, propertyType);
      }
    }

    public static Type DefineDelegateType(
      string fullTypeName,
      ModuleBuilder moduleBuilder,
      Type returnType,
      Type[] parameterTypes)
    {
      TypeBuilder typeBuilder = moduleBuilder.DefineType(fullTypeName, TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass, typeof (MulticastDelegate));
      typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[2]
      {
        typeof (object),
        typeof (IntPtr)
      }).SetImplementationFlags(MethodImplAttributes.CodeTypeMask);
      typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask, returnType, parameterTypes).SetImplementationFlags(MethodImplAttributes.CodeTypeMask);
      return typeBuilder.CreateType();
    }

    public static void EmitLoadType(ILGenerator il, Type type)
    {
      il.Emit(OpCodes.Ldtoken, type);
      il.Emit(OpCodes.Call, typeof (Type).GetMethod("GetTypeFromHandle"));
    }

    public static void EmitLoadMethodInfo(ILGenerator il, MethodInfo methodInfo)
    {
      il.Emit(OpCodes.Ldtoken, methodInfo);
      il.Emit(OpCodes.Call, typeof (MethodBase).GetMethod("GetMethodFromHandle", new Type[1]
      {
        typeof (RuntimeMethodHandle)
      }));
      il.Emit(OpCodes.Castclass, typeof (MethodInfo));
    }

    public static void EmitCreateDelegateInstance(
      ILGenerator il,
      Type delegateType,
      MethodInfo methodInfo)
    {
      MethodInfo method = typeof (Delegate).GetMethod("CreateDelegate", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, new Type[2]
      {
        typeof (Type),
        typeof (MethodInfo)
      }, (ParameterModifier[]) null);
      EmitUtil.EmitLoadType(il, delegateType);
      EmitUtil.EmitLoadMethodInfo(il, methodInfo);
      il.EmitCall(OpCodes.Call, method, (Type[]) null);
      il.Emit(OpCodes.Castclass, delegateType);
    }
  }
}
