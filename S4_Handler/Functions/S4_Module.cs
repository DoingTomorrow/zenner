// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_Module
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_Module
  {
    internal BusModuleInfo ModuleInfo;
    internal S4_ModuleMemory ModuleMemory;

    internal S4_Module(BusModuleInfo moduleInfo, S4_ModuleMemory moduleMemory)
    {
      this.ModuleInfo = moduleInfo;
      this.ModuleMemory = moduleMemory;
    }
  }
}
