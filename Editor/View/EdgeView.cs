using NodeEngine.Editor.Utils;
using NodeEngine.Runtime;
using UnityEditor;
using UnityEngine;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace NodeEngine.Editor.View {
  public class EdgeView : Edge, IDestroyable {
    public NodeTree     NodeTree { get; }
    public Runtime.Edge Asset    { get; }
    
    private NodeView OutputNodeView { get; }
    private NodeView InputNodeView  { get; }
    private int      OutputPortID   { get; }
    private int      InputPortID    { get; }

    
    
    public EdgeView(NodeView outputNodeView, NodeView inputNodeView, Runtime.Edge asset) {
      Asset = asset;
      
      OutputNodeView = outputNodeView;
      InputNodeView  = inputNodeView;
      NodeTree       = Asset.Tree;

      output = outputNodeView.OutputPorts[Asset.OutPortID];
      input  = inputNodeView.InputPorts[Asset.InPortID];

      OutputPortID = OutputNodeView.OutputPorts.IndexOf(output);
      InputPortID  = InputNodeView.InputPorts.IndexOf(input);
    }
    
    
    
    public Runtime.Edge SaveAsset() {
      Asset.Init(
        NodeTree,
        OutputNodeView.Asset,
        InputNodeView.Asset,
        OutputPortID,
        InputPortID);

      return Asset.Save(GetAssetName(), NodeTree.GetEdgesFolder());
    }
    
    
    public void Destroy() {
      var assetPath = SaveUtils.GetSaveAssetPath(GetAssetName(), NodeTree.GetEdgesFolder());
      
      OutputNodeView.InvokeDisconnectPort(output, input);
      InputNodeView.InvokeDisconnectPort(input, output);

      NodeTree.Edges.Remove(Asset);

      Asset.InNode.Edges.Remove(Asset);
      Asset.OutNode.Edges.Remove(Asset);
      
      AssetDatabase.DeleteAsset(assetPath);
      AssetDatabase.Refresh();
    }
    
    
    public string GetAssetName() {
      return $"Edge_{OutputNodeView.GetAssetName()}_({OutputPortID})_to_{InputNodeView.GetAssetName()}_({InputPortID})";
    }
  }
}
