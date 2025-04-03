// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.DeviceViewModels.EditDeviceViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using Microsoft.Win32;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Modules.Configuration;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.LicenseManagement;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.Core.Utils;
using MSS.DIConfiguration;
using MSS.DTO.Meters;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.Structures.Helpers.DeviceHelpers;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.Structures.DeviceViewModels
{
  public class EditDeviceViewModel : DeviceViewModel
  {
    private string _oldMeterName;
    private string _oldMeterSerialNumber;
    private string _oldMeterPrimaryAddress;
    private List<ReplacementMeterDropDownValue> _availableMetersToReplaceWith;

    public EditDeviceViewModel(
      DeviceStateEnum deviceState,
      StructureNodeDTO node,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      List<string> serialNumberList)
      : base(deviceState, node, repositoryFactory, windowFactory, serialNumberList)
    {
      this.MeterDialogTitle = node.NodeType.Name == "Meter" ? CultureResources.GetValue("MSS_Client_Structures_EditDevice_Title") : CultureResources.GetValue("MSS_Client_Structures_EditRadioDevice_Title");
      if (node.StructureType.HasValue && node.StructureType.Value == StructureTypeEnum.Fixed)
        this.MeterDialogTitle += this.GetTenantTitleString();
      this.IsAddMeterButtonVisible = false;
      this.IsEditMeterButtonVisible = true;
      this.IsExistingMeter = true;
      StructureTypeEnum? structureType = node.StructureType;
      StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Fixed;
      this.AreReadWriteButtonsVisibile = (structureType.GetValueOrDefault() == structureTypeEnum1 ? (!structureType.HasValue ? 1 : 0) : 1) != 0 && node.NodeType.Name != "RadioMeter";
      DeviceGroup deviceGroup = new DeviceGroup();
      DeviceModel deviceModel = new DeviceModel();
      if (node.Entity != null)
      {
        if (node.Entity is MeterDTO entity && LicenseHelper.GetDeviceTypes().Contains<string>(entity.DeviceType.GetGMMDeviceModelName()))
        {
          GMMHelper.GetDeviceGroupAndModelBasedOnDeviceType(entity.DeviceType, ref deviceGroup, ref deviceModel);
          this.SelectedDeviceGroup = deviceGroup;
          this.SelectedDeviceType = this.DeviceTypeCollection.FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (item => item.Name == deviceModel.Name));
        }
        else
          Application.Current.Dispatcher.Invoke((Action) (() => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_DeleteStructure_Warning_Title), (IParameter) new ConstructorArgument("message", (object) (Resources.MSS_Client_License_DeviceModelAndGroupNotAvailable + Environment.NewLine + Resources.MSS_Client_ClickOkToContinue)), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)))));
        this.SerialNo = entity.SerialNumber;
        this.AnteriorSerialNumber = entity.SerialNumber;
        this.ShortDeviceNo = entity.ShortDeviceNo;
        this.CompleteDeviceId = entity.CompletDevice;
        RoomType room = entity.Room;
        this.SelectedRoomTypeId = new Guid?(room != null ? room.Id : Guid.Empty);
        double? startValue = entity.StartValue;
        string empty;
        if (!startValue.HasValue)
        {
          empty = string.Empty;
        }
        else
        {
          startValue = entity.StartValue;
          empty = startValue.ToString();
        }
        this.StartValue = empty;
        this.EvaluationFactor = entity.EvaluationFactor;
        MeasureUnit readingUnit = entity.ReadingUnit;
        this.SelectedStartValueUnitId = new Guid?(readingUnit != null ? readingUnit.Id : Guid.Empty);
        this.Impulses = entity.ImpulsValue;
        MeasureUnit impulsUnit = entity.ImpulsUnit;
        this.SelectedImpulsUnitId = new Guid?(impulsUnit != null ? impulsUnit.Id : Guid.Empty);
        Channel channel = entity.Channel;
        this.SelectedChannelId = new Guid?(channel != null ? channel.Id : Guid.Empty);
        ConnectedDeviceType connectedDeviceType = entity.ConnectedDeviceType;
        this.SelectedConnectedDeviceTypeId = new Guid?(connectedDeviceType != null ? connectedDeviceType.Id : Guid.Empty);
        this.ConfigValuesCollection = entity.GMMParameters != null ? ConfigurationHelper.DeserializeConfigurationParameters(entity.GMMParameters) : new List<ConfigurationPerChannel>();
        this.AES = entity.AES;
        this.PrimaryAddress = entity.PrimaryAddress;
        this.Manufacturer = entity.Manufacturer;
        this.InputNumber = entity.InputNumber;
        this.Medium = entity.Medium.ToString();
        this.Generation = entity.Generation;
        this.IsReplaced = entity.IsReplaced;
        this.MeterReplacementHistoryList = entity.MeterReplacementHistoryList;
        this.ReplacedMeterId = entity.ReplacedMeterId;
        string propertyFromDeviceInfo = GMMHelper.GetPropertyFromDeviceInfo(entity.GMMAdditionalInfo, "ZDF:");
        if (propertyFromDeviceInfo != null)
          this.DeviceInfoCollection = ParameterService.GetAllParametersAsList(propertyFromDeviceInfo.TrimEnd('\r'), ';').OrderBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (n => n.Key)).ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>) (k => k.Key), (Func<KeyValuePair<string, string>, string>) (y => y.Value));
        this.IsReadingEnabled = entity.ReadingEnabled;
      }
      this.Name = node.Name;
      this.Description = node.Description;
      if (node.NodeType.Name == "RadioMeter" && node.Entity is MeterDTO entity1 && entity1.MbusRadioMeter != null)
      {
        MbusRadioMeter mbusRadioMeter = entity1.MbusRadioMeter;
        this.City = mbusRadioMeter.City;
        this.Street = mbusRadioMeter.Street;
        this.HouseNumber = mbusRadioMeter.HouseNumber;
        this.HouseNumberSupplement = mbusRadioMeter.HouseNumberSupplement;
        this.ApartmentNumber = mbusRadioMeter.ApartmentNumber;
        this.ZipCode = mbusRadioMeter.ZipCode;
        this.FirstName = mbusRadioMeter.FirstName;
        this.LastName = mbusRadioMeter.LastName;
        this.Location = mbusRadioMeter.Location;
        this.RadioSerialNumber = mbusRadioMeter.RadioSerialNumber;
      }
      this.isSaveAndCreateNewFirstTimePressed = true;
      this.structureBehaviour.InitializeDeviceViewModel((DeviceViewModel) this, deviceState);
      structureType = node.StructureType;
      int num;
      if (structureType.HasValue)
      {
        structureType = node.RootNode.StructureType;
        StructureTypeEnum structureTypeEnum2 = StructureTypeEnum.Physical;
        if ((structureType.GetValueOrDefault() == structureTypeEnum2 ? (structureType.HasValue ? 1 : 0) : 0) != 0)
        {
          num = node.Id != Guid.Empty ? 1 : 0;
          goto label_18;
        }
      }
      num = 0;
