// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.ProxyGenerator
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

#nullable disable
namespace AutoMapper.Impl
{
  public class ProxyGenerator : IProxyGenerator
  {
    private static readonly byte[] privateKey = ProxyGenerator.StringToByteArray("002400000480000094000000060200000024000052534131000400000100010079dfef85ed6ba841717e154f13182c0a6029a40794a6ecd2886c7dc38825f6a4c05b0622723a01cd080f9879126708eef58f134accdc99627947425960ac2397162067507e3c627992aa6b92656ad3380999b30b5d5645ba46cc3fcc6a1de5de7afebcf896c65fb4f9547a6c0c6433045fceccb1fa15e960d519d0cd694b29a4");
    private static readonly byte[] privateKeyToken = ProxyGenerator.StringToByteArray("be96cd2c38ef1005");
    private static readonly MethodInfo delegate_Combine = typeof (Delegate).GetMethod("Combine", BindingFlags.Static | BindingFlags.Public, (Binder) null, new Type[2]
    {
      typeof (Delegate),
      typeof (Delegate)
    }, (ParameterModifier[]) null);
    private static readonly MethodInfo delegate_Remove = typeof (Delegate).GetMethod("Remove", BindingFlags.Static | BindingFlags.Public, (Binder) null, new Type[2]
    {
      typeof (Delegate),
      typeof (Delegate)
    }, (ParameterModifier[]) null);
    private static readonly EventInfo iNotifyPropertyChanged_PropertyChanged = typeof (INotifyPropertyChanged).GetEvent("PropertyChanged", BindingFlags.Instance | BindingFlags.Public);
    private static readonly ConstructorInfo proxyBase_ctor = typeof (ProxyBase).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null);
    private static readonly ModuleBuilder proxyModule = ProxyGenerator.CreateProxyModule();
    private static readonly Dictionary<Type, Type> proxyTypes = new Dictionary<Type, Type>();

