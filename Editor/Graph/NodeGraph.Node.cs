using System;
using NodeEngine.Editor.View;
using NodeEngine.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NodeEngine.Editor.Graph {
  public partial class NodeGraph {
    public NodeView AddNode(Vector2 position, Type nodeType) {
      var instance = ScriptableObject.CreateInstance(nodeType);

      if (instance is not Node node) {
        Object.Destroy(instance);
        return null;
      }

      node.savePosition = position;
      return AddNode(node);
    }
    
    public NodeView AddNode(Node node) {
      node.Init(EditorWindow.ActiveNodeTree);
      var nodeView = node.GetNodeView();

      nodeView.SetPosition(new Rect(node.savePosition, default));
      AddElement(nodeView);
      
      return nodeView;
    }
  }
}
