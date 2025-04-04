// Decompiled with JetBrains decompiler
// Type: WpfKb.Behaviors.BehaviorBase`1
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System;
using System.Windows;
using System.Windows.Interactivity;

#nullable disable
namespace WpfKb.Behaviors
{
  public abstract class BehaviorBase<T> : Behavior<T> where T : FrameworkElement
  {
    private bool _isSetup = true;
    private bool _isHookedUp;
    private WeakReference _weakTarget;

    protected virtual void OnSetup()
    {
    }

    protected virtual void OnCleanup()
    {
    }

    protected override void OnChanged()
    {
      T associatedObject = this.AssociatedObject;
      if ((object) associatedObject != null)
        this.HookupBehavior(associatedObject);
      else
        this.UnHookupBehavior();
    }

    private void OnTarget_Loaded(object sender, RoutedEventArgs e) => this.SetupBehavior();

    private void OnTarget_Unloaded(object sender, RoutedEventArgs e) => this.CleanupBehavior();

    private void HookupBehavior(T target)
    {
      if (this._isHookedUp)
        return;
      this._weakTarget = new WeakReference((object) target);
      this._isHookedUp = true;
      target.Unloaded += new RoutedEventHandler(this.OnTarget_Unloaded);
      target.Loaded += new RoutedEventHandler(this.OnTarget_Loaded);
      this.SetupBehavior();
    }

    private void UnHookupBehavior()
    {
      if (!this._isHookedUp)
        return;
      this._isHookedUp = false;
      T obj = this.AssociatedObject ?? (T) this._weakTarget.Target;
      if ((object) obj != null)
      {
        obj.Unloaded -= new RoutedEventHandler(this.OnTarget_Unloaded);
        obj.Loaded -= new RoutedEventHandler(this.OnTarget_Loaded);
      }
      this.CleanupBehavior();
    }

    private void SetupBehavior()
    {
      if (this._isSetup)
        return;
      this._isSetup = true;
      this.OnSetup();
    }

    private void CleanupBehavior()
    {
      if (!this._isSetup)
        return;
      this._isSetup = false;
      this.OnCleanup();
    }
  }
}
