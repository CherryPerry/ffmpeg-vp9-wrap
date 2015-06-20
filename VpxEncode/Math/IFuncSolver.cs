using System;

namespace VpxEncode.Math
{
  /// <summary>
  /// Interface for solving equations
  /// </summary>
  interface IFuncSolver
  {
    double Solve(Func<double, double> f, double y, double endX = 50000, double startX = 0);
  }
}
