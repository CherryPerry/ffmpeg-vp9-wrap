using System.Collections.Generic;

namespace VpxEncode.Output
{
  public abstract class OutputProcessor
  {
    protected List<ProcessingUnit> units = new List<ProcessingUnit>();

    public ProcessingUnit CreateOne()
    {
      ProcessingUnit pu = new ProcessingUnit();
      pu.OnDataReceived += pu_OnDataReceived;
      units.Add(pu);
      return pu;
    }

    protected abstract void pu_OnDataReceived(object sender, ProcessingUnitDataReceivedEventArgs e);

    public void Destroy(ProcessingUnit pu)
    {
      pu.OnDataReceived -= pu_OnDataReceived;
      units.Remove(pu);
    }
  }
}
