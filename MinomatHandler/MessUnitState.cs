// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MessUnitState
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class MessUnitState
  {
    public TimeOffsetMode TimeOffset { get; set; }

    public HCA_Scale Scale { get; set; }

    public HCA_SensorMode SensorMode { get; set; }

    public bool IsAccuDefect { get; set; }

    public bool IsManipulated { get; set; }

    public bool IsDeviceError { get; set; }

    public static MessUnitState Parse(byte state)
    {
      MessUnitState messUnitState = new MessUnitState();
      messUnitState.TimeOffset = (TimeOffsetMode) Enum.ToObject(typeof (TimeOffsetMode), (int) state & 4);
      messUnitState.Scale = HCA_Scale.Uniform;
      if (((int) state & 4) == 4)
        messUnitState.Scale = HCA_Scale.Product;
      messUnitState.SensorMode = HCA_SensorMode.Single;
      if (((int) state & 8) == 8)
        messUnitState.SensorMode = HCA_SensorMode.Double;
      messUnitState.IsAccuDefect = ((int) state & 32) == 32;
      messUnitState.IsManipulated = ((int) state & 64) == 64;
      messUnitState.IsDeviceError = ((int) state & 128) == 128;
      return messUnitState;
    }

    public override string ToString()
    {
      return string.Format("TimeOffset: {0}, Scale: {1}, SensorMode: {2}, IsAccuDefect: {3}, IsManipulated: {4}, IsDeviceError: {5}", (object) this.TimeOffset, (object) this.Scale, (object) this.SensorMode, (object) this.IsAccuDefect, (object) this.IsManipulated, (object) this.IsDeviceError);
    }
  }
}
