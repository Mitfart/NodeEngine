using System.Collections.Generic;
using NodeEngine.Editor.View;
using NodeEngine.Runtime;

namespace NodeEngine.Editor.Window {
  public partial class NodeTreeEditorWindow {
    private readonly Dictionary<Node, NodeView> _loadNodeViewByNode = new();

    
    
    public void LoadTree() {
      if (ActiveNodeTree == null) return;
      ActiveNodeTree.Init();
      
      ClearLoadData();
      ClearGraph();
      LoadGraph();
    }
    
    
    private void ClearLoadData() {
      _loadNodeViewByNode.Clear();
    }
    
    private void ClearGraph() {
      Graph.ClearElements();
    }

    private void LoadGraph() {
      foreach (var node     in ActiveNodeTree.Nodes)  LoadNode(node);
      foreach (var loadEdge in ActiveNodeTree.Edges)  LoadEdge(loadEdge);
      foreach (var group    in ActiveNodeTree.Groups) LoadGroup(group);
    }
    

    private void LoadNode(Node node) {
      _loadNodeViewByNode[node] = Graph.AddNode(node);
    }
    
    private void LoadEdge(Edge loadEdge) {
      Graph.AddEdge(
        _loadNodeViewByNode[loadEdge.OutNode],
        _loadNodeViewByNode[loadEdge.InNode],
        loadEdge
        );
    }
    
    private void LoadGroup(Group group) {
      var graphGroup = Graph.AddGroup(group);
      
      foreach (var node in group.Nodes)
        graphGroup.AddNode(_loadNodeViewByNode[node]);
    }
  }
}
