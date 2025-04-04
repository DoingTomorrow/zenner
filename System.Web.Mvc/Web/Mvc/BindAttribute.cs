// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.BindAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
  public sealed class BindAttribute : Attribute
  {
    private string _exclude;
    private string[] _excludeSplit = new string[0];
    private string _include;
    private string[] _includeSplit = new string[0];

    public string Exclude
    {
      get => this._exclude ?? string.Empty;
      set
      {
        this._exclude = value;
        this._excludeSplit = AuthorizeAttribute.SplitString(value);
      }
    }

    public string Include
    {
      get => this._include ?? string.Empty;
      set
      {
        this._include = value;
        this._includeSplit = AuthorizeAttribute.SplitString(value);
      }
    }

    public string Prefix { get; set; }

    internal static bool IsPropertyAllowed(
      string propertyName,
      string[] includeProperties,
      string[] excludeProperties)
    {
      bool flag1 = includeProperties == null || includeProperties.Length == 0 || ((IEnumerable<string>) includeProperties).Contains<string>(propertyName, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      bool flag2 = excludeProperties != null && ((IEnumerable<string>) excludeProperties).Contains<string>(propertyName, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      return flag1 && !flag2;
    }

    public bool IsPropertyAllowed(string propertyName)
    {
      return BindAttribute.IsPropertyAllowed(propertyName, this._includeSplit, this._excludeSplit);
    }
  }
}
