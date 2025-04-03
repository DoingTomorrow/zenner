// Decompiled with JetBrains decompiler
// Type: S3_Handler.VMCP_Diagnostic
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System.Text;

#nullable disable
namespace S3_Handler
{
  public class VMCP_Diagnostic
  {
    public VMCP_State VolInputState;
    public byte VMCP_CycleCalibrationPossible;
    public byte VolReceiveErrorCounter;
    public byte VolReceiveWaitQuietCounter;
    public byte RequestedEnergyCycleTime;
    public uint VolumeMeterIdentification;
    public ulong VolumeInputValueBefore;

    public static string GetListApprevations()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("******* VMCP diagnostic addreviations *******");
      stringBuilder.AppendLine("VIS: Volume input state");
      stringBuilder.AppendLine("CCP: Cycle calibratin possible");
      stringBuilder.AppendLine("REC: VMCP cycle protocol receve error counter");
      stringBuilder.AppendLine("WQC: VMCP receiver wait quiet counter");
      return stringBuilder.ToString();
    }

    public static string GetListHeader()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("VIS |");
      stringBuilder.Append("CCP |");
      stringBuilder.Append("REC |");
      stringBuilder.Append("WQC |");
      return stringBuilder.ToString();
    }

    public override string ToString() => this.ToString("b");

    public string ToString(string format)
    {
      StringBuilder stringBuilder = new StringBuilder();
      switch (format)
      {
        case "b":
          stringBuilder.AppendLine();
          stringBuilder.AppendLine("******* VMCP Diagnostic *******");
          stringBuilder.AppendLine("VolInputState ............... " + this.VolInputState.ToString());
          stringBuilder.AppendLine("VMCP_CycleCalibrationPossible " + this.VMCP_CycleCalibrationPossible.ToString());
          stringBuilder.AppendLine("VolReceiveErrorCounter....... " + this.VolReceiveErrorCounter.ToString());
          stringBuilder.AppendLine("VolReceiveWaitQuietCounter .. " + this.VolReceiveWaitQuietCounter.ToString());
          stringBuilder.AppendLine("RequestedEnergyCycleTime .... " + this.RequestedEnergyCycleTime.ToString());
          stringBuilder.AppendLine("VolumeMeterIdentification ... " + this.VolumeMeterIdentification.ToString());
          stringBuilder.AppendLine("VolumeInputValueBefore ...... " + this.VolumeInputValueBefore.ToString());
          stringBuilder.AppendLine();
          break;
        case "l":
          stringBuilder.Append(this.VolInputState.ToString().Substring(0, 3) + " |");
          stringBuilder.Append(this.VMCP_CycleCalibrationPossible.ToString("d3") + " |");
          stringBuilder.Append(this.VolReceiveErrorCounter.ToString("d3") + " |");
          stringBuilder.Append(this.VolReceiveWaitQuietCounter.ToString("d3") + " |");
          break;
        default:
          stringBuilder.AppendLine("Format error");
          stringBuilder.AppendLine("Use \"b\" = block");
          stringBuilder.AppendLine("or  \"l\" = list");
          break;
      }
      return stringBuilder.ToString();
    }
  }
}
