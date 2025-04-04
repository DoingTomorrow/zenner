// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.DialogParticipation
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace MahApps.Metro.Controls.Dialogs
{
  public static class DialogParticipation
  {
    private static readonly IDictionary<object, DependencyObject> ContextRegistrationIndex = (IDictionary<object, DependencyObject>) new Dictionary<object, DependencyObject>();
    public static readonly DependencyProperty RegisterProperty = DependencyProperty.RegisterAttached("Register", typeof (object), typeof (DialogParticipation), new PropertyMetadata((object) null, new PropertyChangedCallback(DialogParticipation.RegisterPropertyChangedCallback)));

    private static void RegisterPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      if (dependencyPropertyChangedEventArgs.OldValue != null)
        DialogParticipation.ContextRegistrationIndex.Remove(dependencyPropertyChangedEventArgs.OldValue);
      if (dependencyPropertyChangedEventArgs.NewValue == null)
        return;
      DialogParticipation.ContextRegistrationIndex[dependencyPropertyChangedEventArgs.NewValue] = dependencyObject;
    }

    public static void SetRegister(DependencyObject element, object context)
    {
      element.SetValue(DialogParticipation.RegisterProperty, context);
    }

    public static object GetRegister(DependencyObject element)
    {
      return element.GetValue(DialogParticipation.RegisterProperty);
    }

    internal static bool IsRegistered(object context)
    {
      return context != null ? DialogParticipation.ContextRegistrationIndex.ContainsKey(context) : throw new ArgumentNullException(nameof (context));
    }

    internal static DependencyObject GetAssociation(object context)
    {
      return context != null ? DialogParticipation.ContextRegistrationIndex[context] : throw new ArgumentNullException(nameof (context));
    }
  }
}
