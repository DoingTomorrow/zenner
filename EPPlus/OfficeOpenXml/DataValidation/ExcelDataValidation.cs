// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.ExcelDataValidation
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.DataValidation.Contracts;
using OfficeOpenXml.Utils;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.DataValidation
{
  public abstract class ExcelDataValidation : XmlHelper, IExcelDataValidation
  {
    private const string _itemElementNodeName = "d:dataValidation";
    private readonly string _errorStylePath = "@errorStyle";
    private readonly string _errorTitlePath = "@errorTitle";
    private readonly string _errorPath = "@error";
    private readonly string _promptTitlePath = "@promptTitle";
    private readonly string _promptPath = "@prompt";
    private readonly string _operatorPath = "@operator";
    private readonly string _showErrorMessagePath = "@showErrorMessage";
    private readonly string _showInputMessagePath = "@showInputMessage";
    private readonly string _typeMessagePath = "@type";
    private readonly string _sqrefPath = "@sqref";
    private readonly string _allowBlankPath = "@allowBlank";
    protected readonly string _formula1Path = "d:formula1";
    protected readonly string _formula2Path = "d:formula2";

    internal ExcelDataValidation(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType)
      : this(worksheet, address, validationType, (XmlNode) null)
    {
    }

    internal ExcelDataValidation(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType,
      XmlNode itemElementNode)
      : this(worksheet, address, validationType, itemElementNode, (XmlNamespaceManager) null)
    {
    }

    internal ExcelDataValidation(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(namespaceManager != null ? namespaceManager : worksheet.NameSpaceManager)
    {
      Require.Argument<string>(address).IsNotNullOrEmpty(nameof (address));
      address = this.CheckAndFixRangeAddress(address);
      if (itemElementNode == null)
      {
        this.TopNode = worksheet.WorksheetXml.SelectSingleNode("//d:dataValidations", worksheet.NameSpaceManager);
        string namespaceURI = this.NameSpaceManager.LookupNamespace("d");
        itemElementNode = (XmlNode) this.TopNode.OwnerDocument.CreateElement("d:dataValidation".Split(':')[1], namespaceURI);
        this.TopNode.AppendChild(itemElementNode);
      }
      this.TopNode = itemElementNode;
      this.ValidationType = validationType;
      this.Address = new ExcelAddress(address);
      if (validationType.AllowOperator)
        this.Operator = ExcelDataValidationOperator.any;
      this.Init();
    }

    private void Init()
    {
      this.SchemaNodeOrder = new string[13]
      {
        "type",
        "errorStyle",
        "operator",
        "allowBlank",
        "showInputMessage",
        "showErrorMessage",
        "errorTitle",
        "error",
        "promptTitle",
        "prompt",
        "sqref",
        "formula1",
        "formula2"
      };
    }

    private string CheckAndFixRangeAddress(string address)
    {
      address = !address.Contains<char>(',') ? address.ToUpper() : throw new FormatException("Multiple addresses may not be commaseparated, use space instead");
      if (Regex.IsMatch(address, "[A-Z]+:[A-Z]+"))
        address = AddressUtility.ParseEntireColumnSelections(address);
      return address;
    }

    private void SetNullableBoolValue(string path, bool? val)
    {
      if (val.HasValue)
        this.SetXmlNodeBool(path, val.Value);
      else
        this.DeleteNode(path);
    }

    public virtual void Validate()
    {
      string address = this.Address.Address;
      if (string.IsNullOrEmpty(this.Formula1Internal))
        throw new InvalidOperationException("Validation of " + address + " failed: Formula1 cannot be empty");
    }

    public bool AllowsOperator => this.ValidationType.AllowOperator;

    public ExcelAddress Address
    {
      get => new ExcelAddress(this.GetXmlNodeString(this._sqrefPath));
      private set
      {
        this.SetXmlNodeString(this._sqrefPath, AddressUtility.ParseEntireColumnSelections(value.Address));
      }
    }

    public ExcelDataValidationType ValidationType
    {
      get => ExcelDataValidationType.GetBySchemaName(this.GetXmlNodeString(this._typeMessagePath));
      private set => this.SetXmlNodeString(this._typeMessagePath, value.SchemaName);
    }

    public ExcelDataValidationOperator Operator
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString(this._operatorPath);
        return !string.IsNullOrEmpty(xmlNodeString) ? (ExcelDataValidationOperator) Enum.Parse(typeof (ExcelDataValidationOperator), xmlNodeString) : ExcelDataValidationOperator.any;
      }
      set
      {
        if (!this.ValidationType.AllowOperator)
          throw new InvalidOperationException("The current validation type does not allow operator to be set");
        this.SetXmlNodeString(this._operatorPath, value.ToString());
      }
    }

    public ExcelDataValidationWarningStyle ErrorStyle
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString(this._errorStylePath);
        return !string.IsNullOrEmpty(xmlNodeString) ? (ExcelDataValidationWarningStyle) Enum.Parse(typeof (ExcelDataValidationWarningStyle), xmlNodeString) : ExcelDataValidationWarningStyle.undefined;
      }
      set
      {
        if (value == ExcelDataValidationWarningStyle.undefined)
          this.DeleteNode(this._errorStylePath);
        this.SetXmlNodeString(this._errorStylePath, value.ToString());
      }
    }

    public bool? AllowBlank
    {
      get => this.GetXmlNodeBoolNullable(this._allowBlankPath);
      set => this.SetNullableBoolValue(this._allowBlankPath, value);
    }

    public bool? ShowInputMessage
    {
      get => this.GetXmlNodeBoolNullable(this._showInputMessagePath);
      set => this.SetNullableBoolValue(this._showInputMessagePath, value);
    }

    public bool? ShowErrorMessage
    {
      get => this.GetXmlNodeBoolNullable(this._showErrorMessagePath);
      set => this.SetNullableBoolValue(this._showErrorMessagePath, value);
    }

    public string ErrorTitle
    {
      get => this.GetXmlNodeString(this._errorTitlePath);
      set => this.SetXmlNodeString(this._errorTitlePath, value);
    }

    public string Error
    {
      get => this.GetXmlNodeString(this._errorPath);
      set => this.SetXmlNodeString(this._errorPath, value);
    }

    public string PromptTitle
    {
      get => this.GetXmlNodeString(this._promptTitlePath);
      set => this.SetXmlNodeString(this._promptTitlePath, value);
    }

    public string Prompt
    {
      get => this.GetXmlNodeString(this._promptPath);
      set => this.SetXmlNodeString(this._promptPath, value);
    }

    protected string Formula1Internal => this.GetXmlNodeString(this._formula1Path);

    protected string Formula2Internal => this.GetXmlNodeString(this._formula2Path);

    protected void SetValue<T>(T? val, string path) where T : struct
    {
      if (!val.HasValue)
        this.DeleteNode(path);
      string str = val.Value.ToString().Replace(',', '.');
      this.SetXmlNodeString(path, str);
    }
  }
}
