// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FactoryHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Reflection;

#nullable disable
namespace NLog.Internal
{
  internal class FactoryHelper
  {
    private FactoryHelper()
    {
    }

    internal static object CreateInstance(Type t)
    {
      ConstructorInfo constructor = t.GetConstructor(ArrayHelper.Empty<Type>());
      return constructor != (ConstructorInfo) null ? constructor.Invoke(ArrayHelper.Empty<object>()) : throw new NLogConfigurationException(string.Format("Cannot access the constructor of type: {0}. Is the required permission granted?", (object) t.FullName));
    }
  }
}
