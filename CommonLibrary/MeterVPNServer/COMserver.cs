// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.MeterVPNServer.COMserver
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace ZENNER.CommonLibrary.MeterVPNServer
{
  [GeneratedCode("System.Xml", "4.8.4084.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [SoapType(Namespace = "urn:MeterVPN")]
  [Serializable]
  public class COMserver : INotifyPropertyChanged
  {
    private string nameField;
    private string ipField;
    private bool onlineField;
    private string certField;
    private string lastSeenField;
    private string trafficField;

    public string Name
    {
      get => this.nameField;
      set
      {
        this.nameField = value;
        this.RaisePropertyChanged(nameof (Name));
      }
    }

    public string IP
    {
      get => this.ipField;
      set
      {
        this.ipField = value;
        this.RaisePropertyChanged(nameof (IP));
      }
    }

    public bool Online
    {
      get => this.onlineField;
      set
      {
        this.onlineField = value;
        this.RaisePropertyChanged(nameof (Online));
      }
    }

    public string Cert
    {
      get => this.certField;
      set
      {
        this.certField = value;
        this.RaisePropertyChanged(nameof (Cert));
      }
    }

    public string LastSeen
    {
      get => this.lastSeenField;
      set
      {
        this.lastSeenField = value;
        this.RaisePropertyChanged(nameof (LastSeen));
      }
    }

    public string Traffic
    {
      get => this.trafficField;
      set
      {
        this.trafficField = value;
        this.RaisePropertyChanged(nameof (Traffic));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
