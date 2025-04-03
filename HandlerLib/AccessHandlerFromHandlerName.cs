// Decompiled with JetBrains decompiler
// Type: HandlerLib.AccessHandlerFromHandlerName
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using PlugInLib;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace HandlerLib
{
  public static class AccessHandlerFromHandlerName
  {
    public static IHandler CreateHandlerAndGetIHandlerInterface(string handlerName)
    {
      Assembly assembly = Assembly.LoadFrom(handlerName + ".dll");
      Type type = ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (x => x.GetInterface("IHandler") != (Type) null)).First<Type>((Func<Type, bool>) (x => x.Name == handlerName + "Functions"));
      return assembly.CreateInstance(type.ToString()) as IHandler;
    }

    public static HandlerFunctionsForProduction CreateHandlerAndGetProductionInterface(
      string handlerName)
    {
      Assembly assembly = Assembly.LoadFrom(handlerName + ".dll");
      Type type = ((IEnumerable<Type>) assembly.GetTypes()).First<Type>((Func<Type, bool>) (x => x.BaseType == typeof (HandlerFunctionsForProduction)));
      return assembly.CreateInstance(type.ToString()) as HandlerFunctionsForProduction;
    }

    public static HandlerFunctionsForProduction GetProductionInterfaceFromIWindowFunctionsInterface(
      IWindowFunctions iWindowfunctions)
    {
      object functions = iWindowfunctions.GetFunctions();
      if (functions == null)
        throw new Exception("HandlerFunctions not available");
      return functions is HandlerFunctionsForProduction ? functions as HandlerFunctionsForProduction : throw new Exception("HandlerFunctionsForProduction not supported");
    }

    public static HandlerFunctionsForProduction GetProductionInterfaceFromHandlerPlugin(
      GmmPlugIn plugIn)
    {
      object obj1 = plugIn.GetPluginInfo().Interface;
      object obj2 = !(obj1.GetType().GetInterface("IWindowFunctions") == (Type) null) ? (obj1 as IWindowFunctions).GetFunctions() : throw new Exception("IWindowFunctions not supported");
      return obj2 is HandlerFunctionsForProduction ? obj2 as HandlerFunctionsForProduction : throw new Exception("HandlerFunctionsForProduction not supported");
    }
  }
}
