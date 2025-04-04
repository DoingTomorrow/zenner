// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Razor.SetModelTypeCodeGenerator
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Web.Mvc.ExpressionUtil;
using System.Web.Razor.Generator;

#nullable disable
namespace System.Web.Mvc.Razor
{
  internal class SetModelTypeCodeGenerator : SetBaseTypeCodeGenerator
  {
    private string _genericTypeFormat;

    public SetModelTypeCodeGenerator(string modelType, string genericTypeFormat)
      : base(modelType)
    {
      this._genericTypeFormat = genericTypeFormat;
    }

    protected virtual string ResolveType(CodeGeneratorContext context, string baseType)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, this._genericTypeFormat, new object[2]
      {
        (object) context.Host.DefaultBaseClass,
        (object) baseType
      });
    }

    public virtual bool Equals(object obj)
    {
      return obj is SetModelTypeCodeGenerator typeCodeGenerator && base.Equals(obj) && string.Equals(this._genericTypeFormat, typeCodeGenerator._genericTypeFormat, StringComparison.Ordinal);
    }

    public virtual int GetHashCode()
    {
      HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
      hashCodeCombiner.AddInt32(base.GetHashCode());
      hashCodeCombiner.AddObject((object) this._genericTypeFormat);
      return hashCodeCombiner.CombinedHash;
    }

    public virtual string ToString() => "Model:" + this.BaseType;
  }
}
