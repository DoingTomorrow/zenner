// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.NominalFlow
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

#nullable disable
namespace EDCL_Handler
{
  public class NominalFlow
  {
    public byte PulseMultiplier { get; private set; }

    public ushort OversizeDiff { get; private set; }

    public ushort UndersizeDiff { get; private set; }

    public ushort BurstDiff { get; private set; }

    public NominalFlow(
      byte pulseMultiplier,
      ushort oversizeDiff,
      ushort undersizeDiff,
      ushort burstDiff)
    {
      this.PulseMultiplier = pulseMultiplier;
      this.OversizeDiff = oversizeDiff;
      this.UndersizeDiff = undersizeDiff;
      this.BurstDiff = burstDiff;
    }

    public string GetNominalFlow()
    {
      if (this.PulseMultiplier == (byte) 1)
      {
        if (this.OversizeDiff == (ushort) 62 && this.UndersizeDiff == (ushort) 625 && this.BurstDiff == (ushort) 188)
          return 2.5.ToString();
        if (this.OversizeDiff == (ushort) 100 && this.UndersizeDiff == (ushort) 1000 && this.BurstDiff == (ushort) 300)
          return 4.ToString();
        if (this.OversizeDiff == (ushort) 158 && this.UndersizeDiff == (ushort) 1575 && this.BurstDiff == (ushort) 472)
          return 6.3.ToString();
        if (this.OversizeDiff == (ushort) 250 && this.UndersizeDiff == (ushort) 2500 && this.BurstDiff == (ushort) 750)
          return 10.ToString();
        if (this.OversizeDiff == (ushort) 400 && this.UndersizeDiff == (ushort) 4000 && this.BurstDiff == (ushort) 1200)
          return 16.ToString();
        if (this.OversizeDiff == (ushort) 625 && this.UndersizeDiff == (ushort) 6250 && this.BurstDiff == (ushort) 1875)
          return 25.ToString();
      }
      else if (this.PulseMultiplier == (byte) 10)
      {
        if (this.OversizeDiff == (ushort) 62 && this.UndersizeDiff == (ushort) 625 && this.BurstDiff == (ushort) 188)
          return 25.ToString();
        if (this.OversizeDiff == (ushort) 100 && this.UndersizeDiff == (ushort) 1000 && this.BurstDiff == (ushort) 300)
          return 40.ToString();
        if (this.OversizeDiff == (ushort) 158 && this.UndersizeDiff == (ushort) 1575 && this.BurstDiff == (ushort) 472)
          return 63.ToString();
        if (this.OversizeDiff == (ushort) 250 && this.UndersizeDiff == (ushort) 2500 && this.BurstDiff == (ushort) 750)
          return 100.ToString();
      }
      else if (this.PulseMultiplier == (byte) 100)
      {
        if (this.OversizeDiff == (ushort) 40 && this.UndersizeDiff == (ushort) 400 && this.BurstDiff == (ushort) 120)
          return 160.ToString();
        if (this.OversizeDiff == (ushort) 62 && this.UndersizeDiff == (ushort) 625 && this.BurstDiff == (ushort) 188)
          return 250.ToString();
        if (this.OversizeDiff == (ushort) 100 && this.UndersizeDiff == (ushort) 1000 && this.BurstDiff == (ushort) 300)
          return 400.ToString();
        if (this.OversizeDiff == (ushort) 158 && this.UndersizeDiff == (ushort) 1575 && this.BurstDiff == (ushort) 472)
          return 630.ToString();
        if (this.OversizeDiff == (ushort) 250 && this.UndersizeDiff == (ushort) 2500 && this.BurstDiff == (ushort) 750)
          return 1000.ToString();
      }
      return string.Empty;
    }

    public void SetNominalFlow(string value)
    {
      if (string.IsNullOrEmpty(value))
        return;
      if (this.PulseMultiplier == (byte) 1)
      {
        double num = 2.5;
        if (num.ToString() == value)
          this.Set((ushort) 62, (ushort) 625, (ushort) 188);
        if (4.ToString() == value)
          this.Set((ushort) 100, (ushort) 1000, (ushort) 300);
        num = 6.3;
        if (num.ToString() == value)
          this.Set((ushort) 158, (ushort) 1575, (ushort) 472);
        if (10.ToString() == value)
          this.Set((ushort) 250, (ushort) 2500, (ushort) 750);
        if (16.ToString() == value)
          this.Set((ushort) 400, (ushort) 4000, (ushort) 1200);
        if (!(25.ToString() == value))
          return;
        this.Set((ushort) 625, (ushort) 6250, (ushort) 1875);
      }
      else if (this.PulseMultiplier == (byte) 10)
      {
        int num = 25;
        if (num.ToString() == value)
          this.Set((ushort) 62, (ushort) 625, (ushort) 188);
        num = 40;
        if (num.ToString() == value)
          this.Set((ushort) 100, (ushort) 1000, (ushort) 300);
        num = 63;
        if (num.ToString() == value)
          this.Set((ushort) 158, (ushort) 1575, (ushort) 472);
        num = 100;
        if (!(num.ToString() == value))
          return;
        this.Set((ushort) 250, (ushort) 2500, (ushort) 750);
      }
      else
      {
        if (this.PulseMultiplier != (byte) 100)
          return;
        int num = 160;
        if (num.ToString() == value)
          this.Set((ushort) 40, (ushort) 400, (ushort) 120);
        num = 250;
        if (num.ToString() == value)
          this.Set((ushort) 62, (ushort) 625, (ushort) 188);
        num = 400;
        if (num.ToString() == value)
          this.Set((ushort) 100, (ushort) 1000, (ushort) 300);
        num = 630;
        if (num.ToString() == value)
          this.Set((ushort) 158, (ushort) 1575, (ushort) 472);
        num = 1000;
        if (num.ToString() == value)
          this.Set((ushort) 250, (ushort) 2500, (ushort) 750);
      }
    }

    private void Set(ushort oversizeDiff, ushort undersizeDiff, ushort burstDiff)
    {
      this.OversizeDiff = oversizeDiff;
      this.UndersizeDiff = undersizeDiff;
      this.BurstDiff = burstDiff;
    }

    public string[] AllowedValues
    {
      get
      {
        if (this.PulseMultiplier == (byte) 1)
          return new string[7]
          {
            2.5.ToString(),
            4.ToString(),
            6.3.ToString(),
            10.ToString(),
            16.ToString(),
            25.ToString(),
            string.Empty
          };
        if (this.PulseMultiplier == (byte) 10)
          return new string[5]
          {
            25.ToString(),
            40.ToString(),
            63.ToString(),
            100.ToString(),
            string.Empty
          };
        if (this.PulseMultiplier != (byte) 100)
          return new string[0];
        return new string[6]
        {
          160.ToString(),
          250.ToString(),
          400.ToString(),
          630.ToString(),
          1000.ToString(),
          string.Empty
        };
      }
    }

    public override string ToString() => this.GetNominalFlow();
  }
}
