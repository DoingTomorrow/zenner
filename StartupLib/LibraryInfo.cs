// Decompiled with JetBrains decompiler
// Type: StartupLib.LibraryInfo
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

#nullable disable
namespace StartupLib
{
  public class LibraryInfo
  {
    public string FileName;
    public bool IsPlugin;
    public string Name;
    public string PluginPath;

    public LibraryInfo()
    {
      this.FileName = "";
      this.IsPlugin = false;
      this.Name = "";
      this.PluginPath = "";
    }

    public override string ToString()
    {
      return !string.IsNullOrEmpty(this.FileName) ? this.FileName : string.Empty;
    }
  }
}
