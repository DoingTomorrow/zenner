// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Settings.SettingsUserControl
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using Microsoft.CSharp.RuntimeBinder;
using MSS.DTO.Clients;
using MSS.DTO.MDM;
using MSS.DTO.Providers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Settings
{
  public partial class SettingsUserControl : UserControl, IComponentConnector
  {
    internal SettingsUserControl UserControlSettings;
    internal RadTabControl RadTabControlSettings;
    internal RadTabItem TabGeneralSettings;
    internal Grid GeneralSettingsGrid;
    internal Button UpdateGeneralSettings;
    internal RadComboBox ComboLanguage;
    internal TextBox BatchSizeTextBox;
    internal RadGridView UserDeviceTypesSettingsGridView;
    internal RadTabItem TabServerSettings;
    internal Grid ServerSettingsGrid;
    internal Button UpdateServerSettings;
    internal RadGridView ClientSettingsGrid;
    internal RadGridView MdmConfigsGridView;
    internal RadTabItem TabMinomatSettings;
    internal Grid MinomatSettingsGrid;
    internal Button UpdateMinomatSettings;
    internal TextBox PollingTextBox;
    internal CheckBox UseMinomatPoolCheckBox;
    internal RadGridView CountriesGridView;
    internal RadGridView ProvidersGridView;
    internal RadTabItem TabItemEquipmentSettings;
    internal Grid EquipmentSettingsGrid;
    internal Button UpdateEquipmentSettings;
    internal Grid EquipmentGrid;
    internal RadComboBox SelectedEquipmentGroupComboBox;
    internal RadComboBox SelectedEquipmentModelComboBox;
    internal ToggleButton equipmentModelButton;
    internal Popup popupEquipmentModel;
    internal StackPanel StaticStackPanel;
    internal Button RefreshPortsButton;
    internal CheckBox ExpertConfigurationModeCheckbox;
    private bool _contentLoaded;

    public SettingsUserControl()
    {
      this.InitializeComponent();
      Windows8Palette.Palette.AccentColor = Color.FromRgb((byte) 15, (byte) 95, (byte) 142);
      this.ClientSettingsGrid.AddHandler(Selector.SelectionChangedEvent, (Delegate) new SelectionChangedEventHandler(this.OnSelectionChanged));
      this.ProvidersGridView.Deleting += new EventHandler<GridViewDeletingEventArgs>(this.ProvidersGridView_Deleting);
      this.ProvidersGridView.RowEditEnded += new EventHandler<GridViewRowEditEndedEventArgs>(this.ProvidersGridView_Editing);
      this.MdmConfigsGridView.Deleting += new EventHandler<GridViewDeletingEventArgs>(this.MdmConfigsGridView_Deleting);
      this.MdmConfigsGridView.RowEditEnded += new EventHandler<GridViewRowEditEndedEventArgs>(this.MdmConfigsGridView_Editing);
    }

    ~SettingsUserControl()
    {
      this.BatchSizeTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
      this.PollingTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
      this.ProvidersGridView.Deleting -= new EventHandler<GridViewDeletingEventArgs>(this.ProvidersGridView_Deleting);
      this.ProvidersGridView.RowEditEnded -= new EventHandler<GridViewRowEditEndedEventArgs>(this.ProvidersGridView_Editing);
      this.MdmConfigsGridView.Deleting -= new EventHandler<GridViewDeletingEventArgs>(this.MdmConfigsGridView_Deleting);
      this.MdmConfigsGridView.RowEditEnded -= new EventHandler<GridViewRowEditEndedEventArgs>(this.MdmConfigsGridView_Editing);
      this.ClientSettingsGrid.RemoveHandler(Selector.SelectionChangedEvent, (Delegate) new SelectionChangedEventHandler(this.OnSelectionChanged));
      this.ClientSettingsGrid.MouseWheel -= new MouseWheelEventHandler(this.Grid_MouseWheel);
      this.MdmConfigsGridView.MouseWheel -= new MouseWheelEventHandler(this.Grid_MouseWheel);
      this.CountriesGridView.MouseWheel -= new MouseWheelEventHandler(this.Grid_MouseWheel);
      this.ProvidersGridView.MouseWheel -= new MouseWheelEventHandler(this.Grid_MouseWheel);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = SettingsUserControl.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    private void ProvidersGridView_Deleting(object sender, GridViewDeletingEventArgs e)
    {
      object dataContext = this.DataContext;
      ProviderDTO providerDto1 = e.Items.First<object>() as ProviderDTO;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, ProviderDTO, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CanExecute", (IEnumerable<Type>) null, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, ProviderDTO, object> target2 = SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, ProviderDTO, object>> p1 = SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "DeleteCommand", typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__0, dataContext);
      ProviderDTO providerDto2 = providerDto1;
      object obj2 = target2((CallSite) p1, obj1, providerDto2);
      if (!target1((CallSite) p2, obj2))
        return;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, ProviderDTO>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, ProviderDTO> target3 = SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, ProviderDTO>> p4 = SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "DeleteCommand", typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__3.Target((CallSite) SettingsUserControl.\u003C\u003Eo__4.\u003C\u003Ep__3, dataContext);
      ProviderDTO providerDto3 = providerDto1;
      target3((CallSite) p4, obj3, providerDto3);
    }

    private void ProvidersGridView_Editing(object sender, GridViewRowEditEndedEventArgs e)
    {
      object dataContext = this.DataContext;
      ProviderDTO editedItem = e.EditedItem as ProviderDTO;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, ProviderDTO, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CanExecute", (IEnumerable<Type>) null, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, ProviderDTO, object> target2 = SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, ProviderDTO, object>> p1 = SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InsertOrEditCommand", typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__0, dataContext);
      ProviderDTO providerDto1 = editedItem;
      object obj2 = target2((CallSite) p1, obj1, providerDto1);
      if (!target1((CallSite) p2, obj2))
        return;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, ProviderDTO>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, ProviderDTO> target3 = SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, ProviderDTO>> p4 = SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InsertOrEditCommand", typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__3.Target((CallSite) SettingsUserControl.\u003C\u003Eo__5.\u003C\u003Ep__3, dataContext);
      ProviderDTO providerDto2 = editedItem;
      target3((CallSite) p4, obj3, providerDto2);
    }

    private void MdmConfigsGridView_Deleting(object sender, GridViewDeletingEventArgs e)
    {
      object dataContext = this.DataContext;
      MDMConfigsDTO mdmConfigsDto1 = e.Items.First<object>() as MDMConfigsDTO;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, MDMConfigsDTO, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CanExecute", (IEnumerable<Type>) null, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, MDMConfigsDTO, object> target2 = SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, MDMConfigsDTO, object>> p1 = SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "DeleteMdmConfigCommand", typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__0.Target((CallSite) SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__0, dataContext);
      MDMConfigsDTO mdmConfigsDto2 = mdmConfigsDto1;
      object obj2 = target2((CallSite) p1, obj1, mdmConfigsDto2);
      if (!target1((CallSite) p2, obj2))
        return;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, MDMConfigsDTO>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, MDMConfigsDTO> target3 = SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, MDMConfigsDTO>> p4 = SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "DeleteMdmConfigCommand", typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__3.Target((CallSite) SettingsUserControl.\u003C\u003Eo__6.\u003C\u003Ep__3, dataContext);
      MDMConfigsDTO mdmConfigsDto3 = mdmConfigsDto1;
      target3((CallSite) p4, obj3, mdmConfigsDto3);
    }

    private void MdmConfigsGridView_Editing(object sender, GridViewRowEditEndedEventArgs e)
    {
      object dataContext = this.DataContext;
      MDMConfigsDTO editedItem = e.EditedItem as MDMConfigsDTO;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, MDMConfigsDTO, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CanExecute", (IEnumerable<Type>) null, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, MDMConfigsDTO, object> target2 = SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, MDMConfigsDTO, object>> p1 = SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InsertOrEditMdmConfigCommand", typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__0, dataContext);
      MDMConfigsDTO mdmConfigsDto1 = editedItem;
      object obj2 = target2((CallSite) p1, obj1, mdmConfigsDto1);
      if (!target1((CallSite) p2, obj2))
        return;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, MDMConfigsDTO>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, MDMConfigsDTO> target3 = SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, MDMConfigsDTO>> p4 = SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InsertOrEditMdmConfigCommand", typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__3.Target((CallSite) SettingsUserControl.\u003C\u003Eo__7.\u003C\u003Ep__3, dataContext);
      MDMConfigsDTO mdmConfigsDto2 = editedItem;
      target3((CallSite) p4, obj3, mdmConfigsDto2);
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      RadGridView radGridView = sender as RadGridView;
      if (radGridView.SelectedItems.Count<object>() == 0)
        return;
      MyClient selectedItem = radGridView.SelectedItems[0] as MyClient;
      if (e.AddedItems.Count != 0 && (e.OriginalSource as RadComboBox).IsDropDownOpen)
      {
        switch (((EnumObj) e.AddedItems[0]).IdEnum)
        {
          case 0:
            selectedItem.IconUrl = "pack://application:,,,/Styles;component/Images/Settings/light-green.png";
            break;
          case 1:
            selectedItem.IconUrl = "pack://application:,,,/Styles;component/Images/Settings/light-red.png";
            break;
          case 2:
            selectedItem.IconUrl = "pack://application:,,,/Styles;component/Images/Settings/light-yellow.png";
            break;
        }
      }
    }

    private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      this.ClientSettingsGrid.CommitEdit();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "RefreshPorts", (IEnumerable<Type>) null, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      SettingsUserControl.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) SettingsUserControl.\u003C\u003Eo__10.\u003C\u003Ep__0, this.DataContext);
    }

    private void Button_TouchDown(object sender, TouchEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (SettingsUserControl.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SettingsUserControl.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "RefreshPorts", (IEnumerable<Type>) null, typeof (SettingsUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      SettingsUserControl.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) SettingsUserControl.\u003C\u003Eo__11.\u003C\u003Ep__0, this.DataContext);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/settings/settingsusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.UserControlSettings = (SettingsUserControl) target;
          break;
        case 2:
          this.RadTabControlSettings = (RadTabControl) target;
          break;
        case 3:
          this.TabGeneralSettings = (RadTabItem) target;
          break;
        case 4:
          this.GeneralSettingsGrid = (Grid) target;
          break;
        case 5:
          this.UpdateGeneralSettings = (Button) target;
          break;
        case 6:
          this.ComboLanguage = (RadComboBox) target;
          break;
        case 7:
          this.BatchSizeTextBox = (TextBox) target;
          this.BatchSizeTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 8:
          this.UserDeviceTypesSettingsGridView = (RadGridView) target;
          break;
        case 9:
          this.TabServerSettings = (RadTabItem) target;
          break;
        case 10:
          this.ServerSettingsGrid = (Grid) target;
          break;
        case 11:
          this.UpdateServerSettings = (Button) target;
          break;
        case 12:
          this.ClientSettingsGrid = (RadGridView) target;
          this.ClientSettingsGrid.MouseWheel += new MouseWheelEventHandler(this.Grid_MouseWheel);
          break;
        case 13:
          this.MdmConfigsGridView = (RadGridView) target;
          this.MdmConfigsGridView.MouseWheel += new MouseWheelEventHandler(this.Grid_MouseWheel);
          break;
        case 14:
          this.TabMinomatSettings = (RadTabItem) target;
          break;
        case 15:
          this.MinomatSettingsGrid = (Grid) target;
          break;
        case 16:
          this.UpdateMinomatSettings = (Button) target;
          break;
        case 17:
          this.PollingTextBox = (TextBox) target;
          this.PollingTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 18:
          this.UseMinomatPoolCheckBox = (CheckBox) target;
          break;
        case 19:
          this.CountriesGridView = (RadGridView) target;
          this.CountriesGridView.MouseWheel += new MouseWheelEventHandler(this.Grid_MouseWheel);
          break;
        case 20:
          this.ProvidersGridView = (RadGridView) target;
          this.ProvidersGridView.MouseWheel += new MouseWheelEventHandler(this.Grid_MouseWheel);
          break;
        case 21:
          this.TabItemEquipmentSettings = (RadTabItem) target;
          break;
        case 22:
          this.EquipmentSettingsGrid = (Grid) target;
          break;
        case 23:
          this.UpdateEquipmentSettings = (Button) target;
          break;
        case 24:
          this.EquipmentGrid = (Grid) target;
          break;
        case 25:
          this.SelectedEquipmentGroupComboBox = (RadComboBox) target;
          break;
        case 26:
          this.SelectedEquipmentModelComboBox = (RadComboBox) target;
          break;
        case 27:
          this.equipmentModelButton = (ToggleButton) target;
          break;
        case 28:
          this.popupEquipmentModel = (Popup) target;
          break;
        case 29:
          this.StaticStackPanel = (StackPanel) target;
          break;
        case 30:
          this.RefreshPortsButton = (Button) target;
          break;
        case 31:
          this.ExpertConfigurationModeCheckbox = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
