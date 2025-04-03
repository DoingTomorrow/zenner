// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3P_Device_Setup
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class S3P_Device_Setup
  {
    private S3_Parameter myParameter;
    private ushort Device_Setup;
    private const ushort HeatingBit = 256;
    private const ushort CoolingBit = 512;

    internal S3P_Device_Setup(S3_Meter myMeter)
    {
      this.myParameter = myMeter.MyParameters.ParameterByName[S3_ParameterNames.Device_Setup.ToString()];
      this.Device_Setup = this.myParameter.GetUshortValue();
    }

    public bool Heating
    {
      get => ((uint) this.Device_Setup & 256U) > 0U;
      set
      {
        if (value)
          this.Device_Setup |= (ushort) 256;
        else
          this.Device_Setup &= (ushort) 65279;
        this.myParameter.SetUshortValue(this.Device_Setup);
      }
    }

    public bool Cooling
    {
      get => ((uint) this.Device_Setup & 512U) > 0U;
      set
      {
        if (value)
          this.Device_Setup |= (ushort) 512;
        else
          this.Device_Setup &= (ushort) 65023;
        this.myParameter.SetUshortValue(this.Device_Setup);
      }
    }

    public ConfigurationParameter.ChangeOverValues EnergyCalculations
    {
      get
      {
        return this.Heating ? (this.Cooling ? ConfigurationParameter.ChangeOverValues.ChangeOver : ConfigurationParameter.ChangeOverValues.Heating) : (this.Cooling ? ConfigurationParameter.ChangeOverValues.Cooling : ConfigurationParameter.ChangeOverValues.None);
      }
      set
      {
        this.Device_Setup &= (ushort) 64767;
        switch (value)
        {
          case ConfigurationParameter.ChangeOverValues.Heating:
            this.Device_Setup |= (ushort) 256;
            break;
          case ConfigurationParameter.ChangeOverValues.ChangeOver:
            this.Device_Setup |= (ushort) 256;
            this.Device_Setup |= (ushort) 512;
            break;
          case ConfigurationParameter.ChangeOverValues.Cooling:
            this.Device_Setup |= (ushort) 512;
            break;
        }
        this.myParameter.SetUshortValue(this.Device_Setup);
      }
    }
  }
}
