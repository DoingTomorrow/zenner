// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.ProgressHandler
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public sealed class ProgressHandler : Progress<ProgressArg>
  {
    private int index;
    private List<double> parts;
    private string baseMessage;
    private string baseMessageOwner;
    private bool ReportLoggerTimesUsed = false;
    private Stopwatch reportStopwatch;
    private uint markCount;
    private List<ProgressSplitArg> splitInfo;
    public List<ProgressTimeArg> ReportLogger;

    public string BaseMessage
    {
      get => this.baseMessage;
      [MethodImpl(MethodImplOptions.NoInlining)] set
      {
        string name = new StackFrame(1, true).GetMethod().Name;
        if (string.IsNullOrEmpty(this.baseMessage))
        {
          this.baseMessage = value;
          this.baseMessageOwner = name;
        }
        else
        {
          if (this.baseMessageOwner != name)
            return;
          this.baseMessage = value;
        }
      }
    }

    public ProgressHandler(Action<ProgressArg> handler)
      : base(handler)
    {
      this.parts = new List<double>((IEnumerable<double>) new double[1]
      {
        100.0
      });
      this.index = 0;
      this.baseMessage = "";
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.ReportLogger != null)
      {
        stringBuilder.AppendLine("*** Progress report log ***");
        foreach (ProgressTimeArg progressTimeArg in this.ReportLogger)
          stringBuilder.AppendLine(progressTimeArg.ToString());
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("*** Progress logger time string ***");
        stringBuilder.AppendLine(this.GetReportLoggerTimesString());
        return stringBuilder.ToString();
      }
      if (this.splitInfo == null)
        return string.Format("{0} of {1}", (object) (this.index + 1), (object) this.parts.Count);
      stringBuilder.AppendLine("*** Split information ***");
      foreach (ProgressSplitArg progressSplitArg in this.splitInfo)
        stringBuilder.AppendLine(progressSplitArg.ToString());
      return stringBuilder.ToString();
    }

    public void Reset(string message) => this.Reset(1, message);

    public void Reset(int newParts = 1, string message = "")
    {
      if (this.ReportLoggerTimesUsed)
      {
        if (message.Length <= 0)
          return;
        this.Report(message);
      }
      else
      {
        this.splitInfo = (List<ProgressSplitArg>) null;
        if (this.reportStopwatch != null)
        {
          this.reportStopwatch.Reset();
          this.reportStopwatch = (Stopwatch) null;
        }
        if (newParts <= 0)
          newParts = 1;
        this.parts.Clear();
        this.baseMessage = "";
        this.index = 0;
        double num = 100.0 / (double) newParts;
        for (int index = 0; index < newParts; ++index)
          this.parts.Add(num);
        this.ReportProgressArg(new ProgressArg(0.0, message));
      }
    }

    public void Split(int subParts)
    {
      if (this.ReportLoggerTimesUsed || subParts <= 0)
        return;
      double num = this.parts[this.index] / (double) subParts;
      List<double> collection = new List<double>(subParts);
      for (int index = 0; index < subParts; ++index)
        collection.Add(num);
      this.parts.RemoveAt(this.index);
      this.parts.InsertRange(this.index, (IEnumerable<double>) collection);
    }

    public void SplitByCountPlusInit(int subPartsCount)
    {
      if (this.ReportLoggerTimesUsed || subPartsCount <= 0)
        return;
      double part = this.parts[this.index];
      double num1 = part / 100.0;
      double num2 = (part - num1) / (double) subPartsCount;
      List<double> collection = new List<double>(subPartsCount + 1);
      collection.Add(num1);
      for (int index = 0; index < subPartsCount; ++index)
        collection.Add(num2);
      this.parts.RemoveAt(this.index);
      this.parts.InsertRange(this.index, (IEnumerable<double>) collection);
    }

    public void Split(double[] subParts)
    {
      if (this.ReportLoggerTimesUsed || subParts == null || subParts.Length == 0)
        return;
      double part = this.parts[this.index];
      double a = 0.0;
      List<double> collection = new List<double>(subParts.Length);
      foreach (double subPart in subParts)
      {
        a += subPart;
        collection.Add(part / 100.0 * subPart);
      }
      if (Math.Round(a) != 100.0)
        throw new ArgumentOutOfRangeException("The sum of the sub parts is not 100%!");
      this.parts.RemoveAt(this.index);
      this.parts.InsertRange(this.index, (IEnumerable<double>) collection);
    }

    public void SplitScaled(double[] subParts)
    {
      if (this.ReportLoggerTimesUsed || subParts == null || subParts.Length == 0)
        return;
      double part = this.parts[this.index];
      double num1 = 0.0;
      List<double> doubleList = new List<double>(subParts.Length);
      foreach (double subPart in subParts)
      {
        num1 += subPart;
        doubleList.Add(part / 100.0 * subPart);
      }
      double num2 = 100.0 / num1;
      double[] subParts1 = new double[subParts.Length];
      for (int index = 0; index < subParts.Length; ++index)
        subParts1[index] = subParts[index] * num2;
      this.Split(subParts1);
    }

    public void Report(string message = "", object tag = null)
    {
      double progressPercentage;
      if (this.ReportLoggerTimesUsed)
      {
        if (this.reportStopwatch != null || this.splitInfo == null)
        {
          progressPercentage = (this.index & 1) != 0 ? 66.0 : 33.0;
          ++this.index;
        }
        else if (this.index >= this.splitInfo.Count)
          progressPercentage = 100.0;
        else if ((int) this.splitInfo[this.index].ReportMark == (int) this.markCount)
        {
          progressPercentage = this.splitInfo[this.index].ProgressPercentage;
          ++this.index;
        }
        else if (this.splitInfo[this.index].ReportMark > this.markCount)
        {
          progressPercentage = this.index <= 0 ? this.splitInfo[this.index].ProgressPercentage : this.splitInfo[this.index - 1].ProgressPercentage;
        }
        else
        {
          do
          {
            ++this.index;
            if (this.index >= this.splitInfo.Count)
              goto label_9;
          }
          while (this.splitInfo[this.index].ReportMark < this.markCount);
          goto label_11;
label_9:
          progressPercentage = 100.0;
          goto label_13;
label_11:
          progressPercentage = this.splitInfo[this.index].ProgressPercentage;
label_13:;
        }
      }
      else
      {
        double a = 0.0;
        for (int index = 0; index <= this.index; ++index)
          a += this.parts[index];
        progressPercentage = Math.Ceiling(a);
        if (this.parts.Count > this.index + 1)
          ++this.index;
      }
      if (tag == null)
        this.ReportProgressArg(new ProgressArg(progressPercentage, (this.baseMessage + " " + message).Trim()));
      else
        this.ReportProgressArg(new ProgressArg(progressPercentage, (this.baseMessage + " " + message).Trim(), tag));
    }

    public void Report(int progress, string message = "")
    {
      this.ReportProgressArg(new ProgressArg((double) progress, (this.baseMessage + " " + message).Trim()));
    }

    private void ReportProgressArg(ProgressArg progressArg)
    {
      if (this.reportStopwatch != null)
      {
        this.ReportLogger.Add(new ProgressTimeArg(this.reportStopwatch.ElapsedMilliseconds, progressArg.Message, progressArg.Tag));
        this.reportStopwatch.Restart();
      }
      this.OnReport(progressArg);
    }

    public void SplitByReportLoggerTimesString(string recordLoggerTimesString = null)
    {
      this.ReportLoggerTimesUsed = true;
      this.index = 0;
      this.markCount = 0U;
      if (string.IsNullOrEmpty(recordLoggerTimesString))
      {
        this.ReportLogger = new List<ProgressTimeArg>();
        this.reportStopwatch = new Stopwatch();
      }
      else
      {
        this.ReportLogger = (List<ProgressTimeArg>) null;
        if (this.reportStopwatch != null)
          this.reportStopwatch.Reset();
        this.reportStopwatch = (Stopwatch) null;
        string[] strArray = recordLoggerTimesString.Split(new char[1]
        {
          ';'
        }, StringSplitOptions.RemoveEmptyEntries);
        if (strArray.Length < 2)
          return;
        StringBuilder stringBuilder = new StringBuilder();
        uint num1 = 0;
        uint num2 = 0;
        this.splitInfo = new List<ProgressSplitArg>();
        foreach (string s in strArray)
        {
          if (s.StartsWith("M"))
          {
            ++num2;
            int num3 = int.Parse(s.Substring(1));
            if ((long) num2 != (long) num3)
              throw new Exception("Illegal mark number: " + s);
          }
          else
          {
            uint num4 = uint.Parse(s);
            num1 += num4;
            this.splitInfo.Add(new ProgressSplitArg()
            {
              ProgressPercentage = (double) num4,
              ReportMark = num2
            });
          }
        }
        double num5 = 100.0 / (double) num1;
        double num6 = 0.0;
        foreach (ProgressSplitArg progressSplitArg in this.splitInfo)
        {
          num6 += progressSplitArg.ProgressPercentage * num5;
          progressSplitArg.ProgressPercentage = num6;
        }
      }
    }

    public string GetReportLoggerTimesString()
    {
      this.ReportLoggerTimesUsed = false;
      this.index = 0;
      if (this.ReportLogger == null || this.ReportLogger.Count < 2)
        return (string) null;
      if (this.reportStopwatch != null)
      {
        this.reportStopwatch.Reset();
        this.reportStopwatch = (Stopwatch) null;
      }
      StringBuilder stringBuilder = new StringBuilder();
      foreach (ProgressTimeArg progressTimeArg in this.ReportLogger)
      {
        if (progressTimeArg.ReportMarkCounter > 0U)
          stringBuilder.Append("M" + progressTimeArg.ReportMarkCounter.ToString());
        else if (progressTimeArg.PrograssTime_ms > 0L)
          stringBuilder.Append(progressTimeArg.PrograssTime_ms.ToString());
        else
          stringBuilder.Append('1');
        stringBuilder.Append(';');
      }
      --stringBuilder.Length;
      stringBuilder.AppendLine();
      int index = 120;
      while (index < stringBuilder.Length - 20)
      {
        for (; index < stringBuilder.Length - 10; ++index)
        {
          if (stringBuilder[index] == ';')
          {
            stringBuilder.Insert(index + 1, Environment.NewLine);
            index += 120;
            break;
          }
        }
      }
      return stringBuilder.ToString();
    }

    public void LoggerMark()
    {
      ++this.markCount;
      if (this.reportStopwatch == null)
        return;
      this.ReportLogger.Add(new ProgressTimeArg(this.markCount));
    }
  }
}
