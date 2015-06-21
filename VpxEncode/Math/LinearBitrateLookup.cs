using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VpxEncode.Math
{
  /// <summary>
  /// IBitrateLookup with line method
  /// </summary>
  sealed class LinearBitrateLookup : IBitrateLookup
  {
    private int TargetLimit;
    private HashSet<Point> Targets = new HashSet<Point>();

    private bool methodInvalid = false;
    private HashSet<int> History = new HashSet<int>();

    public LinearBitrateLookup(int targetLimit)
    {
      TargetLimit = targetLimit;
    }

    public int GetTarget()
    {
      if (methodInvalid)
        return -1;
      return LinearGetTarget(); ;
    }

    private int LinearGetTarget()
    {
      if (Targets.Count == 0)
        return TargetLimit;
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
      int newValue = Linear2Point(left, right);
      return newValue;
    }

    private int Linear2Point(Point t1, Point t2)
    {
      double a = (t1.Current - t2.Current);
      long b = (t2.Target - t1.Target);
      long c = (t1.Target * t2.Current - t2.Target * t1.Current);
      // ax + by + c = 0
      return (int)(-(b / a) * TargetLimit - c / a);
    }

    public void AddPoint(int target, int current)
    {
#if DEBUG
      Debug.WriteLine("{0}\t{1}", target, current);
#endif
      methodInvalid = !(History.Add(current) && Targets.Add(new Point { Current = current, Target = target }));
    }
  }
}
