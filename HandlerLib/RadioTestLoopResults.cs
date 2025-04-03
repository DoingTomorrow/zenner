// Decompiled with JetBrains decompiler
// Type: HandlerLib.RadioTestLoopResults
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace HandlerLib
{
  public class RadioTestLoopResults
  {
    private DateTime StartTime;
    private List<double> RSSI_List;
    private List<double> LQI_List;

    public string MessageText { get; set; }

    public int TestCount { get; set; }

    public int SendCount { get; set; }

    public int ReceiveCount => this.RSSI_List.Count;

    public int NoDataCount { get; set; }

    public int PollingCount { get; set; }

    public double MinRSSI
    {
      get => this.RSSI_List.Count == 0 ? (double) int.MinValue : this.RSSI_List.Min();
    }

    public double MaxRSSI
    {
      get => this.RSSI_List.Count == 0 ? (double) int.MinValue : this.RSSI_List.Max();
    }

    public double AvarageRSSI_Unfilterd
    {
      get
      {
        if (this.RSSI_List.Count == 0)
          return (double) int.MinValue;
        double num = 0.0;
        for (int index = 0; index < this.RSSI_List.Count; ++index)
          num += this.RSSI_List[index];
        return num / (double) this.RSSI_List.Count;
      }
    }

    public double AvarageRSSI
    {
      get
      {
        if (this.RSSI_List.Count == 0)
          return (double) int.MinValue;
        List<double> list = this.RSSI_List.ToList<double>();
        list.Sort();
        double num1 = 0.0;
        int num2 = list.Count / 5;
        for (int index = num2; index < list.Count - num2; ++index)
          num1 += list[index];
        return num1 / (double) (list.Count - 2 * num2);
      }
    }

    public string RSSI_Values
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (int rssi in this.RSSI_List)
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(';');
          stringBuilder.Append(rssi.ToString("0.##"));
        }
        return stringBuilder.ToString();
      }
    }

    public double MinLQI => this.LQI_List.Count == 0 ? (double) int.MinValue : this.LQI_List.Min();

    public double MaxLQI => this.LQI_List.Count == 0 ? (double) int.MinValue : this.LQI_List.Max();

    public double AvarageLQI_Unfilterd
    {
      get
      {
        if (this.LQI_List.Count == 0)
          return (double) int.MinValue;
        double num = 0.0;
        for (int index = 0; index < this.LQI_List.Count; ++index)
          num += this.LQI_List[index];
        return num / (double) this.LQI_List.Count;
      }
    }

    public double AvarageLQI
    {
      get
      {
        if (this.LQI_List.Count == 0)
          return (double) int.MinValue;
        List<double> list = this.LQI_List.ToList<double>();
        list.Sort();
        double num1 = 0.0;
        int num2 = list.Count / 5;
        for (int index = num2; index < list.Count - num2; ++index)
          num1 += list[index];
        return num1 / (double) (list.Count - 2 * num2);
      }
    }

    public string LQI_Values
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (int lqi in this.LQI_List)
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(';');
          stringBuilder.Append(lqi.ToString("0.##"));
        }
        return stringBuilder.ToString();
      }
    }

    public RadioTestLoopResults()
    {
      this.MessageText = string.Empty;
      this.RSSI_List = new List<double>();
      this.LQI_List = new List<double>();
      this.StartTime = DateTime.Now;
    }

    public void AddRSSI(int newRSSI, int? newLQI = null) => this.RSSI_List.Add((double) newRSSI);

    public void AddLoopResults(RadioTestLoopResults radioTestLoopResults)
    {
      if (radioTestLoopResults == null)
        return;
      this.TestCount += radioTestLoopResults.TestCount;
      this.SendCount += radioTestLoopResults.SendCount;
      this.NoDataCount += radioTestLoopResults.NoDataCount;
      this.PollingCount += radioTestLoopResults.PollingCount;
      this.RSSI_List.Add(radioTestLoopResults.AvarageRSSI);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(this.MessageText);
      stringBuilder.AppendLine("SendCount: .. " + this.SendCount.ToString());
      stringBuilder.AppendLine("ReceiveCount: " + this.ReceiveCount.ToString());
      if (this.RSSI_List.Count > 0)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("MinRSSI: ............. " + this.MinRSSI.ToString("0.##"));
        stringBuilder.AppendLine("MaxRSSI: ............. " + this.MaxRSSI.ToString("0.##"));
        stringBuilder.AppendLine("AvarageRSSI .......... " + this.AvarageRSSI.ToString("0.##"));
        stringBuilder.AppendLine("AvarageRSSI_Unfilterd: " + this.AvarageRSSI_Unfilterd.ToString("0.##"));
        stringBuilder.AppendLine("RSSI_Values: ......... " + this.RSSI_Values);
      }
      if (this.LQI_List.Count > 0)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("MinLQI: ............. " + this.MinLQI.ToString("0.##"));
        stringBuilder.AppendLine("MaxLQI: ............. " + this.MaxLQI.ToString("0.##"));
        stringBuilder.AppendLine("AvarageLQI: ......... " + this.AvarageLQI.ToString("0.##"));
        stringBuilder.AppendLine("AvarageLQI_Unfilterd: " + this.AvarageLQI_Unfilterd.ToString("0.##"));
        stringBuilder.AppendLine("LQI_Values: ......... " + this.LQI_Values);
      }
      return stringBuilder.ToString();
    }
  }
}
