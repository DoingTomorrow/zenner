// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.WebPairCollection
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

#nullable disable
namespace RestSharp.Authenticators.OAuth
{
  internal class WebPairCollection : 
    IList<WebPair>,
    ICollection<WebPair>,
    IEnumerable<WebPair>,
    IEnumerable
  {
    private IList<WebPair> _parameters;

    public virtual WebPair this[string name]
    {
      get => this.SingleOrDefault<WebPair>((Func<WebPair, bool>) (p => p.Name.Equals(name)));
    }

    public virtual IEnumerable<string> Names
    {
      get => this._parameters.Select<WebPair, string>((Func<WebPair, string>) (p => p.Name));
    }

    public virtual IEnumerable<string> Values
    {
      get => this._parameters.Select<WebPair, string>((Func<WebPair, string>) (p => p.Value));
    }

    public WebPairCollection(IEnumerable<WebPair> parameters)
    {
      this._parameters = (IList<WebPair>) new List<WebPair>(parameters);
    }

    public WebPairCollection(NameValueCollection collection)
      : this()
    {
      this.AddCollection(collection);
    }

    public virtual void AddRange(NameValueCollection collection) => this.AddCollection(collection);

    private void AddCollection(NameValueCollection collection)
    {
      foreach (WebPair webPair in ((IEnumerable<string>) collection.AllKeys).Select<string, WebPair>((Func<string, WebPair>) (key => new WebPair(key, collection[key]))))
        this._parameters.Add(webPair);
    }

    public WebPairCollection(IDictionary<string, string> collection)
      : this()
    {
      this.AddCollection(collection);
    }

    public void AddCollection(IDictionary<string, string> collection)
    {
      foreach (string key in (IEnumerable<string>) collection.Keys)
        this._parameters.Add(new WebPair(key, collection[key]));
    }

    public WebPairCollection() => this._parameters = (IList<WebPair>) new List<WebPair>(0);

    public WebPairCollection(int capacity)
    {
      this._parameters = (IList<WebPair>) new List<WebPair>(capacity);
    }

    private void AddCollection(IEnumerable<WebPair> collection)
    {
      foreach (WebPair webPair in collection)
        this._parameters.Add(new WebPair(webPair.Name, webPair.Value));
    }

    public virtual void AddRange(WebPairCollection collection)
    {
      this.AddCollection((IEnumerable<WebPair>) collection);
    }

    public virtual void AddRange(IEnumerable<WebPair> collection) => this.AddCollection(collection);

    public virtual void Sort(Comparison<WebPair> comparison)
    {
      List<WebPair> webPairList = new List<WebPair>((IEnumerable<WebPair>) this._parameters);
      webPairList.Sort(comparison);
      this._parameters = (IList<WebPair>) webPairList;
    }

    public virtual bool RemoveAll(IEnumerable<WebPair> parameters)
    {
      bool flag = true;
      WebPair[] array = parameters.ToArray<WebPair>();
      for (int index = 0; index < array.Length; ++index)
      {
        WebPair webPair = array[index];
        flag &= this._parameters.Remove(webPair);
      }
      return flag && array.Length > 0;
    }

    public virtual void Add(string name, string value)
    {
      this._parameters.Add(new WebPair(name, value));
    }

    public virtual IEnumerator<WebPair> GetEnumerator() => this._parameters.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public virtual void Add(WebPair parameter) => this._parameters.Add(parameter);

    public virtual void Clear() => this._parameters.Clear();

    public virtual bool Contains(WebPair parameter) => this._parameters.Contains(parameter);

    public virtual void CopyTo(WebPair[] parameters, int arrayIndex)
    {
      this._parameters.CopyTo(parameters, arrayIndex);
    }

    public virtual bool Remove(WebPair parameter) => this._parameters.Remove(parameter);

    public virtual int Count => this._parameters.Count;

    public virtual bool IsReadOnly => this._parameters.IsReadOnly;

    public virtual int IndexOf(WebPair parameter) => this._parameters.IndexOf(parameter);

    public virtual void Insert(int index, WebPair parameter)
    {
      this._parameters.Insert(index, parameter);
    }

    public virtual void RemoveAt(int index) => this._parameters.RemoveAt(index);

    public virtual WebPair this[int index]
    {
      get => this._parameters[index];
      set => this._parameters[index] = value;
    }
  }
}
