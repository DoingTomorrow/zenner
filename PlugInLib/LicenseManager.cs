// Decompiled with JetBrains decompiler
// Type: PlugInLib.LicenseManager
// Assembly: PlugInLib, Version=2.0.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 0D0A1C6E-D587-46FA-A431-5DFCE0ADBD53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PlugInLib.dll

using NLog;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

#nullable disable
namespace PlugInLib
{
  public static class LicenseManager
  {
    private static Logger logger = LogManager.GetLogger(nameof (LicenseManager));
    private static LicenseInfo currentLicense;

    public static event EventHandler LicenseChanged;

    public static LicenseInfo CurrentLicense
    {
      get => LicenseManager.currentLicense;
      private set
      {
        bool flag = LicenseManager.currentLicense == null && value != null || LicenseManager.currentLicense != null && value == null || value != null && LicenseManager.currentLicense != null && object.ReferenceEquals((object) LicenseManager.currentLicense, (object) value);
        LicenseManager.currentLicense = value;
        if (!flag || LicenseManager.LicenseChanged == null)
          return;
        LicenseManager.LicenseChanged((object) null, (EventArgs) null);
      }
    }

    public static string LicensToXML(object licenseObject, Type licenseType)
    {
      string empty = string.Empty;
      XmlSerializer xmlSerializer = new XmlSerializer(licenseType);
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
      namespaces.Add(string.Empty, string.Empty);
      using (StringWriter stringWriter = new StringWriter())
      {
        xmlSerializer.Serialize((TextWriter) stringWriter, licenseObject, namespaces);
        empty = stringWriter.ToString();
      }
      return empty;
    }

    public static SignedLicense LicensFromXML(string XMLString)
    {
      using (StringReader stringReader = new StringReader(XMLString))
        return new XmlSerializer(typeof (SignedLicense)).Deserialize((TextReader) stringReader) as SignedLicense;
    }

    public static byte[] ApplySignature(LicenseInfo licenseInfo)
    {
      string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PrivateKey.dat");
      string privateKey = File.Exists(path) ? File.ReadAllText(path) : throw new FileNotFoundException("Can not apply the signature! Private key file is missing. Path: " + path, "PrivateKey.dat");
      return LicenseManager.ApplySignature(licenseInfo, privateKey);
    }

    public static byte[] ApplySignature(LicenseInfo licenseInfo, string privateKey)
    {
      if (licenseInfo == null)
        throw new ArgumentNullException(nameof (licenseInfo));
      if (string.IsNullOrEmpty(privateKey))
        throw new ArgumentNullException(nameof (privateKey));
      byte[] bytes = Encoding.Unicode.GetBytes(LicenseManager.LicensToXML((object) licenseInfo, typeof (LicenseInfo)));
      RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
      cryptoServiceProvider.FromXmlString(privateKey);
      return cryptoServiceProvider.SignData(bytes, (object) new SHA1CryptoServiceProvider());
    }

    public static bool VerifySignature(string fullFileText, SignedLicense signedLicense)
    {
      StringReader stringReader = new StringReader(fullFileText);
      StringBuilder stringBuilder = new StringBuilder();
      string str = (string) null;
      while (true)
      {
        string line;
        do
        {
          do
          {
            line = stringReader.ReadLine();
            if (line == null)
              goto label_15;
          }
          while (line.Contains("SignedLicense>"));
          if (line.Contains("xmlns:xsi="))
            goto label_3;
        }
        while (line.Contains("<Signature>"));
        goto label_5;
label_3:
        str = line.Substring(line.IndexOf("xmlns:xsi="));
        continue;
label_5:
        if (line.Contains("<HardwareKey>"))
          LicenseManager.GetTagValueAsByte(line, "HardwareKey");
        else if (line.Contains("<License>"))
          line = str != null ? line.Replace("<License>", "<LicenseInfo " + str) : line.Replace("<License>", "<LicenseInfo>");
        else if (line.Contains("</License>"))
          break;
        if (line.Substring(0, 2) == "  ")
          line = line.Substring(2);
        stringBuilder.AppendLine(line);
      }
      stringBuilder.Append("</LicenseInfo>");
label_15:
      byte[] bytes = Encoding.Unicode.GetBytes(stringBuilder.ToString());
      string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PublicKey.dat");
      if (!File.Exists(path))
        throw new FileNotFoundException("Can not verify the signature! Public key file is missing. Path: " + path, "PublicKey.dat");
      RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
      string xmlString = File.ReadAllText(path);
      cryptoServiceProvider.FromXmlString(xmlString);
      bool flag = cryptoServiceProvider.VerifyData(bytes, (object) new SHA1CryptoServiceProvider(), signedLicense.Signature);
      if (!flag)
        LicenseManager.logger.Fatal("!!! INVALID LICENSE DETECTED !!! EXPECTED LICENSE: " + Environment.NewLine + "license");
      return flag;
    }

