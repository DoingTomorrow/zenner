// Decompiled with JetBrains decompiler
// Type: S4_Handler.UltrasonicState
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System.Text;

#nullable disable
namespace S4_Handler
{
  public class UltrasonicState
  {
    public TransducerPairState TransducerPair1State = (TransducerPairState) null;
    public TransducerPairState TransducerPair2State = (TransducerPairState) null;

    public bool Perfect
    {
      get
      {
        if (this.TransducerPair1State == null)
          return false;
        return this.TransducerPair2State == null ? this.TransducerPair1State.Perfect : this.TransducerPair1State.Perfect && this.TransducerPair2State.Perfect;
      }
    }

    public int ActiveTransducerPairs
    {
      get
      {
        int activeTransducerPairs = 0;
        if (this.TransducerPair1State != null && this.TransducerPair1State.CycleErrorsInPercent < 100.0)
          ++activeTransducerPairs;
        if (this.TransducerPair2State != null && this.TransducerPair2State.CycleErrorsInPercent < 100.0)
          ++activeTransducerPairs;
        return activeTransducerPairs;
      }
    }

    public override string ToString()
    {
      StringBuilder us = new StringBuilder();
      us.AppendLine("***** Ultrasonic state *****");
      us.AppendLine();
      if (this.Perfect)
      {
        us.AppendLine("System perfect");
      }
      else
      {
        us.AppendLine("System not perfect");
        double cycleErrorsInPercent = this.TransducerPair1State.CycleErrorsInPercent;
        if (this.TransducerPair2State != null && this.TransducerPair2State.CycleErrorsInPercent > cycleErrorsInPercent)
          cycleErrorsInPercent = this.TransducerPair2State.CycleErrorsInPercent;
        us.AppendLine("Cycle errors [%] = " + cycleErrorsInPercent.ToString("0.00"));
      }
      us.AppendLine("Active transducer pairs = " + this.ActiveTransducerPairs.ToString());
      us.AppendLine();
      if (this.TransducerPair1State == null)
      {
        us.AppendLine("Illegal transducer states");
      }
      else
      {
        this.PrintTransducerState(this.TransducerPair1State, us, "1");
        if (this.TransducerPair2State != null)
          this.PrintTransducerState(this.TransducerPair2State, us, "2");
      }
      return us.ToString();
    }

    private void PrintTransducerState(
      TransducerPairState singleState,
      StringBuilder us,
      string pair)
    {
      us.AppendLine("--------------------------------");
      us.AppendLine("Transducer pair " + pair);
      us.AppendLine();
      if (singleState.CycleErrorsInPercent == 100.0)
      {
        us.AppendLine("No Signals");
      }
      else
      {
        us.AppendLine("Signals available");
        if (singleState.CycleErrorsInPercent == 0.0)
          us.AppendLine("System stable");
        else
          us.AppendLine("System not stable");
      }
      us.AppendLine();
      us.AppendLine("Successful cycles: " + singleState.SuccessfulCycles.ToString());
      us.AppendLine("Up counts: " + singleState.UpCounts.ToString());
      us.AppendLine("Down counts: " + singleState.DownCounts.ToString());
      us.AppendLine("Diff counts: " + singleState.DiffCounts.ToString());
      us.AppendLine();
    }
  }
}
