// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Configuration.ConfigurationParametersVmPart
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Documents;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Client.UI.Common.Utils;
using MSS.DTO.MessageHandler;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MVVM.Converters;
using MVVM.ViewModel;
using Styles.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Telerik.Windows.Documents.FormatProviders;
using Telerik.Windows.Documents.Model;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.Configuration
{
  public class ConfigurationParametersVmPart : ValidationViewModelBase
  {
    private readonly IDeviceConfigurationParameterWriter _parameterWriter;
    private readonly IDeviceConfigurationParameterCollector _parameterCollector;
    private bool _isExportConfigurationParametersButtonEnabled;
    private bool _isConfigGeneralVisible;
    private bool _isConfigChannel1Visible;
    private bool _isConfigChannel2Visible;
    private bool _isConfigChannel3Visible;
    private bool _isGeneralTabSelected;
    private bool _isChannel1TabSelected;
    private bool _isChannel2TabSelected;
    private bool _isChannel3TabSelected;
    private string _configurationParameterDescription;
    private bool _isConfigurationParameterDescriptionVisible;
    private bool _isWriteButtonEnabled;
    private List<ConfigurationPerChannel> _configValuesCollection;

    public ConfigurationParametersVmPart(
      IDeviceConfigurationParameterCollector parameterCollector,
      IDeviceConfigurationParameterWriter parameterWriter)
    {
      this._parameterCollector = parameterCollector;
      this._parameterWriter = parameterWriter;
      this.ClearCurrentlyShownControls();
      EventPublisher.Register<ConfigurationParameterClicked>(new Action<ConfigurationParameterClicked>(this.ShowParameterDescription));
    }

    public bool IsExportConfigurationParametersButtonEnabled
    {
      get => this._isExportConfigurationParametersButtonEnabled;
      set
      {
        this._isExportConfigurationParametersButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsExportConfigurationParametersButtonEnabled));
      }
    }

    public bool IsConfigGeneralVisible
    {
      get => this._isConfigGeneralVisible;
      set
      {
        this._isConfigGeneralVisible = value;
        this.IsExportConfigurationParametersButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsConfigGeneralVisible));
      }
    }

    public bool IsConfigChannel1Visible
    {
      get => this._isConfigChannel1Visible;
      set
      {
        this._isConfigChannel1Visible = value;
        this.OnPropertyChanged(nameof (IsConfigChannel1Visible));
      }
    }

    public bool IsConfigChannel2Visible
    {
      get => this._isConfigChannel2Visible;
      set
      {
        this._isConfigChannel2Visible = value;
        this.OnPropertyChanged(nameof (IsConfigChannel2Visible));
      }
    }

    public bool IsConfigChannel3Visible
    {
      get => this._isConfigChannel3Visible;
      set
      {
        this._isConfigChannel3Visible = value;
        this.OnPropertyChanged(nameof (IsConfigChannel3Visible));
      }
    }

    public List<Config> DynamicGridTag { get; set; }

    public List<Config> Channel1DynamicGridTag { get; set; }

    public List<Config> Channel2DynamicGridTag { get; set; }

    public List<Config> Channel3DynamicGridTag { get; set; }

    public bool IsGeneralTabSelected
    {
      get => this._isGeneralTabSelected;
      set
      {
        this._isGeneralTabSelected = value;
        this.OnPropertyChanged(nameof (IsGeneralTabSelected));
      }
    }

    public bool IsChannel1TabSelected
    {
      get => this._isChannel1TabSelected;
      set
      {
        this._isChannel1TabSelected = value;
        this.OnPropertyChanged(nameof (IsChannel1TabSelected));
      }
    }

    public bool IsChannel2TabSelected
    {
      get => this._isChannel2TabSelected;
      set
      {
        this._isChannel2TabSelected = value;
        this.OnPropertyChanged(nameof (IsChannel2TabSelected));
      }
    }

    public bool IsChannel3TabSelected
    {
      get => this._isChannel3TabSelected;
      set
      {
        this._isChannel3TabSelected = value;
        this.OnPropertyChanged(nameof (IsChannel3TabSelected));
      }
    }

    public string ConfigurationParameterDescription
    {
      get => this._configurationParameterDescription;
      set
      {
        this._configurationParameterDescription = value;
        this.OnPropertyChanged(nameof (ConfigurationParameterDescription));
      }
    }

    public bool IsConfigurationParameterDescriptionVisible
    {
      get => this._isConfigurationParameterDescriptionVisible;
      set
      {
        this._isConfigurationParameterDescriptionVisible = value;
        this.OnPropertyChanged(nameof (IsConfigurationParameterDescriptionVisible));
      }
    }

    public bool IsWriteButtonEnabled
    {
      get => this._isWriteButtonEnabled;
      set
      {
        this._isWriteButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsWriteButtonEnabled));
      }
    }

    public List<ConfigurationPerChannel> ConfigValuesCollection
    {
      get => this._configValuesCollection;
      set
      {
        this._configValuesCollection = value;
        this.OnPropertyChanged(nameof (ConfigValuesCollection));
      }
    }

    private void ShowParameterDescription(ConfigurationParameterClicked ev)
    {
      if (string.IsNullOrWhiteSpace(ev.Description))
        return;
      this.IsConfigurationParameterDescriptionVisible = true;
      this.ConfigurationParameterDescription = ev.Description;
    }

    public void ClearCurrentlyShownControls()
    {
      this.IsConfigGeneralVisible = false;
      this.IsConfigChannel1Visible = false;
      this.IsConfigChannel2Visible = false;
      this.IsConfigChannel3Visible = false;
      this.IsConfigurationParameterDescriptionVisible = false;
      this.IsWriteButtonEnabled = false;
    }

    private void AjustIsConfigGeneralVisibleValues(int count)
    {
      if (count > 0)
        this.IsConfigGeneralVisible = true;
      if (count > 1)
        this.IsConfigChannel1Visible = true;
      if (count > 2)
        this.IsConfigChannel2Visible = true;
      if (count <= 3)
        return;
      this.IsConfigChannel3Visible = true;
    }

    private bool SetConfigParamsForTabs(
      object parameter,
      out SortedList<OverrideID, ConfigurationParameter> paramsChannel1List,
      out SortedList<OverrideID, ConfigurationParameter> paramsChannel2List,
      out SortedList<OverrideID, ConfigurationParameter> paramsChannel3List,
      out SortedList<OverrideID, ConfigurationParameter> paramsList)
    {
      paramsChannel1List = (SortedList<OverrideID, ConfigurationParameter>) null;
      paramsChannel2List = (SortedList<OverrideID, ConfigurationParameter>) null;
      paramsChannel3List = (SortedList<OverrideID, ConfigurationParameter>) null;
      paramsList = (SortedList<OverrideID, ConfigurationParameter>) null;
      if (!(parameter is CustomMultiBindingConverter.FindCommandParameters commandParameters))
        return true;
      if (commandParameters.Property1 is UIElementCollection property1 && property1.Count > 0)
      {
        paramsChannel1List = this.SetConfigurationParameters(this.Channel1DynamicGridTag, commandParameters.Property1);
        if (paramsChannel1List == null)
          return true;
      }
      if (commandParameters.Property2 is UIElementCollection property2 && property2.Count > 0)
      {
        paramsChannel2List = this.SetConfigurationParameters(this.Channel2DynamicGridTag, commandParameters.Property2);
        if (paramsChannel2List == null)
          return true;
      }
      if (commandParameters.Property3 is UIElementCollection property3 && property3.Count > 0)
      {
        paramsChannel3List = this.SetConfigurationParameters(this.Channel3DynamicGridTag, commandParameters.Property3);
        if (paramsChannel3List == null)
          return true;
      }
      paramsList = this.SetConfigurationParameters(this.DynamicGridTag, commandParameters.Property0);
      return paramsList.Count == 0;
    }

    private SortedList<OverrideID, ConfigurationParameter> SetConfigurationParameters(
      List<Config> dynamicGridTag,
      object parameter = null)
    {
      SortedList<OverrideID, ConfigurationParameter> sortedList = new SortedList<OverrideID, ConfigurationParameter>();
      if (parameter != null && parameter is UIElementCollection elementCollection)
      {
        GridControl grid = elementCollection[0] as GridControl;
        dynamicGridTag.UpdateCheckBoxInDynamicGrid(grid);
      }
      if (dynamicGridTag == null)
        return sortedList;
      foreach (Config config in dynamicGridTag.Where<Config>((Func<Config, bool>) (x => !x.IsReadOnly)).ToList<Config>())
      {
        KeyValuePair<OverrideID, ConfigurationParameter> parameter1 = (KeyValuePair<OverrideID, ConfigurationParameter>) config.Parameter;
        if (parameter1.Key != OverrideID.AESKey || !(config.PropertyValue == "ZENNER DEFAULT KEY"))
        {
          if (string.IsNullOrEmpty(config.PropertyValue))
          {
            sortedList.Add(parameter1.Key, parameter1.Value);
          }
          else
          {
            if (parameter1.Value.ParameterValue == null || parameter1.Value.ParameterValue.ToString() != config.PropertyValue)
              parameter1.Value.SetValueFromStringWin(config.PropertyValue);
            sortedList.Add(parameter1.Key, parameter1.Value);
          }
        }
      }
      return sortedList;
    }

    private List<Config> SortParameters(List<Config> parameters)
    {
      parameters = parameters == null || !(parameters[0].Parameter is KeyValuePair<OverrideID, ConfigurationParameter>) ? (parameters != null ? parameters.OrderByDescending<Config, bool>((Func<Config, bool>) (_ => _.IsReadOnly)).ThenBy<Config, string>((Func<Config, string>) (_ => _.PropertyName)).ToList<Config>() : (List<Config>) null) : (parameters != null ? parameters.OrderByDescending<Config, bool>((Func<Config, bool>) (_ => _.IsReadOnly)).ThenBy<Config, bool>((Func<Config, bool>) (_ => ((KeyValuePair<OverrideID, ConfigurationParameter>) _.Parameter).Value.IsFunction)).ThenBy<Config, string>((Func<Config, string>) (_ => _.PropertyName)).ToList<Config>() : (List<Config>) null);
      return parameters;
    }

    public MSS.DTO.Message.Message ExportConfigurationParametersToPdf(
      string selectedDeviceModelName,
      string safeFileName,
      Stream outputStream)
    {
      RadDocument configurationParameters = GeneratePdfFromData.GeneratePdfFromConfigurationParameters(this.SortParameters(this.DynamicGridTag), this.SortParameters(this.Channel1DynamicGridTag), this.SortParameters(this.Channel2DynamicGridTag), this.SortParameters(this.Channel3DynamicGridTag), selectedDeviceModelName);
      IDocumentFormatProvider providerByExtension = DocumentFormatProvidersManager.GetProviderByExtension(Path.GetExtension(safeFileName));
      if (providerByExtension == null)
        return new MSS.DTO.Message.Message(MessageTypeEnum.Warning, Resources.MSS_Client_UnsupportedFileFormat);
      try
      {
        using (outputStream)
          providerByExtension.Export(configurationParameters, outputStream);
        return new MSS.DTO.Message.Message(MessageTypeEnum.Success, Resources.MSS_MessageCodes_SuccessOperation);
      }
      catch (Exception ex)
      {
        return new MSS.DTO.Message.Message(MessageTypeEnum.Warning, MessageCodes.Error.GetStringValue() + " " + Resources.MSS_Client_UnableToSaveFile + (object) ex);
      }
    }

    public MSS.DTO.Message.Message ReadConfiguration(
      bool isValidInput,
      ConnectionAdjuster connectionAdjuster)
    {
      this.ClearCurrentlyShownControls();
      if (!isValidInput)
        return new MSS.DTO.Message.Message(MessageTypeEnum.Validation, MessageCodes.ValidationError.GetStringValue());
      this.DynamicGridTag = (List<Config>) null;
      this.Channel1DynamicGridTag = (List<Config>) null;
      this.Channel2DynamicGridTag = (List<Config>) null;
      this.Channel3DynamicGridTag = (List<Config>) null;
      this.ConfigValuesCollection = this._parameterCollector.Collect(connectionAdjuster);
      this.AjustIsConfigGeneralVisibleValues(this.ConfigValuesCollection.Count);
      if (this.ConfigValuesCollection.Count > 0)
      {
        EventPublisher.Publish<MeterConfigurationEvent>(new MeterConfigurationEvent()
        {
          ConfigValuesPerChannelList = this.ConfigValuesCollection
        }, (IViewModel) this);
        this.IsWriteButtonEnabled = true;
        return new MSS.DTO.Message.Message(MessageTypeEnum.Success, Resources.MSS_MessageCodes_SuccessOperation);
      }
      EventPublisher.Publish<MeterConfigurationParamsLoadedEvent>(new MeterConfigurationParamsLoadedEvent(), (IViewModel) this);
      return (MSS.DTO.Message.Message) null;
    }

    public MSS.DTO.Message.Message WriteConfiguration(bool isValidInput, object _)
    {
      if (!isValidInput)
        return new MSS.DTO.Message.Message(MessageTypeEnum.Validation, MessageCodes.ValidationError.GetStringValue());
      try
      {
        SortedList<OverrideID, ConfigurationParameter> paramsChannel1List;
        SortedList<OverrideID, ConfigurationParameter> paramsChannel2List;
        SortedList<OverrideID, ConfigurationParameter> paramsChannel3List;
        SortedList<OverrideID, ConfigurationParameter> paramsList;
        if (this.SetConfigParamsForTabs(_, out paramsChannel1List, out paramsChannel2List, out paramsChannel3List, out paramsList))
          return (MSS.DTO.Message.Message) null;
        this._parameterWriter.Write(paramsChannel1List, paramsChannel2List, paramsChannel3List, paramsList);
      }
      finally
      {
        this.IsWriteButtonEnabled = false;
        this.IsConfigGeneralVisible = false;
      }
      return new MSS.DTO.Message.Message(MessageTypeEnum.Success, Resources.MSS_MessageCodes_SuccessOperation);
    }
  }
}
