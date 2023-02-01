using System;
using UnityEditor.Experimental.GraphView;

namespace NodeEngine.Attributes {
  [AttributeUsage(AttributeTargets.Property)]
  public class NodeOutput : Attribute {
    public string        Name     { get; }
    public Port.Capacity Capacity { get; }

    public NodeOutput(string name = "", Port.Capacity capacity = Port.Capacity.Single) { 
      Name     = name;
      Capacity = capacity;
    }
  }
}