label_18:
      this.IsMeterReplacementVisible = num != 0;
      this.IsReplaceMeterButtonVisible = this.IsMeterReplacementVisible;
      this.OldMeterName = node.Name;
      this.OldMeterSerialNumber = node.Entity is MeterDTO entity2 ? entity2.SerialNumber : (string) null;
      this.OldMeterPrimaryAddress = node.Entity is MeterDTO entity3 ? entity3.PrimaryAddress.ToString() : (string) null;
      this.InitAvailableMetersDropDown();
    }

    public string OldMeterName
    {
      get => this._oldMeterName;
      set
      {
        this._oldMeterName = value;
        this.OnPropertyChanged(nameof (OldMeterName));
      }
    }

    public string OldMeterSerialNumber
    {
      get => this._oldMeterSerialNumber;
      set
      {
        this._oldMeterSerialNumber = value;
        this.OnPropertyChanged(nameof (OldMeterSerialNumber));
      }
    }

    public string OldMeterPrimaryAddress
    {
      get => this._oldMeterPrimaryAddress;
      set
      {
        this._oldMeterPrimaryAddress = value;
        this.OnPropertyChanged(nameof (OldMeterPrimaryAddress));
      }
    }

    public List<ReplacementMeterDropDownValue> AvailableMetersToReplaceWith
    {
      get => this._availableMetersToReplaceWith;
      set
      {
        this._availableMetersToReplaceWith = value;
        this.OnPropertyChanged(nameof (AvailableMetersToReplaceWith));
      }
    }

    private void InitAvailableMetersDropDown()
    {
      this.AvailableMetersToReplaceWith = new List<ReplacementMeterDropDownValue>();
      if (this._selectedNode.ParentNode == null)
        return;
      List<string> deviceModelNameList = GMMHelper.GetDeviceModelNameList(this._selectedNode.ParentNode.StructureType);
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) this._selectedNode.ParentNode.SubNodes)
      {
        if (subNode.Id != Guid.Empty && subNode != this._selectedNode && subNode.Entity != null && subNode.Entity is MeterDTO && GMMHelper.IsDeviceIncludedInLicense(new DeviceTypeEnum?((subNode.Entity as MeterDTO).DeviceType), deviceModelNameList))
          this.AvailableMetersToReplaceWith.Add(new ReplacementMeterDropDownValue()
          {
            MeterStructureNode = subNode,
            DisplayValue = (subNode.Entity as MeterDTO).SerialNumber + "   " + (object) (subNode.Entity as MeterDTO).PrimaryAddress
          });
      }
      if (this.AvailableMetersToReplaceWith.Any<ReplacementMeterDropDownValue>())
      {
        this.AvailableMetersToReplaceWith = this.AvailableMetersToReplaceWith.OrderBy<ReplacementMeterDropDownValue, string>((Func<ReplacementMeterDropDownValue, string>) (item => !(item.MeterStructureNode.Entity is MeterDTO entity) ? (string) null : entity.SerialNumber)).ToList<ReplacementMeterDropDownValue>();
        this.AvailableMetersToReplaceWith.Insert(0, (ReplacementMeterDropDownValue) null);
      }
    }

    public ICommand ImportTranslationRulesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          ViewModelBase viewModelBase = (ViewModelBase) null;
          Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
          OpenFileDialog openFileDialog = new OpenFileDialog()
          {
            Filter = "Translation Rules (*.xml)|*.xml|All files (*.*)|*.*",
            Title = "Import translation rules",
            RestoreDirectory = true
          };
          bool? nullable = openFileDialog.ShowDialog();
          if (nullable.HasValue && nullable.Value && !string.IsNullOrEmpty(openFileDialog.FileName))
          {
            try
            {
              TranslationRulesManager.ImportRulesIntoDatabase(openFileDialog.FileName);
              viewModelBase = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
            }
            catch (Exception ex)
            {
              this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_Warning_Title), (IParameter) new ConstructorArgument("message", (object) ex.Message), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
            }
          }
          else
            viewModelBase = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
          this.MessageUserControl = viewModelBase;
        }));
      }
    }
  }
}
