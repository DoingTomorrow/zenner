// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_Modules
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_Modules
  {
    internal List<S4_ModuleObjects> ModuleList = new List<S4_ModuleObjects>();
    internal S4_ModuleObjects SelectedModule;
    private S4_ModuleCommands ModuleCommands;

    internal S4_Modules(S4_ModuleCommands moduleCommands) => this.ModuleCommands = moduleCommands;

    internal S4_ModuleObjects SelectModule(BusModuleInfo moduleInfo)
    {
      S4_ModuleObjects s4ModuleObjects = this.ModuleList.FirstOrDefault<S4_ModuleObjects>((Func<S4_ModuleObjects, bool>) (x => x.ModuleInfo == moduleInfo));
      if (s4ModuleObjects == null)
      {
        s4ModuleObjects = new S4_ModuleObjects(this.ModuleCommands, moduleInfo);
        this.ModuleList.Add(s4ModuleObjects);
      }
      this.SelectedModule = s4ModuleObjects;
      return this.SelectedModule;
    }
  }
}
