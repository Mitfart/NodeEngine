using NodeEngine.Editor.View;
using UnityEditor.Experimental.GraphView;

namespace NodeEngine.Editor.Graph {
  public partial class NodeGraph {
    private GraphViewChange OnChange(GraphViewChange ev) {
      OnCreateEdges(ev);
      OnRemoveElements(ev);
      return ev;
    }

    
    private void OnCreateEdges(GraphViewChange ev) {
      if (ev.edgesToCreate == null) return;

      foreach (var edge in ev.edgesToCreate)
        OnCreateEdge(edge);
    }
    
    private void OnCreateEdge(Edge edge) {
      if (edge.isGhostEdge) return;
      
      var outPort = edge.output;
      var inPort  = edge.input;
      var outNode = (NodeView)outPort.node;
      var inNode  = (NodeView)inPort.node;

      RemoveElement(edge);
      var edgeView = AddEdge(outNode, inNode, outPort, inPort);
      edgeView.output = outPort;
      edgeView.input  = inPort;
      
      outNode.InvokeConnectPort(outPort, inPort);
      inNode.InvokeConnectPort(inPort, outPort);
      
      EditorWindow.SaveEdge(edgeView);
    }
    
    
    private void OnRemoveElements(GraphViewChange ev) {
      if (ev.elementsToRemove == null) return;

      foreach (var element in ev.elementsToRemove) {
        if (element is IDestroyable view)
          view.Destroy();
      }
    }
  }
}
