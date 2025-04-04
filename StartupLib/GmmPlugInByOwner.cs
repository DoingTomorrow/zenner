// Decompiled with JetBrains decompiler
// Type: StartupLib.GmmPlugInByOwner
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using PlugInLib;
using System;
using System.Windows;

#nullable disable
namespace StartupLib
{
  public abstract class GmmPlugInByOwner : GmmPlugIn
  {
    public virtual string ShowMainWindow(Window owner) => throw new NotImplementedException();

    public virtual string ShowMainWindow(
      Window owner,
      GmmPlugInExtendedOptions options,
      object parameter = null)
    {
      throw new NotImplementedException();
    }
  }
}
