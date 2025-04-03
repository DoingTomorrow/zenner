// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.DeliveryNote.XML_ZR_Converter
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace ZENNER.CommonLibrary.DeliveryNote
{
  public class XML_ZR_Converter : Converter
  {
    public ZENNER.CommonLibrary.DeliveryNote.DeliveryNote readDeliveryNoteFile(
      string completeFilePath)
    {
      ZENNER.CommonLibrary.DeliveryNote.DeliveryNote deliveryNote = new ZENNER.CommonLibrary.DeliveryNote.DeliveryNote();
      XmlDocument xmlDocument = new XmlDocument();
      try
      {
        xmlDocument.Load(completeFilePath);
        XmlElement documentElement = xmlDocument.DocumentElement;
        XmlNode firstChild = documentElement.FirstChild;
        DateTime exact = DateTime.ParseExact(firstChild.Attributes["DeliveryDate"].InnerText, "dd.MM.yyyy HH:mm:ss", (IFormatProvider) CultureInfo.InvariantCulture);
        int int32 = Convert.ToInt32(firstChild.Attributes["DeliveryNumber"].InnerText);
        deliveryNote.DeliveryNotDate = exact;
        deliveryNote.DeliveryNoteNr = int32;
        for (int i = 1; i < documentElement.ChildNodes.Count; ++i)
        {
          Device_DeliveryNote deviceDeliveryNote = new Device_DeliveryNote();
          Paramter_Device paramterDevice = new Paramter_Device();
          foreach (XmlNode xmlNode in documentElement.ChildNodes[i])
          {
            Tuple<bool, Converter.ParameterNaming> tuple = this.keyFromString(xmlNode.Name);
            if (!tuple.Item1)
              throw new InvalidCastException("Casting failed of :" + xmlNode.Name);
            deviceDeliveryNote.Parameters.Add(new Paramter_Device(tuple.Item2, xmlNode.InnerText));
          }
          deliveryNote.Devices.Add(deviceDeliveryNote);
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to read deliveryNote File: " + completeFilePath + Environment.NewLine + ex.Message);
      }
      return deliveryNote;
    }

    public bool createDeliveryFile(
      string path,
      string fileName,
      out string completeFilePath,
      ZENNER.CommonLibrary.DeliveryNote.DeliveryNote deliveryNote)
    {
      completeFilePath = Path.Combine(path, fileName + ".xml");
      XmlDocument xmlDocument = new XmlDocument();
      XmlNode element1 = (XmlNode) xmlDocument.CreateElement(fileName);
      xmlDocument.AppendChild(element1);
      XmlNode element2 = (XmlNode) xmlDocument.CreateElement("DeliveryHead");
      XmlAttribute attribute1 = xmlDocument.CreateAttribute("DeliveryDate");
      attribute1.InnerText = deliveryNote.DeliveryNotDate.ToString();
      element2.Attributes.Append(attribute1);
      XmlAttribute attribute2 = xmlDocument.CreateAttribute("DeliveryNumber");
      attribute2.InnerText = deliveryNote.DeliveryNoteNr.ToString();
      element2.Attributes.Append(attribute2);
      element1.AppendChild(element2);
      foreach (Device_DeliveryNote device in deliveryNote.Devices)
      {
        if (device.Parameters.Count != 0)
        {
          XmlNode element3 = (XmlNode) xmlDocument.CreateElement("Device");
          element1.AppendChild(element3);
          foreach (Paramter_Device parameter in device.Parameters)
          {
            XmlNode element4 = (XmlNode) xmlDocument.CreateElement(Regex.Replace(this.translateKey(parameter.NameOfParameter), " ", ""));
            element4.InnerText = parameter.Value;
            element3.AppendChild(element4);
          }
        }
      }
      try
      {
        xmlDocument.Save(completeFilePath);
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to save delivery note" + ex.Message);
      }
      return true;
    }
  }
}
