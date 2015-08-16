using System.Collections.Generic;
using System.Linq;

namespace VpxEncode
{
  /// <summary>
  /// IBitrateLookup with line method
  /// </summary>
  public sealed class LinearCrfLookup
  {
    public const ushort MIN_CRF = 15, MAX_CRF = 63;

    struct Point
    {
      public ushort Target;
      public double Current;
      public Point(double current, ushort target) { Target = target; Current = current; }
      public override string ToString() => $"{Target} -> {Current}";
      public override int GetHashCode() => Target ^ (int)Current;
    }

    private double TargetLimit;
    private HashSet<Point> Targets = new HashSet<Point>();

    private bool methodInvalid = false;
    private HashSet<ushort> History = new HashSet<ushort>();

    private readonly ushort StartCrf;

    public LinearCrfLookup(double targetLimit, ushort start = 32)
    {
      TargetLimit = targetLimit;
      StartCrf = start;
    }

    public ushort GetTarget()
    {
      if (methodInvalid)
        return 0;
      return LinearGetTarget();
    }

    private ushort LinearGetTarget()
    {
      if (Targets.Count == 0)
        return StartCrf;
      var sorted = Targets.OrderBy(x => x.Target);
      Point left = sorted.Where(x => x.Current < TargetLimit).LastOrDefault();
      Point right = sorted.Where(x => x.Current > TargetLimit).FirstOrDefault();
      if (right.Current == 0)
      {
        right = left;
        var list = sorted.ToList();
        int index = list.IndexOf(right);
        if (index > 0)
          left = list[index - 1];
        else
          left = default(Point);
      }
      ushort newValue = Linear2Point(left, right);

      // check this suggestion
      Point p = Targets.FirstOrDefault(x => x.Target == newValue);
      if (p.Current != 0)
      {
        if (TargetLimit > p.Current)
        {
          // increase while we not tried or until we exceed limit
          do
          {
            newValue++;
            p = Targets.FirstOrDefault(x => x.Target == newValue);
            if (p.Current > TargetLimit)
              return 0;
            if (p.Current == 0)
              break;
          } while (newValue >= MIN_CRF);
          if (newValue < MIN_CRF)
            return 0;
        }
        else
        {
          // if we already tried this crf, make try for crf - 1
          while (Targets.FirstOrDefault(x => x.Target == newValue).Current != 0)
            newValue--;
        }
      }

      return ReverseToOutput(newValue);
    }

    private ushort Linear2Point(Point t1, Point t2)
    {
      double a = t1.Current - t2.Current;
      int b = t2.Target - t1.Target;
      double c = t1.Target * t2.Current - t2.Target * t1.Current;
      // ax + by + c = 0
      return (ushort)(-(b / a) * TargetLimit - c / a);
    }

    public void AddPoint(ushort target, double current)
    {
      target = ReverseInput(target);
      // if next one > limit && current < limit
      if (current < TargetLimit && Targets.FirstOrDefault(x => x.Target == target + 1).Current > TargetLimit)
        // no reason to do next one, exit
        methodInvalid = true;

      // if previous one < limit && current > limit
      double prev = Targets.FirstOrDefault(x => x.Target == target - 1).Current;
      if (current > TargetLimit && prev < TargetLimit && prev != 0)
        // no reason to do previous one, exit
        methodInvalid = true;

      Targets.Add(new Point(current, target));
    }

    private ushort ReverseInput(ushort value)
    {
      if (value > MAX_CRF)
        value = MAX_CRF;
      if (value < MIN_CRF)
        value = MIN_CRF;
      return (ushort)(MAX_CRF - value);
    }

    private ushort ReverseToOutput(ushort value)
    {
      if (value > MAX_CRF - MIN_CRF)
        value = MAX_CRF - MIN_CRF;
      if (value < 0)
        value = 0;
      return (ushort)(MAX_CRF - value);
    }
  }
}
