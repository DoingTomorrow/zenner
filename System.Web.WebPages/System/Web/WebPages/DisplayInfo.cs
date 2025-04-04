// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.DisplayInfo
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.WebPages
{
  public class DisplayInfo
  {
    public DisplayInfo(string filePath, IDisplayMode displayMode)
    {
      if (filePath == null)
        throw new ArgumentNullException(nameof (filePath));
      if (displayMode == null)
        throw new ArgumentNullException(nameof (displayMode));
      this.FilePath = filePath;
      this.DisplayMode = displayMode;
    }

    public IDisplayMode DisplayMode { get; private set; }

    public string FilePath { get; private set; }
  }
}
