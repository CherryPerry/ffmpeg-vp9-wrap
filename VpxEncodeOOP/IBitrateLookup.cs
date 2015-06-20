namespace Vp9Encode
{
  /// <summary>
  /// Interface for caclulating target bitrate to fit the limit
  /// </summary>
  interface IBitrateLookup
  {
    void AddPoint(int target, int current);
    int GetTarget();
  }
}
