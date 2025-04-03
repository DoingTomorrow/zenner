// Decompiled with JetBrains decompiler
// Type: HandlerLib.FirmwareData
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace HandlerLib
{
  public class FirmwareData
  {
    public string ProgrammerFileAsString;
    public string ProgFileName;
    public string SourceInfo;
    public List<KeyValuePair<string, string>> Options = new List<KeyValuePair<string, string>>();

    public void WriteToFile(string filePathAndName)
    {
      using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
        streamWriter.Write(this.ProgrammerFileAsString);
    }
  }
}
