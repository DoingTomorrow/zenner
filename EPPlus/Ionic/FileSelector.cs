﻿// Decompiled with JetBrains decompiler
// Type: Ionic.FileSelector
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Ionic
{
  internal class FileSelector
  {
    internal SelectionCriterion _Criterion;

    public FileSelector(string selectionCriteria)
      : this(selectionCriteria, true)
    {
    }

    public FileSelector(string selectionCriteria, bool traverseDirectoryReparsePoints)
    {
      if (!string.IsNullOrEmpty(selectionCriteria))
        this._Criterion = FileSelector._ParseCriterion(selectionCriteria);
      this.TraverseReparsePoints = traverseDirectoryReparsePoints;
    }

    public string SelectionCriteria
    {
      get => this._Criterion == null ? (string) null : this._Criterion.ToString();
      set
      {
        if (value == null)
          this._Criterion = (SelectionCriterion) null;
        else if (value.Trim() == "")
          this._Criterion = (SelectionCriterion) null;
        else
          this._Criterion = FileSelector._ParseCriterion(value);
      }
    }

    public bool TraverseReparsePoints { get; set; }

    private static string NormalizeCriteriaExpression(string source)
    {
      string[][] strArray = new string[11][]
      {
        new string[2]{ "([^']*)\\(\\(([^']+)", "$1( ($2" },
        new string[2]{ "(.)\\)\\)", "$1) )" },
        new string[2]{ "\\((\\S)", "( $1" },
        new string[2]{ "(\\S)\\)", "$1 )" },
        new string[2]{ "^\\)", " )" },
        new string[2]{ "(\\S)\\(", "$1 (" },
        new string[2]{ "\\)(\\S)", ") $1" },
        new string[2]{ "(=)('[^']*')", "$1 $2" },
        new string[2]{ "([^ !><])(>|<|!=|=)", "$1 $2" },
        new string[2]{ "(>|<|!=|=)([^ =])", "$1 $2" },
        new string[2]{ "/", "\\" }
      };
      string input = source;
      for (int index = 0; index < strArray.Length; ++index)
      {
        string pattern = FileSelector.RegexAssertions.PrecededByEvenNumberOfSingleQuotes + strArray[index][0] + FileSelector.RegexAssertions.FollowedByEvenNumberOfSingleQuotesAndLineEnd;
        input = Regex.Replace(input, pattern, strArray[index][1]);
      }
      string pattern1 = "/" + FileSelector.RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd;
      return Regex.Replace(Regex.Replace(input, pattern1, "\\"), " " + FileSelector.RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd, "\u0006");
    }

    private static SelectionCriterion _ParseCriterion(string s)
    {
      if (s == null)
        return (SelectionCriterion) null;
      s = FileSelector.NormalizeCriteriaExpression(s);
      if (s.IndexOf(" ") == -1)
        s = "name = " + s;
      string[] strArray = s.Trim().Split(' ', '\t');
      if (strArray.Length < 3)
        throw new ArgumentException(s);
      SelectionCriterion criterion = (SelectionCriterion) null;
      Stack<FileSelector.ParseState> parseStateStack = new Stack<FileSelector.ParseState>();
      Stack<SelectionCriterion> selectionCriterionStack = new Stack<SelectionCriterion>();
      parseStateStack.Push(FileSelector.ParseState.Start);
      for (int startIndex = 0; startIndex < strArray.Length; ++startIndex)
      {
        string lower = strArray[startIndex].ToLower();
        switch (lower)
        {
          case "and":
          case "xor":
          case "or":
            FileSelector.ParseState parseState1 = parseStateStack.Peek();
            if (parseState1 != FileSelector.ParseState.CriterionDone)
              throw new ArgumentException(string.Join(" ", strArray, startIndex, strArray.Length - startIndex));
            if (strArray.Length <= startIndex + 3)
              throw new ArgumentException(string.Join(" ", strArray, startIndex, strArray.Length - startIndex));
            LogicalConjunction logicalConjunction = (LogicalConjunction) Enum.Parse(typeof (LogicalConjunction), strArray[startIndex].ToUpper(), true);
            criterion = (SelectionCriterion) new CompoundCriterion()
            {
              Left = criterion,
              Right = (SelectionCriterion) null,
              Conjunction = logicalConjunction
            };
            parseStateStack.Push(parseState1);
            parseStateStack.Push(FileSelector.ParseState.ConjunctionPending);
            selectionCriterionStack.Push(criterion);
            break;
          case "(":
            switch (parseStateStack.Peek())
            {
              case FileSelector.ParseState.Start:
              case FileSelector.ParseState.OpenParen:
              case FileSelector.ParseState.ConjunctionPending:
                if (strArray.Length <= startIndex + 4)
                  throw new ArgumentException(string.Join(" ", strArray, startIndex, strArray.Length - startIndex));
                parseStateStack.Push(FileSelector.ParseState.OpenParen);
                break;
              default:
                throw new ArgumentException(string.Join(" ", strArray, startIndex, strArray.Length - startIndex));
            }
            break;
          case ")":
            parseStateStack.Pop();
            int num1 = parseStateStack.Peek() == FileSelector.ParseState.OpenParen ? (int) parseStateStack.Pop() : throw new ArgumentException(string.Join(" ", strArray, startIndex, strArray.Length - startIndex));
            parseStateStack.Push(FileSelector.ParseState.CriterionDone);
            break;
          case "atime":
          case "ctime":
          case "mtime":
            if (strArray.Length <= startIndex + 2)
              throw new ArgumentException(string.Join(" ", strArray, startIndex, strArray.Length - startIndex));
            DateTime exact;
            try
            {
              exact = DateTime.ParseExact(strArray[startIndex + 2], "yyyy-MM-dd-HH:mm:ss", (IFormatProvider) null);
            }
            catch (FormatException ex1)
            {
              try
              {
                exact = DateTime.ParseExact(strArray[startIndex + 2], "yyyy/MM/dd-HH:mm:ss", (IFormatProvider) null);
              }
              catch (FormatException ex2)
              {
                try
                {
                  exact = DateTime.ParseExact(strArray[startIndex + 2], "yyyy/MM/dd", (IFormatProvider) null);
                }
                catch (FormatException ex3)
                {
                  try
                  {
                    exact = DateTime.ParseExact(strArray[startIndex + 2], "MM/dd/yyyy", (IFormatProvider) null);
                  }
                  catch (FormatException ex4)
                  {
                    exact = DateTime.ParseExact(strArray[startIndex + 2], "yyyy-MM-dd", (IFormatProvider) null);
                  }
                }
              }
            }
            DateTime universalTime = DateTime.SpecifyKind(exact, DateTimeKind.Local).ToUniversalTime();
            criterion = (SelectionCriterion) new TimeCriterion()
            {
              Which = (WhichTime) Enum.Parse(typeof (WhichTime), strArray[startIndex], true),
              Operator = (ComparisonOperator) EnumUtil.Parse(typeof (ComparisonOperator), strArray[startIndex + 1]),
              Time = universalTime
            };
            startIndex += 2;
            parseStateStack.Push(FileSelector.ParseState.CriterionDone);
            break;
          case "length":
          case "size":
            if (strArray.Length <= startIndex + 2)
              throw new ArgumentException(string.Join(" ", strArray, startIndex, strArray.Length - startIndex));
            string str1 = strArray[startIndex + 2];
            long num2 = !str1.ToUpper().EndsWith("K") ? (!str1.ToUpper().EndsWith("KB") ? (!str1.ToUpper().EndsWith("M") ? (!str1.ToUpper().EndsWith("MB") ? (!str1.ToUpper().EndsWith("G") ? (!str1.ToUpper().EndsWith("GB") ? long.Parse(strArray[startIndex + 2]) : long.Parse(str1.Substring(0, str1.Length - 2)) * 1024L * 1024L * 1024L) : long.Parse(str1.Substring(0, str1.Length - 1)) * 1024L * 1024L * 1024L) : long.Parse(str1.Substring(0, str1.Length - 2)) * 1024L * 1024L) : long.Parse(str1.Substring(0, str1.Length - 1)) * 1024L * 1024L) : long.Parse(str1.Substring(0, str1.Length - 2)) * 1024L) : long.Parse(str1.Substring(0, str1.Length - 1)) * 1024L;
            criterion = (SelectionCriterion) new SizeCriterion()
            {
              Size = num2,
              Operator = (ComparisonOperator) EnumUtil.Parse(typeof (ComparisonOperator), strArray[startIndex + 1])
            };
            startIndex += 2;
            parseStateStack.Push(FileSelector.ParseState.CriterionDone);
            break;
          case "filename":
          case "name":
            if (strArray.Length <= startIndex + 2)
              throw new ArgumentException(string.Join(" ", strArray, startIndex, strArray.Length - startIndex));
            ComparisonOperator comparisonOperator1 = (ComparisonOperator) EnumUtil.Parse(typeof (ComparisonOperator), strArray[startIndex + 1]);
            switch (comparisonOperator1)
            {
              case ComparisonOperator.EqualTo:
              case ComparisonOperator.NotEqualTo:
                string str2 = strArray[startIndex + 2];
                if (str2.StartsWith("'") && str2.EndsWith("'"))
                  str2 = str2.Substring(1, str2.Length - 2).Replace("\u0006", " ");
                criterion = (SelectionCriterion) new NameCriterion()
                {
                  MatchingFileSpec = str2,
                  Operator = comparisonOperator1
                };
                startIndex += 2;
                parseStateStack.Push(FileSelector.ParseState.CriterionDone);
                break;
              default:
                throw new ArgumentException(string.Join(" ", strArray, startIndex, strArray.Length - startIndex));
            }
            break;
          case "attrs":
          case "attributes":
          case "type":
            if (strArray.Length <= startIndex + 2)
              throw new ArgumentException(string.Join(" ", strArray, startIndex, strArray.Length - startIndex));
            ComparisonOperator comparisonOperator2 = (ComparisonOperator) EnumUtil.Parse(typeof (ComparisonOperator), strArray[startIndex + 1]);
            switch (comparisonOperator2)
            {
              case ComparisonOperator.EqualTo:
              case ComparisonOperator.NotEqualTo:
                SelectionCriterion selectionCriterion;
                if (!(lower == "type"))
                  selectionCriterion = (SelectionCriterion) new AttributesCriterion()
                  {
                    AttributeString = strArray[startIndex + 2],
                    Operator = comparisonOperator2
                  };
                else
                  selectionCriterion = (SelectionCriterion) new TypeCriterion()
                  {
                    AttributeString = strArray[startIndex + 2],
                    Operator = comparisonOperator2
                  };
                criterion = selectionCriterion;
                startIndex += 2;
                parseStateStack.Push(FileSelector.ParseState.CriterionDone);
                break;
              default:
                throw new ArgumentException(string.Join(" ", strArray, startIndex, strArray.Length - startIndex));
            }
            break;
          case "":
            parseStateStack.Push(FileSelector.ParseState.Whitespace);
            break;
          default:
            throw new ArgumentException("'" + strArray[startIndex] + "'");
        }
        FileSelector.ParseState parseState2 = parseStateStack.Peek();
        if (parseState2 == FileSelector.ParseState.CriterionDone)
        {
          int num3 = (int) parseStateStack.Pop();
          if (parseStateStack.Peek() == FileSelector.ParseState.ConjunctionPending)
          {
            while (parseStateStack.Peek() == FileSelector.ParseState.ConjunctionPending)
            {
              CompoundCriterion compoundCriterion = selectionCriterionStack.Pop() as CompoundCriterion;
              compoundCriterion.Right = criterion;
              criterion = (SelectionCriterion) compoundCriterion;
              int num4 = (int) parseStateStack.Pop();
              parseState2 = parseStateStack.Pop();
              if (parseState2 != FileSelector.ParseState.CriterionDone)
                throw new ArgumentException("??");
            }
          }
          else
            parseStateStack.Push(FileSelector.ParseState.CriterionDone);
        }
        if (parseState2 == FileSelector.ParseState.Whitespace)
        {
          int num5 = (int) parseStateStack.Pop();
        }
      }
      return criterion;
    }

    public override string ToString() => "FileSelector(" + this._Criterion.ToString() + ")";

    private bool Evaluate(string filename) => this._Criterion.Evaluate(filename);

    [Conditional("SelectorTrace")]
    private void SelectorTrace(string format, params object[] args)
    {
      if (this._Criterion == null || !this._Criterion.Verbose)
        return;
      Console.WriteLine(format, args);
    }

    public ICollection<string> SelectFiles(string directory)
    {
      return (ICollection<string>) this.SelectFiles(directory, false);
    }

    public ReadOnlyCollection<string> SelectFiles(string directory, bool recurseDirectories)
    {
      if (this._Criterion == null)
        throw new ArgumentException("SelectionCriteria has not been set");
      List<string> stringList = new List<string>();
      try
      {
        if (Directory.Exists(directory))
        {
          foreach (string file in Directory.GetFiles(directory))
          {
            if (this.Evaluate(file))
              stringList.Add(file);
          }
          if (recurseDirectories)
          {
            foreach (string directory1 in Directory.GetDirectories(directory))
            {
              if (this.TraverseReparsePoints || (File.GetAttributes(directory1) & FileAttributes.ReparsePoint) == (FileAttributes) 0)
              {
                if (this.Evaluate(directory1))
                  stringList.Add(directory1);
                stringList.AddRange((IEnumerable<string>) this.SelectFiles(directory1, recurseDirectories));
              }
            }
          }
        }
      }
      catch (UnauthorizedAccessException ex)
      {
      }
      catch (IOException ex)
      {
      }
      return stringList.AsReadOnly();
    }

    private bool Evaluate(ZipEntry entry) => this._Criterion.Evaluate(entry);

    public ICollection<ZipEntry> SelectEntries(ZipFile zip)
    {
      if (zip == null)
        throw new ArgumentNullException(nameof (zip));
      List<ZipEntry> zipEntryList = new List<ZipEntry>();
      foreach (ZipEntry entry in zip)
      {
        if (this.Evaluate(entry))
          zipEntryList.Add(entry);
      }
      return (ICollection<ZipEntry>) zipEntryList;
    }

    public ICollection<ZipEntry> SelectEntries(ZipFile zip, string directoryPathInArchive)
    {
      if (zip == null)
        throw new ArgumentNullException(nameof (zip));
      List<ZipEntry> zipEntryList = new List<ZipEntry>();
      string str = directoryPathInArchive == null ? (string) null : directoryPathInArchive.Replace("/", "\\");
      if (str != null)
      {
        while (str.EndsWith("\\"))
          str = str.Substring(0, str.Length - 1);
      }
      foreach (ZipEntry entry in zip)
      {
        if ((directoryPathInArchive == null || Path.GetDirectoryName(entry.FileName) == directoryPathInArchive || Path.GetDirectoryName(entry.FileName) == str) && this.Evaluate(entry))
          zipEntryList.Add(entry);
      }
      return (ICollection<ZipEntry>) zipEntryList;
    }

    private enum ParseState
    {
      Start,
      OpenParen,
      CriterionDone,
      ConjunctionPending,
      Whitespace,
    }

    private static class RegexAssertions
    {
      public static readonly string PrecededByOddNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*'[^']*)";
      public static readonly string FollowedByOddNumberOfSingleQuotesAndLineEnd = "(?=[^']*'(?:[^']*'[^']*')*[^']*$)";
      public static readonly string PrecededByEvenNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*[^']*)";
      public static readonly string FollowedByEvenNumberOfSingleQuotesAndLineEnd = "(?=(?:[^']*'[^']*')*[^']*$)";
    }
  }
}