    private static byte[] GetTagValueAsByte(string line, string tag)
    {
      string str1 = "<" + tag + ">";
      string str2 = "</" + tag + ">";
      int num1 = line.IndexOf(str1);
      if (num1 < 0)
        return (byte[]) null;
      int num2 = line.IndexOf(str2, num1 + 1);
      if (num2 < 0)
        return (byte[]) null;
      int startIndex = num1 + str1.Length;
      int length1 = num2 - startIndex;
      int length2 = line.Substring(startIndex, length1).Length;
      return Encoding.Unicode.GetBytes(line);
    }

    public static void VerifyLicense(string fileName)
    {
      LicenseManager.logger.Trace("VerifyLicense started");
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException(nameof (fileName), "Can not verify the license file!");
      if (!File.Exists(fileName))
        throw new FileNotFoundException("Can not verify the license file!", fileName);
      string hardwareUniqueKey = HardwareKeyGenerator.GetHardwareUniqueKey();
      string str = File.ReadAllText(fileName);
      SignedLicense signedLicense = LicenseManager.LicensFromXML(str);
      LicenseManager.logger.Trace("signedLicense loaded");
      try
      {
        LicenseManager.VerifyLicense(str, hardwareUniqueKey, signedLicense);
      }
      catch (Exception ex)
      {
        LicenseManager.logger.Trace("License veryfication faild. Try BackwardsCompatible");
        string backwardsCompatible = HardwareKeyGenerator.GetHardwareUniqueKeyBackwardsCompatible();
        LicenseManager.logger.Trace("KeyBackwardsCompatible loaded");
        if (!(signedLicense.License.HardwareKey == backwardsCompatible))
          throw ex;
        LicenseManager.VerifyLicense(str, backwardsCompatible, signedLicense);
      }
      LicenseManager.logger.Trace("License veryfied");
    }

    public static void VerifyLicense(string hardwareKey, string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException(nameof (fileName), "Can not verify the license file!");
      string str = File.Exists(fileName) ? File.ReadAllText(fileName) : throw new FileNotFoundException("Can not verify the license file!", fileName);
      SignedLicense signedLicense = LicenseManager.LicensFromXML(str);
      LicenseManager.VerifyLicense(str, hardwareKey, signedLicense);
    }

    public static void VerifyLicense(
      string fullFileContent,
      string hardwareKey,
      SignedLicense signedLicense)
    {
      if (string.IsNullOrEmpty(hardwareKey))
        throw new ArgumentNullException(nameof (hardwareKey), "Can not verify the license file!");
      if (signedLicense == null)
        throw new ArgumentNullException(nameof (signedLicense), "Can not verify the license file!");
      if (signedLicense.License == null)
        throw new ArgumentNullException("signedLicense.License", "Can not verify the license file!");
      if (signedLicense.License.HardwareKey != hardwareKey)
        throw new ArgumentException("The license is not compatible with this hardware key. Hardware key from the license: " + signedLicense.License.HardwareKey);
      if (!LicenseManager.VerifySignature(fullFileContent, signedLicense))
        throw new ArgumentException("License file signature is not valid!");
      if (!string.IsNullOrEmpty(signedLicense.License.ValidityPeriod))
      {
        int result;
        if (!int.TryParse(signedLicense.License.ValidityPeriod, out result))
          throw new ArgumentException("License file has a invalid validity period! Value: " + signedLicense.License.ValidityPeriod);
        if (signedLicense.License.ValidityStartDate.Value.AddMonths(result) < DateTime.Today)
          throw new Exception("Your license has expired. You can no longer use this product.");
      }
      LicenseManager.CurrentLicense = signedLicense.License;
    }

    public static bool LicenseExpired()
    {
      return LicenseManager.CurrentLicense != null ? LicenseManager.LicenseExpired(LicenseManager.CurrentLicense) : throw new ArgumentNullException("CurrentLicense");
    }

    public static bool LicenseExpired(LicenseInfo licenseInfo)
    {
      if (string.IsNullOrEmpty(licenseInfo.ValidityPeriod))
        return false;
      int months = int.Parse(licenseInfo.ValidityPeriod);
      return licenseInfo.ValidityStartDate.Value.AddMonths(months) < DateTime.Now;
    }

    public static bool IsCustomerLicenseInstalled(string CustomerName)
    {
      return LicenseManager.CurrentLicense.Customer == CustomerName;
    }

    public static void Dispose() => LicenseManager.CurrentLicense = (LicenseInfo) null;
  }
}
