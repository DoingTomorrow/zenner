// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.DFA
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime
{
  internal abstract class DFA
  {
    public const bool debug = false;
    protected short[] eot;
    protected short[] eof;
    protected char[] min;
    protected char[] max;
    protected short[] accept;
    protected short[] special;
    protected short[][] transition;
    protected int decisionNumber;
    public DFA.SpecialStateTransitionHandler specialStateTransitionHandler;
    protected BaseRecognizer recognizer;

    public int Predict(IIntStream input)
    {
      int marker = input.Mark();
      int s1 = 0;
      try
      {
        char ch;
        while (true)
        {
          int s2 = (int) this.special[s1];
          if (s2 >= 0)
          {
            s1 = this.specialStateTransitionHandler(this, s2, input);
            if (s1 != -1)
              input.Consume();
            else
              break;
          }
          else if (this.accept[s1] < (short) 1)
          {
            ch = (char) input.LA(1);
            if ((int) ch >= (int) this.min[s1] && (int) ch <= (int) this.max[s1])
            {
              int num = (int) this.transition[s1][(int) ch - (int) this.min[s1]];
              if (num < 0)
              {
                if (this.eot[s1] >= (short) 0)
                {
                  s1 = (int) this.eot[s1];
                  input.Consume();
                }
                else
                  goto label_11;
              }
              else
              {
                s1 = num;
                input.Consume();
              }
            }
            else if (this.eot[s1] >= (short) 0)
            {
              s1 = (int) this.eot[s1];
              input.Consume();
            }
            else
              goto label_15;
          }
          else
            goto label_6;
        }
        this.NoViableAlt(s1, input);
        return 0;
label_6:
        return (int) this.accept[s1];
label_11:
        this.NoViableAlt(s1, input);
        return 0;
label_15:
        if ((int) ch == (int) (ushort) Token.EOF && this.eof[s1] >= (short) 0)
          return (int) this.accept[(int) this.eof[s1]];
        this.NoViableAlt(s1, input);
        return 0;
      }
      finally
      {
        input.Rewind(marker);
      }
    }

    protected void NoViableAlt(int s, IIntStream input)
    {
      if (this.recognizer.state.backtracking > 0)
      {
        this.recognizer.state.failed = true;
      }
      else
      {
        NoViableAltException nvae = new NoViableAltException(this.Description, this.decisionNumber, s, input);
        this.Error(nvae);
        throw nvae;
      }
    }

    public virtual void Error(NoViableAltException nvae)
    {
    }

    public virtual int SpecialStateTransition(int s, IIntStream input) => -1;

    public virtual string Description => "n/a";

    public static short[] UnpackEncodedString(string encodedString)
    {
      int length = 0;
      for (int index = 0; index < encodedString.Length; index += 2)
        length += (int) encodedString[index];
      short[] numArray = new short[length];
      int num = 0;
      for (int index1 = 0; index1 < encodedString.Length; index1 += 2)
      {
        char ch1 = encodedString[index1];
        char ch2 = encodedString[index1 + 1];
        for (int index2 = 1; index2 <= (int) ch1; ++index2)
          numArray[num++] = (short) ch2;
      }
      return numArray;
    }

    public static short[][] UnpackEncodedStringArray(string[] encodedStrings)
    {
      short[][] numArray = new short[encodedStrings.Length][];
      for (int index = 0; index < encodedStrings.Length; ++index)
        numArray[index] = DFA.UnpackEncodedString(encodedStrings[index]);
      return numArray;
    }

    public static char[] UnpackEncodedStringToUnsignedChars(string encodedString)
    {
      int length = 0;
      for (int index = 0; index < encodedString.Length; index += 2)
        length += (int) encodedString[index];
      char[] unsignedChars = new char[length];
      int num = 0;
      for (int index1 = 0; index1 < encodedString.Length; index1 += 2)
      {
        char ch1 = encodedString[index1];
        char ch2 = encodedString[index1 + 1];
        for (int index2 = 1; index2 <= (int) ch1; ++index2)
          unsignedChars[num++] = ch2;
      }
      return unsignedChars;
    }

    public int SpecialTransition(int state, int symbol) => 0;

    public delegate int SpecialStateTransitionHandler(DFA dfa, int s, IIntStream input);
  }
}
