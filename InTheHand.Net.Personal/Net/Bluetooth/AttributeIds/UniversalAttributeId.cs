// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net.Bluetooth.AttributeIds
{
  public static class UniversalAttributeId
  {
    public const ServiceAttributeId ServiceRecordHandle = (ServiceAttributeId) 0;
    public const ServiceAttributeId ServiceClassIdList = (ServiceAttributeId) 1;
    public const ServiceAttributeId ServiceRecordState = (ServiceAttributeId) 2;
    public const ServiceAttributeId ServiceId = (ServiceAttributeId) 3;
    public const ServiceAttributeId ProtocolDescriptorList = (ServiceAttributeId) 4;
    public const ServiceAttributeId BrowseGroupList = (ServiceAttributeId) 5;
    public const ServiceAttributeId LanguageBaseAttributeIdList = (ServiceAttributeId) 6;
    public const ServiceAttributeId ServiceInfoTimeToLive = (ServiceAttributeId) 7;
    public const ServiceAttributeId ServiceAvailability = (ServiceAttributeId) 8;
    public const ServiceAttributeId BluetoothProfileDescriptorList = (ServiceAttributeId) 9;
    public const ServiceAttributeId DocumentationUrl = (ServiceAttributeId) 10;
    public const ServiceAttributeId ClientExecutableUrl = (ServiceAttributeId) 11;
    public const ServiceAttributeId IconUrl = (ServiceAttributeId) 12;
    [StringWithLanguageBase]
    public const ServiceAttributeId ServiceName = (ServiceAttributeId) 0;
    [StringWithLanguageBase]
    public const ServiceAttributeId ServiceDescription = (ServiceAttributeId) 1;
    [StringWithLanguageBase]
    public const ServiceAttributeId ProviderName = (ServiceAttributeId) 2;
    public const ServiceAttributeId AdditionalProtocolDescriptorLists = (ServiceAttributeId) 13;
  }
}
