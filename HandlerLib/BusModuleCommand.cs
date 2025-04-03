// Decompiled with JetBrains decompiler
// Type: HandlerLib.BusModuleCommand
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum BusModuleCommand : byte
  {
    BUS_ASYNC_NO_COMMAND,
    BUS_ASYNC_MODULE_INITIALISATION,
    BUS_ASYNC_CHANGE_STATE,
    BUS_ASYNC_SAVE_SETUP_TO_MASTER,
    BUS_ASYNC_LOAD_SETUP_FROM_MASTER,
    BUS_ASYNC_TRANSPARENT_TO_MODULE,
    BUS_ASYNC_TRANSPARENT_FROM_MODULE,
    BUS_ASYNC_WAIT_FOR_NEXT_CYCLE,
    BUS_ASYNC_GET_LAST_CYCLE_ANSWER,
    BUS_ASYNC_GET_MEMORY_RANGES,
    BUS_ASYNC_READ_MEMORY,
    BUS_ASYNC_WRITE_MEMORY,
  }
}
