// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Compiler.CompilerContext
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Meta;
using ProtoBuf.Serializers;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace ProtoBuf.Compiler
{
  internal sealed class CompilerContext
  {
    private readonly DynamicMethod method;
    private static int next;
    private readonly bool isStatic;
    private readonly RuntimeTypeModel.SerializerPair[] methodPairs;
    private readonly bool isWriter;
    private readonly bool nonPublic;
    private readonly Local inputValue;
    private readonly string assemblyName;
    private readonly ILGenerator il;
    private MutableList locals = new MutableList();
    private int nextLabel;
    private BasicList knownTrustedAssemblies;
    private BasicList knownUntrustedAssemblies;
    private readonly TypeModel model;
    private readonly CompilerContext.ILVersion metadataVersion;

    public TypeModel Model => this.model;

    internal CodeLabel DefineLabel() => new CodeLabel(this.il.DefineLabel(), this.nextLabel++);

    [Conditional("DEBUG_COMPILE")]
    private void TraceCompile(string value)
    {
    }

    internal void MarkLabel(CodeLabel label) => this.il.MarkLabel(label.Value);

    public static ProtoSerializer BuildSerializer(IProtoSerializer head, TypeModel model)
    {
      Type expectedType = head.ExpectedType;
      try
      {
        CompilerContext compilerContext = new CompilerContext(expectedType, true, true, model, typeof (object));
        compilerContext.LoadValue(compilerContext.InputValue);
        compilerContext.CastFromObject(expectedType);
        compilerContext.WriteNullCheckedTail(expectedType, head, (Local) null);
        compilerContext.Emit(OpCodes.Ret);
        return (ProtoSerializer) compilerContext.method.CreateDelegate(typeof (ProtoSerializer));
      }
      catch (Exception ex)
      {
        string str = expectedType.FullName;
        str = string.IsNullOrEmpty(str) ? expectedType.Name : throw new InvalidOperationException("It was not possible to prepare a serializer for: " + str, ex);
      }
    }

    public static ProtoDeserializer BuildDeserializer(IProtoSerializer head, TypeModel model)
    {
      Type expectedType = head.ExpectedType;
      CompilerContext ctx = new CompilerContext(expectedType, false, true, model, typeof (object));
      using (Local local = new Local(ctx, expectedType))
      {
        if (!Helpers.IsValueType(expectedType))
        {
          ctx.LoadValue(ctx.InputValue);
          ctx.CastFromObject(expectedType);
          ctx.StoreValue(local);
        }
        else
        {
          ctx.LoadValue(ctx.InputValue);
          CodeLabel label1 = ctx.DefineLabel();
          CodeLabel label2 = ctx.DefineLabel();
          ctx.BranchIfTrue(label1, true);
          ctx.LoadAddress(local, expectedType);
          ctx.EmitCtor(expectedType);
          ctx.Branch(label2, true);
          ctx.MarkLabel(label1);
          ctx.LoadValue(ctx.InputValue);
          ctx.CastFromObject(expectedType);
          ctx.StoreValue(local);
          ctx.MarkLabel(label2);
        }
        head.EmitRead(ctx, local);
        if (head.ReturnsValue)
          ctx.StoreValue(local);
        ctx.LoadValue(local);
        ctx.CastToObject(expectedType);
      }
      ctx.Emit(OpCodes.Ret);
      return (ProtoDeserializer) ctx.method.CreateDelegate(typeof (ProtoDeserializer));
    }

    internal void Return() => this.Emit(OpCodes.Ret);

    private static bool IsObject(Type type) => type == typeof (object);

    internal void CastToObject(Type type)
    {
      if (CompilerContext.IsObject(type))
        return;
      if (Helpers.IsValueType(type))
        this.il.Emit(OpCodes.Box, type);
      else
        this.il.Emit(OpCodes.Castclass, this.MapType(typeof (object)));
    }

    internal void CastFromObject(Type type)
    {
      if (CompilerContext.IsObject(type))
        return;
      if (Helpers.IsValueType(type))
      {
        if (this.MetadataVersion == CompilerContext.ILVersion.Net1)
        {
          this.il.Emit(OpCodes.Unbox, type);
          this.il.Emit(OpCodes.Ldobj, type);
        }
        else
          this.il.Emit(OpCodes.Unbox_Any, type);
      }
      else
        this.il.Emit(OpCodes.Castclass, type);
    }

    internal MethodBuilder GetDedicatedMethod(int metaKey, bool read)
    {
      if (this.methodPairs == null)
        return (MethodBuilder) null;
      for (int index = 0; index < this.methodPairs.Length; ++index)
      {
        if (this.methodPairs[index].MetaKey == metaKey)
          return !read ? this.methodPairs[index].Serialize : this.methodPairs[index].Deserialize;
      }
      throw new ArgumentException("Meta-key not found", nameof (metaKey));
    }

    internal int MapMetaKeyToCompiledKey(int metaKey)
    {
      if (metaKey < 0 || this.methodPairs == null)
        return metaKey;
      for (int compiledKey = 0; compiledKey < this.methodPairs.Length; ++compiledKey)
      {
        if (this.methodPairs[compiledKey].MetaKey == metaKey)
          return compiledKey;
      }
      throw new ArgumentException("Key could not be mapped: " + metaKey.ToString(), nameof (metaKey));
    }

    internal bool NonPublic => this.nonPublic;

    public Local InputValue => this.inputValue;

    internal CompilerContext(
      ILGenerator il,
      bool isStatic,
      bool isWriter,
      RuntimeTypeModel.SerializerPair[] methodPairs,
      TypeModel model,
      CompilerContext.ILVersion metadataVersion,
      string assemblyName,
      Type inputType,
      string traceName)
    {
      if (il == null)
        throw new ArgumentNullException(nameof (il));
      if (methodPairs == null)
        throw new ArgumentNullException(nameof (methodPairs));
      if (model == null)
        throw new ArgumentNullException(nameof (model));
      this.assemblyName = !Helpers.IsNullOrEmpty(assemblyName) ? assemblyName : throw new ArgumentNullException(nameof (assemblyName));
      this.isStatic = isStatic;
      this.methodPairs = methodPairs;
      this.il = il;
      this.isWriter = isWriter;
      this.model = model;
      this.metadataVersion = metadataVersion;
      if (!(inputType != (Type) null))
        return;
      this.inputValue = new Local((CompilerContext) null, inputType);
    }

    private CompilerContext(
      Type associatedType,
      bool isWriter,
      bool isStatic,
      TypeModel model,
      Type inputType)
    {
      if (model == null)
        throw new ArgumentNullException(nameof (model));
      this.metadataVersion = CompilerContext.ILVersion.Net2;
      this.isStatic = isStatic;
      this.isWriter = isWriter;
      this.model = model;
      this.nonPublic = true;
      Type returnType;
      Type[] parameterTypes;
      if (isWriter)
      {
        returnType = typeof (void);
        parameterTypes = new Type[2]
        {
          typeof (object),
          typeof (ProtoWriter)
        };
      }
      else
      {
        returnType = typeof (object);
        parameterTypes = new Type[2]
        {
          typeof (object),
          typeof (ProtoReader)
        };
      }
      this.method = new DynamicMethod("proto_" + Interlocked.Increment(ref CompilerContext.next).ToString(), returnType, parameterTypes, associatedType.IsInterface ? typeof (object) : associatedType, true);
      this.il = this.method.GetILGenerator();
      if (!(inputType != (Type) null))
        return;
      this.inputValue = new Local((CompilerContext) null, inputType);
    }

    private void Emit(OpCode opcode) => this.il.Emit(opcode);

    public void LoadValue(string value)
    {
      if (value == null)
        this.LoadNullRef();
      else
        this.il.Emit(OpCodes.Ldstr, value);
    }

    public void LoadValue(float value) => this.il.Emit(OpCodes.Ldc_R4, value);

    public void LoadValue(double value) => this.il.Emit(OpCodes.Ldc_R8, value);

    public void LoadValue(long value) => this.il.Emit(OpCodes.Ldc_I8, value);

    public void LoadValue(int value)
    {
      switch (value)
      {
        case -1:
          this.Emit(OpCodes.Ldc_I4_M1);
          break;
        case 0:
          this.Emit(OpCodes.Ldc_I4_0);
          break;
        case 1:
          this.Emit(OpCodes.Ldc_I4_1);
          break;
        case 2:
          this.Emit(OpCodes.Ldc_I4_2);
          break;
        case 3:
          this.Emit(OpCodes.Ldc_I4_3);
          break;
        case 4:
          this.Emit(OpCodes.Ldc_I4_4);
          break;
        case 5:
          this.Emit(OpCodes.Ldc_I4_5);
          break;
        case 6:
          this.Emit(OpCodes.Ldc_I4_6);
          break;
        case 7:
          this.Emit(OpCodes.Ldc_I4_7);
          break;
        case 8:
          this.Emit(OpCodes.Ldc_I4_8);
          break;
        default:
          if (value >= (int) sbyte.MinValue && value <= (int) sbyte.MaxValue)
          {
            this.il.Emit(OpCodes.Ldc_I4_S, (sbyte) value);
            break;
          }
          this.il.Emit(OpCodes.Ldc_I4, value);
          break;
      }
    }

    internal LocalBuilder GetFromPool(Type type)
    {
      int count = this.locals.Count;
      for (int index = 0; index < count; ++index)
      {
        LocalBuilder local = (LocalBuilder) this.locals[index];
        if (local != null && local.LocalType == type)
        {
          this.locals[index] = (object) null;
          return local;
        }
      }
      return this.il.DeclareLocal(type);
    }

    internal void ReleaseToPool(LocalBuilder value)
    {
      int count = this.locals.Count;
      for (int index = 0; index < count; ++index)
      {
        if (this.locals[index] == null)
        {
          this.locals[index] = (object) value;
          return;
        }
      }
      this.locals.Add((object) value);
    }

    public void LoadReaderWriter() => this.Emit(this.isStatic ? OpCodes.Ldarg_1 : OpCodes.Ldarg_2);

    public void StoreValue(Local local)
    {
      if (local == this.InputValue)
      {
        byte num = this.isStatic ? (byte) 0 : (byte) 1;
        this.il.Emit(OpCodes.Starg_S, num);
      }
      else
      {
        switch (local.Value.LocalIndex)
        {
          case 0:
            this.Emit(OpCodes.Stloc_0);
            break;
          case 1:
            this.Emit(OpCodes.Stloc_1);
            break;
          case 2:
            this.Emit(OpCodes.Stloc_2);
            break;
          case 3:
            this.Emit(OpCodes.Stloc_3);
            break;
          default:
            this.il.Emit(this.UseShortForm(local) ? OpCodes.Stloc_S : OpCodes.Stloc, local.Value);
            break;
        }
      }
    }

    public void LoadValue(Local local)
    {
      if (local == null)
        return;
      if (local == this.InputValue)
      {
        this.Emit(this.isStatic ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1);
      }
      else
      {
        switch (local.Value.LocalIndex)
        {
          case 0:
            this.Emit(OpCodes.Ldloc_0);
            break;
          case 1:
            this.Emit(OpCodes.Ldloc_1);
            break;
          case 2:
            this.Emit(OpCodes.Ldloc_2);
            break;
          case 3:
            this.Emit(OpCodes.Ldloc_3);
            break;
          default:
            this.il.Emit(this.UseShortForm(local) ? OpCodes.Ldloc_S : OpCodes.Ldloc, local.Value);
            break;
        }
      }
    }

    public Local GetLocalWithValue(Type type, Local fromValue)
    {
      if (fromValue != null)
      {
        if (fromValue.Type == type)
          return fromValue.AsCopy();
        this.LoadValue(fromValue);
        if (!Helpers.IsValueType(type) && (fromValue.Type == (Type) null || !type.IsAssignableFrom(fromValue.Type)))
          this.Cast(type);
      }
      Local local = new Local(this, type);
      this.StoreValue(local);
      return local;
    }

    internal void EmitBasicRead(string methodName, Type expectedType)
    {
      MethodInfo method = this.MapType(typeof (ProtoReader)).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (method == (MethodInfo) null || method.ReturnType != expectedType || method.GetParameters().Length != 0)
        throw new ArgumentException(nameof (methodName));
      this.LoadReaderWriter();
      this.EmitCall(method);
    }

    internal void EmitBasicRead(Type helperType, string methodName, Type expectedType)
    {
      MethodInfo method = helperType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      if (method == (MethodInfo) null || method.ReturnType != expectedType || method.GetParameters().Length != 1)
        throw new ArgumentException(nameof (methodName));
      this.LoadReaderWriter();
      this.EmitCall(method);
    }

    internal void EmitBasicWrite(string methodName, Local fromValue)
    {
      if (Helpers.IsNullOrEmpty(methodName))
        throw new ArgumentNullException(nameof (methodName));
      this.LoadValue(fromValue);
      this.LoadReaderWriter();
      this.EmitCall(this.GetWriterMethod(methodName));
    }

    private MethodInfo GetWriterMethod(string methodName)
    {
      Type type = this.MapType(typeof (ProtoWriter));
      foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!(method.Name != methodName))
        {
          ParameterInfo[] parameters = method.GetParameters();
          if (parameters.Length == 2 && parameters[1].ParameterType == type)
            return method;
        }
      }
      throw new ArgumentException("No suitable method found for: " + methodName, nameof (methodName));
    }

    internal void EmitWrite(Type helperType, string methodName, Local valueFrom)
    {
      MethodInfo method = !Helpers.IsNullOrEmpty(methodName) ? helperType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) : throw new ArgumentNullException(nameof (methodName));
      if (method == (MethodInfo) null || method.ReturnType != this.MapType(typeof (void)))
        throw new ArgumentException(nameof (methodName));
      this.LoadValue(valueFrom);
      this.LoadReaderWriter();
      this.EmitCall(method);
    }

    public void EmitCall(MethodInfo method) => this.EmitCall(method, (Type) null);

    public void EmitCall(MethodInfo method, Type targetType)
    {
      this.CheckAccessibility((MemberInfo) method);
      OpCode opcode;
      if (method.IsStatic || Helpers.IsValueType(method.DeclaringType))
      {
        opcode = OpCodes.Call;
      }
      else
      {
        opcode = OpCodes.Callvirt;
        if (targetType != (Type) null && Helpers.IsValueType(targetType) && !Helpers.IsValueType(method.DeclaringType))
          this.Constrain(targetType);
      }
      this.il.EmitCall(opcode, method, (Type[]) null);
    }

    public void LoadNullRef() => this.Emit(OpCodes.Ldnull);

    internal void WriteNullCheckedTail(Type type, IProtoSerializer tail, Local valueFrom)
    {
      if (Helpers.IsValueType(type))
      {
        if (Helpers.GetUnderlyingType(type) == (Type) null)
        {
          tail.EmitWrite(this, valueFrom);
        }
        else
        {
          using (Local localWithValue = this.GetLocalWithValue(type, valueFrom))
          {
            this.LoadAddress(localWithValue, type);
            this.LoadValue(type.GetProperty("HasValue"));
            CodeLabel label = this.DefineLabel();
            this.BranchIfFalse(label, false);
            this.LoadAddress(localWithValue, type);
            this.EmitCall(type.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
            tail.EmitWrite(this, (Local) null);
            this.MarkLabel(label);
          }
        }
      }
      else
      {
        this.LoadValue(valueFrom);
        this.CopyValue();
        CodeLabel label1 = this.DefineLabel();
        CodeLabel label2 = this.DefineLabel();
        this.BranchIfTrue(label1, true);
        this.DiscardValue();
        this.Branch(label2, false);
        this.MarkLabel(label1);
        tail.EmitWrite(this, (Local) null);
        this.MarkLabel(label2);
      }
    }

    internal void ReadNullCheckedTail(Type type, IProtoSerializer tail, Local valueFrom)
    {
      Type underlyingType;
      if (Helpers.IsValueType(type) && (underlyingType = Helpers.GetUnderlyingType(type)) != (Type) null)
      {
        if (tail.RequiresOldValue)
        {
          using (Local localWithValue = this.GetLocalWithValue(type, valueFrom))
          {
            this.LoadAddress(localWithValue, type);
            this.EmitCall(type.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
          }
        }
        tail.EmitRead(this, (Local) null);
        if (!tail.ReturnsValue)
          return;
        this.EmitCtor(type, underlyingType);
      }
      else
        tail.EmitRead(this, valueFrom);
    }

    public void EmitCtor(Type type) => this.EmitCtor(type, Helpers.EmptyTypes);

    public void EmitCtor(ConstructorInfo ctor)
    {
      if (ctor == (ConstructorInfo) null)
        throw new ArgumentNullException(nameof (ctor));
      this.CheckAccessibility((MemberInfo) ctor);
      this.il.Emit(OpCodes.Newobj, ctor);
    }

    public void EmitCtor(Type type, params Type[] parameterTypes)
    {
      if (Helpers.IsValueType(type) && parameterTypes.Length == 0)
      {
        this.il.Emit(OpCodes.Initobj, type);
      }
      else
      {
        ConstructorInfo constructor = Helpers.GetConstructor(type, parameterTypes, true);
        if (constructor == (ConstructorInfo) null)
          throw new InvalidOperationException("No suitable constructor found for " + type.FullName);
        this.EmitCtor(constructor);
      }
    }

    private bool InternalsVisible(Assembly assembly)
    {
      if (Helpers.IsNullOrEmpty(this.assemblyName))
        return false;
      if (this.knownTrustedAssemblies != null && this.knownTrustedAssemblies.IndexOfReference((object) assembly) >= 0)
        return true;
      if (this.knownUntrustedAssemblies != null && this.knownUntrustedAssemblies.IndexOfReference((object) assembly) >= 0)
        return false;
      bool flag = false;
      Type attributeType = this.MapType(typeof (InternalsVisibleToAttribute));
      if (attributeType == (Type) null)
        return false;
      foreach (InternalsVisibleToAttribute customAttribute in assembly.GetCustomAttributes(attributeType, false))
      {
        if (customAttribute.AssemblyName == this.assemblyName || customAttribute.AssemblyName.StartsWith(this.assemblyName + ","))
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        if (this.knownTrustedAssemblies == null)
          this.knownTrustedAssemblies = new BasicList();
        this.knownTrustedAssemblies.Add((object) assembly);
      }
      else
      {
        if (this.knownUntrustedAssemblies == null)
          this.knownUntrustedAssemblies = new BasicList();
        this.knownUntrustedAssemblies.Add((object) assembly);
      }
      return flag;
    }

    internal void CheckAccessibility(MemberInfo member)
    {
      if (member == (MemberInfo) null)
        throw new ArgumentNullException(nameof (member));
      if (this.NonPublic)
        return;
      MemberTypes memberType = member.MemberType;
      bool flag;
      switch (memberType)
      {
        case MemberTypes.Constructor:
          ConstructorInfo constructorInfo = (ConstructorInfo) member;
          flag = constructorInfo.IsPublic || (constructorInfo.IsAssembly || constructorInfo.IsFamilyOrAssembly) && this.InternalsVisible(constructorInfo.DeclaringType.Assembly);
          break;
        case MemberTypes.Field:
          FieldInfo fieldInfo = (FieldInfo) member;
          flag = fieldInfo.IsPublic || (fieldInfo.IsAssembly || fieldInfo.IsFamilyOrAssembly) && this.InternalsVisible(fieldInfo.DeclaringType.Assembly);
          break;
        case MemberTypes.Method:
          MethodInfo methodInfo = (MethodInfo) member;
          flag = methodInfo.IsPublic || (methodInfo.IsAssembly || methodInfo.IsFamilyOrAssembly) && this.InternalsVisible(methodInfo.DeclaringType.Assembly);
          if (!flag && (member is MethodBuilder || member.DeclaringType == this.MapType(typeof (TypeModel))))
          {
            flag = true;
            break;
          }
          break;
        case MemberTypes.Property:
          flag = true;
          break;
        case MemberTypes.TypeInfo:
          Type type1 = (Type) member;
          flag = type1.IsPublic || this.InternalsVisible(type1.Assembly);
          break;
        case MemberTypes.NestedType:
          Type type2 = (Type) member;
          do
          {
            flag = type2.IsNestedPublic || type2.IsPublic || (type2.DeclaringType == (Type) null || type2.IsNestedAssembly || type2.IsNestedFamORAssem) && this.InternalsVisible(type2.Assembly);
          }
          while (flag && (type2 = type2.DeclaringType) != (Type) null);
          break;
        default:
          throw new NotSupportedException(memberType.ToString());
      }
      if (flag)
        return;
      if (memberType == MemberTypes.TypeInfo || memberType == MemberTypes.NestedType)
        throw new InvalidOperationException("Non-public type cannot be used with full dll compilation: " + ((Type) member).FullName);
      throw new InvalidOperationException("Non-public member cannot be used with full dll compilation: " + member.DeclaringType.FullName + "." + member.Name);
    }

    public void LoadValue(FieldInfo field)
    {
      this.CheckAccessibility((MemberInfo) field);
      this.il.Emit(field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, field);
    }

    public void StoreValue(FieldInfo field)
    {
      this.CheckAccessibility((MemberInfo) field);
      this.il.Emit(field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field);
    }

    public void LoadValue(PropertyInfo property)
    {
      this.CheckAccessibility((MemberInfo) property);
      this.EmitCall(Helpers.GetGetMethod(property, true, true));
    }

    public void StoreValue(PropertyInfo property)
    {
      this.CheckAccessibility((MemberInfo) property);
      this.EmitCall(Helpers.GetSetMethod(property, true, true));
    }

    internal static void LoadValue(ILGenerator il, int value)
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
          il.Emit(OpCodes.Ldc_I4, value);
          break;
      }
    }

    private bool UseShortForm(Local local) => local.Value.LocalIndex < 256;

    internal void LoadAddress(Local local, Type type)
    {
      if (Helpers.IsValueType(type))
      {
        if (local == null)
          throw new InvalidOperationException("Cannot load the address of a struct at the head of the stack");
        if (local == this.InputValue)
          this.il.Emit(OpCodes.Ldarga_S, this.isStatic ? (byte) 0 : (byte) 1);
        else
          this.il.Emit(this.UseShortForm(local) ? OpCodes.Ldloca_S : OpCodes.Ldloca, local.Value);
      }
      else
        this.LoadValue(local);
    }

    internal void Branch(CodeLabel label, bool @short)
    {
      this.il.Emit(@short ? OpCodes.Br_S : OpCodes.Br, label.Value);
    }

    internal void BranchIfFalse(CodeLabel label, bool @short)
    {
      this.il.Emit(@short ? OpCodes.Brfalse_S : OpCodes.Brfalse, label.Value);
    }

    internal void BranchIfTrue(CodeLabel label, bool @short)
    {
      this.il.Emit(@short ? OpCodes.Brtrue_S : OpCodes.Brtrue, label.Value);
    }

    internal void BranchIfEqual(CodeLabel label, bool @short)
    {
      this.il.Emit(@short ? OpCodes.Beq_S : OpCodes.Beq, label.Value);
    }

    internal void CopyValue() => this.Emit(OpCodes.Dup);

    internal void BranchIfGreater(CodeLabel label, bool @short)
    {
      this.il.Emit(@short ? OpCodes.Bgt_S : OpCodes.Bgt, label.Value);
    }

    internal void BranchIfLess(CodeLabel label, bool @short)
    {
      this.il.Emit(@short ? OpCodes.Blt_S : OpCodes.Blt, label.Value);
    }

    internal void DiscardValue() => this.Emit(OpCodes.Pop);

    public void Subtract() => this.Emit(OpCodes.Sub);

    public void Switch(CodeLabel[] jumpTable)
    {
      if (jumpTable.Length <= 128)
      {
        Label[] labels = new Label[jumpTable.Length];
        for (int index = 0; index < labels.Length; ++index)
          labels[index] = jumpTable[index].Value;
        this.il.Emit(OpCodes.Switch, labels);
      }
      else
      {
        using (Local localWithValue = this.GetLocalWithValue(this.MapType(typeof (int)), (Local) null))
        {
          int length1 = jumpTable.Length;
          int num1 = 0;
          int length2 = length1 / 128;
          if (length1 % 128 != 0)
            ++length2;
          Label[] labels1 = new Label[length2];
          for (int index = 0; index < length2; ++index)
            labels1[index] = this.il.DefineLabel();
          CodeLabel label = this.DefineLabel();
          this.LoadValue(localWithValue);
          this.LoadValue(128);
          this.Emit(OpCodes.Div);
          this.il.Emit(OpCodes.Switch, labels1);
          this.Branch(label, false);
          Label[] labels2 = new Label[128];
          for (int index1 = 0; index1 < length2; ++index1)
          {
            this.il.MarkLabel(labels1[index1]);
            int length3 = Math.Min(128, length1);
            length1 -= length3;
            if (labels2.Length != length3)
              labels2 = new Label[length3];
            int num2 = num1;
            for (int index2 = 0; index2 < length3; ++index2)
              labels2[index2] = jumpTable[num1++].Value;
            this.LoadValue(localWithValue);
            if (num2 != 0)
            {
              this.LoadValue(num2);
              this.Emit(OpCodes.Sub);
            }
            this.il.Emit(OpCodes.Switch, labels2);
            if (length1 != 0)
              this.Branch(label, false);
          }
          this.MarkLabel(label);
        }
      }
    }

    internal void EndFinally() => this.il.EndExceptionBlock();

    internal void BeginFinally() => this.il.BeginFinallyBlock();

    internal void EndTry(CodeLabel label, bool @short)
    {
      this.il.Emit(@short ? OpCodes.Leave_S : OpCodes.Leave, label.Value);
    }

    internal CodeLabel BeginTry() => new CodeLabel(this.il.BeginExceptionBlock(), this.nextLabel++);

    internal void Constrain(Type type) => this.il.Emit(OpCodes.Constrained, type);

    internal void TryCast(Type type) => this.il.Emit(OpCodes.Isinst, type);

    internal void Cast(Type type) => this.il.Emit(OpCodes.Castclass, type);

    public IDisposable Using(Local local)
    {
      return (IDisposable) new CompilerContext.UsingBlock(this, local);
    }

    internal void Add() => this.Emit(OpCodes.Add);

    internal void LoadLength(Local arr, bool zeroIfNull)
    {
      if (zeroIfNull)
      {
        CodeLabel label1 = this.DefineLabel();
        CodeLabel label2 = this.DefineLabel();
        this.LoadValue(arr);
        this.CopyValue();
        this.BranchIfTrue(label1, true);
        this.DiscardValue();
        this.LoadValue(0);
        this.Branch(label2, true);
        this.MarkLabel(label1);
        this.Emit(OpCodes.Ldlen);
        this.Emit(OpCodes.Conv_I4);
        this.MarkLabel(label2);
      }
      else
      {
        this.LoadValue(arr);
        this.Emit(OpCodes.Ldlen);
        this.Emit(OpCodes.Conv_I4);
      }
    }

    internal void CreateArray(Type elementType, Local length)
    {
      this.LoadValue(length);
      this.il.Emit(OpCodes.Newarr, elementType);
    }

    internal void LoadArrayValue(Local arr, Local i)
    {
      Type elementType = arr.Type.GetElementType();
      this.LoadValue(arr);
      this.LoadValue(i);
      switch (Helpers.GetTypeCode(elementType))
      {
        case ProtoTypeCode.SByte:
          this.Emit(OpCodes.Ldelem_I1);
          break;
        case ProtoTypeCode.Byte:
          this.Emit(OpCodes.Ldelem_U1);
          break;
        case ProtoTypeCode.Int16:
          this.Emit(OpCodes.Ldelem_I2);
          break;
        case ProtoTypeCode.UInt16:
          this.Emit(OpCodes.Ldelem_U2);
          break;
        case ProtoTypeCode.Int32:
          this.Emit(OpCodes.Ldelem_I4);
          break;
        case ProtoTypeCode.UInt32:
          this.Emit(OpCodes.Ldelem_U4);
          break;
        case ProtoTypeCode.Int64:
          this.Emit(OpCodes.Ldelem_I8);
          break;
        case ProtoTypeCode.UInt64:
          this.Emit(OpCodes.Ldelem_I8);
          break;
        case ProtoTypeCode.Single:
          this.Emit(OpCodes.Ldelem_R4);
          break;
        case ProtoTypeCode.Double:
          this.Emit(OpCodes.Ldelem_R8);
          break;
        default:
          if (Helpers.IsValueType(elementType))
          {
            this.il.Emit(OpCodes.Ldelema, elementType);
            this.il.Emit(OpCodes.Ldobj, elementType);
            break;
          }
          this.Emit(OpCodes.Ldelem_Ref);
          break;
      }
    }

    internal void LoadValue(Type type)
    {
      this.il.Emit(OpCodes.Ldtoken, type);
      this.EmitCall(this.MapType(typeof (Type)).GetMethod("GetTypeFromHandle"));
    }

    internal void ConvertToInt32(ProtoTypeCode typeCode, bool uint32Overflow)
    {
      switch (typeCode)
      {
        case ProtoTypeCode.SByte:
        case ProtoTypeCode.Byte:
        case ProtoTypeCode.Int16:
        case ProtoTypeCode.UInt16:
          this.Emit(OpCodes.Conv_I4);
          break;
        case ProtoTypeCode.Int32:
          break;
        case ProtoTypeCode.UInt32:
          this.Emit(uint32Overflow ? OpCodes.Conv_Ovf_I4_Un : OpCodes.Conv_Ovf_I4);
          break;
        case ProtoTypeCode.Int64:
          this.Emit(OpCodes.Conv_Ovf_I4);
          break;
        case ProtoTypeCode.UInt64:
          this.Emit(OpCodes.Conv_Ovf_I4_Un);
          break;
        default:
          throw new InvalidOperationException("ConvertToInt32 not implemented for: " + typeCode.ToString());
      }
    }

    internal void ConvertFromInt32(ProtoTypeCode typeCode, bool uint32Overflow)
    {
      switch (typeCode)
      {
        case ProtoTypeCode.SByte:
          this.Emit(OpCodes.Conv_Ovf_I1);
          break;
        case ProtoTypeCode.Byte:
          this.Emit(OpCodes.Conv_Ovf_U1);
          break;
        case ProtoTypeCode.Int16:
          this.Emit(OpCodes.Conv_Ovf_I2);
          break;
        case ProtoTypeCode.UInt16:
          this.Emit(OpCodes.Conv_Ovf_U2);
          break;
        case ProtoTypeCode.Int32:
          break;
        case ProtoTypeCode.UInt32:
          this.Emit(uint32Overflow ? OpCodes.Conv_Ovf_U4 : OpCodes.Conv_U4);
          break;
        case ProtoTypeCode.Int64:
          this.Emit(OpCodes.Conv_I8);
          break;
        case ProtoTypeCode.UInt64:
          this.Emit(OpCodes.Conv_U8);
          break;
        default:
          throw new InvalidOperationException();
      }
    }

    internal void LoadValue(Decimal value)
    {
      if (value == 0M)
      {
        this.LoadValue(typeof (Decimal).GetField("Zero"));
      }
      else
      {
        int[] bits = Decimal.GetBits(value);
        this.LoadValue(bits[0]);
        this.LoadValue(bits[1]);
        this.LoadValue(bits[2]);
        this.LoadValue(bits[3] >>> 31);
        this.LoadValue(bits[3] >> 16 & (int) byte.MaxValue);
        this.EmitCtor(this.MapType(typeof (Decimal)), this.MapType(typeof (int)), this.MapType(typeof (int)), this.MapType(typeof (int)), this.MapType(typeof (bool)), this.MapType(typeof (byte)));
      }
    }

    internal void LoadValue(Guid value)
    {
      if (value == Guid.Empty)
      {
        this.LoadValue(typeof (Guid).GetField("Empty"));
      }
      else
      {
        byte[] byteArray = value.ToByteArray();
        this.LoadValue((int) byteArray[0] | (int) byteArray[1] << 8 | (int) byteArray[2] << 16 | (int) byteArray[3] << 24);
        this.LoadValue((int) (short) ((int) byteArray[4] | (int) byteArray[5] << 8));
        this.LoadValue((int) (short) ((int) byteArray[6] | (int) byteArray[7] << 8));
        for (int index = 8; index <= 15; ++index)
          this.LoadValue((int) byteArray[index]);
        this.EmitCtor(this.MapType(typeof (Guid)), this.MapType(typeof (int)), this.MapType(typeof (short)), this.MapType(typeof (short)), this.MapType(typeof (byte)), this.MapType(typeof (byte)), this.MapType(typeof (byte)), this.MapType(typeof (byte)), this.MapType(typeof (byte)), this.MapType(typeof (byte)), this.MapType(typeof (byte)), this.MapType(typeof (byte)));
      }
    }

    internal void LoadSerializationContext()
    {
      this.LoadReaderWriter();
      this.LoadValue((this.isWriter ? typeof (ProtoWriter) : typeof (ProtoReader)).GetProperty("Context"));
    }

    internal Type MapType(Type type) => this.model.MapType(type);

    public CompilerContext.ILVersion MetadataVersion => this.metadataVersion;

    internal bool AllowInternal(PropertyInfo property)
    {
      return this.NonPublic || this.InternalsVisible(Helpers.GetAssembly(property.DeclaringType));
    }

    private sealed class UsingBlock : IDisposable
    {
      private Local local;
      private CompilerContext ctx;
      private CodeLabel label;

      public UsingBlock(CompilerContext ctx, Local local)
      {
        if (ctx == null)
          throw new ArgumentNullException(nameof (ctx));
        Type type = local != null ? local.Type : throw new ArgumentNullException(nameof (local));
        if ((Helpers.IsValueType(type) || Helpers.IsSealed(type)) && !ctx.MapType(typeof (IDisposable)).IsAssignableFrom(type))
          return;
        this.local = local;
        this.ctx = ctx;
        this.label = ctx.BeginTry();
      }

      public void Dispose()
      {
        if (this.local == null || this.ctx == null)
          return;
        this.ctx.EndTry(this.label, false);
        this.ctx.BeginFinally();
        Type type1 = this.ctx.MapType(typeof (IDisposable));
        MethodInfo method = type1.GetMethod(nameof (Dispose));
        Type type2 = this.local.Type;
        if (Helpers.IsValueType(type2))
        {
          this.ctx.LoadAddress(this.local, type2);
          if (this.ctx.MetadataVersion == CompilerContext.ILVersion.Net1)
          {
            this.ctx.LoadValue(this.local);
            this.ctx.CastToObject(type2);
          }
          else
            this.ctx.Constrain(type2);
          this.ctx.EmitCall(method);
        }
        else
        {
          CodeLabel label = this.ctx.DefineLabel();
          if (type1.IsAssignableFrom(type2))
          {
            this.ctx.LoadValue(this.local);
            this.ctx.BranchIfFalse(label, true);
            this.ctx.LoadAddress(this.local, type2);
          }
          else
          {
            using (Local local = new Local(this.ctx, type1))
            {
              this.ctx.LoadValue(this.local);
              this.ctx.TryCast(type1);
              this.ctx.CopyValue();
              this.ctx.StoreValue(local);
              this.ctx.BranchIfFalse(label, true);
              this.ctx.LoadAddress(local, type1);
            }
          }
          this.ctx.EmitCall(method);
          this.ctx.MarkLabel(label);
        }
        this.ctx.EndFinally();
        this.local = (Local) null;
        this.ctx = (CompilerContext) null;
        this.label = new CodeLabel();
      }
    }

    public enum ILVersion
    {
      Net1,
      Net2,
    }
  }
}
