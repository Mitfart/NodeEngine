using System.Linq;
using NodeEngine.Editor.View;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Edge = NodeEngine.Runtime.Edge;

namespace NodeEngine.Editor.Graph {
  public partial class NodeGraph {
    public EdgeView AddEdge(NodeView outNodeView, NodeView inNodeView, Edge asset) {
      var edge = new EdgeView(
        outputNodeView: outNodeView,
        inputNodeView: inNodeView,
        asset
      );

      
      edge.output = outNodeView.OutputPorts.First(pair => pair.Value.Name.Equals(edge.Connection.source.Name)).Key;
      edge.input  = inNodeView.InputPorts.First(pair => pair.Value.Name.Equals(edge.Connection.target.Name)).Key;
      
      edge.output.Connect(edge);
      edge.input.Connect(edge);

      AddElement(edge);
      return edge;
    }

    public EdgeView AddEdge(NodeView outNodeView, NodeView inNodeView, Port outPort, Port inPort) {
      var edge = ScriptableObject.CreateInstance<Edge>();
      edge.Init(
        EditorWindow.ActiveNodeTree,
        outNodeView.Asset,
        inNodeView.Asset,
        new Edge.ConnectionMap {
          source = outNodeView.OutputPorts[outPort],
          target = inNodeView.InputPorts[inPort]
        });
      
      return AddEdge(outNodeView, inNodeView, edge);
    }
  }
}
