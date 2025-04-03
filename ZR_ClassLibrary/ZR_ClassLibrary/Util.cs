// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.Util
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using Microsoft.VisualBasic;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace ZR_ClassLibrary
{
  public static class Util
  {
    private static Logger logger = LogManager.GetLogger(nameof (Util));
    private static Random random = new Random();
    private static DateTime lastCallOfApplicationDoEvents = SystemValues.DateTimeNow;
    private const int UPDATE_GUI_INTERVAL = 300;
    private const int MUST_BE_LESS_THAN = 100000000;
    private const string subkey = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\";

    public static Version GMM_Version
    {
      get
      {
        try
        {
          return typeof (Util).Assembly.GetName().Version;
        }
        catch (Exception ex)
        {
          Util.logger.Error(ex.Message);
          return new Version(0, 0);
        }
      }
    }

    public static byte[] DecryptCBC_AES_128(byte[] key, byte[] IV, byte[] encrypted)
    {
      if (encrypted == null || encrypted.Length == 0)
        throw new ArgumentNullException(nameof (encrypted));
      if (key == null || key.Length == 0)
        throw new ArgumentNullException(nameof (key));
      if (IV == null || IV.Length == 0)
        throw new ArgumentNullException(nameof (IV));
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.Mode = CipherMode.CBC;
      rijndaelManaged.IV = IV;
      rijndaelManaged.Key = key;
      rijndaelManaged.Padding = PaddingMode.Zeros;
      using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor())
      {
        using (MemoryStream memoryStream1 = new MemoryStream(encrypted))
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream1, decryptor, CryptoStreamMode.Read))
          {
            using (MemoryStream memoryStream2 = new MemoryStream())
            {
              byte[] buffer = new byte[encrypted.Length];
              while (true)
              {
                int count = cryptoStream.Read(buffer, 0, buffer.Length);
                if (count > 0)
                  memoryStream2.Write(buffer, 0, count);
                else
                  break;
              }
              return memoryStream2.ToArray();
            }
          }
        }
      }
    }

    public static ushort CalculatesCRC16_CC430(byte[] buffer)
    {
      return Util.CalculatesCRC16_CC430(buffer, 0, buffer.Length);
    }

    public static ushort CalculatesCRC16_CC430(byte[] buffer, int offset, int size)
    {
      ushort crc = ushort.MaxValue;
      for (int index = offset; index < size + offset; ++index)
        crc = Util.crc_calc_ccitt_(crc, buffer[index]);
      return crc;
    }

    public static ushort CalculatesCRC16_CC430(List<byte> buffer)
    {
      ushort crc = ushort.MaxValue;
      foreach (byte data in buffer)
        crc = Util.crc_calc_ccitt_(crc, data);
      return crc;
    }

    private static ushort crc_calc_ccitt_(ushort crc, byte data)
    {
      byte num = 0;
      for (byte index = 0; index < (byte) 8; ++index)
      {
        if (((uint) data & 1U << (int) index) > 0U)
          num |= (byte) (128U >> (int) index);
      }
      crc ^= (ushort) ((uint) num << 8);
      for (byte index = 0; index < (byte) 8; ++index)
      {
        if (((uint) crc & 32768U) > 0U)
          crc = (ushort) ((int) crc << 1 ^ 4129);
        else
          crc <<= 1;
      }
      return crc;
    }

    public static ushort CalculatesCRC16(byte[] data) => Util.CalculatesCRC16(data, 0, data.Length);

    public static ushort CalculatesCRC16(byte[] data, int offset, int length)
    {
      ushort num = ushort.MaxValue;
      for (int index1 = offset; index1 < length; ++index1)
      {
        num ^= (ushort) ((uint) data[index1] << 8);
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if ((ushort) ((uint) num & 32768U) > (ushort) 0)
            num = (ushort) ((uint) (ushort) ((uint) num << 1) ^ 4129U);
          else
            num <<= 1;
        }
      }
      return num;
    }

    public static string SerializeObjectToXML(object item)
    {
      try
      {
        using (XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream) new MemoryStream(), Encoding.UTF8)
        {
          Formatting = Formatting.Indented
        })
        {
          new XmlSerializer(item.GetType()).Serialize((XmlWriter) xmlTextWriter, item);
          MemoryStream baseStream = (MemoryStream) xmlTextWriter.BaseStream;
          byte[] array = baseStream.ToArray();
          string xml = new UTF8Encoding(true).GetString(array, 0, array.Length);
          baseStream.Close();
          return xml;
        }
      }
      catch (Exception ex)
      {
        Debug.Write(ex.ToString());
        return (string) null;
      }
    }

    public static TOutput[] ConvertAll<TInput, TOutput>(
      TInput[] array,
      Converter<TInput, TOutput> converter)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (converter == null)
        throw new ArgumentNullException(nameof (converter));
      TOutput[] outputArray = new TOutput[array.Length];
      for (int index = 0; index < array.Length; ++index)
        outputArray[index] = converter(array[index]);
      return outputArray;
    }

    public static byte[] Zip(byte[] buffer)
    {
      using (MemoryStream src = new MemoryStream(buffer))
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (GZipStream dest = new GZipStream((Stream) memoryStream, CompressionMode.Compress))
            Util.CopyTo((Stream) src, (Stream) dest);
          return memoryStream.ToArray();
        }
      }
    }

    public static byte[] Unzip(byte[] bytes)
    {
      using (MemoryStream memoryStream = new MemoryStream(bytes))
      {
        using (MemoryStream dest = new MemoryStream())
        {
          using (GZipStream src = new GZipStream((Stream) memoryStream, CompressionMode.Decompress))
            Util.CopyTo((Stream) src, (Stream) dest);
          return dest.ToArray();
        }
      }
    }

    public static void CopyTo(Stream src, Stream dest)
    {
      byte[] buffer = new byte[4096];
      int count;
      while ((count = src.Read(buffer, 0, buffer.Length)) != 0)
        dest.Write(buffer, 0, count);
    }

    public static T[] GetPreFilledArray<T>(T fillItem, int count)
    {
      T[] preFilledArray = new T[count];
      for (int index = 0; index < count; ++index)
        preFilledArray[index] = fillItem;
      return preFilledArray;
    }

    public static bool Wait(long ms, string reason, ICancelable cancelableModul)
    {
      return Util.Wait(ms, reason, cancelableModul, (Logger) null);
    }

    public static bool Wait(long ms, string reason, ICancelable cancelableModul, Logger logger)
    {
      if (ms <= 0L)
        return true;
      DateTime dateTime = SystemValues.DateTimeNow.AddMilliseconds((double) ms);
      if (logger != null && logger.IsTraceEnabled)
        logger.Trace("Wait " + ms.ToString() + "ms " + reason);
      while (!cancelableModul.BreakRequest)
      {
        double totalMilliseconds = dateTime.Subtract(SystemValues.DateTimeNow).TotalMilliseconds;
        int millisecondsTimeout = 0;
        if (totalMilliseconds >= 0.0 && totalMilliseconds <= (double) int.MaxValue)
          millisecondsTimeout = Convert.ToInt32(totalMilliseconds);
        if (millisecondsTimeout > 0)
        {
          Thread.Sleep(millisecondsTimeout);
          if ((SystemValues.DateTimeNow - Util.lastCallOfApplicationDoEvents).TotalMilliseconds >= 300.0)
          {
            Application.DoEvents();
            Util.lastCallOfApplicationDoEvents = SystemValues.DateTimeNow;
          }
        }
        else
          break;
      }
      if (logger != null && logger.IsTraceEnabled && cancelableModul.BreakRequest)
        logger.Trace("User has canceled all processes of " + cancelableModul.ToString());
      return !cancelableModul.BreakRequest;
    }

    public static int GetDivisorWithoutResidue(int number, int divisor)
    {
      for (int divisorWithoutResidue = divisor; divisorWithoutResidue > 1; --divisorWithoutResidue)
      {
        if (number % divisorWithoutResidue == 0)
          return divisorWithoutResidue;
      }
      return 1;
    }

    public static string GetPublicIP()
    {
      WebResponse response = WebRequest.Create("http://checkip.dyndns.org/").GetResponse();
      StreamReader streamReader = new StreamReader(response.GetResponseStream());
      string end = streamReader.ReadToEnd();
      streamReader.Close();
      response.Close();
      int startIndex = end.IndexOf("Address: ") + 9;
      int num = end.LastIndexOf("</body>");
      return end.Substring(startIndex, num - startIndex);
    }

    public static Dictionary<string, bool> GetPortNames()
    {
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      Util.SearchRegistryForPortNames("SYSTEM\\CurrentControlSet\\Enum", dictionary, new List<string>((IEnumerable<string>) SerialPort.GetPortNames()));
      return Util.SortDictionaryByKey(dictionary);
    }

    private static void SearchRegistryForPortNames(
      string startKey,
      Dictionary<string, bool> portNames,
      List<string> portNamesToMatch)
    {
      if (portNames.Count >= portNamesToMatch.Count)
        return;
      RegistryKey localMachine = Registry.LocalMachine;
      RegistryKey registryKey;
      try
      {
        registryKey = localMachine.OpenSubKey(startKey);
        if (registryKey == null)
          return;
      }
      catch (SecurityException ex)
      {
        return;
      }
      List<string> stringList = new List<string>((IEnumerable<string>) registryKey.GetSubKeyNames());
      if (stringList.Contains("Device Parameters") && startKey != "SYSTEM\\CurrentControlSet\\Enum")
      {
        object obj = Registry.GetValue("HKEY_LOCAL_MACHINE\\" + startKey + "\\Device Parameters", "PortName", (object) null);
        if (obj == null)
          return;
        string key = obj.ToString();
        if (!key.StartsWith("COM"))
          return;
        bool flag1 = false;
        foreach (string str in portNamesToMatch)
        {
          if (str.StartsWith(key.ToString()))
          {
            flag1 = true;
            break;
          }
        }
        if (!flag1 || portNames.ContainsKey(key))
          return;
        bool flag2 = registryKey.Name.IndexOf("BTHENUM") > 0;
        if (!portNames.ContainsKey(key))
          portNames.Add(key, flag2);
      }
      else
      {
        foreach (string str in stringList)
          Util.SearchRegistryForPortNames(startKey + "\\" + str, portNames, portNamesToMatch);
      }
    }

    public static Dictionary<string, bool> SortDictionaryByKey(Dictionary<string, bool> values)
    {
      string[] array = new List<string>((IEnumerable<string>) values.Keys).ToArray();
      Array.Sort<string>(array, (IComparer<string>) new Util.AlphanumComparator());
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      foreach (string key in array)
        dictionary.Add(key, values[key]);
      return dictionary;
    }

    public static Dictionary<string, string> SortDictionaryByValue(Dictionary<string, string> values)
    {
      return values.OrderBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (entry => entry.Value)).ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>) (pair => pair.Key), (Func<KeyValuePair<string, string>, string>) (pair => pair.Value));
    }

    public static bool ArraysEqual(short[] a1, short[] a2)
    {
      if (a1 == a2)
        return true;
      if (a1 == null || a2 == null || a1.Length != a2.Length)
        return false;
      for (int index = 0; index < a1.Length; ++index)
      {
        if ((int) a1[index] != (int) a2[index])
          return false;
      }
      return true;
    }

    public static bool ArraysEqual(byte[] a1, byte[] a2)
    {
      if (a1 == a2)
        return true;
      if (a1 == null || a2 == null || a1.Length != a2.Length)
        return false;
      for (int index = 0; index < a1.Length; ++index)
      {
        if ((int) a1[index] != (int) a2[index])
          return false;
      }
      return true;
    }

    public static bool IsValidPath(string path)
    {
      return new Regex("^(([a-zA-Z]\\:)|(\\\\))(\\\\{1}|((\\\\{1})[^\\\\]([^/:*?<>\"|]*))+)$").IsMatch(path);
    }

    public static byte[] ConvertInt16ArrayToByteArray(short[] data)
    {
      if (data == null || data.Length == 0)
        return (byte[]) null;
      MemoryStream memoryStream = new MemoryStream(data.Length * 2);
      for (int index = 0; index < data.Length; ++index)
      {
        byte num1 = Convert.ToByte((int) data[index] & (int) byte.MaxValue);
        byte num2 = Convert.ToByte((int) data[index] >> 8 & (int) byte.MaxValue);
        memoryStream.WriteByte(num1);
        memoryStream.WriteByte(num2);
      }
      return memoryStream.ToArray();
    }

    public static byte[] ConvertLongToByteArray(long obj, int arrayLength)
    {
      byte[] byteArray = new byte[arrayLength];
      for (int index = 0; index < arrayLength; ++index)
        byteArray[index] = (byte) (obj >> index * 8);
      return byteArray;
    }

    public static short[] ConvertByteArrayToInt16Array(byte[] buffer)
    {
      if (buffer == null || buffer.Length < 1)
        return (short[]) null;
      short[] int16Array = new short[buffer.Length / 2];
      int index1 = 0;
      for (int index2 = 0; index2 < buffer.Length; index2 += 2)
      {
        int16Array[index1] = (short) buffer[index2];
        short num = (short) ((int) (short) buffer[index2 + 1] << 8);
        int16Array[index1] |= num;
        ++index1;
      }
      return int16Array;
    }

    public static byte[] ConvertByteArrayToByteArrayEndianess(byte[] buffer, int byteSize)
    {
      if (buffer == null || buffer.Length < 1 || byteSize % 8 != 0 || byteSize < 16 || byteSize > 64)
        return (byte[]) null;
      byte[] byteArrayEndianess = new byte[buffer.Length];
      int index1 = 0;
      int num = byteSize / 8 - 1;
      try
      {
        for (int index2 = 0; index2 < buffer.Length - num; index2 += num + 1)
        {
          byteArrayEndianess[index1] = buffer[index2 + num];
          byteArrayEndianess[index1 + 1] = buffer[index2 + num - 1];
          if (byteSize == 32)
          {
            byteArrayEndianess[index1 + 2] = buffer[index2 + num - 2];
            byteArrayEndianess[index1 + 3] = buffer[index2 + num - 3];
          }
          if (byteSize == 64)
          {
            byteArrayEndianess[index1 + 4] = buffer[index2 + num - 4];
            byteArrayEndianess[index1 + 5] = buffer[index2 + num - 5];
            byteArrayEndianess[index1 + 6] = buffer[index2 + num - 6];
            byteArrayEndianess[index1 + 7] = buffer[index2 + num - 7];
          }
          index1 += num + 1;
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return byteArrayEndianess;
    }

    public static byte[] HexStringToByteArray(string hex)
    {
      hex = hex.Replace(" ", string.Empty);
      hex = hex.Replace("-", string.Empty);
      hex = hex.Replace(".", string.Empty);
      hex = hex.Replace(":", string.Empty);
      int length = hex.Length;
      byte[] byteArray = length % 2 == 0 ? new byte[length / 2] : throw new ArgumentException("Hex-string is invalid!");
      for (int startIndex = 0; startIndex < length; startIndex += 2)
        byteArray[startIndex / 2] = Convert.ToByte(hex.Substring(startIndex, 2), 16);
      return byteArray;
    }

    public static byte[] StringToByteArray(string str) => new ASCIIEncoding().GetBytes(str);

    public static string ByteArrayToString(byte[] arr)
    {
      return new ASCIIEncoding().GetString(arr, 0, arr.Length);
    }

    public static string ByteArrayToHexStringFormated(byte[] arr)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < arr.Length; ++index)
      {
        if (index > 0)
        {
          if ((index & 7) == 0)
            stringBuilder.Append(':');
          else if ((index & 3) == 0)
            stringBuilder.Append('.');
          else
            stringBuilder.Append(' ');
        }
        stringBuilder.Append(arr[index].ToString("x02"));
      }
      return stringBuilder.ToString();
    }

    public static string ByteArrayToHexString(byte[] buffer)
    {
      return buffer == null ? string.Empty : Util.ByteArrayToHexString(buffer, 0, buffer.Length);
    }

    public static string ByteArrayToHexString(byte[] buffer, int startIndex)
    {
      return buffer == null ? string.Empty : Util.ByteArrayToHexString(buffer, startIndex, buffer.Length - startIndex);
    }

    public static string ByteArrayToHexString(byte[] buffer, int startIndex, int length)
    {
      if (buffer == null)
        return string.Empty;
      char[] chArray = new char[length * 2];
      int num1 = 0;
      int index = 0;
      while (num1 < length)
      {
        byte num2 = (byte) ((uint) buffer[startIndex + num1] >> 4);
        chArray[index] = num2 > (byte) 9 ? (char) ((int) num2 + 55) : (char) ((int) num2 + 48);
        byte num3 = (byte) ((uint) buffer[startIndex + num1] & 15U);
        int num4;
        chArray[num4 = index + 1] = num3 > (byte) 9 ? (char) ((int) num3 + 55) : (char) ((int) num3 + 48);
        ++num1;
        index = num4 + 1;
      }
      return new string(chArray, 0, chArray.Length);
    }

    public static bool AreEqual(object expected, object actual)
    {
      if (expected == null && actual == null)
        return true;
      if (expected == null || actual == null || expected.GetType() != actual.GetType())
        return false;
      int num;
      switch (expected)
      {
        case double x1:
          return Util.AreEqual(x1, (double) actual);
        case Decimal x2:
          return Util.AreEqual(x2, (Decimal) actual);
        case DateTime _:
          num = actual is DateTime ? 1 : 0;
          break;
        default:
          num = 0;
          break;
      }
      if (num != 0)
        return ((DateTime) expected - (DateTime) actual).Duration() <= new TimeSpan(9999999L);
      return expected is DateTime? && actual is DateTime? ? (((DateTime?) expected).Value - ((DateTime?) actual).Value).Duration() <= new TimeSpan(9999999L) : expected.Equals(actual);
    }

    public static bool AreEqual(Decimal x, Decimal y, Decimal epsilon) => Math.Abs(x - y) < epsilon;

    public static bool AreEqual(double x, double y) => Math.Abs(x - y) < 1E-07;

    public static bool AreEqual(Decimal x, Decimal y) => Math.Abs(x - y) < 0.0000001M;

    public static bool ByteArrayCompare(List<byte> a1, byte[] a2, int size)
    {
      if (a1.Count < size || a2.Length < size)
        return false;
      for (int index = 0; index < size; ++index)
      {
        if ((int) a1[index] != (int) a2[index])
          return false;
      }
      return true;
    }

    public static bool ByteArrayCompare(byte[] a1, byte[] a2)
    {
      if (a1.Length != a2.Length)
        return false;
      for (int index = 0; index < a1.Length; ++index)
      {
        if ((int) a1[index] != (int) a2[index])
          return false;
      }
      return true;
    }

    public static Dictionary<int, byte[]> Reverse(SortedList values)
    {
      if (values == null)
        return (Dictionary<int, byte[]>) null;
      Dictionary<int, byte[]> dictionary = new Dictionary<int, byte[]>(values.Count);
      for (int index = values.Count - 1; index >= 0; --index)
      {
        int key = (int) values.GetKey(index);
        byte[] byIndex = (byte[]) values.GetByIndex(index);
        dictionary.Add(key, byIndex);
      }
      return dictionary;
    }

    public static string ReverseString(string s)
    {
      char[] charArray = s.ToCharArray();
      Array.Reverse((Array) charArray);
      return new string(charArray);
    }

    public static bool IsValidBCD(string bcdStr)
    {
      foreach (char ch in bcdStr)
      {
        if (ch < '0' || ch > '9')
          return false;
      }
      return true;
    }

    public static long ConvertInt64ToBcdInt64(long value)
    {
      long bcdInt64 = 0;
      for (int index = 0; index < 8; ++index)
      {
        long num = value % 10L;
        bcdInt64 |= num << index * 4;
        value /= 10L;
      }
      return bcdInt64;
    }

    public static uint ConvertUnt32ToBcdUInt32(uint value)
    {
      uint bcdUint32 = 0;
      for (int index = 0; index < 8; ++index)
      {
        uint num = value % 10U;
        bcdUint32 |= num << index * 4;
        value /= 10U;
      }
      return bcdUint32;
    }

    public static long? ConvertBcdInt64ToInt64(byte byte1, byte byte2, byte byte3, byte byte4)
    {
      if (byte1 == byte.MaxValue && byte2 == byte.MaxValue && byte3 == byte.MaxValue && byte4 == byte.MaxValue)
        return new long?();
      return byte1 == (byte) 0 && byte2 == (byte) 0 && byte3 == (byte) 0 && byte4 == (byte) 0 ? new long?(0L) : new long?(Util.ConvertBcdInt64ToInt64((long) byte1 << 24 | (long) byte2 << 16 | (long) byte3 << 8 | (long) byte4));
    }

    public static uint ConvertBcdUInt32ToUInt32(uint bcd)
    {
      uint num = 1;
      uint uint32 = 0;
      for (; bcd > 0U; bcd >>= 4)
      {
        uint32 += (bcd & 15U) * num;
        num *= 10U;
      }
      return uint32;
    }

    public static int ConvertBcdInt32ToInt32(int bcd)
    {
      int num = 1;
      int int32 = 0;
      for (; bcd > 0; bcd >>= 4)
      {
        int32 += (bcd & 15) * num;
        num *= 10;
      }
      return int32;
    }

    public static long ConvertBcdInt64ToInt64(long bcd)
    {
      long num = 1;
      long int64 = 0;
      for (; bcd > 0L; bcd >>= 4)
      {
        int64 += (bcd & 15L) * num;
        num *= 10L;
      }
      return int64;
    }

    public static string[] GetNamesOfEnum(System.Type enumType)
    {
      FieldInfo[] fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
      List<string> stringList = new List<string>();
      if (fields != null)
      {
        foreach (FieldInfo fieldInfo in fields)
          stringList.Add(fieldInfo.Name);
      }
      return stringList.ToArray();
    }

    public static bool IsValidTimePoint(
      DateTime date,
      DateTime start,
      DateTime end,
      bool ignoreTime)
    {
      if (ignoreTime)
      {
        start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0, DateTimeKind.Utc);
        end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59, DateTimeKind.Utc);
      }
      return start <= date && date <= end;
    }

    public static byte[] Combine(byte[] a, byte[] b)
    {
      byte[] dst = new byte[a.Length + b.Length];
      Buffer.BlockCopy((Array) a, 0, (Array) dst, 0, a.Length);
      Buffer.BlockCopy((Array) b, 0, (Array) dst, a.Length, b.Length);
      return dst;
    }

    public static uint GetRandomUInt32(uint min, uint max)
    {
      double num = Util.random.NextDouble();
      return (uint) ((double) (max - min) * num + (double) min);
    }

    public static int GetRandomInteger(int min, int max)
    {
      double num = Util.random.NextDouble();
      return (int) ((double) (max - min) * num) + min;
    }

    public static double GetRandomDouble(double min, double max)
    {
      double num = Util.random.NextDouble();
      return (max - min) * num + min;
    }

    public static Decimal GetRandomDecimal(double min, double max)
    {
      double num = Util.random.NextDouble();
      return Convert.ToDecimal((max - min) * num + min);
    }

    public static uint GetSecureRandomUInt32()
    {
      RandomNumberGenerator randomNumberGenerator = (RandomNumberGenerator) new RNGCryptoServiceProvider();
      byte[] data = new byte[4];
      randomNumberGenerator.GetBytes(data);
      return BitConverter.ToUInt32(data, 0);
    }

    public static ulong GetSecureRandomUInt64()
    {
      RandomNumberGenerator randomNumberGenerator = (RandomNumberGenerator) new RNGCryptoServiceProvider();
      byte[] data = new byte[8];
      randomNumberGenerator.GetBytes(data);
      return BitConverter.ToUInt64(data, 0);
    }

    public static bool IsNumeric(object expression)
    {
      if (expression == null)
        return false;
      if (string.IsNullOrEmpty(expression.ToString()))
        return false;
      try
      {
        double.Parse(Convert.ToString(expression), NumberStyles.Any, (IFormatProvider) NumberFormatInfo.InvariantInfo);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool IsInteger(string num) => new Regex("^\\d+$").Match(num).Success;

    public static bool IsIP(string IP)
    {
      return Regex.IsMatch(IP, "\\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$\\b");
    }

    public static bool TryParseToUInt32(string strValue, out uint value)
    {
      return uint.TryParse(strValue, out value);
    }

    public static bool TryParseToDateTime(string strValue, out DateTime value)
    {
      value = DateTime.MinValue;
      try
      {
        value = DateTime.Parse(strValue);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool TryParseToInt64(string strValue, out long value)
    {
      return long.TryParse(strValue, out value);
    }

    public static bool TryParseToInt32(string strValue, out int value)
    {
      return int.TryParse(strValue, out value);
    }

    public static bool TryParseToUInt16(string strValue, out ushort value)
    {
      return ushort.TryParse(strValue, out value);
    }

    public static bool TryParseToByte(string strValue, out byte value)
    {
      return byte.TryParse(strValue, out value);
    }

    public static bool TryParseToInt16(string strValue, out short value)
    {
      return short.TryParse(strValue, out value);
    }

    public static int RssiToRssi_dBm(byte rssi)
    {
      int num = (int) rssi;
      if (rssi >= (byte) 128)
        num -= 256;
      return num / 2 - 74;
    }

    public static int RoundDown(int dividend, int divisor)
    {
      return (int) Math.Floor((double) (dividend / divisor));
    }

    public static int RoundUp(int dividend, int divisor)
    {
      int num = dividend / divisor;
      return dividend % divisor == 0 || divisor > 0 != dividend > 0 ? num : num + 1;
    }

    public static int GetStableHash(string s)
    {
      uint num1 = 0;
      foreach (byte num2 in Encoding.Unicode.GetBytes(s))
      {
        uint num3 = num1 + (uint) num2;
        uint num4 = num3 + (num3 << 10);
        num1 = num4 ^ num4 >> 6;
      }
      uint num5 = num1 + (num1 << 3);
      uint num6 = num5 ^ num5 >> 11;
      return (int) ((num6 + (num6 << 15)) % 100000000U);
    }

    public static string ElapsedToString(TimeSpan elapsed)
    {
      if ((long) elapsed.TotalHours > 0L)
        return string.Format("{0:0}h {1:0}m {2:0}s {3:0}ms", (object) elapsed.TotalHours, (object) elapsed.Minutes, (object) elapsed.Seconds, (object) elapsed.Milliseconds);
      if ((long) elapsed.TotalMinutes > 0L)
        return string.Format("{0:0}m {1:0}s {2:0}ms", (object) elapsed.TotalMinutes, (object) elapsed.Seconds, (object) elapsed.Milliseconds);
      if ((long) elapsed.TotalSeconds > 0L)
        return string.Format("{0:0}s {1:0}ms", (object) elapsed.TotalSeconds, (object) elapsed.Milliseconds);
      return (long) elapsed.TotalMilliseconds > 0L ? string.Format("{0:0}ms", (object) elapsed.TotalMilliseconds) : string.Empty;
    }

    public static byte[] ImageToByte(Image img)
    {
      return (byte[]) new ImageConverter().ConvertTo((object) img, typeof (byte[]));
    }

    public static Image ByteToImage(byte[] img)
    {
      return (Image) new ImageConverter().ConvertFrom((object) img);
    }

    public static string[] RemoveEmptyEntries(string[] values)
    {
      if (values == null)
        return (string[]) null;
      List<string> stringList = new List<string>();
      foreach (string str in values)
      {
        if (!string.IsNullOrEmpty(str))
          stringList.Add(str);
      }
      return stringList.ToArray();
    }

    public static bool IsEmptyString(string str) => str == null || str == string.Empty;

    public static bool IsEmptyString(object obj)
    {
      return obj == null || obj == DBNull.Value || obj.ToString() == string.Empty;
    }

    public static string ToString(string str) => str == null ? string.Empty : str;

    public static string ToString(object obj)
    {
      return obj == null || obj == DBNull.Value ? string.Empty : obj.ToString();
    }

    public static object ToDBString(string str)
    {
      return str == null || str == string.Empty ? (object) DBNull.Value : (object) str;
    }

    public static object ToDBString(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return (object) DBNull.Value;
      string str = obj.ToString();
      return str == string.Empty ? (object) DBNull.Value : (object) str;
    }

    public static byte[] ToBinary(object obj)
    {
      return obj == null || obj == DBNull.Value ? new byte[0] : (byte[]) obj;
    }

    public static object ToDBBinary(object obj)
    {
      return obj == null || obj == DBNull.Value ? (object) DBNull.Value : obj;
    }

    public static object ToDBBinary(byte[] aby)
    {
      return aby == null || aby.Length == 0 ? (object) DBNull.Value : (object) aby;
    }

    public static DateTime ToDateTime(DateTime dt) => dt;

    public static DateTime ToDateTime(object obj)
    {
      return obj == null || obj == DBNull.Value || !(obj.GetType() == System.Type.GetType("System.DateTime")) && !Information.IsDate(obj) ? DateTime.MinValue : Convert.ToDateTime(obj);
    }

    public static string ToDateString(object obj)
    {
      return obj == null || obj == DBNull.Value || !(obj.GetType() == System.Type.GetType("System.DateTime")) && !Information.IsDate(obj) ? string.Empty : Convert.ToDateTime(obj).ToShortDateString();
    }

    public static string ToDateString(DateTime dt)
    {
      return dt == DateTime.MinValue ? string.Empty : dt.ToShortDateString();
    }

    public static string ToTimeString(DateTime dt)
    {
      return dt == DateTime.MinValue ? string.Empty : dt.ToShortTimeString();
    }

    public static object ToDBDateTime(DateTime dt)
    {
      return dt == DateTime.MinValue ? (object) DBNull.Value : (object) dt;
    }

    public static object ToDBDateTime(object obj)
    {
      if (obj == null || obj == DBNull.Value || !Information.IsDate(obj))
        return (object) DBNull.Value;
      DateTime dateTime = Convert.ToDateTime(obj);
      return dateTime == DateTime.MinValue ? (object) DBNull.Value : (object) dateTime;
    }

    public static bool IsEmptyGuid(Guid g) => g == Guid.Empty;

    public static bool IsEmptyGuid(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return true;
      string s = obj.ToString();
      return s == string.Empty || XmlConvert.ToGuid(s) == Guid.Empty;
    }

    public static Guid ToGuid(Guid g) => g;

    public static Guid ToGuid(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return Guid.Empty;
      if (obj.GetType() == System.Type.GetType("System.Guid"))
        return (Guid) obj;
      string s = obj.ToString();
      return s == string.Empty ? Guid.Empty : XmlConvert.ToGuid(s);
    }

    public static object ToDBGuid(Guid g) => g == Guid.Empty ? (object) DBNull.Value : (object) g;

    public static object ToDBGuid(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return (object) DBNull.Value;
      if (obj.GetType() == System.Type.GetType("System.Guid"))
        return obj;
      string s = obj.ToString();
      if (s == string.Empty)
        return (object) DBNull.Value;
      Guid guid = XmlConvert.ToGuid(s);
      return guid == Guid.Empty ? (object) DBNull.Value : (object) guid;
    }

    public static int ToInteger(int n) => n;

    public static int ToInteger(object obj) => Util.ToInteger(obj, 0);

    public static int ToInteger(object obj, int defaultValue)
    {
      if (obj == null || obj == DBNull.Value)
        return defaultValue;
      if (obj.GetType() == System.Type.GetType("System.Int32"))
        return Convert.ToInt32(obj);
      if (obj.GetType() == System.Type.GetType("System.Boolean"))
        return Convert.ToBoolean(obj) ? 1 : 0;
      if (obj.GetType() == System.Type.GetType("System.Single"))
        return Convert.ToInt32(Math.Floor((double) (float) obj));
      string s = obj.ToString();
      return s == string.Empty ? defaultValue : int.Parse(s, NumberStyles.Any);
    }

    public static long ToLong(long n) => n;

    public static long ToLong(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return 0;
      System.Type type = obj.GetType();
      if (type == typeof (long) || type == typeof (bool) || type == typeof (double))
        return Convert.ToInt64(obj);
      string s = obj.ToString();
      return s == string.Empty ? 0L : long.Parse(s, NumberStyles.Any);
    }

    public static short ToShort(short n) => n;

    public static short ToShort(int n) => (short) n;

    public static short ToShort(long n) => (short) n;

    public static short ToShort(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return 0;
      if (obj.GetType() == System.Type.GetType("System.Int32") || obj.GetType() == System.Type.GetType("System.Int16"))
        return Convert.ToInt16(obj);
      string s = obj.ToString();
      return s == string.Empty ? (short) 0 : short.Parse(s, NumberStyles.Any);
    }

    public static object ToDBInteger(int n) => (object) n;

    public static object ToDBInteger(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return (object) DBNull.Value;
      if (obj.GetType() == System.Type.GetType("System.Int32"))
        return obj;
      string str = obj.ToString();
      return str == string.Empty || !Information.IsNumeric((object) str) ? (object) DBNull.Value : (object) int.Parse(str, NumberStyles.Any);
    }

    public static float ToFloat(float f) => f;

    public static float ToFloat(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return 0.0f;
      if (obj.GetType() == System.Type.GetType("System.Double"))
        return Convert.ToSingle(obj);
      string str = obj.ToString();
      return str == string.Empty || !Information.IsNumeric((object) str) ? 0.0f : float.Parse(str, NumberStyles.Any);
    }

    public static float ToFloat(string str)
    {
      return str == null || str == string.Empty || !Information.IsNumeric((object) str) ? 0.0f : float.Parse(str, NumberStyles.Any);
    }

    public static object ToDBFloat(float f) => (object) f;

    public static object ToDBFloat(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return (object) DBNull.Value;
      if (obj.GetType() == System.Type.GetType("System.Double"))
        return obj;
      string str = obj.ToString();
      return str == string.Empty || !Information.IsNumeric((object) str) ? (object) DBNull.Value : (object) float.Parse(str, NumberStyles.Any);
    }

    public static double ToDouble(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return 0.0;
      if (obj.GetType() == typeof (double) || obj.GetType() == typeof (Decimal))
        return Convert.ToDouble(obj);
      string str = obj.ToString();
      return str == string.Empty || !Information.IsNumeric((object) str) ? 0.0 : double.Parse(str, NumberStyles.Any);
    }

    public static double ToDouble(string str)
    {
      return str == null || str == string.Empty || !Information.IsNumeric((object) str) ? 0.0 : double.Parse(str, NumberStyles.Any);
    }

    public static Decimal ToDecimal(Decimal d) => d;

    public static Decimal ToDecimal(double d) => Convert.ToDecimal(d);

    public static Decimal ToDecimal(float f) => Convert.ToDecimal(f);

    public static Decimal ToDecimal(long l) => Convert.ToDecimal(l);

    public static Decimal ToDecimal(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return 0M;
      if (obj.GetType() == System.Type.GetType("System.Decimal"))
        return Convert.ToDecimal(obj);
      string s = obj.ToString();
      return s == string.Empty ? 0M : Decimal.Parse(s, NumberStyles.Any);
    }

    public static object ToDBDecimal(Decimal d) => (object) d;

    public static object ToDBDecimal(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return (object) DBNull.Value;
      if (obj.GetType() == System.Type.GetType("System.Decimal"))
        return obj;
      string str = obj.ToString();
      return str == string.Empty || !Information.IsNumeric((object) str) ? (object) DBNull.Value : (object) Decimal.Parse(str, NumberStyles.Any);
    }

    public static bool ToBoolean(bool b) => b;

    public static bool ToBoolean(int n) => n != 0;

    public static bool ToBoolean(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return false;
      if (obj.GetType() == System.Type.GetType("System.Int32"))
        return Convert.ToInt32(obj) != 0;
      if (obj.GetType() == System.Type.GetType("System.SByte"))
        return Convert.ToSByte(obj) != (sbyte) 0;
      if (obj.GetType() == System.Type.GetType("System.Int16"))
        return Convert.ToInt16(obj) != (short) 0;
      if (obj.GetType() == System.Type.GetType("System.Decimal"))
        return !(Convert.ToDecimal(obj) == 0M);
      if (obj.GetType() == System.Type.GetType("System.String"))
      {
        string lower = obj.ToString().ToLower();
        return lower == "true" || lower == "on" || lower == "1";
      }
      return !(obj.GetType() != System.Type.GetType("System.Boolean")) && bool.Parse(obj.ToString());
    }

    public static object ToDBBoolean(bool b) => (object) (b ? 1 : 0);

    public static object ToDBBoolean(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return (object) DBNull.Value;
      return obj.GetType() != System.Type.GetType("System.Boolean") ? (object) false : (object) (Convert.ToBoolean(obj) ? 1 : 0);
    }

    public static ulong TwoUInt32ToUInt64(uint left, uint right)
    {
      return (ulong) left << 32 | (ulong) right;
    }

    public static void SaveToFileBinary(string path, object obj)
    {
      using (Stream serializationStream = (Stream) new FileStream(path, FileMode.Create))
        new BinaryFormatter().Serialize(serializationStream, obj);
    }

    public static object LoadFromFileBinary(string path)
    {
      using (Stream serializationStream = (Stream) new FileStream(path, FileMode.Open))
        return new BinaryFormatter().Deserialize(serializationStream);
    }

    public static bool IsValidTimePoint(int year, int month, int day, int hour, int minute)
    {
      return year >= 0 && year <= 2060 && month >= 1 && month <= 12 && day >= 1 && day <= 31 && hour >= 0 && hour <= 24 && minute >= 0 && minute <= 60;
    }

    public static DateTime? ConvertToDateTime_MBus_CP32_TypeF(byte[] buffer, int offset)
    {
      if (((int) buffer[offset] & 128) == 128)
        return new DateTime?();
      int minute = (int) buffer[offset] & 63;
      if (minute > 59)
        return new DateTime?();
      int hour = (int) buffer[offset + 1] & 31;
      if (hour > 23)
        return new DateTime?();
      int day = (int) buffer[offset + 2] & 31;
      if (day < 1 || day > 31)
        return new DateTime?();
      int month = (int) buffer[offset + 3] & 15;
      if (month < 1 || month > 12)
        return new DateTime?();
      int num = (int) buffer[offset + 2] >> 5 | ((int) buffer[offset + 3] & 240) >> 1;
      if (num > 99)
        return new DateTime?();
      int year = num + 2000;
      try
      {
        return new DateTime?(new DateTime(year, month, day, hour, minute, 0));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public static DateTime? ConvertToDate_MBus_CP16_TypeG(byte[] buffer, int offset)
    {
      int day = (int) buffer[offset] & 31;
      if (day < 1 || day > 31)
        return new DateTime?();
      int month = (int) buffer[offset + 1] & 15;
      if (month < 1 || month > 12)
        return new DateTime?();
      int num = (int) buffer[offset] >> 5 | ((int) buffer[offset + 1] & 240) >> 1;
      if (num > 99)
        return new DateTime?();
      int year = num + 2000;
      try
      {
        return new DateTime?(new DateTime(year, month, day));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public static byte[] ConvertToMBus_CP16_TypeG_FromDate(DateTime myDate)
    {
      byte[] numArray = new byte[2];
      ushort num = (ushort) ((uint) (ushort) ((uint) (ushort) (0U | (uint) (ushort) myDate.Day) | (uint) (ushort) ((uint) (ushort) myDate.Month << 8)) | (uint) (ushort) ((uint) (ushort) (myDate.Year - 2000) << 5));
      numArray = BitConverter.GetBytes(num);
      try
      {
        return BitConverter.GetBytes(num);
      }
      catch
      {
        return (byte[]) null;
      }
    }

    public static bool TryParse(string s, out int result) => int.TryParse(s, out result);

    public static bool TryParse(
      string s,
      NumberStyles numberStyles,
      IFormatProvider provider,
      out int result)
    {
      return int.TryParse(s, numberStyles, provider, out result);
    }

    public static bool TryParse(string s, out byte result) => byte.TryParse(s, out result);

    public static bool TryParse(string s, out short result) => short.TryParse(s, out result);

    public static bool TryParse(string s, out long result) => long.TryParse(s, out result);

    public static bool TryParse(string s, out Decimal result) => Decimal.TryParse(s, out result);

    public static bool TryParse(string s, out float result) => float.TryParse(s, out result);

    public static bool TryParse(string s, out double result) => double.TryParse(s, out result);

    public static bool TryParse(string s, out sbyte result) => sbyte.TryParse(s, out result);

    public static bool TryParse(string s, out uint result) => uint.TryParse(s, out result);

    public static bool TryParse(
      string s,
      NumberStyles numberStyles,
      IFormatProvider provider,
      out uint result)
    {
      return uint.TryParse(s, numberStyles, provider, out result);
    }

    public static bool TryParse(string s, out ulong result) => ulong.TryParse(s, out result);

    public static bool TryParse(string s, out ushort result) => ushort.TryParse(s, out result);

    public static bool TryParse(string s, out DateTime result) => DateTime.TryParse(s, out result);

    public static bool TryParse(string s, out bool result) => bool.TryParse(s, out result);

    public static bool IsNet45OrNewer()
    {
      return System.Type.GetType("System.Reflection.ReflectionContext", false) != (System.Type) null;
    }

    public static bool checkDotNetVersionOrLater(string version)
    {
      int releaseKeyFromRegistry = Util.getDotNetReleaseKeyFromRegistry();
      return Util.getDotNetReleaseKeyFromString(version) <= releaseKeyFromRegistry;
    }

    public static string getDotNetVersion()
    {
      string dotNetVersion = string.Empty;
      RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\");
      if (registryKey != null && registryKey.GetValue("Release") != null)
        dotNetVersion = Util.CheckFor45PlusVersion((int) registryKey.GetValue("Release"));
      return dotNetVersion;
    }

    private static string CheckFor45PlusVersion(int releaseKey)
    {
      if (releaseKey >= 533320)
        return "4.8.1 or later";
      if (releaseKey >= 528040)
        return "4.8";
      if (releaseKey >= 461808)
        return "4.7.2";
      if (releaseKey >= 461308)
        return "4.7.1";
      if (releaseKey >= 460798)
        return "4.7";
      if (releaseKey >= 394802)
        return "4.6.2";
      if (releaseKey >= 394254)
        return "4.6.1";
      if (releaseKey >= 393295)
        return "4.6";
      if (releaseKey >= 379893)
        return "4.5.2";
      if (releaseKey >= 378675)
        return "4.5.1";
      return releaseKey >= 378389 ? "4.5" : string.Empty;
    }

    private static int getDotNetReleaseKeyFromRegistry()
    {
      RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\");
      return registryKey != null && registryKey.GetValue("Release") != null ? (int) registryKey.GetValue("Release") : 0;
    }

    private static int getDotNetReleaseKeyFromString(string dotNetVersion)
    {
      if (dotNetVersion.Contains("4.8.1"))
        return 533320;
      if (dotNetVersion.Contains("4.8"))
        return 528040;
      if (dotNetVersion.Contains("4.7.2"))
        return 461808;
      if (dotNetVersion.Contains("4.7.1"))
        return 461308;
      if (dotNetVersion.Contains("4.7"))
        return 460798;
      if (dotNetVersion.Contains("4.6.2"))
        return 394802;
      if (dotNetVersion.Contains("4.6.1"))
        return 394254;
      if (dotNetVersion.Contains("4.6"))
        return 393295;
      if (dotNetVersion.Contains("4.5.2"))
        return 379893;
      if (dotNetVersion.Contains("4.5.1"))
        return 378675;
      return dotNetVersion.Contains("4.5") ? 378389 : 0;
    }

    public static string ArrayToString(List<long> list, string separator)
    {
      if (list == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (long num in list)
        stringBuilder.Append(num.ToString()).Append(separator);
      return stringBuilder.ToString();
    }

    public static ulong SwapBytes(ulong x)
    {
      x = x >> 32 | x << 32;
      x = (x & 18446462603027742720UL) >> 16 | (ulong) (((long) x & 281470681808895L) << 16);
      return (x & 18374966859414961920UL) >> 8 | (ulong) (((long) x & 71777214294589695L) << 8);
    }

    public static uint SwapBytes(uint x)
    {
      x = x >> 16 | x << 16;
      return (x & 4278255360U) >> 8 | (uint) (((int) x & 16711935) << 8);
    }

    public static ushort SwapBytes(ushort x)
    {
      return (ushort) (((int) x & 65280) >> 8 | ((int) x & (int) byte.MaxValue) << 8);
    }

    public static DateTime? ConvertToDateTime_SystemTime48(byte[] buffer, int startIndex = 0)
    {
      int num1 = startIndex;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = index1 + 1;
      byte num3 = numArray1[index1];
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num4 = index2 + 1;
      byte month = numArray2[index2];
      byte[] numArray3 = buffer;
      int index3 = num4;
      int num5 = index3 + 1;
      byte day = numArray3[index3];
      byte[] numArray4 = buffer;
      int index4 = num5;
      int num6 = index4 + 1;
      byte hour = numArray4[index4];
      byte[] numArray5 = buffer;
      int index5 = num6;
      int num7 = index5 + 1;
      byte minute = numArray5[index5];
      byte[] numArray6 = buffer;
      int index6 = num7;
      int num8 = index6 + 1;
      byte second = numArray6[index6];
      if (month == (byte) 0 || day == (byte) 0)
        return new DateTime?();
      try
      {
        return new DateTime?(new DateTime(2000 + (int) num3, (int) month, (int) day, (int) hour, (int) minute, (int) second));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public class AlphanumComparator : IComparer<string>
    {
      public int Compare(string s1, string s2)
      {
        if (s1 == null || s2 == null)
          return 0;
        int length1 = s1.Length;
        int length2 = s2.Length;
        int index1 = 0;
        int index2 = 0;
        while (index1 < length1 && index2 < length2)
        {
          char c1 = s1[index1];
          char c2 = s2[index2];
          char[] chArray1 = new char[length1];
          int num1 = 0;
          char[] chArray2 = new char[length2];
          int num2 = 0;
          do
          {
            chArray1[num1++] = c1;
            ++index1;
            if (index1 < length1)
              c1 = s1[index1];
            else
              break;
          }
          while (char.IsDigit(c1) == char.IsDigit(chArray1[0]));
          do
          {
            chArray2[num2++] = c2;
            ++index2;
            if (index2 < length2)
              c2 = s2[index2];
            else
              break;
          }
          while (char.IsDigit(c2) == char.IsDigit(chArray2[0]));
          string s = new string(chArray1);
          string str = new string(chArray2);
          int num3 = !char.IsDigit(chArray1[0]) || !char.IsDigit(chArray2[0]) ? s.CompareTo(str) : int.Parse(s).CompareTo(int.Parse(str));
          if (num3 != 0)
            return num3;
        }
        return length1 - length2;
      }
    }
  }
}
