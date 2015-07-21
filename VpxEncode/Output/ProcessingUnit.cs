using System;
using System.Diagnostics;

namespace VpxEncode.Output
{
  public sealed class ProcessingUnit
  {
    public int Token { get; private set; }

    public event EventHandler<ProcessingUnitDataReceivedEventArgs> OnDataReceived;

    public ProcessingUnit()
    {
      Token = new Random().Next();
    }

    public void Write(string line) => RaiseEvent(line);

    public void DataReceived(object sender, DataReceivedEventArgs data) => RaiseEvent(data.Data);

    private void RaiseEvent(string text) => OnDataReceived?.Invoke(this, new ProcessingUnitDataReceivedEventArgs(Token, text));
  }
}
