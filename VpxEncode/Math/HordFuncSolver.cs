using System;

namespace VpxEncode.Math
{
  public class HordFuncSolver : IFuncSolver
  {
    public double Solve(Func<double, double> f, double y, double endX = 100000, double startX = 0)
    {
      double a = startX, b = endX;
      double c = a - f(a) * (b - a) / (f(b) - f(a));
      throw new NotImplementedException();
    }
  }
}
