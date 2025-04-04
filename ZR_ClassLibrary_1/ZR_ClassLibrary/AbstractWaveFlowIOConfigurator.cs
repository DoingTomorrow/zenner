// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.AbstractWaveFlowIOConfigurator
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public abstract class AbstractWaveFlowIOConfigurator
  {
    public abstract string GetLastErrorString();

    public abstract bool GetNumberOfIOs(out int IoCount);

    public abstract bool GetIOSettings(int IO, out DeviceIOSettings IOSettings);

    public abstract bool SetIOSettings(int IO, DeviceIOSettings IOSettings);

    public abstract bool WriteSettingsToDevice();

    public abstract bool ReadSettingsFromDevice();
  }
}
