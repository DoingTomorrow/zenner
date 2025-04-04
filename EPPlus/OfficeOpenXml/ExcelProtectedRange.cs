// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelProtectedRange
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Utils;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelProtectedRange : XmlHelper
  {
    private ExcelAddress _address;
    private eProtectedRangeAlgorithm _algorithm = eProtectedRangeAlgorithm.SHA512;

    public string Name
    {
      get => this.GetXmlNodeString("@name");
      set => this.SetXmlNodeString("@name", value);
    }

    public ExcelAddress Address
    {
      get
      {
        if (this._address == null)
          this._address = new ExcelAddress(this.GetXmlNodeString("@sqref"));
        return this._address;
      }
      set
      {
        this.SetXmlNodeString("@sqref", SqRefUtility.ToSqRefAddress(value.Address));
        this._address = value;
      }
    }

    internal ExcelProtectedRange(
      string name,
      ExcelAddress address,
      XmlNamespaceManager ns,
      XmlNode topNode)
      : base(ns, topNode)
    {
      this.Name = name;
      this.Address = address;
    }

    public void SetPassword(string password)
    {
      byte[] bytes = Encoding.Unicode.GetBytes(password);
      RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
      byte[] numArray1 = new byte[16];
      randomNumberGenerator.GetBytes(numArray1);
      this.Algorithm = eProtectedRangeAlgorithm.SHA512;
      this.SpinCount = this.SpinCount < 100000 ? 100000 : this.SpinCount;
      SHA512CryptoServiceProvider cryptoServiceProvider = new SHA512CryptoServiceProvider();
      byte[] numArray2 = new byte[bytes.Length + numArray1.Length];
      Array.Copy((Array) numArray1, (Array) numArray2, numArray1.Length);
      Array.Copy((Array) bytes, 0, (Array) numArray2, 16, bytes.Length);
      byte[] hash = cryptoServiceProvider.ComputeHash(numArray2);
      for (int index = 0; index < this.SpinCount; ++index)
      {
        byte[] numArray3 = new byte[hash.Length + 4];
        Array.Copy((Array) hash, (Array) numArray3, hash.Length);
        Array.Copy((Array) BitConverter.GetBytes(index), 0, (Array) numArray3, hash.Length, 4);
        hash = cryptoServiceProvider.ComputeHash(numArray3);
      }
      this.Salt = Convert.ToBase64String(numArray1);
      this.Hash = Convert.ToBase64String(hash);
    }

    public string SecurityDescriptor
    {
      get => this.GetXmlNodeString("@securityDescriptor");
      set => this.SetXmlNodeString("@securityDescriptor", value);
    }

    internal int SpinCount
    {
      get => this.GetXmlNodeInt("@spinCount");
      set
      {
        this.SetXmlNodeString("@spinCount", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    internal string Salt
    {
      get => this.GetXmlNodeString("@saltValue");
      set => this.SetXmlNodeString("@saltValue", value);
    }

    internal string Hash
    {
      get => this.GetXmlNodeString("@hashValue");
      set => this.SetXmlNodeString("@hashValue", value);
    }

    internal eProtectedRangeAlgorithm Algorithm
    {
      get
      {
        return (eProtectedRangeAlgorithm) Enum.Parse(typeof (eProtectedRangeAlgorithm), this.GetXmlNodeString("@algorithmName").Replace("-", ""));
      }
      set
      {
        string str = value.ToString();
        if (str.StartsWith("SHA"))
          str = str.Insert(3, "-");
        else if (str.StartsWith("RIPEMD"))
          str = str.Insert(6, "-");
        this.SetXmlNodeString("@algorithmName", str);
      }
    }
  }
}
