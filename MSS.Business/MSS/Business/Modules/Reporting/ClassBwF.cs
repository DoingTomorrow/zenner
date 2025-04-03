// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Reporting.ClassBwF
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;

#nullable disable
namespace MSS.Business.Modules.Reporting
{
  [Serializable]
  public class ClassBwF
  {
    private string _gruppe;
    private string _dsc;
    private string _typ;
    private string _bwf;

    public string GroupName
    {
      get => this._gruppe;
      set => this._gruppe = value;
    }

    public string Description
    {
      get => this._dsc;
      set => this._dsc = value;
    }

    public string Name
    {
      get => this._typ;
      set => this._typ = value;
    }

    public string EvaluationFactor
    {
      get => this._bwf;
      set => this._bwf = value;
    }
  }
}
