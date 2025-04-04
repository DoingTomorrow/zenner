// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.ExcelDataValidationCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.DataValidation.Contracts;
using OfficeOpenXml.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.DataValidation
{
  public class ExcelDataValidationCollection : 
    XmlHelper,
    IEnumerable<IExcelDataValidation>,
    IEnumerable
  {
    private const string DataValidationPath = "//d:dataValidations";
    private List<IExcelDataValidation> _validations = new List<IExcelDataValidation>();
    private ExcelWorksheet _worksheet;
    private readonly string DataValidationItemsPath = string.Format("{0}/d:dataValidation", (object) "//d:dataValidations");

    internal ExcelDataValidationCollection(ExcelWorksheet worksheet)
      : base(worksheet.NameSpaceManager, (XmlNode) worksheet.WorksheetXml.DocumentElement)
    {
      Require.Argument<ExcelWorksheet>(worksheet).IsNotNull<ExcelWorksheet>(nameof (worksheet));
      this._worksheet = worksheet;
      this.SchemaNodeOrder = worksheet.SchemaNodeOrder;
      XmlNodeList xmlNodeList = worksheet.WorksheetXml.SelectNodes(this.DataValidationItemsPath, worksheet.NameSpaceManager);
      if (xmlNodeList != null && xmlNodeList.Count > 0)
      {
        foreach (XmlNode itemElementNode in xmlNodeList)
        {
          if (itemElementNode.Attributes["sqref"] != null && itemElementNode.Attributes["type"] != null)
          {
            string address = itemElementNode.Attributes["sqref"].Value;
            this._validations.Add((IExcelDataValidation) ExcelDataValidationFactory.Create(ExcelDataValidationType.GetBySchemaName(itemElementNode.Attributes["type"].Value), worksheet, address, itemElementNode));
          }
        }
      }
      if (this._validations.Count <= 0)
        return;
      this.OnValidationCountChanged();
    }

    private void EnsureRootElementExists()
    {
      if (this._worksheet.WorksheetXml.SelectSingleNode("//d:dataValidations", this._worksheet.NameSpaceManager) != null)
        return;
      this.CreateNode("//d:dataValidations".TrimStart('/'));
    }

    private void OnValidationCountChanged()
    {
      if (this.TopNode == null)
        return;
      this.SetXmlNodeString("@count", this._validations.Count.ToString());
    }

    private XmlNode GetRootNode()
    {
      this.EnsureRootElementExists();
      this.TopNode = this._worksheet.WorksheetXml.SelectSingleNode("//d:dataValidations", this._worksheet.NameSpaceManager);
      return this.TopNode;
    }

    private void ValidateAddress(string address, IExcelDataValidation validatingValidation)
    {
      Require.Argument<string>(address).IsNotNullOrEmpty(nameof (address));
      ExcelAddress address1 = new ExcelAddress(address);
      if (this._validations.Count <= 0)
        return;
      foreach (IExcelDataValidation validation in this._validations)
      {
        if ((validatingValidation == null || validatingValidation != validation) && validation.Address.Collide((ExcelAddressBase) address1) != ExcelAddressBase.eAddressCollition.No)
          throw new InvalidOperationException(string.Format("The address ({0}) collides with an existing validation ({1})", (object) address, (object) validation.Address.Address));
      }
    }

    private void ValidateAddress(string address)
    {
      this.ValidateAddress(address, (IExcelDataValidation) null);
    }

    internal void ValidateAll()
    {
      foreach (IExcelDataValidation validation in this._validations)
      {
        validation.Validate();
        this.ValidateAddress(validation.Address.Address, validation);
      }
    }

    public IExcelDataValidationInt AddIntegerValidation(string address)
    {
      this.ValidateAddress(address);
      this.EnsureRootElementExists();
      ExcelDataValidationInt dataValidationInt = new ExcelDataValidationInt(this._worksheet, address, ExcelDataValidationType.Whole);
      this._validations.Add((IExcelDataValidation) dataValidationInt);
      this.OnValidationCountChanged();
      return (IExcelDataValidationInt) dataValidationInt;
    }

    public IExcelDataValidationDecimal AddDecimalValidation(string address)
    {
      this.ValidateAddress(address);
      this.EnsureRootElementExists();
      ExcelDataValidationDecimal validationDecimal = new ExcelDataValidationDecimal(this._worksheet, address, ExcelDataValidationType.Decimal);
      this._validations.Add((IExcelDataValidation) validationDecimal);
      this.OnValidationCountChanged();
      return (IExcelDataValidationDecimal) validationDecimal;
    }

    public IExcelDataValidationList AddListValidation(string address)
    {
      this.ValidateAddress(address);
      this.EnsureRootElementExists();
      ExcelDataValidationList dataValidationList = new ExcelDataValidationList(this._worksheet, address, ExcelDataValidationType.List);
      this._validations.Add((IExcelDataValidation) dataValidationList);
      this.OnValidationCountChanged();
      return (IExcelDataValidationList) dataValidationList;
    }

    public IExcelDataValidationInt AddTextLengthValidation(string address)
    {
      this.ValidateAddress(address);
      this.EnsureRootElementExists();
      ExcelDataValidationInt dataValidationInt = new ExcelDataValidationInt(this._worksheet, address, ExcelDataValidationType.TextLength);
      this._validations.Add((IExcelDataValidation) dataValidationInt);
      this.OnValidationCountChanged();
      return (IExcelDataValidationInt) dataValidationInt;
    }

    public IExcelDataValidationDateTime AddDateTimeValidation(string address)
    {
      this.ValidateAddress(address);
      this.EnsureRootElementExists();
      ExcelDataValidationDateTime validationDateTime = new ExcelDataValidationDateTime(this._worksheet, address, ExcelDataValidationType.DateTime);
      this._validations.Add((IExcelDataValidation) validationDateTime);
      this.OnValidationCountChanged();
      return (IExcelDataValidationDateTime) validationDateTime;
    }

    public IExcelDataValidationTime AddTimeValidation(string address)
    {
      this.ValidateAddress(address);
      this.EnsureRootElementExists();
      ExcelDataValidationTime dataValidationTime = new ExcelDataValidationTime(this._worksheet, address, ExcelDataValidationType.Time);
      this._validations.Add((IExcelDataValidation) dataValidationTime);
      this.OnValidationCountChanged();
      return (IExcelDataValidationTime) dataValidationTime;
    }

    public IExcelDataValidationCustom AddCustomValidation(string address)
    {
      this.ValidateAddress(address);
      this.EnsureRootElementExists();
      ExcelDataValidationCustom validationCustom = new ExcelDataValidationCustom(this._worksheet, address, ExcelDataValidationType.Custom);
      this._validations.Add((IExcelDataValidation) validationCustom);
      this.OnValidationCountChanged();
      return (IExcelDataValidationCustom) validationCustom;
    }

    public bool Remove(IExcelDataValidation item)
    {
      if (!(item is ExcelDataValidation))
        throw new InvalidCastException("The supplied item must inherit OfficeOpenXml.DataValidation.ExcelDataValidation");
      Require.Argument<IExcelDataValidation>(item).IsNotNull<IExcelDataValidation>(nameof (item));
      this.TopNode.RemoveChild(((XmlHelper) item).TopNode);
      bool flag = this._validations.Remove(item);
      if (flag)
        this.OnValidationCountChanged();
      return flag;
    }

    public int Count => this._validations.Count;

    public IExcelDataValidation this[int index]
    {
      get => this._validations[index];
      set => this._validations[index] = value;
    }

    public IExcelDataValidation this[string address]
    {
      get
      {
        ExcelAddress searchedAddress = new ExcelAddress(address);
        return this._validations.Find((Predicate<IExcelDataValidation>) (x => x.Address.Collide((ExcelAddressBase) searchedAddress) != ExcelAddressBase.eAddressCollition.No));
      }
    }

    public IEnumerable<IExcelDataValidation> FindAll(Predicate<IExcelDataValidation> match)
    {
      return (IEnumerable<IExcelDataValidation>) this._validations.FindAll(match);
    }

    public IExcelDataValidation Find(Predicate<IExcelDataValidation> match)
    {
      return this._validations.Find(match);
    }

    public void Clear()
    {
      this.DeleteAllNode(this.DataValidationItemsPath.TrimStart('/'));
      this._validations.Clear();
    }

    public void RemoveAll(Predicate<IExcelDataValidation> match)
    {
      foreach (IExcelDataValidation excelDataValidation in this._validations.FindAll(match))
      {
        if (!(excelDataValidation is ExcelDataValidation))
          throw new InvalidCastException("The supplied item must inherit OfficeOpenXml.DataValidation.ExcelDataValidation");
        this.TopNode.SelectSingleNode("//d:dataValidations".TrimStart('/'), this.NameSpaceManager).RemoveChild(((XmlHelper) excelDataValidation).TopNode);
      }
      this._validations.RemoveAll(match);
      this.OnValidationCountChanged();
    }

    IEnumerator<IExcelDataValidation> IEnumerable<IExcelDataValidation>.GetEnumerator()
    {
      return (IEnumerator<IExcelDataValidation>) this._validations.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._validations.GetEnumerator();
  }
}
