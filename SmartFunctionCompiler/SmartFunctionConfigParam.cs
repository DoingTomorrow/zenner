// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.SmartFunctionConfigParam
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace SmartFunctionCompiler
{
  public class SmartFunctionConfigParam
  {
    public string FunctionName;
    public string ParameterName;
    public string ParameterValue;

    public SmartFunctionConfigParam(
      DeviceCharacteristics deviceCharacteristics,
      List<SmartFunctionIdentAndCode> awailableFunctions,
      string owerwriteDefinition)
    {
      if (owerwriteDefinition == null)
        throw new ArgumentNullException(nameof (owerwriteDefinition));
      if (awailableFunctions == null)
        throw new ArgumentNullException(nameof (awailableFunctions));
      string[] strArray1 = owerwriteDefinition.Split(new char[1]
      {
        ':'
      }, StringSplitOptions.RemoveEmptyEntries);
      this.FunctionName = strArray1.Length == 2 ? strArray1[0] : throw new ArgumentException("Illegal owerwriteDefinition format. (split by :)");
      string[] strArray2 = strArray1[1].Split(new char[1]
      {
        '='
      }, StringSplitOptions.RemoveEmptyEntries);
      this.ParameterName = strArray2.Length == 2 ? strArray2[0] : throw new ArgumentException("Illegal owerwriteDefinition format. (split by =)");
      this.ParameterValue = strArray2[1];
      SmartFunctionIdentAndCode functionIdentAndCode;
      if (this.FunctionName.EndsWith("*"))
      {
        string startOfFunctionName = this.FunctionName.Replace("*", "");
        functionIdentAndCode = awailableFunctions.FirstOrDefault<SmartFunctionIdentAndCode>((Func<SmartFunctionIdentAndCode, bool>) (x => x.Name.StartsWith(startOfFunctionName)));
      }
      else
        functionIdentAndCode = awailableFunctions.FirstOrDefault<SmartFunctionIdentAndCode>((Func<SmartFunctionIdentAndCode, bool>) (x => x.Name == this.FunctionName));
      this.FunctionName = functionIdentAndCode != null ? functionIdentAndCode.Name : throw new Exception("Function not available: " + this.FunctionName);
      if (new SmartFunction(functionIdentAndCode.Code).AllParameters.FirstOrDefault<RuntimeParameter>((Func<RuntimeParameter, bool>) (x => x.Name == this.ParameterName)) == null)
        throw new Exception("Parameter not available: " + this.FunctionName + "." + this.ParameterName);
      if (!this.ParameterValue.Contains("Q"))
        return;
      if (deviceCharacteristics == null)
        throw new ArgumentNullException(nameof (deviceCharacteristics));
      string[] strArray3 = this.ParameterValue.Split(new char[1]
      {
        '*'
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray3.Length != 2)
        throw new ArgumentException("Illegal relative flow value. (split by *)");
      float result;
      if (!float.TryParse(strArray3[1], NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        throw new ArgumentException("Illegal relative flow value. (not a float number)");
      double num;
      switch (strArray3[0])
      {
        case "Q1":
          num = (double) deviceCharacteristics.MinimumFlowrateQ1_qmPerHour;
          break;
        case "Q2":
          num = (double) deviceCharacteristics.TransitionalFlowrateQ2_qmPerHour;
          break;
        case "Q3":
          num = (double) deviceCharacteristics.PermanentFlowrateQ3_qmPerHour;
          break;
        case "Q4":
          num = (double) deviceCharacteristics.OverloadFlowrateQ4_qmPerHour;
          break;
        default:
          throw new ArgumentException("Illegal relative flow value. (not allowed Qx token)");
      }
      if (num == 0.0)
        throw new Exception("Parameter formular: '" + this.ParameterValue + "' has as result 0 because of '" + strArray3[0] + "' is not defined.");
      this.ParameterValue = (num * (double) result).ToString();
    }
  }
}
