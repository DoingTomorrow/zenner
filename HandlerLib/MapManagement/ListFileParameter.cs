// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.ListFileParameter
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib.MapManagement
{
  public class ListFileParameter
  {
    public string FieldName { get; set; }

    public uint Align { get; set; }

    public string Path { get; set; }

    public string Section { get; set; }

    public string Type { get; set; }

    public string Type_add { get; set; }

    public string Type_add_extra { get; set; }

    public bool IsArray { get; set; }

    public int ArraySize { get; set; }

    public string[] InitValues { get; set; }

    public bool IsOK { get; set; }

    public bool ShowInMAP { get; set; }

    public ListFileParameter Clone()
    {
      return new ListFileParameter()
      {
        FieldName = this.FieldName,
        Align = this.Align,
        Section = this.Section,
        Path = this.Path,
        Type = this.Type,
        Type_add = this.Type_add,
        Type_add_extra = this.Type_add_extra,
        ArraySize = this.ArraySize,
        InitValues = this.InitValues,
        IsOK = this.IsOK,
        ShowInMAP = this.ShowInMAP
      };
    }

    public void UpdateParameter(ListFileParameter LFP)
    {
      if (!this.FieldName.Equals(LFP.FieldName))
        return;
      this.Align = LFP.Align;
      this.Section = LFP.Section;
      this.Path = LFP.Path;
      this.Type = LFP.Type;
      this.Type_add = LFP.Type_add;
      this.Type_add_extra = LFP.Type_add_extra;
      this.ArraySize = LFP.ArraySize;
      this.InitValues = LFP.InitValues;
      this.IsOK = this.checkOK();
    }

    public bool checkOK()
    {
      this.IsOK = this.InitValues != null && !string.IsNullOrEmpty(this.FieldName) && !string.IsNullOrEmpty(this.Type) && !string.IsNullOrEmpty(this.InitValues[0]);
      return this.IsOK;
    }
  }
}
