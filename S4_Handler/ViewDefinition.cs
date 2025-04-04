// Decompiled with JetBrains decompiler
// Type: S4_Handler.ViewDefinition
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

#nullable disable
namespace S4_Handler
{
  internal class ViewDefinition
  {
    internal S4_MenuManager.DS0 S0;
    internal S4_MenuManager.DS1 S1;
    internal S4_MenuManager.DST Timeout;
    internal S4_MenuManager.DSF Flags;

    internal ViewDefinition(
      S4_MenuManager.DS0 s0,
      S4_MenuManager.DS1 s1,
      S4_MenuManager.DST timeout,
      S4_MenuManager.DSF flags)
    {
      this.S0 = s0;
      this.S1 = s1;
      this.Timeout = timeout;
      this.Flags = flags;
    }

    internal bool IsEqual(ViewDefinition compareView)
    {
      return this.S0 == compareView.S0 && this.S1 == compareView.S1 && this.Timeout == compareView.Timeout && this.Flags == compareView.Flags;
    }

    public override string ToString()
    {
      return this.S0.ToString() + "; " + this.S1.ToString() + "; " + this.Timeout.ToString() + "; " + this.Flags.ToString();
    }
  }
}
