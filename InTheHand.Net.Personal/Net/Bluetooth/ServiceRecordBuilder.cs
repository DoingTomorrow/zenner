// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ServiceRecordBuilder
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public class ServiceRecordBuilder
  {
    private const bool m_allowDuplicates = false;
    private const string ErrorMsg_Duplicate = "ServiceRecordBuilder is configured to allow only one of each attribute id.";
    private const string ErrorMsg_NoServiceClasses = "Record has no Service Class IDs.";
    private string m_ServiceName;
    private string m_ProviderName;
    private string m_ServiceDescription;
    private ArrayList m_classIds = new ArrayList();
    private BluetoothProtocolDescriptorType m_ProtocolType = BluetoothProtocolDescriptorType.Rfcomm;
    private List<ServiceRecordBuilder.BtPdlItem> m_profileDescrs = new List<ServiceRecordBuilder.BtPdlItem>();
    private List<ServiceAttribute> m_customList = new List<ServiceAttribute>();

    public ServiceRecord ServiceRecord
    {
      get
      {
        List<ServiceAttribute> serviceAttributeList = new List<ServiceAttribute>();
        bool flag = false;
        serviceAttributeList.Add(new ServiceAttribute((ServiceAttributeId) 1, new ServiceElement(ElementType.ElementSequence, (IList<ServiceElement>) this.BuildServiceClassIdList())));
        ServiceElement serviceElement1;
        switch (this.m_ProtocolType)
        {
          case BluetoothProtocolDescriptorType.None:
            serviceElement1 = (ServiceElement) null;
            break;
          case BluetoothProtocolDescriptorType.L2Cap:
            serviceElement1 = ServiceRecordHelper.CreateL2CapProtocolDescriptorList();
            break;
          case BluetoothProtocolDescriptorType.Rfcomm:
            serviceElement1 = ServiceRecordHelper.CreateRfcommProtocolDescriptorList();
            break;
          case BluetoothProtocolDescriptorType.GeneralObex:
            serviceElement1 = ServiceRecordHelper.CreateGoepProtocolDescriptorList();
            break;
          default:
            throw new InvalidOperationException("Unknown protocol type: " + this.m_ProtocolType.ToString() + ".");
        }
        if (serviceElement1 != null)
          serviceAttributeList.Add(new ServiceAttribute((ServiceAttributeId) 4, serviceElement1));
        if (this.m_ServiceName != null)
        {
          serviceAttributeList.Add(new ServiceAttribute((ServiceAttributeId) 256, new ServiceElement(ElementType.TextString, (object) this.m_ServiceName)));
          flag = true;
        }
        if (this.m_ProviderName != null)
        {
          serviceAttributeList.Add(new ServiceAttribute((ServiceAttributeId) 258, new ServiceElement(ElementType.TextString, (object) this.m_ProviderName)));
          flag = true;
        }
        if (this.m_ServiceDescription != null)
        {
          serviceAttributeList.Add(new ServiceAttribute((ServiceAttributeId) 257, new ServiceElement(ElementType.TextString, (object) this.m_ServiceDescription)));
          flag = true;
        }
        if (flag)
          serviceAttributeList.Add(new ServiceAttribute((ServiceAttributeId) 6, ServiceRecordBuilder.CreateEnglishUtf8PrimaryLanguageServiceElement()));
        if (this.m_profileDescrs.Count != 0)
        {
          ArrayList arrayList = new ArrayList();
          foreach (ServiceRecordBuilder.BtPdlItem profileDescr in this.m_profileDescrs)
          {
            ServiceElement serviceElement2 = new ServiceElement(ElementType.ElementSequence, new ServiceElement[2]
            {
              ServiceRecordBuilder.ServiceElementFromUuid((object) profileDescr.m_classId),
              new ServiceElement(ElementType.UInt16, (object) (ushort) ((uint) profileDescr.m_version * 256U + (uint) profileDescr.m_subVersion))
            });
            arrayList.Add((object) serviceElement2);
          }
          serviceAttributeList.Add(new ServiceAttribute((ServiceAttributeId) 9, new ServiceElement(ElementType.ElementSequence, (object) arrayList.ToArray(typeof (ServiceElement)))));
        }
        serviceAttributeList.AddRange((IEnumerable<ServiceAttribute>) this.m_customList);
        ServiceRecordBuilder.ReportIfDuplicates(serviceAttributeList, true);
        serviceAttributeList.Sort((Comparison<ServiceAttribute>) ((x, y) => x.IdAsOrdinalNumber.CompareTo(y.IdAsOrdinalNumber)));
        return new ServiceRecord((IList<ServiceAttribute>) serviceAttributeList);
      }
    }

    private static void ReportIfDuplicates(List<ServiceAttribute> list, bool storedList)
    {
      Dictionary<ServiceAttributeId, ServiceAttribute> dictionary = new Dictionary<ServiceAttributeId, ServiceAttribute>(list.Count);
      foreach (ServiceAttribute serviceAttribute in list)
      {
        if (dictionary.ContainsKey(serviceAttribute.Id))
        {
          if (storedList)
            throw new InvalidOperationException("ServiceRecordBuilder is configured to allow only one of each attribute id.");
          throw new ArgumentException("ServiceRecordBuilder is configured to allow only one of each attribute id.");
        }
        dictionary.Add(serviceAttribute.Id, serviceAttribute);
      }
    }

    private List<ServiceElement> BuildServiceClassIdList()
    {
      List<ServiceElement> serviceElementList = new List<ServiceElement>();
      if (this.m_classIds.Count == 0)
        throw new InvalidOperationException("Record has no Service Class IDs.");
      foreach (object classId in this.m_classIds)
        serviceElementList.Add(ServiceRecordBuilder.ServiceElementFromUuid(classId));
      return serviceElementList;
    }

    private static ServiceElement ServiceElementFromUuid(object classRaw)
    {
      ServiceElement serviceElement = (ServiceElement) null;
      uint num = 99;
      bool flag;
      if (classRaw is Guid protocolGuid)
      {
        if (ServiceRecordUtilities.IsUuid32Value(protocolGuid))
        {
          num = ServiceRecordUtilities.GetAsUuid32Value(protocolGuid);
          flag = true;
        }
        else
        {
          serviceElement = new ServiceElement(ElementType.Uuid128, (object) protocolGuid);
          flag = false;
        }
      }
      else
      {
        num = (uint) (int) classRaw;
        flag = true;
      }
      if (flag)
      {
        try
        {
          serviceElement = new ServiceElement(ElementType.Uuid16, (object) Convert.ToUInt16(num));
        }
        catch (OverflowException ex)
        {
          serviceElement = new ServiceElement(ElementType.Uuid32, (object) num);
        }
      }
      return serviceElement;
    }

    private static ServiceElement CreateEnglishUtf8PrimaryLanguageServiceElement()
    {
      return LanguageBaseItem.CreateElementSequenceFromList(new LanguageBaseItem[1]
      {
        LanguageBaseItem.CreateEnglishUtf8PrimaryLanguageItem()
      });
    }

    public string ServiceName
    {
      get => this.m_ServiceName;
      set => this.m_ServiceName = value;
    }

    public string ProviderName
    {
      get => this.m_ProviderName;
      set => this.m_ProviderName = value;
    }

    public string ServiceDescription
    {
      get => this.m_ServiceDescription;
      set => this.m_ServiceDescription = value;
    }

    public BluetoothProtocolDescriptorType ProtocolType
    {
      get => this.m_ProtocolType;
      set => this.m_ProtocolType = value;
    }

    public void AddServiceClass(Guid uuid128) => this.m_classIds.Add((object) uuid128);

    [CLSCompliant(false)]
    public void AddServiceClass(ushort uuid16) => this.m_classIds.Add((object) (int) uuid16);

    [CLSCompliant(false)]
    public void AddServiceClass(uint uuid32) => this.m_classIds.Add((object) (int) uuid32);

    public void AddServiceClass(int uuid16or32) => this.m_classIds.Add((object) uuid16or32);

    public void AddBluetoothProfileDescriptor(Guid classId, byte majorVersion, byte minorVersion)
    {
      this.m_profileDescrs.Add(new ServiceRecordBuilder.BtPdlItem(classId, majorVersion, minorVersion));
    }

    public void AddCustomAttributes(IEnumerable<ServiceAttribute> serviceAttributes)
    {
      List<ServiceAttribute> list = new List<ServiceAttribute>((IEnumerable<ServiceAttribute>) this.m_customList);
      list.AddRange(serviceAttributes);
      ServiceRecordBuilder.ReportIfDuplicates(list, false);
      this.m_customList = list;
    }

    public void AddCustomAttributes(IEnumerable serviceAttributes)
    {
      List<ServiceAttribute> list = new List<ServiceAttribute>((IEnumerable<ServiceAttribute>) this.m_customList);
      foreach (object serviceAttribute1 in serviceAttributes)
      {
        if (!(serviceAttribute1 is ServiceAttribute serviceAttribute2))
          throw new ArgumentException("Every item in the list must be a ServiceAttribute");
        list.Add(serviceAttribute2);
      }
      ServiceRecordBuilder.ReportIfDuplicates(list, false);
      this.m_customList = list;
    }

    public void AddCustomAttributes(params ServiceAttribute[] serviceAttributes)
    {
      this.AddCustomAttributes((IEnumerable<ServiceAttribute>) serviceAttributes);
    }

    public void AddCustomAttribute(ServiceAttribute serviceAttribute)
    {
      this.AddCustomAttributes(serviceAttribute);
    }

    public void AddCustomAttribute(ServiceAttributeId id, ElementType elementType, object value)
    {
      ServiceElement serviceElement;
      switch (ServiceRecordParser.GetEtdForType(elementType))
      {
        case ElementTypeDescriptor.UnsignedInteger:
        case ElementTypeDescriptor.TwosComplementInteger:
          serviceElement = ServiceElement.CreateNumericalServiceElement(elementType, value);
          break;
        default:
          serviceElement = new ServiceElement(elementType, value);
          break;
      }
      this.AddCustomAttribute(new ServiceAttribute(id, serviceElement));
    }

    [CLSCompliant(false)]
    public void AddCustomAttribute(ushort id, ElementType elementType, object value)
    {
      this.AddCustomAttribute((ServiceAttributeId) id, elementType, value);
    }

    public static ServiceRecordBuilder FromJsr82ServerUri(string url)
    {
      ServiceRecordBuilder serviceRecordBuilder = new ServiceRecordBuilder();
      Match match = Regex.Match(url, "^([a-z0-9]+)://localhost:([0-9a-fA-F]{32})(?:;([a-zA-Z]+)=([a-zA-Z0-9 _-]+))*$");
      if (!match.Success)
        throw new ArgumentException("Invalid URI format.");
      string str = match.Groups[1].Value;
      string g = match.Groups[2].Value;
      switch (str)
      {
        case "btl2cap":
          serviceRecordBuilder.ProtocolType = BluetoothProtocolDescriptorType.L2Cap;
          break;
        case "btspp":
          serviceRecordBuilder.ProtocolType = BluetoothProtocolDescriptorType.Rfcomm;
          break;
        case "btgoep":
          serviceRecordBuilder.ProtocolType = BluetoothProtocolDescriptorType.GeneralObex;
          break;
        default:
          throw new ArgumentException("Unknown JSR82 URI scheme part.");
      }
      Guid uuid128 = new Guid(g);
      serviceRecordBuilder.AddServiceClass(uuid128);
      for (int groupnum = 3; groupnum < match.Groups.Count; groupnum += 2)
      {
        if ("NAME".Equals(match.Groups[groupnum].Value.ToUpper(CultureInfo.InvariantCulture)))
          serviceRecordBuilder.ServiceName = match.Groups[groupnum + 1].Value;
      }
      return serviceRecordBuilder;
    }

    internal struct BtPdlItem
    {
      internal readonly Guid m_classId;
      internal readonly byte m_version;
      internal readonly byte m_subVersion;

      internal BtPdlItem(Guid classId, byte version, byte subVersion)
      {
        this.m_classId = classId;
        this.m_version = version;
        this.m_subVersion = subVersion;
      }
    }
  }
}
