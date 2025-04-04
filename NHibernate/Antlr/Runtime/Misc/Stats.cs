// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Misc.Stats
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.IO;

#nullable disable
namespace Antlr.Runtime.Misc
{
  internal class Stats
  {
    public static double Stddev(int[] X)
    {
      int length = X.Length;
      if (length <= 1)
        return 0.0;
      double num1 = Stats.Avg(X);
      double num2 = 0.0;
      for (int index = 0; index < length; ++index)
        num2 += ((double) X[index] - num1) * ((double) X[index] - num1);
      return Math.Sqrt(num2 / (double) (length - 1));
    }

    public static double Avg(int[] X)
    {
      double num = 0.0;
      int length = X.Length;
      if (length == 0)
        return 0.0;
      for (int index = 0; index < length; ++index)
        num += (double) X[index];
      return num >= 0.0 ? num / (double) length : 0.0;
    }

    public static int Min(int[] X)
    {
      int maxValue = int.MaxValue;
      int length = X.Length;
      if (length == 0)
        return 0;
      for (int index = 0; index < length; ++index)
      {
        if (X[index] < maxValue)
          maxValue = X[index];
      }
      return maxValue;
    }

    public static int Max(int[] X)
    {
      int minValue = int.MinValue;
      int length = X.Length;
      if (length == 0)
        return 0;
      for (int index = 0; index < length; ++index)
      {
        if (X[index] > minValue)
          minValue = X[index];
      }
      return minValue;
    }

    public static int Sum(int[] X)
    {
      int num = 0;
      int length = X.Length;
      if (length == 0)
        return 0;
      for (int index = 0; index < length; ++index)
        num += X[index];
      return num;
    }

    public static void WriteReport(string filename, string data)
    {
      string absoluteFileName = Stats.GetAbsoluteFileName(filename);
      FileInfo fileInfo = new FileInfo(absoluteFileName);
      fileInfo.Directory.Create();
      try
      {
        StreamWriter streamWriter = new StreamWriter(fileInfo.FullName, true);
        streamWriter.WriteLine(data);
        streamWriter.Close();
      }
      catch (IOException ex)
      {
        ErrorManager.InternalError((object) ("can't write stats to " + absoluteFileName), (Exception) ex);
      }
    }

    public static string GetAbsoluteFileName(string filename)
    {
      return Path.Combine(Path.Combine(Environment.CurrentDirectory, Constants.ANTLRWORKS_DIR), filename);
    }
  }
}
