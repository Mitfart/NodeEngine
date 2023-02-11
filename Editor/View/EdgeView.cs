using NodeEngine.Editor.Utils;
using NodeEngine.Runtime;
using UnityEditor;
using UnityEngine;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace NodeEngine.Editor.View {
  public class EdgeView : Edge, IDestroyable {
    public Runtime.Edge               Asset      { get; }
    public NodeTree                   NodeTree   => Asset.Tree;
    public Runtime.Edge.ConnectionMap Connection => Asset.Connection;
    
    private NodeView OutputNodeView { get; }
    private NodeView InputNodeView  { get; }
    



    public EdgeView(NodeView outputNodeView, NodeView inputNodeView, Runtime.Edge asset) {
      Asset = asset;
      
      OutputNodeView = outputNodeView;
      InputNodeView  = inputNodeView;
    }
    
    public Runtime.Edge SaveAsset() {
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
      return $"Edge_{OutputNodeView.GetAssetName()}_({Connection.source.Name})_to_{InputNodeView.GetAssetName()}_({Connection.target.Name})";
    }
  }
}
