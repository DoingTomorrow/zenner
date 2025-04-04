// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.FormulaDependency
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class FormulaDependency
  {
    private List<RangeAddress> _referencedBy = new List<RangeAddress>();
    private List<RangeAddress> _references = new List<RangeAddress>();

    public FormulaDependency(ParsingScope scope)
    {
      this.ScopeId = scope.ScopeId;
      this.Address = scope.Address;
    }

    public Guid ScopeId { get; private set; }

    public RangeAddress Address { get; private set; }

    public virtual void AddReferenceFrom(RangeAddress rangeAddress)
    {
      if (this.Address.CollidesWith(rangeAddress) || this._references.Exists((Predicate<RangeAddress>) (x => x.CollidesWith(rangeAddress))))
        throw new CircularReferenceException("Circular reference detected at " + rangeAddress.ToString());
      this._referencedBy.Add(rangeAddress);
    }

    public virtual void AddReferenceTo(RangeAddress rangeAddress)
    {
      if (this.Address.CollidesWith(rangeAddress) || this._referencedBy.Exists((Predicate<RangeAddress>) (x => x.CollidesWith(rangeAddress))))
        throw new CircularReferenceException("Circular reference detected at " + rangeAddress.ToString());
      this._references.Add(rangeAddress);
    }
  }
}
