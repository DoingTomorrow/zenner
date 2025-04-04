// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.TriggerAction
// Assembly: System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 794D0242-5078-4CF1-BEBC-5ADC9BB01BDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Windows.Interactivity.dll

using System.Globalization;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

#nullable disable
namespace System.Windows.Interactivity
{
  [DefaultTrigger(typeof (UIElement), typeof (EventTrigger), "MouseLeftButtonDown")]
  [DefaultTrigger(typeof (ButtonBase), typeof (EventTrigger), "Click")]
  public abstract class TriggerAction : Animatable, IAttachedObject
  {
    private bool isHosted;
    private DependencyObject associatedObject;
    private Type associatedObjectTypeConstraint;
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(nameof (IsEnabled), typeof (bool), typeof (TriggerAction), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));

    public bool IsEnabled
    {
      get => (bool) this.GetValue(TriggerAction.IsEnabledProperty);
      set => this.SetValue(TriggerAction.IsEnabledProperty, (object) value);
    }

    protected DependencyObject AssociatedObject
    {
      get
      {
        this.ReadPreamble();
        return this.associatedObject;
      }
    }

    protected virtual Type AssociatedObjectTypeConstraint
    {
      get
      {
        this.ReadPreamble();
        return this.associatedObjectTypeConstraint;
      }
    }

    internal bool IsHosted
    {
      get
      {
        this.ReadPreamble();
        return this.isHosted;
      }
      set
      {
        this.WritePreamble();
        this.isHosted = value;
        this.WritePostscript();
      }
    }

    internal TriggerAction(Type associatedObjectTypeConstraint)
    {
      this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
    }

    internal void CallInvoke(object parameter)
    {
      if (!this.IsEnabled)
        return;
      this.Invoke(parameter);
    }

    protected abstract void Invoke(object parameter);

    protected virtual void OnAttached()
    {
    }

    protected virtual void OnDetaching()
    {
    }

    protected override Freezable CreateInstanceCore()
    {
      return (Freezable) Activator.CreateInstance(this.GetType());
    }

    DependencyObject IAttachedObject.AssociatedObject => this.AssociatedObject;

    public void Attach(DependencyObject dependencyObject)
    {
      if (dependencyObject == this.AssociatedObject)
        return;
      if (this.AssociatedObject != null)
        throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
      if (dependencyObject != null && !this.AssociatedObjectTypeConstraint.IsAssignableFrom(dependencyObject.GetType()))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.TypeConstraintViolatedExceptionMessage, new object[3]
        {
          (object) this.GetType().Name,
          (object) dependencyObject.GetType().Name,
          (object) this.AssociatedObjectTypeConstraint.Name
        }));
      this.WritePreamble();
      this.associatedObject = dependencyObject;
      this.WritePostscript();
      this.OnAttached();
    }

    public void Detach()
    {
      this.OnDetaching();
      this.WritePreamble();
      this.associatedObject = (DependencyObject) null;
      this.WritePostscript();
    }
  }
}
