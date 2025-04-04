// Decompiled with JetBrains decompiler
// Type: NLog.Config.MethodFactory`2
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NLog.Config
{
  internal class MethodFactory<TClassAttributeType, TMethodAttributeType> : 
    INamedItemFactory<MethodInfo, MethodInfo>,
    IFactory
    where TClassAttributeType : Attribute
    where TMethodAttributeType : NameBaseAttribute
  {
    private readonly Dictionary<string, MethodInfo> _nameToMethodInfo = new Dictionary<string, MethodInfo>();

    public IDictionary<string, MethodInfo> AllRegisteredItems
    {
      get => (IDictionary<string, MethodInfo>) this._nameToMethodInfo;
    }

    public void ScanTypes(Type[] types, string prefix)
    {
      foreach (Type type in types)
      {
        try
        {
          this.RegisterType(type, prefix);
        }
        catch (Exception ex)
        {
          InternalLogger.Error(ex, "Failed to add type '{0}'.", (object) type.FullName);
          if (ex.MustBeRethrown())
            throw;
        }
      }
    }

    public void RegisterType(Type type, string itemNamePrefix)
    {
      if (!type.IsDefined(typeof (TClassAttributeType), false))
        return;
      foreach (MethodInfo method in type.GetMethods())
      {
        foreach (TMethodAttributeType customAttribute in (TMethodAttributeType[]) method.GetCustomAttributes(typeof (TMethodAttributeType), false))
          this.RegisterDefinition(itemNamePrefix + customAttribute.Name, method);
      }
    }

    public void Clear() => this._nameToMethodInfo.Clear();

    public void RegisterDefinition(string name, MethodInfo methodInfo)
    {
      this._nameToMethodInfo[name] = methodInfo;
    }

    public bool TryCreateInstance(string name, out MethodInfo result)
    {
      return this._nameToMethodInfo.TryGetValue(name, out result);
    }

    public MethodInfo CreateInstance(string name)
    {
      MethodInfo result;
      if (this.TryCreateInstance(name, out result))
        return result;
      throw new NLogConfigurationException("Unknown function: '" + name + "'");
    }

    public bool TryGetDefinition(string name, out MethodInfo result)
    {
      return this._nameToMethodInfo.TryGetValue(name, out result);
    }
  }
}
