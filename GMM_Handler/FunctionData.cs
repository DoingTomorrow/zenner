// Decompiled with JetBrains decompiler
// Type: GMM_Handler.FunctionData
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.Collections;
using System.Text;

#nullable disable
namespace GMM_Handler
{
  public class FunctionData
  {
    public SortedList NeadedResources = new SortedList();
    public SortedList SuppliedResources = new SortedList();
    public ushort Number;
    public FunctionLocalisableType FunctionType;
    public string Name;
    public string FullName;
    public string Group;
    public string ShortInfo;
    public string Description;
    public string Symbolname;
    public int Version;
    public bool NewestVersion;
    public int FirmwareVersionMin;
    public int FirmwareVersionMax;
    public bool IsVisible;
    public int Column;
    public int Row;
    public StringBuilder AdditionalMessages;
  }
}
