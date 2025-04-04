// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.UrlDataList
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages
{
  internal class UrlDataList : IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable
  {
    private List<string> _urlData;

    public UrlDataList(string pathInfo)
    {
      if (string.IsNullOrEmpty(pathInfo))
        this._urlData = new List<string>();
      else
        this._urlData = ((IEnumerable<string>) pathInfo.Split('/')).ToList<string>();
    }

    public int Count => this._urlData.Count;

    public bool IsReadOnly => true;

    public string this[int index]
    {
      get => index >= this._urlData.Count ? string.Empty : this._urlData[index];
      set => throw new NotSupportedException(WebPageResources.UrlData_ReadOnly);
    }

    public int IndexOf(string item) => this._urlData.IndexOf(item);

    public void Insert(int index, string item)
    {
      throw new NotSupportedException(WebPageResources.UrlData_ReadOnly);
    }

    public void RemoveAt(int index)
    {
      throw new NotSupportedException(WebPageResources.UrlData_ReadOnly);
    }

    public void Add(string item)
    {
      throw new NotSupportedException(WebPageResources.UrlData_ReadOnly);
    }

    public void Clear() => throw new NotSupportedException(WebPageResources.UrlData_ReadOnly);

    public bool Contains(string item) => this._urlData.Contains(item);

    public void CopyTo(string[] array, int arrayIndex) => this._urlData.CopyTo(array, arrayIndex);

    public bool Remove(string item)
    {
      throw new NotSupportedException(WebPageResources.UrlData_ReadOnly);
    }

    public IEnumerator<string> GetEnumerator()
    {
      return (IEnumerator<string>) this._urlData.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._urlData.GetEnumerator();
  }
}
