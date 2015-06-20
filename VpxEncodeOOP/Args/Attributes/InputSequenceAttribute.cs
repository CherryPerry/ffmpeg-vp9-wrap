using System;

namespace Vp9Encode.Args.Attributes
{
  [AttributeUsage(AttributeTargets.Class)]
  class InputSequenceAttribute : Attribute
  {
    public string InputSequence { get; set; }

    public InputSequenceAttribute(string inputSequence)
    {
      InputSequence = inputSequence;
    }
  }
}
