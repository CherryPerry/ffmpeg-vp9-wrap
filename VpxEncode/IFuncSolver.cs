using System;

namespace Vp9Encode
{
  /// <summary>
  /// Interface for solving equations
  /// </summary>
  interface IFuncSolver
  {
    double Solve(Func<double, double> func, double y, double endX = 100000, double startX = 0);
  }
}
