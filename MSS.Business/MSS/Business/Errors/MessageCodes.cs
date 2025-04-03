// Decompiled with JetBrains decompiler
// Type: MSS.Business.Errors.MessageCodes
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Utils.Utils;

#nullable disable
namespace MSS.Business.Errors
{
  public enum MessageCodes
  {
    [StringEnum("MSS_MessageCodes_SuccessOperation")] Success_Operation,
    [StringEnum("MSS_MessageCodes_SuccessSave")] Success_Save,
    [StringEnum("MSS_Client_OperationCancelledMessage")] OperationCancelled,
    [StringEnum("MSS_Client_ValidationError")] ValidationError,
    [StringEnum("MSS_MessageCodes_UserDuplicate")] User_Duplicate,
    [StringEnum("MSS_MessageCodes_Success_Remove_Structure")] Success_Remove_Structure,
    [StringEnum("MSS_MessageCodes_Success_Delete_Structure")] Success_Delete_Structure,
    [StringEnum("MSS_Error_Message_Invalid_Page_Size")] PageSize_Not_A_Number,
    [StringEnum("MSS_Error_Message_Invalid_Batch_Size")] BatchSize_Not_A_Number,
    [StringEnum("MSS_MessageCodes_Refresh_Successfully")] Refresh_Successfully,
    [StringEnum("MSS_MessageCodes_Server_Not_Available")] Server_Not_Available,
    [StringEnum("MSS_Error_Message_Server_Not_Found")] Server_Not_Found,
    [StringEnum("MSS_Error_Message_Invalid_IP")] Invalid_IP,
    [StringEnum("MSS_Success_Message_Test_Config")] Success_Assign_Test_Config,
    [StringEnum("MSS_MessageCodes_SuccessDownload")] Success_Download,
    [StringEnum("MSS_MessageCodes_SuccessUpdate")] Success_Update,
    [StringEnum("MSS_MessageCodes_SuccessSend")] Success_Send,
    [StringEnum("MSS_Client_Structures_Search")] No_Item_found,
    [StringEnum("MSS_MessageCodes_Error_Print_Size")] Error_Print_Width,
    [StringEnum("MSS_MessageCodes_Error_Print_Canceled")] Error_Print_Canceled,
    [StringEnum("MSS_Error_Title")] Error,
    [StringEnum("MSS_Warning_Title")] Warning,
    [StringEnum("MSS_ExportOrder_Error_Title")] Error_ExportOrder,
    [StringEnum("MSS_ImportOrder_Error_Title")] Error_ImportOrder,
    [StringEnum("MSS_ExportOrder_Error_Message_LockedStructure")] Error_LockedStructure,
    [StringEnum("MSS_ExportOrder_Error_Message_LockedOrder")] Error_LockedOrder,
    [StringEnum("MSS_ExportOrder_Error_Message_FixedStructure")] Error_ExportNonFixedStructure,
    [StringEnum("MSS_ExportOrder_Error_Message_IncompleteStructure")] Error_ExportReadingOrder_IncompleteStructure,
    [StringEnum("MSS_ExportOrder_Error_Message")] Error_Export_NoExportableMeter,
    [StringEnum("MSS_ExportOrder_Error_Message_TenantWithoutValidMeters")] Error_Export_TenantNoExportableMeters,
    [StringEnum("MSS_ExportOrder_Error_Message_MissingMeterMandatoryData")] Error_Export_MeterMissingMandatoryData,
    [StringEnum("MSS_Error_Order_TenantsNotUnique_Message")] Error_Order_TenantsNotUnique_Message,
    [StringEnum("MSS_Error_Order_MetersNotUnique_Message")] Error_Order_MetersNotUnique_Message,
    [StringEnum("MSS_Order_Warning_Title")] Warning_UpdateDueDateStructureValue_Title,
    [StringEnum("MSS_Order_UpdateDueDateStructureValue_Message")] Warning_UpdateDueDateStructureValue_Message,
    [StringEnum("MSS_Error_Incorrect_Format")] Error_Incorrect_Format,
    [StringEnum("MSS_DeleteStructure_Warning_Title")] Warning_DeleteStructure_Title,
    [StringEnum("MSS_DeleteFixedStructure_Warning_Message")] Warning_DeleteFixedStructure,
    [StringEnum("MSS_DeleteLogicalStructure_Warning_Message")] Warning_DeleteLogicalStructure,
    [StringEnum("MSS_DeleteStructure_Warning_Message")] Warning_DeleteStructure_Message,
    [StringEnum("MSS_DeletePhysicalNodeInReadingOrder_Warning_Message")] Warning_DeletePhysicalNodeInReadingOrder_Message,
    [StringEnum("MSS_Minoconnect_AccessError")] Error_Minoconnect_Access,
    [StringEnum("ERR_FAILED_TO_SAS_IMPORT")] Error_SAS_Import,
    [StringEnum("MSS_Error_MigrationFailed")] Error_MigrationFailed,
    [StringEnum("Error_ImportRadioMeters_MediumFormat")] Error_ImportRadioMeters_MediumFormat,
    [StringEnum("Error_ImportRadioMeters_ReadingEnabledFormat")] Error_ImportRadioMeters_ReadingEnabledFormat,
  }
}
