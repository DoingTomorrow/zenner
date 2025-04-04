// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.TriggerBase
// Assembly: System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 794D0242-5078-4CF1-BEBC-5ADC9BB01BDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Windows.Interactivity.dll

using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media.Animation;

#nullable disable
namespace System.Windows.Interactivity
{
  [ContentProperty("Actions")]
  public abstract class TriggerBase : Animatable, IAttachedObject
  {
    private DependencyObject associatedObject;
    private Type associatedObjectTypeConstraint;
    private static readonly DependencyPropertyKey ActionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof (Actions), typeof (TriggerActionCollection), typeof (TriggerBase), (PropertyMetadata) new FrameworkPropertyMetadata());
    public static readonly DependencyProperty ActionsProperty = TriggerBase.ActionsPropertyKey.DependencyProperty;

    internal TriggerBase(Type associatedObjectTypeConstraint)
    {
      this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
      TriggerActionCollection actionCollection = new TriggerActionCollection();
      this.SetValue(TriggerBase.ActionsPropertyKey, (object) actionCollection);
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

    public TriggerActionCollection Actions
    {
      get => (TriggerActionCollection) this.GetValue(TriggerBase.ActionsProperty);
    }

    public event EventHandler<PreviewInvokeEventArgs> PreviewInvoke;

    protected void InvokeActions(object parameter)
    {
      if (this.PreviewInvoke != null)
      {
        PreviewInvokeEventArgs e = new PreviewInvokeEventArgs();
        this.PreviewInvoke((object) this, e);
        if (e.Cancelling)
          return;
      }
      foreach (TriggerAction action in (FreezableCollection<TriggerAction>) this.Actions)
        action.CallInvoke(parameter);
    }

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
        throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerMultipleTimesExceptionMessage);
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
      this.Actions.Attach(dependencyObject);
      this.OnAttached();
    }

    public void Detach()
    {
      this.OnDetaching();
      this.WritePreamble();
      this.associatedObject = (DependencyObject) null;
      this.WritePostscript();
      this.Actions.Detach();
    }
  }
}
