// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.NameResolvedEventArgs
// Assembly: System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 794D0242-5078-4CF1-BEBC-5ADC9BB01BDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Windows.Interactivity.dll

#nullable disable
namespace System.Windows.Interactivity
{
  internal sealed class NameResolvedEventArgs : EventArgs
  {
    private object oldObject;
    private object newObject;

    public object OldObject => this.oldObject;

    public object NewObject => this.newObject;

    public NameResolvedEventArgs(object oldObject, object newObject)
    {
      this.oldObject = oldObject;
      this.newObject = newObject;
    }
  }
}