    private static ModuleBuilder CreateProxyModule()
    {
      AssemblyName name = new AssemblyName("AutoMapper.Proxies");
      name.SetPublicKey(ProxyGenerator.privateKey);
      name.SetPublicKeyToken(ProxyGenerator.privateKeyToken);
      return AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run).DefineDynamicModule("AutoMapper.Proxies.emit");
    }

    private static Type CreateProxyType(Type interfaceType)
    {
      if (!interfaceType.IsInterface)
        throw new ArgumentException("Only interfaces can be proxied", nameof (interfaceType));
      string name = string.Format("Proxy<{0}>", (object) Regex.Replace(interfaceType.AssemblyQualifiedName ?? interfaceType.FullName ?? interfaceType.Name, "[\\s,]+", "_"));
      List<Type> source = new List<Type>() { interfaceType };
      source.AddRange((IEnumerable<Type>) interfaceType.GetInterfaces());
      TypeBuilder owner = ProxyGenerator.proxyModule.DefineType(name, TypeAttributes.Public | TypeAttributes.Sealed, typeof (ProxyBase), source.ToArray());
      ILGenerator ilGenerator1 = owner.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes).GetILGenerator();
      ilGenerator1.Emit(OpCodes.Ldarg_0);
      ilGenerator1.Emit(OpCodes.Call, ProxyGenerator.proxyBase_ctor);
      ilGenerator1.Emit(OpCodes.Ret);
      FieldBuilder fieldBuilder = (FieldBuilder) null;
      if (typeof (INotifyPropertyChanged).IsAssignableFrom(interfaceType))
      {
        fieldBuilder = owner.DefineField("PropertyChanged", typeof (PropertyChangedEventHandler), FieldAttributes.Private);
        MethodBuilder methodInfoBody1 = owner.DefineMethod("add_PropertyChanged", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask | MethodAttributes.SpecialName, typeof (void), new Type[1]
        {
          typeof (PropertyChangedEventHandler)
        });
        ILGenerator ilGenerator2 = methodInfoBody1.GetILGenerator();
        ilGenerator2.Emit(OpCodes.Ldarg_0);
        ilGenerator2.Emit(OpCodes.Dup);
        ilGenerator2.Emit(OpCodes.Ldfld, (FieldInfo) fieldBuilder);
        ilGenerator2.Emit(OpCodes.Ldarg_1);
        ilGenerator2.Emit(OpCodes.Call, ProxyGenerator.delegate_Combine);
        ilGenerator2.Emit(OpCodes.Castclass, typeof (PropertyChangedEventHandler));
        ilGenerator2.Emit(OpCodes.Stfld, (FieldInfo) fieldBuilder);
        ilGenerator2.Emit(OpCodes.Ret);
        MethodBuilder methodInfoBody2 = owner.DefineMethod("remove_PropertyChanged", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask | MethodAttributes.SpecialName, typeof (void), new Type[1]
        {
          typeof (PropertyChangedEventHandler)
        });
        ILGenerator ilGenerator3 = methodInfoBody2.GetILGenerator();
        ilGenerator3.Emit(OpCodes.Ldarg_0);
        ilGenerator3.Emit(OpCodes.Dup);
        ilGenerator3.Emit(OpCodes.Ldfld, (FieldInfo) fieldBuilder);
        ilGenerator3.Emit(OpCodes.Ldarg_1);
        ilGenerator3.Emit(OpCodes.Call, ProxyGenerator.delegate_Remove);
        ilGenerator3.Emit(OpCodes.Castclass, typeof (PropertyChangedEventHandler));
        ilGenerator3.Emit(OpCodes.Stfld, (FieldInfo) fieldBuilder);
        ilGenerator3.Emit(OpCodes.Ret);
        owner.DefineMethodOverride((MethodInfo) methodInfoBody1, ProxyGenerator.iNotifyPropertyChanged_PropertyChanged.GetAddMethod());
        owner.DefineMethodOverride((MethodInfo) methodInfoBody2, ProxyGenerator.iNotifyPropertyChanged_PropertyChanged.GetRemoveMethod());
      }
      List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();
      foreach (PropertyInfo propertyInfo in source.Where<Type>((Func<Type, bool>) (intf => intf != typeof (INotifyPropertyChanged))).SelectMany<Type, PropertyInfo>((Func<Type, IEnumerable<PropertyInfo>>) (intf => (IEnumerable<PropertyInfo>) intf.GetProperties())))
      {
        if (propertyInfo.CanWrite)
          propertyInfoList.Insert(0, propertyInfo);
        else
          propertyInfoList.Add(propertyInfo);
      }
      Dictionary<string, PropertyEmitter> dictionary = new Dictionary<string, PropertyEmitter>();
      foreach (PropertyInfo propertyInfo in propertyInfoList)
      {
        PropertyEmitter propertyEmitter;
        if (dictionary.TryGetValue(propertyInfo.Name, out propertyEmitter))
        {
          if (propertyEmitter.PropertyType != propertyInfo.PropertyType && (propertyInfo.CanWrite || !propertyInfo.PropertyType.IsAssignableFrom(propertyEmitter.PropertyType)))
            throw new ArgumentException(string.Format("The interface has a conflicting property {0}", (object) propertyInfo.Name), nameof (interfaceType));
        }
        else
          dictionary.Add(propertyInfo.Name, propertyEmitter = new PropertyEmitter(owner, propertyInfo.Name, propertyInfo.PropertyType, fieldBuilder));
        if (propertyInfo.CanRead)
          owner.DefineMethodOverride((MethodInfo) propertyEmitter.GetGetter(propertyInfo.PropertyType), propertyInfo.GetGetMethod());
        if (propertyInfo.CanWrite)
          owner.DefineMethodOverride((MethodInfo) propertyEmitter.GetSetter(propertyInfo.PropertyType), propertyInfo.GetSetMethod());
      }
      return owner.CreateType();
    }

    public Type GetProxyType(Type interfaceType)
    {
      if (interfaceType == (Type) null)
        throw new ArgumentNullException(nameof (interfaceType));
      lock (ProxyGenerator.proxyTypes)
      {
        Type proxyType;
        if (!ProxyGenerator.proxyTypes.TryGetValue(interfaceType, out proxyType))
          ProxyGenerator.proxyTypes.Add(interfaceType, proxyType = ProxyGenerator.CreateProxyType(interfaceType));
        return proxyType;
      }
    }

    private static byte[] StringToByteArray(string hex)
    {
      int length = hex.Length;
      byte[] byteArray = new byte[length / 2];
      for (int startIndex = 0; startIndex < length; startIndex += 2)
        byteArray[startIndex / 2] = Convert.ToByte(hex.Substring(startIndex, 2), 16);
      return byteArray;
    }
  }
}
