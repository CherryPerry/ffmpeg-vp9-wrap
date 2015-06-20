using System;
using System.Collections.Generic;
using System.Linq;

namespace VpxEncode.Math
{
  /// <summary>
  /// IBitrateLookup with Lagrange method
  /// </summary>
  sealed class LagrangeBitrateLookup : IBitrateLookup
  {
    private int TargetLimit;
    private HashSet<Point> Targets;
    private List<Point> AsList;
    private IFuncSolver Solver;

    public LagrangeBitrateLookup(int targetLimit, IFuncSolver solver)
    {
      TargetLimit = targetLimit;
      Targets = new HashSet<Point>();
      Solver = solver;
    }

    public int GetTarget()
    {
      return Calculate();
    }

    private int Calculate()
    {
      AsList = Targets.ToList();
      return (int)Solver.Solve((x) =>
      {
        return GetLagrangePolynomialValue(x);
      }, TargetLimit);
    }

    private double GetLagrangePolynomialValue(double x)
    {
      if (AsList == null || AsList.Count != Targets.Count)
        throw new ArgumentException("Called not from Calculate!");

      double sum = 0;
      for (int i = 0; i < AsList.Count; i++)
        sum += AsList[i].Current * GetLagrangeBasisPolynomial(x, i);
      return sum;
    }

    private double GetLagrangeBasisPolynomial(double x, int n)
    {
      if (AsList == null || AsList.Count != Targets.Count)
        throw new ArgumentException("Called not from Calculate!");

      double p = 1;
      for (int i = 0; i < AsList.Count; i++)
        if (i != n)
          p *= (x - AsList[i].Target) / (AsList[n].Target - AsList[i].Target);
      return p;
    }

    public void AddPoint(int target, int current)
    {
      Targets.Add(new Point { Current = current, Target = target });
    }
  }
}
