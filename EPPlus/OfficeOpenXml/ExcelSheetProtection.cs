// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelSheetProtection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Encryption;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public sealed class ExcelSheetProtection : XmlHelper
  {
    private const string _isProtectedPath = "d:sheetProtection/@sheet";
    private const string _allowSelectLockedCellsPath = "d:sheetProtection/@selectLockedCells";
    private const string _allowSelectUnlockedCellsPath = "d:sheetProtection/@selectUnlockedCells";
    private const string _allowObjectPath = "d:sheetProtection/@objects";
    private const string _allowScenariosPath = "d:sheetProtection/@scenarios";
    private const string _allowFormatCellsPath = "d:sheetProtection/@formatCells";
    private const string _allowFormatColumnsPath = "d:sheetProtection/@formatColumns";
    private const string _allowFormatRowsPath = "d:sheetProtection/@formatRows";
    private const string _allowInsertColumnsPath = "d:sheetProtection/@insertColumns";
    private const string _allowInsertRowsPath = "d:sheetProtection/@insertRows";
    private const string _allowInsertHyperlinksPath = "d:sheetProtection/@insertHyperlinks";
    private const string _allowDeleteColumns = "d:sheetProtection/@deleteColumns";
    private const string _allowDeleteRowsPath = "d:sheetProtection/@deleteRows";
    private const string _allowSortPath = "d:sheetProtection/@sort";
    private const string _allowAutoFilterPath = "d:sheetProtection/@autoFilter";
    private const string _allowPivotTablesPath = "d:sheetProtection/@pivotTables";
    private const string _passwordPath = "d:sheetProtection/@password";

    internal ExcelSheetProtection(XmlNamespaceManager nsm, XmlNode topNode, ExcelWorksheet ws)
      : base(nsm, topNode)
    {
      this.SchemaNodeOrder = ws.SchemaNodeOrder;
    }

    public bool IsProtected
    {
      get => this.GetXmlNodeBool("d:sheetProtection/@sheet", false);
      set
      {
        this.SetXmlNodeBool("d:sheetProtection/@sheet", value, false);
        if (value)
        {
          this.AllowEditObject = true;
          this.AllowEditScenarios = true;
        }
        else
          this.DeleteAllNode("d:sheetProtection/@sheet");
      }
    }

    public bool AllowSelectLockedCells
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@selectLockedCells", false);
      set => this.SetXmlNodeBool("d:sheetProtection/@selectLockedCells", !value, false);
    }

    public bool AllowSelectUnlockedCells
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@selectUnlockedCells", false);
      set => this.SetXmlNodeBool("d:sheetProtection/@selectUnlockedCells", !value, false);
    }

    public bool AllowEditObject
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@objects", false);
      set => this.SetXmlNodeBool("d:sheetProtection/@objects", !value, false);
    }

    public bool AllowEditScenarios
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@scenarios", false);
      set => this.SetXmlNodeBool("d:sheetProtection/@scenarios", !value, false);
    }

    public bool AllowFormatCells
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@formatCells", true);
      set => this.SetXmlNodeBool("d:sheetProtection/@formatCells", !value, true);
    }

    public bool AllowFormatColumns
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@formatColumns", true);
      set => this.SetXmlNodeBool("d:sheetProtection/@formatColumns", !value, true);
    }

    public bool AllowFormatRows
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@formatRows", true);
      set => this.SetXmlNodeBool("d:sheetProtection/@formatRows", !value, true);
    }

    public bool AllowInsertColumns
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@insertColumns", true);
      set => this.SetXmlNodeBool("d:sheetProtection/@insertColumns", !value, true);
    }

    public bool AllowInsertRows
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@insertRows", true);
      set => this.SetXmlNodeBool("d:sheetProtection/@insertRows", !value, true);
    }

    public bool AllowInsertHyperlinks
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@insertHyperlinks", true);
      set => this.SetXmlNodeBool("d:sheetProtection/@insertHyperlinks", !value, true);
    }

    public bool AllowDeleteColumns
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@deleteColumns", true);
      set => this.SetXmlNodeBool("d:sheetProtection/@deleteColumns", !value, true);
    }

    public bool AllowDeleteRows
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@deleteRows", true);
      set => this.SetXmlNodeBool("d:sheetProtection/@deleteRows", !value, true);
    }

    public bool AllowSort
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@sort", true);
      set => this.SetXmlNodeBool("d:sheetProtection/@sort", !value, true);
    }

    public bool AllowAutoFilter
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@autoFilter", true);
      set => this.SetXmlNodeBool("d:sheetProtection/@autoFilter", !value, true);
    }

    public bool AllowPivotTables
    {
      get => !this.GetXmlNodeBool("d:sheetProtection/@pivotTables", true);
      set => this.SetXmlNodeBool("d:sheetProtection/@pivotTables", !value, true);
    }

    public void SetPassword(string Password)
    {
      if (!this.IsProtected)
        this.IsProtected = true;
      Password = Password.Trim();
      if (Password == "")
      {
        XmlNode node = this.TopNode.SelectSingleNode("d:sheetProtection/@password", this.NameSpaceManager);
        if (node == null)
          return;
        (node as XmlAttribute).OwnerElement.Attributes.Remove(node as XmlAttribute);
      }
      else
        this.SetXmlNodeString("d:sheetProtection/@password", ((int) EncryptedPackageHandler.CalculatePasswordHash(Password)).ToString("x"));
    }
  }
}
