// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.ISerialPortWrapper
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal interface ISerialPortWrapper
  {
    [MonitoringDescription("SerialDataReceived")]
    event SerialDataReceivedEventHandler DataReceived;

    void Close();

    void Open();

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    Stream BaseStream { get; }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    int BytesToRead { get; }

    [Browsable(true)]
    [DefaultValue(0)]
    [MonitoringDescription("Handshake")]
    Handshake Handshake { get; set; }

    [MonitoringDescription("PortName")]
    [Browsable(true)]
    [DefaultValue("COM1")]
    string PortName { get; set; }

    [MonitoringDescription("ReadBufferSize")]
    [DefaultValue(4096)]
    [Browsable(true)]
    int ReadBufferSize { get; set; }

    [DefaultValue(2048)]
    [MonitoringDescription("WriteBufferSize")]
    [Browsable(true)]
    int WriteBufferSize { get; set; }
  }
}
