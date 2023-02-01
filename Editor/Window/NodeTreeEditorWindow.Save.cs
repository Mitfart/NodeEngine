using NodeEngine.Editor.View;
using UnityEditor;
using UnityEngine;

namespace NodeEngine.Editor.Window {
  public partial class NodeTreeEditorWindow {
    public void SaveTree() {
      if (Application.isPlaying) return;
      
      if (ActiveNodeTree == null) return;

      Graph.graphElements.ForEach(
        element => {
          switch (element) {
            case NodeView nodeView: SaveNode(nodeView); break;
            case EdgeView edge:     SaveEdge(edge); break;
            case GroupView group:   SaveGroup(group); break;
          }
        });

      EditorUtility.SetDirty(ActiveNodeTree);
      AssetDatabase.Refresh();
    }
    
    
    public void SaveNode(NodeView nodeView) {
      if (Application.isPlaying) return;
      
      if (!ActiveNodeTree.Nodes.Contains(nodeView.Asset)) {
        ActiveNodeTree.Nodes.Add(nodeView.Asset);
        nodeView.SaveAsset();
      }
      
      EditorUtility.SetDirty(nodeView.Asset);
    }
    public void SaveEdge(EdgeView edge) {
      if (Application.isPlaying) return;
      
      if (!ActiveNodeTree.Edges.Contains(edge.Asset)) {
        ActiveNodeTree.Edges.Add(edge.Asset);
        edge.SaveAsset();
      }

      if (!edge.Asset.InNode.Edges.Contains(edge.Asset)) {
        edge.Asset.InNode.Edges.Add(edge.Asset);
      }
      
      if (!edge.Asset.OutNode.Edges.Contains(edge.Asset)) {
        edge.Asset.OutNode.Edges.Add(edge.Asset);
      }
        
      
      EditorUtility.SetDirty(edge.Asset);
    }
    public void SaveGroup(GroupView groupView) {
      if (Application.isPlaying) return;
      
      if (!ActiveNodeTree.Groups.Contains(groupView.Asset)) {
        ActiveNodeTree.Groups.Add(groupView.Asset);
        groupView.SaveAsset();
      }
      
      EditorUtility.SetDirty(groupView.Asset);
    }
  }
}
