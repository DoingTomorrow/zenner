// Decompiled with JetBrains decompiler
// Type: S4_Handler.VIF_Definition
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

#nullable disable
namespace S4_Handler
{
  internal class VIF_Definition
  {
    internal S4_MenuManager.DS0 UnitID;
    internal byte[] VolumeVIFs;
    internal byte[] FlowVIFs;

    internal VIF_Definition(S4_MenuManager.DS0 unitID, byte[] volumeVIFs, byte[] flowVIFs)
    {
      this.UnitID = unitID;
      this.VolumeVIFs = volumeVIFs;
      this.FlowVIFs = flowVIFs;
    }
  }
}
