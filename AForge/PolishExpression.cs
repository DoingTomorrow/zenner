// Decompiled with JetBrains decompiler
// Type: AForge.PolishExpression
// Assembly: AForge, Version=2.2.5.0, Culture=neutral, PublicKeyToken=c1db6ff4eaa06aeb
// MVID: D4933F01-4742-407D-982E-D47DDB340621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.dll

using System;
using System.Collections;

#nullable disable
namespace AForge
{
  public static class PolishExpression
  {
    public static double Evaluate(string expression, double[] variables)
    {
      string[] strArray = expression.Trim().Split(' ');
      Stack stack = new Stack();
      foreach (string s in strArray)
      {
        if (char.IsDigit(s[0]))
          stack.Push((object) double.Parse(s));
        else if (s[0] == '$')
        {
          stack.Push((object) variables[int.Parse(s.Substring(1))]);
        }
        else
        {
          double num = (double) stack.Pop();
          switch (s)
          {
            case "+":
              stack.Push((object) ((double) stack.Pop() + num));
              continue;
            case "-":
              stack.Push((object) ((double) stack.Pop() - num));
              continue;
            case "*":
              stack.Push((object) ((double) stack.Pop() * num));
              continue;
            case "/":
              stack.Push((object) ((double) stack.Pop() / num));
              continue;
            case "sin":
              stack.Push((object) Math.Sin(num));
              continue;
            case "cos":
              stack.Push((object) Math.Cos(num));
              continue;
            case "ln":
              stack.Push((object) Math.Log(num));
              continue;
            case "exp":
              stack.Push((object) Math.Exp(num));
              continue;
            case "sqrt":
              stack.Push((object) Math.Sqrt(num));
              continue;
            default:
              throw new ArgumentException("Unsupported function: " + s);
          }
        }
      }
      return stack.Count == 1 ? (double) stack.Pop() : throw new ArgumentException("Incorrect expression.");
    }
  }
}
