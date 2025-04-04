// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelProtection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Encryption;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelProtection : XmlHelper
  {
    private const string workbookPasswordPath = "d:workbookProtection/@workbookPassword";
    private const string lockStructurePath = "d:workbookProtection/@lockStructure";
    private const string lockWindowsPath = "d:workbookProtection/@lockWindows";
    private const string lockRevisionPath = "d:workbookProtection/@lockRevision";

    internal ExcelProtection(XmlNamespaceManager ns, XmlNode topNode, ExcelWorkbook wb)
      : base(ns, topNode)
    {
      this.SchemaNodeOrder = wb.SchemaNodeOrder;
    }

    public void SetPassword(string Password)
    {
      if (string.IsNullOrEmpty(Password))
        this.DeleteNode("d:workbookProtection/@workbookPassword");
      else
        this.SetXmlNodeString("d:workbookProtection/@workbookPassword", ((int) EncryptedPackageHandler.CalculatePasswordHash(Password)).ToString("x"));
    }

    public bool LockStructure
    {
      get => this.GetXmlNodeBool("d:workbookProtection/@lockStructure", false);
      set => this.SetXmlNodeBool("d:workbookProtection/@lockStructure", value, false);
    }

    public bool LockWindows
    {
      get => this.GetXmlNodeBool("d:workbookProtection/@lockWindows", false);
      set => this.SetXmlNodeBool("d:workbookProtection/@lockWindows", value, false);
    }

    public bool LockRevision
    {
      get => this.GetXmlNodeBool("d:workbookProtection/@lockRevision", false);
      set => this.SetXmlNodeBool("d:workbookProtection/@lockRevision", value, false);
    }
  }
}
