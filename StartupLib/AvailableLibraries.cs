// Decompiled with JetBrains decompiler
// Type: StartupLib.AvailableLibraries
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using PlugInLib;
using System;
using System.Collections.Generic;

#nullable disable
namespace StartupLib
{
  public class AvailableLibraries
  {
    public DateTime FileCreateDate;
    public string FileCreationReason;
    public int DllsInFolder;
    public List<LibraryInfo> LibrariesInfo;
    public List<FullRightInfo> LibraryRights;

    public AvailableLibraries()
    {
      this.LibrariesInfo = new List<LibraryInfo>();
      this.LibraryRights = new List<FullRightInfo>();
    }
  }
}
