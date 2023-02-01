using NodeEngine.Editor.View;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NodeEngine.Editor.Graph {
  public partial class NodeGraph {
    public EdgeView AddEdge(NodeView outNodeView, NodeView inNodeView, Runtime.Edge asset) {
      var edge = new EdgeView(
        outputNodeView: outNodeView,
        inputNodeView: inNodeView,
        asset
      );
      
      edge.output.Connect(edge);
      edge.input.Connect(edge);

      AddElement(edge);
      return edge;
    }

    public EdgeView AddEdge(NodeView outNodeView, NodeView inNodeView, Port outPort, Port inPort) {
      var edge = ScriptableObject.CreateInstance<Runtime.Edge>();
      edge.Init(
        EditorWindow.ActiveNodeTree,
        outNodeView.Asset,
        inNodeView.Asset,
        outNodeView.OutputPorts.IndexOf(outPort),
        inNodeView.InputPorts.IndexOf(inPort));
      
      return AddEdge(outNodeView, inNodeView, edge);
    }
  }
}
