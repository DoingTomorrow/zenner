// Decompiled with JetBrains decompiler
// Type: AsyncCom.MeterVPNServer.COMserver
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace AsyncCom.MeterVPNServer
{
  [GeneratedCode("System.Xml", "4.8.4084.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [SoapType(Namespace = "urn:MeterVPN")]
  [Serializable]
  public class COMserver
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
      set => this.nameField = value;
    }

    public string IP
    {
      get => this.ipField;
      set => this.ipField = value;
    }

    public bool Online
    {
      get => this.onlineField;
      set => this.onlineField = value;
    }

    public string Cert
    {
      get => this.certField;
      set => this.certField = value;
    }

    public string LastSeen
    {
      get => this.lastSeenField;
      set => this.lastSeenField = value;
    }

    public string Traffic
    {
      get => this.trafficField;
      set => this.trafficField = value;
    }
  }
}
