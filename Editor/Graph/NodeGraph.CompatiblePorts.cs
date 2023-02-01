using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace NodeEngine.Editor.Graph {
  public partial class NodeGraph {
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
      return ports
            .Where(
               endPort =>
                 endPort.direction != startPort.direction
              && endPort.node      != startPort.node
              && endPort.portType  == startPort.portType)
            .ToList();
    }
  }
}
