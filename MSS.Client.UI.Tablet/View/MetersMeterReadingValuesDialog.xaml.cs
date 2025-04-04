// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Meters.MeterReadingValuesDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using Microsoft.CSharp.RuntimeBinder;
using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Meters
{
  public partial class MeterReadingValuesDialog : ResizableMetroWindow, IComponentConnector
  {
    internal RadGridView RadioTestRunGridView;
    internal Button exportButton;
    internal Button exportCsvButton;
    internal Button exportPdfButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public MeterReadingValuesDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~MeterReadingValuesDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.exportButton.TouchEnter -= new EventHandler<TouchEventArgs>(this.ExportButton_OnTouchEnter);
      this.exportButton.Click -= new RoutedEventHandler(this.ExportButton_OnClick);
      this.exportCsvButton.TouchEnter -= new EventHandler<TouchEventArgs>(this.ExportCsvButton_OnTouchEnter);
      this.exportCsvButton.Click -= new RoutedEventHandler(this.ExportCsvButton_OnClick);
      this.exportPdfButton.TouchEnter -= new EventHandler<TouchEventArgs>(this.ExportPdfButton_OnTouchEnter);
      this.exportPdfButton.Click -= new RoutedEventHandler(this.ExportPdfButton_OnClick);
    }

    private void ExportButton_OnTouchEnter(object sender, TouchEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, DataItemCollection, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FilteredRows", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__0, this.DataContext, this.RadioTestRunGridView.Items);
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CanExecute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, CompositeFilterDescriptorCollection, object> target2 = MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>> p2 = MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__1.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__1, this.DataContext);
      CompositeFilterDescriptorCollection filterDescriptors1 = this.RadioTestRunGridView.FilterDescriptors;
      object obj3 = target2((CallSite) p2, obj2, filterDescriptors1);
      if (target1((CallSite) p3, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, CompositeFilterDescriptorCollection> target3 = MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>> p5 = MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__4.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__2.\u003C\u003Ep__4, this.DataContext);
        CompositeFilterDescriptorCollection filterDescriptors2 = this.RadioTestRunGridView.FilterDescriptors;
        target3((CallSite) p5, obj4, filterDescriptors2);
      }
      e.Handled = false;
    }

    private void ExportButton_OnClick(object sender, RoutedEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, DataItemCollection, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FilteredRows", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__0, this.DataContext, this.RadioTestRunGridView.Items);
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CanExecute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, CompositeFilterDescriptorCollection, object> target2 = MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>> p2 = MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__1.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__1, this.DataContext);
      CompositeFilterDescriptorCollection filterDescriptors1 = this.RadioTestRunGridView.FilterDescriptors;
      object obj3 = target2((CallSite) p2, obj2, filterDescriptors1);
      if (target1((CallSite) p3, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, CompositeFilterDescriptorCollection> target3 = MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>> p5 = MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__4.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__3.\u003C\u003Ep__4, this.DataContext);
        CompositeFilterDescriptorCollection filterDescriptors2 = this.RadioTestRunGridView.FilterDescriptors;
        target3((CallSite) p5, obj4, filterDescriptors2);
      }
      e.Handled = false;
    }

    private void ExportCsvButton_OnTouchEnter(object sender, TouchEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, DataItemCollection, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FilteredRows", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__0, this.DataContext, this.RadioTestRunGridView.Items);
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CanExecute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, CompositeFilterDescriptorCollection, object> target2 = MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>> p2 = MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueToCSVCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__1.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__1, this.DataContext);
      CompositeFilterDescriptorCollection filterDescriptors1 = this.RadioTestRunGridView.FilterDescriptors;
      object obj3 = target2((CallSite) p2, obj2, filterDescriptors1);
      if (target1((CallSite) p3, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, CompositeFilterDescriptorCollection> target3 = MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>> p5 = MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueToCSVCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__4.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__4.\u003C\u003Ep__4, this.DataContext);
        CompositeFilterDescriptorCollection filterDescriptors2 = this.RadioTestRunGridView.FilterDescriptors;
        target3((CallSite) p5, obj4, filterDescriptors2);
      }
      e.Handled = false;
    }

    private void ExportCsvButton_OnClick(object sender, RoutedEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, DataItemCollection, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FilteredRows", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__0, this.DataContext, this.RadioTestRunGridView.Items);
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CanExecute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, CompositeFilterDescriptorCollection, object> target2 = MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>> p2 = MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueToCSVCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__1.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__1, this.DataContext);
      CompositeFilterDescriptorCollection filterDescriptors1 = this.RadioTestRunGridView.FilterDescriptors;
      object obj3 = target2((CallSite) p2, obj2, filterDescriptors1);
      if (target1((CallSite) p3, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, CompositeFilterDescriptorCollection> target3 = MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>> p5 = MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueToCSVCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__4.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__5.\u003C\u003Ep__4, this.DataContext);
        CompositeFilterDescriptorCollection filterDescriptors2 = this.RadioTestRunGridView.FilterDescriptors;
        target3((CallSite) p5, obj4, filterDescriptors2);
      }
      e.Handled = false;
    }

    private void ExportPdfButton_OnTouchEnter(object sender, TouchEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, DataItemCollection, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FilteredRows", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__0.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__0, this.DataContext, this.RadioTestRunGridView.Items);
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CanExecute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, CompositeFilterDescriptorCollection, object> target2 = MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>> p2 = MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueToPDFCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__1.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__1, this.DataContext);
      CompositeFilterDescriptorCollection filterDescriptors1 = this.RadioTestRunGridView.FilterDescriptors;
      object obj3 = target2((CallSite) p2, obj2, filterDescriptors1);
      if (target1((CallSite) p3, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, CompositeFilterDescriptorCollection> target3 = MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>> p5 = MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueToPDFCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__4.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__6.\u003C\u003Ep__4, this.DataContext);
        CompositeFilterDescriptorCollection filterDescriptors2 = this.RadioTestRunGridView.FilterDescriptors;
        target3((CallSite) p5, obj4, filterDescriptors2);
      }
      e.Handled = false;
    }

    private void ExportPdfButton_OnClick(object sender, RoutedEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, DataItemCollection, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FilteredRows", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__0, this.DataContext, this.RadioTestRunGridView.Items);
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CanExecute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, CompositeFilterDescriptorCollection, object> target2 = MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, CompositeFilterDescriptorCollection, object>> p2 = MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueToPDFCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__1.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__1, this.DataContext);
      CompositeFilterDescriptorCollection filterDescriptors1 = this.RadioTestRunGridView.FilterDescriptors;
      object obj3 = target2((CallSite) p2, obj2, filterDescriptors1);
      if (target1((CallSite) p3, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, CompositeFilterDescriptorCollection> target3 = MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, CompositeFilterDescriptorCollection>> p5 = MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ExportReadingValueToPDFCommand", typeof (MeterReadingValuesDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__4.Target((CallSite) MeterReadingValuesDialog.\u003C\u003Eo__7.\u003C\u003Ep__4, this.DataContext);
        CompositeFilterDescriptorCollection filterDescriptors2 = this.RadioTestRunGridView.FilterDescriptors;
        target3((CallSite) p5, obj4, filterDescriptors2);
      }
      e.Handled = false;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/meters/meterreadingvaluesdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.RadioTestRunGridView = (RadGridView) target;
          break;
        case 2:
          this.exportButton = (Button) target;
          this.exportButton.TouchEnter += new EventHandler<TouchEventArgs>(this.ExportButton_OnTouchEnter);
          this.exportButton.Click += new RoutedEventHandler(this.ExportButton_OnClick);
          break;
        case 3:
          this.exportCsvButton = (Button) target;
          this.exportCsvButton.TouchEnter += new EventHandler<TouchEventArgs>(this.ExportCsvButton_OnTouchEnter);
          this.exportCsvButton.Click += new RoutedEventHandler(this.ExportCsvButton_OnClick);
          break;
        case 4:
          this.exportPdfButton = (Button) target;
          this.exportPdfButton.TouchEnter += new EventHandler<TouchEventArgs>(this.ExportPdfButton_OnTouchEnter);
          this.exportPdfButton.Click += new RoutedEventHandler(this.ExportPdfButton_OnClick);
          break;
        case 5:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
