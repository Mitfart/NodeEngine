using System.Collections.Generic;
using NodeEngine.Attributes;
using NodeEngine.Editor.View;
using UnityEngine;

namespace NodeEngine.Runtime {
  [NonSearchableNode]
  public class Node : ScriptableObject {
    [field: SerializeField] public bool       IsInitialized { get; private set; }
    [field: SerializeField] public NodeTree   Tree          { get; private set; }
    [field: SerializeField] public List<Edge> Edges         { get; private set; }
    
    public virtual string Title => GetType().Name;
    
    
    public void Init(NodeTree tree) {
      if (IsInitialized) return;
      IsInitialized = true;
      
      Tree  = tree;
      Edges = new List<Edge>();
      
      OnInit();
    }
    
    public Node Clone(NodeTree newNodeTree) {
      var newNode = (Node) CreateInstance(this.GetType());

      newNode.IsInitialized = IsInitialized;
      newNode.Tree          = newNodeTree;
      newNode.Edges         = new List<Edge>(new Edge[Edges.Count]);
      
      OnClone(newNode);
#if UNITY_EDITOR
      newNode.savePosition = savePosition;
#endif
      return newNode;
    }
    
    
    protected virtual void OnInit()              { }
    protected virtual void OnClone(Node newNode) { }
    
    
    
#if UNITY_EDITOR
    public Vector2 savePosition;
    
    public NodeView GetNodeView() {
      var nodeView = new NodeView(this, Tree);
      OnGetNodeView(nodeView);
      return nodeView;
    }

    protected virtual void OnGetNodeView(NodeView nodeView) { }
#endif
  }
}
