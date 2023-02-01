using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace NodeEngine.Runtime {
  [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
  public class NodeTree : ScriptableObject {
    [field: SerializeField] public bool        IsInitialized { get; private set; }
    [field: SerializeField] public List<Group> Groups        { get; private set; }
    [field: SerializeField] public List<Node>  Nodes         { get; private set; }
    [field: SerializeField] public List<Edge>  Edges         { get; private set; }


    
    public void Init() {
      if (IsInitialized) return;
      IsInitialized = true;

      Groups = new List<Group>();
      Nodes  = new List<Node>();
      Edges  = new List<Edge>();
      
      OnInit();
    }
    
    protected virtual void OnInit() { }
    
    
    
    public NodeTree Clone() {
      var newNodeTree = (NodeTree) CreateInstance(this.GetType());

      newNodeTree.IsInitialized = IsInitialized;

      newNodeTree.Nodes  = CloneNodes(newNodeTree);
      newNodeTree.Edges  = CloneEdges(newNodeTree);
      newNodeTree.Groups = CloneGroups(newNodeTree);
      
      OnClone(newNodeTree);
      newNodeTree.name = $"{name}_CLONE";

      return newNodeTree;
    }

    protected virtual void OnClone(NodeTree newNodeTree) { }
    
    private List<Node> CloneNodes(NodeTree newNodeTree) {
      var newNodes = new List<Node>(Nodes.Count);
      
      foreach (var node in Nodes) 
        newNodes.Add(node.Clone(newNodeTree));
      
      return newNodes;
    }
    private List<Edge> CloneEdges(NodeTree newNodeTree) {
      var newEdges = new List<Edge>(Edges.Count);
      
      foreach (var edge in Edges) 
        newEdges.Add(edge.Clone(newNodeTree, newNodeTree.Nodes));
      
      return newEdges;
    }
    private List<Group> CloneGroups(NodeTree newNodeTree) {
      var newGroups = new List<Group>(Groups.Count);
      
      foreach (var group in Groups) 
        newGroups.Add(group.Clone(newNodeTree, newNodeTree.Nodes));
      
      return newGroups;
    }
  }
}
