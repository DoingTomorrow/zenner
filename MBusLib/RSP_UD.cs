// Decompiled with JetBrains decompiler
// Type: MBusLib.RSP_UD
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  public sealed class RSP_UD : IPrintable
  {
    public List<MBusFrame> Frames { get; private set; }

    public uint? ID => this.GetLastRecord()?.Header.ID;

    public byte? Address => this.GetLastFrame()?.Address;

    public string Manufacturer => this.GetLastRecord()?.Header.ManufacturerString;

    public string Medium => this.GetLastRecord()?.Header.MediumString;

    public byte? Generation => this.GetLastRecord()?.Header.Generation;

    public override string ToString()
    {
      return string.Format("#{0}, MAN:{1}, MED:{2}, GEN:{3}", (object) this.ID, (object) this.Manufacturer, (object) this.Medium, (object) this.Generation);
    }

    public string Print(int spaces = 0)
    {
      if (this.Frames == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (MBusFrame frame in this.Frames)
      {
        string hexString = Util.ByteArrayToHexString((IEnumerable<byte>) frame.ToByteArray());
        stringBuilder.Append(' ', spaces).Append("RAW: ").AppendLine(hexString);
        VariableDataStructure variableDataStructure = VariableDataStructure.Parse(frame);
        if (variableDataStructure != null)
          stringBuilder.AppendLine(variableDataStructure.Print(spaces));
      }
      return stringBuilder.ToString();
    }

    public void Add(byte[] buffer) => this.Add(MBusFrame.Parse(buffer));

    public void Add(MBusFrame frame)
    {
      if (this.Frames == null)
        this.Frames = new List<MBusFrame>();
      this.Frames.Add(frame);
    }

    public MBusFrame GetLastFrame()
    {
      return this.Frames == null || this.Frames.Count == 0 ? (MBusFrame) null : this.Frames[this.Frames.Count - 1];
    }

    public List<VariableDataStructure> GetRecords()
    {
      return this.Frames == null || this.Frames.Count <= 0 || !this.Frames[0].IsVariableDataStructure ? (List<VariableDataStructure>) null : this.Frames.Select<MBusFrame, VariableDataStructure>((Func<MBusFrame, VariableDataStructure>) (item => VariableDataStructure.Parse(item))).ToList<VariableDataStructure>();
    }

    public VariableDataStructure GetLastRecord()
    {
      MBusFrame lastFrame = this.GetLastFrame();
      return lastFrame == null && !lastFrame.IsVariableDataStructure ? (VariableDataStructure) null : VariableDataStructure.Parse(lastFrame);
    }

    public List<MBusValue> GetValues()
    {
      List<VariableDataStructure> records = this.GetRecords();
      if (records == null)
        return (List<MBusValue>) null;
      List<MBusValue> values = new List<MBusValue>();
      foreach (VariableDataStructure variableDataStructure in records)
      {
        if (variableDataStructure.Records != null)
        {
          foreach (VariableDataBlock record in variableDataStructure.Records)
            values.Add(record.Value);
        }
      }
      return values;
    }
  }
}
