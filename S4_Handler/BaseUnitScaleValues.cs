// Decompiled with JetBrains decompiler
// Type: S4_Handler.BaseUnitScaleValues
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

#nullable disable
namespace S4_Handler
{
  internal class BaseUnitScaleValues
  {
    internal _UNIT_SCALE_ ScaleUnit;
    internal string DisplayString;
    internal uint VIF_value;

    internal BaseUnitScaleValues(_UNIT_SCALE_ scaleUnit, string displayString, uint vif_value)
    {
      this.ScaleUnit = scaleUnit;
      this.DisplayString = displayString;
      this.VIF_value = vif_value;
    }
  }
}
