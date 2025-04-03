// Decompiled with JetBrains decompiler
// Type: S3_Handler.OverwriteWorkMeter
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class OverwriteWorkMeter
  {
    internal static Logger OverwriteWorkMeterLogger = LogManager.GetLogger(nameof (OverwriteWorkMeter));
    internal static SortedList<string, OverwriteProperties> OverwriteSelectionShortcutsList;
    internal static SortedList<OverwriteSelections, OverwriteProperties> OverwriteShortcutFromSelection;
    internal static string[] TimeSettingsParameterList = new string[2]
    {
      S3_ParameterNames.Bak_TimeZoneInQuarterHours.ToString(),
      S3_ParameterNames.Bak_DueDateMonth.ToString()
    };
    internal static string[] CycleSettingsList = new string[8]
    {
      S3_ParameterNames.Con_TdcVolMeasuringCycleTime.ToString(),
      S3_ParameterNames.Con_FastCycleVolThreshold.ToString(),
      S3_ParameterNames.volumeCycleTimeCounterInit.ToString(),
      S3_ParameterNames.temperaturCycleTimeCounterInit.ToString(),
      S3_ParameterNames.temperaturCycleTimeSlotCounterInit.ToString(),
      S3_ParameterNames.fastCycleOffCounterInit.ToString(),
      S3_ParameterNames.radioCycleTimeCounterInit.ToString(),
      S3_ParameterNames.radioPower.ToString()
    };
    internal static string[] TypeIdentificationParametersList_Protected = new string[6]
    {
      S3_ParameterNames.Con_MeterInfoId.ToString(),
      S3_ParameterNames.Con_BaseTypeId.ToString(),
      S3_ParameterNames.Con_MeterTypeId.ToString(),
      S3_ParameterNames.Con_SAP_MaterialNumber.ToString(),
      S3_ParameterNames.Con_SAP_ProductionOrderNumber.ToString(),
      S3_ParameterNames.Con_ProductionOrderNumber.ToString()
    };
    internal static string[] TypeVariantParametersList_NotProtected = new string[12]
    {
      S3_ParameterNames.SerDev0_Medium_Generation.ToString(),
      S3_ParameterNames.SerDev0_SelectedList_PrimaryAddress.ToString(),
      S3_ParameterNames.SerDev0_SelectedList.ToString(),
      S3_ParameterNames.SerDev1_Medium_Generation.ToString(),
      S3_ParameterNames.SerDev1_SelectedList_PrimaryAddress.ToString(),
      S3_ParameterNames.SerDev1_SelectedList.ToString(),
      S3_ParameterNames.SerDev2_Medium_Generation.ToString(),
      S3_ParameterNames.SerDev2_SelectedList_PrimaryAddress.ToString(),
      S3_ParameterNames.SerDev2_SelectedList.ToString(),
      S3_ParameterNames.SerDev3_Medium_Generation.ToString(),
      S3_ParameterNames.SerDev3_SelectedList_PrimaryAddress.ToString(),
      S3_ParameterNames.SerDev3_SelectedList.ToString()
    };
    internal static string[] DeviceIdentificationParametersList_NotProtected = new string[12]
    {
      S3_ParameterNames.SerDev0_IdentNo.ToString(),
      S3_ParameterNames.SerDev0_Manufacturer.ToString(),
      S3_ParameterNames.SerDev0_RadioId.ToString(),
      S3_ParameterNames.SerDev1_IdentNo.ToString(),
      S3_ParameterNames.SerDev1_Manufacturer.ToString(),
      S3_ParameterNames.SerDev1_RadioId.ToString(),
      S3_ParameterNames.SerDev2_IdentNo.ToString(),
      S3_ParameterNames.SerDev2_Manufacturer.ToString(),
      S3_ParameterNames.SerDev2_RadioId.ToString(),
      S3_ParameterNames.SerDev3_IdentNo.ToString(),
      S3_ParameterNames.SerDev3_Manufacturer.ToString(),
      S3_ParameterNames.SerDev3_RadioId.ToString()
    };
    internal static string[] DeviceIdentificationParametersList_Protected = new string[20]
    {
      S3_ParameterNames.Con_MeterId.ToString(),
      S3_ParameterNames.Con_SerialNumber.ToString(),
      S3_ParameterNames.Con_Manufacturer.ToString(),
      S3_ParameterNames.Con_Medium_Generation.ToString(),
      S3_ParameterNames.Con_SelectedList_PrimaryAddress.ToString(),
      S3_ParameterNames.PrintedSerialNumber.ToString(),
      S3_ParameterNames.SerDev0_IdentNo.ToString(),
      S3_ParameterNames.SerDev0_Manufacturer.ToString(),
      S3_ParameterNames.SerDev0_Medium_Generation.ToString(),
      S3_ParameterNames.SerDev0_SelectedList_PrimaryAddress.ToString(),
      S3_ParameterNames.SerDev0_SelectedList.ToString(),
      S3_ParameterNames.SerDev0_RadioId.ToString(),
      S3_ParameterNames.ApprovalRevison.ToString(),
      S3_ParameterNames.cfg_lora_deveui_0.ToString(),
      S3_ParameterNames.cfg_lora_appeui_0.ToString(),
      S3_ParameterNames.cfb_lora_AppKey_0.ToString(),
      S3_ParameterNames.FD_LoRa_AppEUI.ToString(),
      S3_ParameterNames.FD_LoRa_AppKey.ToString(),
      S3_ParameterNames.FD_LoRa_DevEUI.ToString(),
      S3_ParameterNames.FD_Obis_Medium.ToString()
    };
    internal static string[] VolumeParameterList = new string[5]
    {
      S3_ParameterNames.Con_VolFactor.ToString(),
      S3_ParameterNames.Con_CalFlowFactor.ToString(),
      S3_ParameterNames.volumeUnitIndex.ToString(),
      S3_ParameterNames.flowUnitIndex.ToString(),
      S3_ParameterNames.VolInputSetup.ToString()
    };
    internal static string[] IO_SettingsList = new string[10]
    {
      S3_ParameterNames.PulsOutFrequence.ToString(),
      S3_ParameterNames.Cal_FaktorInput_n_0.ToString(),
      S3_ParameterNames.Cal_FaktorInput_n_1.ToString(),
      S3_ParameterNames.Cal_FaktorInput_n_2.ToString(),
      S3_ParameterNames.Cal_TeilerFaktorInput_n_0.ToString(),
      S3_ParameterNames.Cal_TeilerFaktorInput_n_1.ToString(),
      S3_ParameterNames.Cal_TeilerFaktorInput_n_2.ToString(),
      S3_ParameterNames.input0UnitIndex.ToString(),
      S3_ParameterNames.input1UnitIndex.ToString(),
      S3_ParameterNames.input2UnitIndex.ToString()
    };
    internal static string[] UltrasonicSettingsList = new string[165]
    {
      S3_ParameterNames.Con_tdc_cal_geometry_factor.ToString(),
      S3_ParameterNames.Con_tdc_cal_threshold_zero_flow_detection.ToString(),
      S3_ParameterNames.Con_tdc_cal_threshold_zero_flow_detection_2.ToString(),
      S3_ParameterNames.Con_tdc_cal_threshold_with_offset_zero_flow_detection.ToString(),
      S3_ParameterNames.Con_tdc_cal_threshold_overload_flow_detection.ToString(),
      S3_ParameterNames.Con_tdc_cal_threshold_overload_flow_value.ToString(),
      S3_ParameterNames.Con_tdc_cal_threshold_zero_backflow_detection.ToString(),
      S3_ParameterNames.Con_tdc_cal_threshold_overload_backflow_detection.ToString(),
      S3_ParameterNames.Con_tdc_cal_threshold_overload_backflow_value.ToString(),
      S3_ParameterNames.Con_tdc_cal_threshold_max_bub_detection.ToString(),
      S3_ParameterNames.Con_tdc_cal_threshold_min_bub_detection.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_0.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_1.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_2.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_3.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_4.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_5.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_6.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_7.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_8.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_9.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_10.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_11.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_12.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_13.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_14.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_15.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_16.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_17.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_18.ToString(),
      S3_ParameterNames.Con_volcorr_x_value_19.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_0.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_1.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_2.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_3.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_4.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_5.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_6.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_7.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_8.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_9.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_10.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_11.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_12.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_13.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_14.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_15.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_16.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_17.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_18.ToString(),
      S3_ParameterNames.Con_volcorr_y_value_19.ToString(),
      S3_ParameterNames.TDC_MapTemp.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_0.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_1.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_2.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_3.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_4.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_5.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_6.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_7.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_8.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_9.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_10.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_11.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_12.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_13.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_14.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_15.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_16.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_17.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_18.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_19.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_20.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_21.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_22.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_23.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_24.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_25.ToString(),
      S3_ParameterNames.Con_tdc_temp_value_26.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_0.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_1.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_2.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_3.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_4.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_5.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_6.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_7.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_8.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_9.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_10.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_11.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_12.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_13.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_14.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_15.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_16.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_17.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_18.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_19.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_20.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_21.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_22.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_23.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_24.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_25.ToString(),
      S3_ParameterNames.Con_tdc_sv_value_26.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_0.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_1.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_2.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_3.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_4.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_5.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_6.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_7.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_8.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_9.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_10.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_11.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_12.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_13.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_14.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_15.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_16.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_17.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_18.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_19.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_20.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_21.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_22.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_23.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_24.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_25.ToString(),
      S3_ParameterNames.Con_tdc_blc_value_26.ToString(),
      S3_ParameterNames.Con_tdc_gp22_config_tab_value_0.ToString(),
      S3_ParameterNames.Con_tdc_gp22_config_tab_value_1.ToString(),
      S3_ParameterNames.Con_tdc_gp22_config_tab_value_2.ToString(),
      S3_ParameterNames.Con_tdc_gp22_config_tab_value_3.ToString(),
      S3_ParameterNames.Con_tdc_gp22_config_tab_value_4.ToString(),
      S3_ParameterNames.Con_tdc_gp22_config_tab_value_5.ToString(),
      S3_ParameterNames.Con_tdc_gp22_config_tab_value_6.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_x_0.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_x_1.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_x_2.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_x_3.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_x_4.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_x_5.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_y_0.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_y_1.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_y_2.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_y_3.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_y_4.ToString(),
      S3_ParameterNames.Con_tdc_fwd_temp_y_5.ToString(),
      S3_ParameterNames.Con_tdc_error_counter_measure_max.ToString(),
      S3_ParameterNames.Con_tdc_cal_resonator_counter_max.ToString(),
      S3_ParameterNames.Con_tdc_fwd_offset_adjust_counter_max.ToString(),
      S3_ParameterNames.Con_tdc_start_ref_offset_level.ToString(),
      S3_ParameterNames.Con_tdc_max_offset_decrease_value.ToString(),
      S3_ParameterNames.Con_tdc_pw_max_value.ToString(),
      S3_ParameterNames.Con_tdc_pw_max_abs_diff_value.ToString(),
      S3_ParameterNames.Con_tdc_max_zero_flow_counter_value.ToString(),
      S3_ParameterNames.Con_tdc_pw_readjustment_threshold_max.ToString(),
      S3_ParameterNames.Con_tdc_pw_readjustment_threshold_min.ToString(),
      S3_ParameterNames.Con_tdc_calib_factor.ToString(),
      S3_ParameterNames.Con_Temp_Display_Range_Max.ToString(),
      S3_ParameterNames.Con_Temp_Display_Range_Min.ToString()
    };
    internal static string[] EnergyParameterList = new string[14]
    {
      S3_ParameterNames.Con_EnergyFactor.ToString(),
      S3_ParameterNames.Con_CalPowerFactor.ToString(),
      S3_ParameterNames.Device_Setup.ToString(),
      S3_ParameterNames.Device_Setup_2.ToString(),
      S3_ParameterNames.Heap_SelectTime.ToString(),
      S3_ParameterNames.Con_Waerme_Grenze_DeltaT_min.ToString(),
      S3_ParameterNames.Con_Kaelte_Grenze_DeltaT_min.ToString(),
      S3_ParameterNames.Con_HeatThreshold.ToString(),
      S3_ParameterNames.Con_Feste_Fuehlertemperatur_VL.ToString(),
      S3_ParameterNames.Con_Feste_Fuehlertemperatur_RL.ToString(),
      S3_ParameterNames.Con_Tariff_RefTemp.ToString(),
      S3_ParameterNames.energyUnitIndex.ToString(),
      S3_ParameterNames.powerUnitIndex.ToString(),
      S3_ParameterNames.Con_water_glycol_value_e20.ToString()
    };
    internal static string[] TemperatureParameterList = new string[11]
    {
      S3_ParameterNames.Con_Temp_Display_Range_Max.ToString(),
      S3_ParameterNames.Con_Temp_Display_Range_Min.ToString(),
      S3_ParameterNames.Con_cal_adc_diff_ref2_ref1_0.ToString(),
      S3_ParameterNames.Con_cal_adc_diff_ref2_ref1_1.ToString(),
      S3_ParameterNames.Con_cal_tf_a0_0.ToString(),
      S3_ParameterNames.Con_cal_tf_a0_1.ToString(),
      S3_ParameterNames.Con_cal_tf_a1_0.ToString(),
      S3_ParameterNames.Con_cal_tf_a1_1.ToString(),
      S3_ParameterNames.Con_cal_tf_a2_0.ToString(),
      S3_ParameterNames.Con_cal_tf_a2_1.ToString(),
      S3_ParameterNames.Con_Adc_Low_Bat_Error_Treshold.ToString()
    };
    internal static string[] RTC_AndRadioCalibrationParameterList = new string[3]
    {
      S3_ParameterNames.Con_QuarzOffsetErr.ToString(),
      S3_ParameterNames.radio_frequency_deviation.ToString(),
      S3_ParameterNames.radio_frequency_inc_dec.ToString()
    };
    internal static string[] TemperatureLimitsParameterList = new string[2]
    {
      S3_ParameterNames.Con_Temp_Display_Range_Max.ToString(),
      S3_ParameterNames.Con_Temp_Display_Range_Min.ToString()
    };
    internal static string[] AccumulatedValuesList = new string[16]
    {
      S3_ParameterNames.Bak_VolSum.ToString(),
      S3_ParameterNames.Vol_VolDisplay.ToString(),
      S3_ParameterNames.Bak_HeatEnergySum.ToString(),
      S3_ParameterNames.Energy_HeatEnergyDisplay.ToString(),
      S3_ParameterNames.Bak_ColdEnergySum.ToString(),
      S3_ParameterNames.Energy_ColdEnergyDisplay.ToString(),
      S3_ParameterNames.Bak_Tariff0EnergySum.ToString(),
      S3_ParameterNames.Energy_Tariff0EnergyDisplay.ToString(),
      S3_ParameterNames.Bak_Tariff1EnergySum.ToString(),
      S3_ParameterNames.Energy_Tariff1EnergyDisplay.ToString(),
      S3_ParameterNames.Cal_DisplayInput_n_0.ToString(),
      S3_ParameterNames.Cal_DisplayInput_n_1.ToString(),
      S3_ParameterNames.Cal_DisplayInput_n_2.ToString(),
      S3_ParameterNames.Cal_DisplayRestInput_n_0.ToString(),
      S3_ParameterNames.Cal_DisplayRestInput_n_1.ToString(),
      S3_ParameterNames.Cal_DisplayRestInput_n_2.ToString()
    };
    internal static string[] OverwriteNotChangeCheckList = new string[4]
    {
      S3_ParameterNames.energyUnitIndex.ToString(),
      S3_ParameterNames.powerUnitIndex.ToString(),
      S3_ParameterNames.volumeUnitIndex.ToString(),
      S3_ParameterNames.flowUnitIndex.ToString()
    };
    internal static string[] OverwriteNotChangeCompareList = new string[16]
    {
      S3_ParameterNames.Con_EnergyFactor.ToString(),
      S3_ParameterNames.Con_CalPowerFactor.ToString(),
      S3_ParameterNames.energyUnitIndex.ToString(),
      S3_ParameterNames.powerUnitIndex.ToString(),
      S3_ParameterNames.Con_VolFactor.ToString(),
      S3_ParameterNames.Con_CalFlowFactor.ToString(),
      S3_ParameterNames.volumeUnitIndex.ToString(),
      S3_ParameterNames.flowUnitIndex.ToString(),
      S3_ParameterNames.Con_cal_adc_diff_ref2_ref1_0.ToString(),
      S3_ParameterNames.Con_cal_adc_diff_ref2_ref1_1.ToString(),
      S3_ParameterNames.Con_cal_tf_a0_0.ToString(),
      S3_ParameterNames.Con_cal_tf_a0_1.ToString(),
      S3_ParameterNames.Con_cal_tf_a1_0.ToString(),
      S3_ParameterNames.Con_cal_tf_a1_1.ToString(),
      S3_ParameterNames.Con_cal_tf_a2_0.ToString(),
      S3_ParameterNames.Con_cal_tf_a2_1.ToString()
    };
    private S3_HandlerFunctions MyFunctions;
    private S3_AllMeters MyMeters;
    private S3_Meter sourceMeter;
    private ulong[] NoChangeParameters;

    static OverwriteWorkMeter()
    {
      OverwriteWorkMeter.OverwriteSelectionShortcutsList = new SortedList<string, OverwriteProperties>();
      OverwriteWorkMeter.OverwriteShortcutFromSelection = new SortedList<OverwriteSelections, OverwriteProperties>();
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.TimeSettings, "TM", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.CycleSettings, "CS", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.TypeIdentificationParameters, "TI", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.DeviceIdentificationParameters, "DI", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.VolumeParameter, "VP", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.EnergyParameter, "ES", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.IO_Settings, "IO", true);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.AccumulatedValues, "AV", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.EnergyModifications, "EM", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.UltrasonicSettings, "US", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.TemperatureSettings, "TS", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.TemperatureLimits, "TL", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.WritePermissionTable, "WP", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.RTC_AndRadioCalibrations, "RR", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.Functions, "FU", true);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.Loggers, "LO", true);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.MBusLists, "ML", true);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.RadioLists, "RL", true);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.ResetLoggers, "LR", true);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.ResetCalibrationDate, "CR", false);
      OverwriteWorkMeter.RegisterOverwriteProperties(OverwriteSelections.ResetAccumulatedValues, "AR", false);
      S3_HandlerFunctions.OverwriteFromType_On_ProductionTypeProgramming = new bool[21]
      {
        true,
        true,
        true,
        false,
        true,
        true,
        true,
        false,
        true,
        true,
        false,
        true,
        true,
        false,
        true,
        true,
        true,
        true,
        true,
        false,
        true
      };
      S3_HandlerFunctions.OverwriteSelectionShortcuts = OverwriteWorkMeter.OverwriteSelectionShortcutsList.Keys.ToArray<string>();
    }

    private static void RegisterOverwriteProperties(
      OverwriteSelections overwriteSelection,
      string shortcut,
      bool freeForFeldConfiguration)
    {
      OverwriteProperties overwriteProperties = new OverwriteProperties(overwriteSelection, shortcut, freeForFeldConfiguration);
      OverwriteWorkMeter.OverwriteSelectionShortcutsList.Add(shortcut, overwriteProperties);
      OverwriteWorkMeter.OverwriteShortcutFromSelection.Add(overwriteSelection, overwriteProperties);
    }

    public OverwriteWorkMeter(S3_HandlerFunctions MyFunctions, S3_AllMeters AllMeters)
    {
      this.MyFunctions = MyFunctions;
      this.MyMeters = AllMeters;
    }

    internal bool OverwriteWorkFromTypeForTypeOverWrite()
    {
      S3_AllMeters.S3_AllMetersLogger.Info(this.MyMeters.openNlogLineHeader + "OverwriteWorkFromTypeForTypeoverwrite");
      if (!this.OverwriteRun(this.MyMeters.TypeMeter, new bool[21]
      {
        true,
        true,
        true,
        false,
        false,
        true,
        true,
        false,
        true,
        false,
        false,
        true,
        true,
        false,
        true,
        true,
        true,
        true,
        true,
        false,
        true
      }, OverwriteOptions.Clone))
        return false;
      this.MyMeters.WorkMeter.MyMeterScaling.GarantInputBaseSettings(this.MyMeters.TypeMeter.MyTransmitParameterManager.AreVirtualDevicesUsed());
      return true;
    }

    internal bool OverwriteWorkFromTypeForConfigurator(bool useCompactMBusList)
    {
      S3_AllMeters.S3_AllMetersLogger.Info(this.MyMeters.openNlogLineHeader + nameof (OverwriteWorkFromTypeForConfigurator));
      if (!this.OverwriteRun(this.MyMeters.TypeMeter, new bool[21]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        true,
        true,
        true,
        false,
        false,
        false
      }, OverwriteOptions.Clone))
        return false;
      if (this.MyMeters.TypeMeter.TypeOverrideString != null)
      {
        OverwriteRequirementsList requirementsList = new OverwriteRequirementsList(this.MyMeters.TypeMeter.TypeOverrideString);
        int index1 = requirementsList.OverReqList.IndexOfKey(OverwriteListTypes.OverwriteType);
        if (index1 >= 0 && !this.OverwriteWorkFromParameterType(requirementsList.OverReqList.Values[index1], OverwriteOptions.None))
          return false;
        if (useCompactMBusList)
        {
          int index2 = requirementsList.OverReqList.IndexOfKey(OverwriteListTypes.MBusCompactType);
          if (index2 >= 0 && !this.OverwriteWorkFromParameterType(requirementsList.OverReqList.Values[index2], OverwriteOptions.None))
            return false;
        }
        else
        {
          int index3 = requirementsList.OverReqList.IndexOfKey(OverwriteListTypes.MBusVirtualDevType);
          if (index3 >= 0 && !this.OverwriteWorkFromParameterType(requirementsList.OverReqList.Values[index3], OverwriteOptions.None))
            return false;
        }
        this.MyMeters.WorkMeter.MyMeterScaling.GarantInputBaseSettings(!useCompactMBusList);
      }
      string typeCreationString = OverwriteWorkMeter.CreateParameterTypeUsingStringFromTypeCreationString(this.MyMeters.WorkMeter.TypeCreationString);
      if (typeCreationString.Length > 0)
      {
        foreach (OverwriteRequirements requirements in (IEnumerable<OverwriteRequirements>) new OverwriteRequirementsList(typeCreationString, useCompactMBusList).OverReqList.Values)
        {
          if (!this.OverwriteWorkFromParameterType(requirements, OverwriteOptions.None))
            return false;
        }
        this.MyMeters.WorkMeter.MyMeterScaling.GarantInputBaseSettings(!useCompactMBusList);
      }
      return true;
    }

    internal bool OverwriteWorkWithResetValues(OverwriteOptions owOptions)
    {
      S3_AllMeters.S3_AllMetersLogger.Info(this.MyMeters.openNlogLineHeader + nameof (OverwriteWorkWithResetValues));
      return this.OverwriteRun(this.MyMeters.TypeMeter, new bool[21]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        true,
        true
      }, owOptions);
    }

    internal bool OverwriteWorkFromParameterType(
      OverwriteRequirements requirements,
      OverwriteOptions owOptions)
    {
      S3_AllMeters.S3_AllMetersLogger.Info(this.MyMeters.openNlogLineHeader + nameof (OverwriteWorkFromParameterType));
      try
      {
        S3_Meter typeMeter;
        if (!this.MyMeters.GetDbType(requirements.ParameterTypeMeterInfoId, out typeMeter))
          return false;
        bool[] overwriteSelection = new bool[21];
        for (int index = 0; index < requirements.SelectedOverwriteSections.Count; ++index)
          overwriteSelection[(int) requirements.SelectedOverwriteSections.Keys[index]] = true;
        return this.OverwriteRun(typeMeter, overwriteSelection, owOptions);
      }
      catch (Exception ex)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Overwrite from parameter type error");
        stringBuilder.AppendLine("Exception:");
        stringBuilder.AppendLine(ex.ToString());
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, stringBuilder.ToString());
      }
    }

    internal bool OverwriteRun(
      S3_Meter sourceMeter,
      bool[] overwriteSelection,
      OverwriteOptions owOptions)
    {
      if (S3_AllMeters.S3_AllMetersLogger.IsInfoEnabled)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Overwrite selected: ");
        bool flag1 = false;
        for (int index = 0; index < overwriteSelection.Length; ++index)
        {
          if (overwriteSelection[index])
          {
            if (flag1)
              stringBuilder.Append(';');
            else
              flag1 = true;
            OverwriteSelections overwriteSelections = (OverwriteSelections) index;
            stringBuilder.Append(overwriteSelections.ToString());
          }
        }
        S3_AllMeters.S3_AllMetersLogger.Info(this.MyMeters.openNlogLineHeader + stringBuilder.ToString());
        stringBuilder.Length = 0;
        stringBuilder.Append("Overwrite not selected: ");
        bool flag2 = false;
        for (int index = 0; index < overwriteSelection.Length; ++index)
        {
          if (!overwriteSelection[index])
          {
            if (flag2)
              stringBuilder.Append(';');
            else
              flag2 = true;
            OverwriteSelections overwriteSelections = (OverwriteSelections) index;
            stringBuilder.Append(overwriteSelections.ToString());
          }
        }
        S3_AllMeters.S3_AllMetersLogger.Info(this.MyMeters.openNlogLineHeader + stringBuilder.ToString());
      }
      this.sourceMeter = sourceMeter;
      if (this.sourceMeter.overwriteHistorie.Count > 0)
      {
        this.MyMeters.WorkMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("{"));
        foreach (OverwriteHistoryItem overwriteHistoryItem in this.sourceMeter.overwriteHistorie)
          this.MyMeters.WorkMeter.overwriteHistorie.Add(overwriteHistoryItem.Clone());
        this.sourceMeter.overwriteHistorie.Clear();
        this.MyMeters.WorkMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("}"));
      }
      this.MyMeters.WorkMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem(this.sourceMeter.MyIdentification.FirmwareVersion, this.sourceMeter.MyIdentification.SAP_MaterialNumber, this.sourceMeter.MyIdentification.MeterInfoId, overwriteSelection, owOptions));
      if (this.IsOverwriteWithoutMeter(overwriteSelection))
      {
        if ((owOptions & OverwriteOptions.Clone) != 0 && !this.MyMeters.NewWorkMeter("overwrite without source meter"))
          return false;
        S3_AllMeters.S3_AllMetersLogger.Info(this.MyMeters.openNlogLineHeader + "Overwrite without source meter");
      }
      else
      {
        if (sourceMeter == null)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Source for overwrite not loaded");
          return false;
        }
        if (sourceMeter == this.MyMeters.TypeMeter)
        {
          S3_AllMeters.S3_AllMetersLogger.Info(this.MyMeters.openNlogLineHeader + "Overwrite from type. MeterInfoId: " + sourceMeter.MyIdentification.MeterInfoId.ToString());
          if ((owOptions & OverwriteOptions.Clone) != 0 && !this.MyMeters.NewWorkMeter("overwrite from type meter object"))
            return false;
        }
        else
        {
          if ((owOptions & OverwriteOptions.Clone) != 0 && !this.MyMeters.NewWorkMeter("overwrite from source meter object"))
            return false;
          this.MyFunctions.MyMeters.WorkMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("WorkMeter created from source meter object for overwrite undo"));
        }
      }
      bool flag = false;
      try
      {
        if (overwriteSelection[15] && (!this.IsFirmwareCompatible(OverwriteSelections.Loggers) || !this.OverWrite_Loggers()) || overwriteSelection[14] && (!this.IsFirmwareCompatible(OverwriteSelections.Functions) || !this.OverWrite_Functions()) || overwriteSelection[16] && (!this.IsFirmwareCompatible(OverwriteSelections.MBusLists) || !this.OverWrite_MBusLists()) || overwriteSelection[17] && (!this.IsFirmwareCompatible(OverwriteSelections.RadioLists) || !this.OverWrite_RadioLists()) || overwriteSelection[12] && (!this.IsFirmwareCompatible(OverwriteSelections.WritePermissionTable) || !this.OverWrite_WritePermission()) || overwriteSelection[0] && (!this.IsFirmwareCompatible(OverwriteSelections.TimeSettings) || !this.OverWrite_Parameters(OverwriteWorkMeter.TimeSettingsParameterList)) || overwriteSelection[1] && (!this.IsFirmwareCompatible(OverwriteSelections.CycleSettings) || !this.OverWrite_CycleAndModeSettings()))
          return false;
        if (overwriteSelection[2])
        {
          if (!this.IsFirmwareCompatible(OverwriteSelections.TypeIdentificationParameters))
            return false;
          if (!this.MyMeters.WorkMeter.IsWriteProtected)
          {
            if (!this.OverWrite_Parameters(OverwriteWorkMeter.TypeIdentificationParametersList_Protected))
              return false;
            if (sourceMeter != null)
              this.MyMeters.WorkMeter.TypeCreationString = sourceMeter.TypeCreationString;
          }
          else if (!this.AreAllParametersEqual(OverwriteWorkMeter.TypeIdentificationParametersList_Protected, out string _))
          {
            ZR_ClassLibMessages.AddWarning("Meter has write protection!");
            ZR_ClassLibMessages.AddWarning("Can not change type information!");
            if (!this.IsTypeParameterEqual("Con_BaseTypeId"))
            {
              ZR_ClassLibMessages.AddWarning(" ");
              ZR_ClassLibMessages.AddWarning("!!! Configurator will not work trouble-free! It will work on old BaseType !!!");
              ZR_ClassLibMessages.AddWarning("Can not change base type!");
            }
          }
          if (!this.OverWrite_Parameters(OverwriteWorkMeter.TypeVariantParametersList_NotProtected))
            return false;
          this.MyMeters.WorkMeter.MyIdentification.LoadDeviceIdFromParameter();
        }
        if (overwriteSelection[3])
        {
          if (!this.IsFirmwareCompatible(OverwriteSelections.DeviceIdentificationParameters) || !this.MyMeters.WorkMeter.IsWriteProtected && !this.OverWrite_Parameters(OverwriteWorkMeter.DeviceIdentificationParametersList_Protected) || !this.OverWrite_Parameters(OverwriteWorkMeter.DeviceIdentificationParametersList_NotProtected))
            return false;
          this.MyMeters.WorkMeter.MyIdentification.LoadDeviceIdFromParameter();
        }
        if (overwriteSelection[5] && (!this.IsFirmwareCompatible(OverwriteSelections.EnergyParameter) || !this.OverWrite_Parameters(OverwriteWorkMeter.EnergyParameterList)) || overwriteSelection[8] && (!this.IsFirmwareCompatible(OverwriteSelections.EnergyModifications) || !this.CopyEnergyModifications()) || overwriteSelection[4] && (!this.IsFirmwareCompatible(OverwriteSelections.VolumeParameter) || !this.OverWrite_Parameters(OverwriteWorkMeter.VolumeParameterList)) || overwriteSelection[6] && (!this.IsFirmwareCompatible(OverwriteSelections.IO_Settings) || !this.OverWrite_Parameters(OverwriteWorkMeter.IO_SettingsList)) || overwriteSelection[9] && (!this.IsFirmwareCompatible(OverwriteSelections.UltrasonicSettings) || !this.OverWrite_Parameters(OverwriteWorkMeter.UltrasonicSettingsList) || !this.OverWrite_UltrasonicMatrixFlags()) || overwriteSelection[10] && (!this.IsFirmwareCompatible(OverwriteSelections.TemperatureSettings) || !this.OverWrite_Parameters(OverwriteWorkMeter.TemperatureParameterList)) || overwriteSelection[11] && (!this.IsFirmwareCompatible(OverwriteSelections.TemperatureLimits) || !this.OverWrite_Parameters(OverwriteWorkMeter.TemperatureLimitsParameterList)) || overwriteSelection[13] && (!this.IsFirmwareCompatible(OverwriteSelections.RTC_AndRadioCalibrations) || !this.OverWrite_Parameters(OverwriteWorkMeter.RTC_AndRadioCalibrationParameterList)) || (owOptions & OverwriteOptions.Compile) != 0 && (!this.MyMeters.WorkMeter.MyMeterScaling.ReadSettingsFromParameter() || !this.MyMeters.WorkMeter.MyMeterScaling.WriteParameterDependencies() || !this.MyMeters.WorkMeter.MyResources.ReloadResources() || !this.MyMeters.WorkMeter.Compile()) || overwriteSelection[7] && (!this.IsFirmwareCompatible(OverwriteSelections.AccumulatedValues) || !this.OverWrite_Parameters(OverwriteWorkMeter.AccumulatedValuesList)) || overwriteSelection[20] && !this.ResetParameters(OverwriteWorkMeter.AccumulatedValuesList) || overwriteSelection[18] && !this.MyMeters.WorkMeter.MyLoggerManager.ResetAndClearAllLoggers() || overwriteSelection[19] && !this.ResetCalibrationDate())
          return false;
        flag = true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription("Exception:");
      }
      finally
      {
        if (!flag)
        {
          this.MyMeters.Undo();
          ZR_ClassLibMessages.AddErrorDescription("Overwrite canceled");
        }
      }
      return flag;
    }

    private bool IsFirmwareCompatible(OverwriteSelections theSelection)
    {
      if (this.MyFunctions.MyDatabase.HardwareAndFirmwareInfos.IsMapCompatibility(this.MyMeters.WorkMeter.MyIdentification.MapId, this.sourceMeter.MyIdentification.MapId, theSelection != OverwriteSelections.all ? OverwriteWorkMeter.OverwriteShortcutFromSelection[theSelection].shortcut : "full"))
        return true;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Firmware not compatible for overwrite", S3_AllMeters.S3_AllMetersLogger);
      return false;
    }

    private bool IsTypeParameterEqual(string parameterName)
    {
      S3_Parameter s3Parameter1 = this.MyMeters.TypeMeter.MyParameters.ParameterByName[parameterName];
      S3_Parameter s3Parameter2 = this.MyMeters.WorkMeter.MyParameters.ParameterByName[parameterName];
      return (long) s3Parameter1.GetUlongValue() == (long) s3Parameter2.GetUlongValue();
    }

    private bool AreAllParametersEqual(string[] parameterList, out string notEqualParameter)
    {
      notEqualParameter = (string) null;
      try
      {
        for (int index = 0; index < parameterList.Length; ++index)
        {
          if (!this.IsTypeParameterEqual(parameterList[index]))
          {
            notEqualParameter = parameterList[index];
            return false;
          }
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Compare type parameter");
        return false;
      }
      return true;
    }

    private bool IsOverwriteWithoutMeter(bool[] overwriteSelection)
    {
      for (int index = 0; index < overwriteSelection.Length; ++index)
      {
        if (overwriteSelection[index] && index != 18 && index != 20 && index != 19)
          return false;
      }
      return true;
    }

    private bool OverWrite_Parameters(string[] parameterList)
    {
      S3_Parameter s3Parameter1 = (S3_Parameter) null;
      try
      {
        for (int index1 = 0; index1 < parameterList.Length; ++index1)
        {
          if (this.sourceMeter.MyParameters.ParameterByName.ContainsKey(parameterList[index1]) && this.MyMeters.WorkMeter.MyParameters.ParameterByName.ContainsKey(parameterList[index1]))
          {
            s3Parameter1 = this.sourceMeter.MyParameters.ParameterByName[parameterList[index1]];
            S3_Parameter s3Parameter2 = this.MyMeters.WorkMeter.MyParameters.ParameterByName[parameterList[index1]];
            if (s3Parameter2.sourceMemoryBlock != null)
              s3Parameter2.sourceMemoryBlock = (S3_MemoryBlock) null;
            if (OverwriteWorkMeter.OverwriteWorkMeterLogger.IsWarnEnabled && s3Parameter1.BlockStartAddress != s3Parameter2.BlockStartAddress)
            {
              Logger overwriteWorkMeterLogger = OverwriteWorkMeter.OverwriteWorkMeterLogger;
              string[] strArray = new string[6]
              {
                "Parameter '",
                s3Parameter1.Name,
                "': Adress moved from 0x",
                null,
                null,
                null
              };
              int blockStartAddress = s3Parameter1.BlockStartAddress;
              strArray[3] = blockStartAddress.ToString("x04");
              strArray[4] = " to 0x";
              blockStartAddress = s3Parameter2.BlockStartAddress;
              strArray[5] = blockStartAddress.ToString("x04");
              string message = string.Concat(strArray);
              overwriteWorkMeterLogger.Warn(message);
            }
            bool flag = false;
            if (s3Parameter1.Name == "Con_water_glycol_value_e20")
            {
              int blockStartAddress1 = s3Parameter1.BlockStartAddress;
              int blockStartAddress2 = s3Parameter2.BlockStartAddress;
              for (int index2 = 0; index2 < 15; ++index2)
              {
                int Address1 = blockStartAddress1 + index2 * 4;
                int Address2 = blockStartAddress2 + index2 * 4;
                float floatValue = s3Parameter1.MyMeter.MyDeviceMemory.GetFloatValue(Address1);
                flag = s3Parameter2.MyMeter.MyDeviceMemory.SetFloatValue(Address2, floatValue);
              }
            }
            else if (s3Parameter1.Name == S3_ParameterNames.TDC_MapTemp.ToString())
            {
              int minValue = (int) s3Parameter1.Statics.MinValue;
              int maxValue = (int) s3Parameter1.Statics.MaxValue;
              int blockStartAddress3 = s3Parameter1.BlockStartAddress;
              int blockStartAddress4 = s3Parameter2.BlockStartAddress;
              int num1 = 0;
              while (num1 < minValue)
              {
                short shortValue = s3Parameter1.MyMeter.MyDeviceMemory.GetShortValue(blockStartAddress3);
                flag = s3Parameter2.MyMeter.MyDeviceMemory.SetShortValue(blockStartAddress4, shortValue);
                ++num1;
                blockStartAddress3 += 2;
                blockStartAddress4 += 2;
              }
              int num2 = 0;
              while (num2 < maxValue)
              {
                float floatValue = s3Parameter1.MyMeter.MyDeviceMemory.GetFloatValue(blockStartAddress3);
                flag = s3Parameter2.MyMeter.MyDeviceMemory.SetFloatValue(blockStartAddress4, floatValue);
                ++num2;
                blockStartAddress3 += 4;
                blockStartAddress4 += 4;
              }
              for (int index3 = 0; index3 < minValue; ++index3)
              {
                int num3 = 0;
                while (num3 < maxValue)
                {
                  float floatValue = s3Parameter1.MyMeter.MyDeviceMemory.GetFloatValue(blockStartAddress3);
                  flag = s3Parameter2.MyMeter.MyDeviceMemory.SetFloatValue(blockStartAddress4, floatValue);
                  ++num3;
                  blockStartAddress3 += 4;
                  blockStartAddress4 += 4;
                }
              }
            }
            else if (s3Parameter1.Name == S3_ParameterNames.Heap_SelectTime.ToString())
            {
              s3Parameter2.sourceMemoryBlock = (S3_MemoryBlock) null;
              flag = s3Parameter2.SetUintValue(0U);
            }
            else if (s3Parameter1.ByteSize == 4)
              flag = s3Parameter2.SetUintValue(s3Parameter1.GetUintValue());
            else if (s3Parameter1.ByteSize == 2)
              flag = s3Parameter2.SetUshortValue(s3Parameter1.GetUshortValue());
            else if (s3Parameter1.ByteSize == 1)
            {
              s3Parameter1.GetByteValue();
              flag = s3Parameter2.SetByteValue(s3Parameter1.GetByteValue());
            }
            else if (s3Parameter1.ByteSize == 8)
              flag = s3Parameter2.SetUlongValue(s3Parameter1.GetUlongValue());
            if (!flag)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "OverWrite_Parameter(SetValue): " + s3Parameter1.Name);
              return false;
            }
          }
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString());
        if (s3Parameter1 == null)
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, nameof (OverWrite_Parameters));
        else
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "OverWrite_Parameter: " + s3Parameter1.Name);
        return false;
      }
      return true;
    }

    private bool CopyEnergyModifications()
    {
      if (!this.MyMeters.WorkMeter.IsWriteProtected)
      {
        new S3P_Device_Setup_2(this.MyMeters.WorkMeter).VolumeMeterFlowMounting = new S3P_Device_Setup_2(this.sourceMeter).VolumeMeterFlowMounting;
        new S3P_Device_Setup(this.MyMeters.WorkMeter).EnergyCalculations = new S3P_Device_Setup(this.sourceMeter).EnergyCalculations;
      }
      return true;
    }

    internal bool ResetAccumulatedValues()
    {
      return this.ResetParameters(OverwriteWorkMeter.AccumulatedValuesList, true);
    }

    private bool ResetParameters(string[] parameterList)
    {
      return this.ResetParameters(parameterList, false);
    }

    private bool ResetParameters(string[] parameterList, bool onlyIfNotProtected)
    {
      S3_Parameter s3Parameter = (S3_Parameter) null;
      try
      {
        for (int index1 = 0; index1 < parameterList.Length; ++index1)
        {
          int index2 = this.MyMeters.WorkMeter.MyParameters.ParameterByName.IndexOfKey(parameterList[index1]);
          if (index2 >= 0)
          {
            s3Parameter = this.MyMeters.WorkMeter.MyParameters.ParameterByName.Values[index2];
            if (!onlyIfNotProtected || !this.MyMeters.WorkMeter.IsWriteProtected || !s3Parameter.IsProtected)
            {
              bool flag = false;
              if (s3Parameter.ByteSize == 4)
                flag = s3Parameter.SetUintValue(0U);
              else if (s3Parameter.ByteSize == 2)
                flag = s3Parameter.SetUshortValue((ushort) 0);
              else if (s3Parameter.ByteSize == 1)
                flag = s3Parameter.SetByteValue((byte) 0);
              else if (s3Parameter.ByteSize == 8)
                flag = s3Parameter.SetUlongValue(0UL);
              if (!flag)
              {
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "OverWrite_Parameter(SetValue): " + s3Parameter.Name);
                return false;
              }
              s3Parameter.sourceMemoryBlock = (S3_MemoryBlock) null;
            }
          }
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString());
        if (s3Parameter == null)
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Reset_Parameters");
        else
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Reset_Parameters: " + s3Parameter.Name);
        return false;
      }
      return true;
    }

    private bool OverWrite_Functions()
    {
      return this.sourceMeter.MyFunctionManager.Clone(this.MyMeters.WorkMeter);
    }

    private bool OverWrite_Loggers()
    {
      return this.MyMeters.WorkMeter.MyLoggerManager.CrateLoggersFromBaseMeter(this.MyMeters.TypeMeter);
    }

    private bool OverWrite_CycleAndModeSettings()
    {
      if (this.sourceMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.DeviceSetupNp.ToString()))
      {
        byte byteValue1 = this.sourceMeter.MyParameters.ParameterByName[S3_ParameterNames.DeviceSetupNp.ToString()].GetByteValue();
        S3_Parameter s3Parameter = this.MyMeters.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.DeviceSetupNp.ToString()];
        byte byteValue2 = s3Parameter.GetByteValue();
        byte NewValue = ((uint) byteValue1 & 128U) <= 0U ? (byte) ((uint) byteValue2 & 4294967167U) : (byte) ((uint) byteValue2 | 128U);
        s3Parameter.SetByteValue(NewValue);
      }
      return this.OverWrite_Parameters(OverwriteWorkMeter.CycleSettingsList);
    }

    private bool OverWrite_MBusLists()
    {
      return this.sourceMeter.MyTransmitParameterManager.Clone(this.MyMeters.WorkMeter);
    }

    private bool OverWrite_RadioLists() => true;

    private bool OverWrite_WritePermission()
    {
      this.MyMeters.WorkMeter.MyWriteProtTableManager = this.sourceMeter.MyWriteProtTableManager.Clone(this.MyMeters.WorkMeter);
      return true;
    }

    private bool ResetCalibrationDate() => true;

    private bool OverWrite_UltrasonicMatrixFlags()
    {
      if (!this.MyMeters.WorkMeter.IsWriteProtected)
        new S3P_Device_Setup_2(this.MyMeters.WorkMeter).UltrasonicFactorMatrixUsed = new S3P_Device_Setup_2(this.sourceMeter).UltrasonicFactorMatrixUsed;
      return true;
    }

    internal static string CreateParameterTypeUsingStringFromTypeCreationString(
      string typeCreationString)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!string.IsNullOrEmpty(typeCreationString))
      {
        string[] strArray1 = typeCreationString.Split(';');
        for (int index1 = 1; index1 < strArray1.Length; ++index1)
        {
          bool flag = false;
          string[] strArray2 = strArray1[index1].Split('=');
          try
          {
            string[] strArray3 = strArray2[1].Split(',');
            for (int index2 = 0; index2 < strArray3.Length; ++index2)
            {
              if (OverwriteWorkMeter.OverwriteSelectionShortcutsList[strArray3[index2]].freeForFeldConfiguration)
              {
                if (!flag)
                {
                  flag = true;
                  if (stringBuilder.Length > 0)
                    stringBuilder.Append(';');
                  stringBuilder.Append(strArray2[0] + "=");
                }
                else
                  stringBuilder.Append(',');
                stringBuilder.Append(strArray3[index2]);
              }
            }
          }
          catch
          {
            throw new Exception("GetParameterTypeOverwriteString generation error");
          }
        }
      }
      return stringBuilder.ToString();
    }

    internal bool IsTypeOverwritePossible(bool[] overwriteSelection)
    {
      this.SaveNoChangeParameters();
      string notEqualParameter;
      if (this.AreAllParametersEqual(OverwriteWorkMeter.OverwriteNotChangeCheckList, out notEqualParameter))
        return true;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Overwrite not possible! Can not change parameter: " + notEqualParameter);
      return false;
    }

    internal bool IsTypeOverwritePerfect() => !this.AreAllNoChangeParametersUnchanged() || true;

    private bool SaveNoChangeParameters()
    {
      if (this.NoChangeParameters == null)
        this.NoChangeParameters = new ulong[OverwriteWorkMeter.OverwriteNotChangeCompareList.Length];
      S3_Parameter s3Parameter = (S3_Parameter) null;
      try
      {
        for (int index = 0; index < OverwriteWorkMeter.OverwriteNotChangeCompareList.Length; ++index)
        {
          s3Parameter = this.MyMeters.WorkMeter.MyParameters.ParameterByName[OverwriteWorkMeter.OverwriteNotChangeCompareList[index]];
          this.NoChangeParameters[index] = s3Parameter.GetUlongValue();
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString());
        if (s3Parameter == null)
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "SaveParameters");
        else
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "SaveParameters: " + s3Parameter.Name);
        return false;
      }
      return true;
    }

    private bool AreAllNoChangeParametersUnchanged()
    {
      S3_Parameter s3Parameter = (S3_Parameter) null;
      try
      {
        for (int index = 0; index < OverwriteWorkMeter.OverwriteNotChangeCompareList.Length; ++index)
        {
          s3Parameter = this.MyMeters.WorkMeter.MyParameters.ParameterByName[OverwriteWorkMeter.OverwriteNotChangeCompareList[index]];
          if ((long) this.NoChangeParameters[index] != (long) s3Parameter.GetUlongValue())
            return false;
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString());
        if (s3Parameter == null)
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "AreNotChangeParametersUnchanged");
        else
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "AreNotChangeParametersUnchanged: " + s3Parameter.Name);
        return false;
      }
      return true;
    }
  }
}
