using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace NodeEngine.Runtime {
  [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
  public class Group : ScriptableObject {
    [field: SerializeField] public bool       IsInitialized { get; private set; }
    [field: SerializeField] public NodeTree   Tree          { get; private set; }
    [field: SerializeField] public List<Node> Nodes         { get; private set; }

    public void Init(
      NodeTree   tree, 
      List<Node> nodes) {
      if (IsInitialized) return;

      IsInitialized = true;
      Tree          = tree;
      Nodes         = new List<Node>(nodes);
    }
    
    public Group Clone(
      NodeTree   newNodeTree, 
      List<Node> newNodes) {
      var newGroup = (Group) CreateInstance(this.GetType());

      newGroup.Tree  = newNodeTree;
      newGroup.Nodes = new List<Node>(Nodes.Count);

      foreach (var node in Nodes)
        newGroup.Nodes.Add(newNodes[Tree.Nodes.IndexOf(node)]);
      
      return newGroup;
    }
  }
}
