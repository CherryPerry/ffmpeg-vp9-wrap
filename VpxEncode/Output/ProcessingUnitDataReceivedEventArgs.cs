using System;

namespace VpxEncode.Output
{
  public sealed class ProcessingUnitDataReceivedEventArgs : EventArgs
  {
    public string Data { get; private set; }
    public int Token { get; private set; }

    public ProcessingUnitDataReceivedEventArgs(int token, string data)
      : base()
    {
      Data = data;
      Token = token;
    }
  }
}
