// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.OfficeProperties
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public sealed class OfficeProperties : XmlHelper
  {
    private const string TitlePath = "dc:title";
    private const string SubjectPath = "dc:subject";
    private const string AuthorPath = "dc:creator";
    private const string CommentsPath = "dc:description";
    private const string KeywordsPath = "cp:keywords";
    private const string LastModifiedByPath = "cp:lastModifiedBy";
    private const string LastPrintedPath = "cp:lastPrinted";
    private const string CategoryPath = "cp:category";
    private const string ContentStatusPath = "cp:contentStatus";
    private const string ApplicationPath = "xp:Properties/xp:Application";
    private const string HyperlinkBasePath = "xp:Properties/xp:HyperlinkBase";
    private const string AppVersionPath = "xp:Properties/xp:AppVersion";
    private const string CompanyPath = "xp:Properties/xp:Company";
    private const string ManagerPath = "xp:Properties/xp:Manager";
    private XmlDocument _xmlPropertiesCore;
    private XmlDocument _xmlPropertiesExtended;
    private XmlDocument _xmlPropertiesCustom;
    private Uri _uriPropertiesCore = new Uri("/docProps/core.xml", UriKind.Relative);
    private Uri _uriPropertiesExtended = new Uri("/docProps/app.xml", UriKind.Relative);
    private Uri _uriPropertiesCustom = new Uri("/docProps/custom.xml", UriKind.Relative);
    private XmlHelper _coreHelper;
    private XmlHelper _extendedHelper;
    private XmlHelper _customHelper;
    private ExcelPackage _package;

    internal OfficeProperties(ExcelPackage package, XmlNamespaceManager ns)
      : base(ns)
    {
      this._package = package;
      this._coreHelper = XmlHelperFactory.Create(ns, this.CorePropertiesXml.SelectSingleNode("cp:coreProperties", this.NameSpaceManager));
      this._extendedHelper = XmlHelperFactory.Create(ns, (XmlNode) this.ExtendedPropertiesXml);
      this._customHelper = XmlHelperFactory.Create(ns, (XmlNode) this.CustomPropertiesXml);
    }

    public XmlDocument CorePropertiesXml
    {
      get
      {
        if (this._xmlPropertiesCore == null)
          this._xmlPropertiesCore = this.GetXmlDocument(string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?><cp:coreProperties xmlns:cp=\"{0}\" xmlns:dc=\"{1}\" xmlns:dcterms=\"{2}\" xmlns:dcmitype=\"{3}\" xmlns:xsi=\"{4}\"></cp:coreProperties>", (object) "http://schemas.openxmlformats.org/package/2006/metadata/core-properties", (object) "http://purl.org/dc/elements/1.1/", (object) "http://purl.org/dc/terms/", (object) "http://purl.org/dc/dcmitype/", (object) "http://www.w3.org/2001/XMLSchema-instance"), this._uriPropertiesCore, "application/vnd.openxmlformats-package.core-properties+xml", "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties");
        return this._xmlPropertiesCore;
      }
    }

    private XmlDocument GetXmlDocument(
      string startXml,
      Uri uri,
      string contentType,
      string relationship)
    {
      XmlDocument xmlDocument;
      if (this._package.Package.PartExists(uri))
      {
        xmlDocument = this._package.GetXmlFromUri(uri);
      }
      else
      {
        xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(startXml);
        StreamWriter writer = new StreamWriter((Stream) this._package.Package.CreatePart(uri, contentType).GetStream(FileMode.Create, FileAccess.Write));
        xmlDocument.Save((TextWriter) writer);
        this._package.Package.Flush();
        this._package.Package.CreateRelationship(UriHelper.GetRelativeUri(new Uri("/xl", UriKind.Relative), uri), TargetMode.Internal, relationship);
        this._package.Package.Flush();
      }
      return xmlDocument;
    }

    public string Title
    {
      get => this._coreHelper.GetXmlNodeString("dc:title");
      set => this._coreHelper.SetXmlNodeString("dc:title", value);
    }

    public string Subject
    {
      get => this._coreHelper.GetXmlNodeString("dc:subject");
      set => this._coreHelper.SetXmlNodeString("dc:subject", value);
    }

    public string Author
    {
      get => this._coreHelper.GetXmlNodeString("dc:creator");
      set => this._coreHelper.SetXmlNodeString("dc:creator", value);
    }

    public string Comments
    {
      get => this._coreHelper.GetXmlNodeString("dc:description");
      set => this._coreHelper.SetXmlNodeString("dc:description", value);
    }

    public string Keywords
    {
      get => this._coreHelper.GetXmlNodeString("cp:keywords");
      set => this._coreHelper.SetXmlNodeString("cp:keywords", value);
    }

    public string LastModifiedBy
    {
      get => this._coreHelper.GetXmlNodeString("cp:lastModifiedBy");
      set => this._coreHelper.SetXmlNodeString("cp:lastModifiedBy", value);
    }

    public string LastPrinted
    {
      get => this._coreHelper.GetXmlNodeString("cp:lastPrinted");
      set => this._coreHelper.SetXmlNodeString("cp:lastPrinted", value);
    }

    public string Category
    {
      get => this._coreHelper.GetXmlNodeString("cp:category");
      set => this._coreHelper.SetXmlNodeString("cp:category", value);
    }

    public string Status
    {
      get => this._coreHelper.GetXmlNodeString("cp:contentStatus");
      set => this._coreHelper.SetXmlNodeString("cp:contentStatus", value);
    }

    public XmlDocument ExtendedPropertiesXml
    {
      get
      {
        if (this._xmlPropertiesExtended == null)
          this._xmlPropertiesExtended = this.GetXmlDocument(string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?><Properties xmlns:vt=\"{0}\" xmlns=\"{1}\"></Properties>", (object) "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes", (object) "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties"), this._uriPropertiesExtended, "application/vnd.openxmlformats-officedocument.extended-properties+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties");
        return this._xmlPropertiesExtended;
      }
    }

    public string Application
    {
      get => this._extendedHelper.GetXmlNodeString("xp:Properties/xp:Application");
    }

    public Uri HyperlinkBase
    {
      get
      {
        return new Uri(this._extendedHelper.GetXmlNodeString("xp:Properties/xp:HyperlinkBase"), UriKind.Absolute);
      }
      set
      {
        this._extendedHelper.SetXmlNodeString("xp:Properties/xp:HyperlinkBase", value.AbsoluteUri);
      }
    }

    public string AppVersion
    {
      get => this._extendedHelper.GetXmlNodeString("xp:Properties/xp:AppVersion");
    }

    public string Company
    {
      get => this._extendedHelper.GetXmlNodeString("xp:Properties/xp:Company");
      set => this._extendedHelper.SetXmlNodeString("xp:Properties/xp:Company", value);
    }

    public string Manager
    {
      get => this._extendedHelper.GetXmlNodeString("xp:Properties/xp:Manager");
      set => this._extendedHelper.SetXmlNodeString("xp:Properties/xp:Manager", value);
    }

    private string GetExtendedPropertyValue(string propertyName)
    {
      string extendedPropertyValue = (string) null;
      XmlNode xmlNode = this.ExtendedPropertiesXml.SelectSingleNode(string.Format("xp:Properties/xp:{0}", (object) propertyName), this.NameSpaceManager);
      if (xmlNode != null)
        extendedPropertyValue = xmlNode.InnerText;
      return extendedPropertyValue;
    }

    public XmlDocument CustomPropertiesXml
    {
      get
      {
        if (this._xmlPropertiesCustom == null)
          this._xmlPropertiesCustom = this.GetXmlDocument(string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?><Properties xmlns:vt=\"{0}\" xmlns=\"{1}\"></Properties>", (object) "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes", (object) "http://schemas.openxmlformats.org/officeDocument/2006/custom-properties"), this._uriPropertiesCustom, "application/vnd.openxmlformats-officedocument.custom-properties+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties");
        return this._xmlPropertiesCustom;
      }
    }

    public object GetCustomPropertyValue(string propertyName)
    {
      if (!(this.CustomPropertiesXml.SelectSingleNode(string.Format("ctp:Properties/ctp:property[@name='{0}']", (object) propertyName), this.NameSpaceManager) is XmlElement xmlElement))
        return (object) null;
      string innerText = xmlElement.LastChild.InnerText;
      switch (xmlElement.LastChild.LocalName)
      {
        case "filetime":
          DateTime result1;
          return DateTime.TryParse(innerText, out result1) ? (object) result1 : (object) null;
        case "i4":
          int result2;
          return int.TryParse(innerText, out result2) ? (object) result2 : (object) null;
        case "r8":
          double result3;
          return double.TryParse(innerText, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result3) ? (object) result3 : (object) null;
        case "bool":
          switch (innerText)
          {
            case "true":
              return (object) true;
            case "false":
              return (object) false;
            default:
              return (object) null;
          }
        default:
          return (object) innerText;
      }
    }

    public void SetCustomPropertyValue(string propertyName, object value)
    {
      XmlNode xmlNode1 = this.CustomPropertiesXml.SelectSingleNode("ctp:Properties", this.NameSpaceManager);
      if (!(this.CustomPropertiesXml.SelectSingleNode(string.Format("ctp:Properties/ctp:property[@name='{0}']", (object) propertyName), this.NameSpaceManager) is XmlElement newChild))
      {
        XmlNode xmlNode2 = this.CustomPropertiesXml.SelectSingleNode("ctp:Properties/ctp:property[not(@pid <= preceding-sibling::ctp:property/@pid) and not(@pid <= following-sibling::ctp:property/@pid)]", this.NameSpaceManager);
        int num;
        if (xmlNode2 == null)
        {
          num = 2;
        }
        else
        {
          int result;
          if (!int.TryParse(xmlNode2.Attributes["pid"].Value, out result))
            result = 2;
          num = result + 1;
        }
        newChild = this.CustomPropertiesXml.CreateElement("property", "http://schemas.openxmlformats.org/officeDocument/2006/custom-properties");
        newChild.SetAttribute("fmtid", "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}");
        newChild.SetAttribute("pid", num.ToString());
        newChild.SetAttribute("name", propertyName);
        xmlNode1.AppendChild((XmlNode) newChild);
      }
      else
      {
        while (newChild.ChildNodes.Count > 0)
          newChild.RemoveChild(newChild.ChildNodes[0]);
      }
      XmlElement element;
      switch (value)
      {
        case bool _:
          element = this.CustomPropertiesXml.CreateElement("vt", "bool", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
          element.InnerText = value.ToString().ToLower();
          break;
        case DateTime dateTime:
          element = this.CustomPropertiesXml.CreateElement("vt", "filetime", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
          element.InnerText = dateTime.AddHours(-1.0).ToString("yyyy-MM-ddTHH:mm:ssZ");
          break;
        case short _:
        case int _:
          element = this.CustomPropertiesXml.CreateElement("vt", "i4", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
          element.InnerText = value.ToString();
          break;
        case double _:
        case Decimal _:
        case float _:
        case long _:
          element = this.CustomPropertiesXml.CreateElement("vt", "r8", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
          switch (value)
          {
            case double num1:
              element.InnerText = num1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              break;
            case float num2:
              element.InnerText = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              break;
            case Decimal num3:
              element.InnerText = num3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              break;
            default:
              element.InnerText = value.ToString();
              break;
          }
          break;
        default:
          element = this.CustomPropertiesXml.CreateElement("vt", "lpwstr", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
          element.InnerText = value.ToString();
          break;
      }
      newChild.AppendChild((XmlNode) element);
    }

    internal void Save()
    {
      if (this._xmlPropertiesCore != null)
        this._package.SavePart(this._uriPropertiesCore, this._xmlPropertiesCore);
      if (this._xmlPropertiesExtended != null)
        this._package.SavePart(this._uriPropertiesExtended, this._xmlPropertiesExtended);
      if (this._xmlPropertiesCustom == null)
        return;
      this._package.SavePart(this._uriPropertiesCustom, this._xmlPropertiesCustom);
    }
  }
}
