// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.EventTrigger
// Assembly: System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 794D0242-5078-4CF1-BEBC-5ADC9BB01BDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Windows.Interactivity.dll

#nullable disable
namespace System.Windows.Interactivity
{
  public class EventTrigger : EventTriggerBase<object>
  {
    public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(nameof (EventName), typeof (string), typeof (EventTrigger), (PropertyMetadata) new FrameworkPropertyMetadata((object) "Loaded", new PropertyChangedCallback(EventTrigger.OnEventNameChanged)));

    public EventTrigger()
    {
    }

    public EventTrigger(string eventName) => this.EventName = eventName;

    public string EventName
    {
      get => (string) this.GetValue(EventTrigger.EventNameProperty);
      set => this.SetValue(EventTrigger.EventNameProperty, (object) value);
    }

    protected override string GetEventName() => this.EventName;

    private static void OnEventNameChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
      ((EventTriggerBase) sender).OnEventNameChanged((string) args.OldValue, (string) args.NewValue);
    }
  }
}
