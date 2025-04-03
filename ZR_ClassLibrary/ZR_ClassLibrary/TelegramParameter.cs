// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.TelegramParameter
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class TelegramParameter
  {
    public string Name;
    public string Parent;
    public string Description;
    public string Type;
    public string RD_Type;
    public int Address;
    public int BitMask;
    public int ByteLength;
    public int RD_Factor;
    public int RD_Divisor;
    public bool UseK;
    public bool UseMulDiv;
    private System.Type t;
    public long ValueIdent;
    public int OverrideID;
    public string RD_Data;

    public System.Type ParameterType
    {
      get => this.t;
      set
      {
        this.t = value;
        if (value != (System.Type) null)
          this.tParameterType = value.AssemblyQualifiedName;
        else
          this.tParameterType = string.Empty;
      }
    }

    public string tParameterType
    {
      get => this.t != (System.Type) null ? this.t.AssemblyQualifiedName : string.Empty;
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this.t = System.Type.GetType(value);
      }
    }

    public override string ToString() => this.Name;
  }
}
