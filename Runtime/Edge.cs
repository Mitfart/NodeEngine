using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace NodeEngine.Runtime {
  [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
  public class Edge : ScriptableObject {
    [field: SerializeField] public bool     IsInitialized { get; private set; }
    [field: SerializeField] public NodeTree Tree          { get; private set; }
    [field: SerializeField] public Node     OutNode       { get; private set; }
    [field: SerializeField] public Node     InNode        { get; private set; }
    [field: SerializeField] public int      OutPortID     { get; private set; }
    [field: SerializeField] public int      InPortID      { get; private set; }


    public void Init(
      NodeTree tree,
      Node     outNode,
      Node     inNode,
      int      outPortID,
      int      inPortID
    ) {
      if (IsInitialized) return;

      IsInitialized = true;
      Tree          = tree;
      OutNode       = outNode;
      InNode        = inNode;
      OutPortID     = outPortID;
      InPortID      = inPortID;
    }
    
    
    public Edge Clone(NodeTree newNodeTree, List<Node> newNodes) {
      var newEdge = (Edge) CreateInstance(this.GetType());

      newEdge.Tree      = newNodeTree;
      newEdge.OutNode   = newNodes[Tree.Nodes.IndexOf(OutNode)];
      newEdge.InNode    = newNodes[Tree.Nodes.IndexOf(InNode)];
      newEdge.OutPortID = OutPortID;
      newEdge.InPortID  = InPortID;

      newEdge.OutNode.Edges[OutNode.Edges.IndexOf(this)] = newEdge;
      newEdge.InNode.Edges[InNode.Edges.IndexOf(this)]   = newEdge;

      return newEdge;
    }
  }
}
