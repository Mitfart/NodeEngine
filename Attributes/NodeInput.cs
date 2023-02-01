using System;
using UnityEditor.Experimental.GraphView;

namespace NodeEngine.Attributes {
  [AttributeUsage(AttributeTargets.Property)]
  public class NodeInput : Attribute {
    public string        Name     { get; }
    public Port.Capacity Capacity { get; }

    public NodeInput(string name = "", Port.Capacity capacity = Port.Capacity.Single) {
      Name     = name;
      Capacity = capacity;
    }
  }
}
