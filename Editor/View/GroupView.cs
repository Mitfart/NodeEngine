using System.Collections.Generic;
using System.Linq;
using NodeEngine.Editor.Utils;
using NodeEngine.Runtime;
using UnityEditor;
using UnityEngine;
using Group = UnityEditor.Experimental.GraphView.Group;
using Node = NodeEngine.Runtime.Node;

namespace NodeEngine.Editor.View {
  public class GroupView : Group, IDestroyable {
    public  Runtime.Group  Asset    { get; }
    private NodeTree       NodeTree { get; }
    private List<NodeView> Nodes    { get; }
    
    
    
    public GroupView(Runtime.Group asset, NodeTree nodeTree) {
      NodeTree = nodeTree;
      Asset    = asset;
      title    = asset.name;

      Nodes = new List<NodeView>();
    }
    
    public GroupView(string groupTitle, NodeTree nodeTree) {
      NodeTree = nodeTree;
      Asset    = ScriptableObject.CreateInstance<Runtime.Group>();
      title    = MakeTitleUnique(groupTitle);

      Nodes = new List<NodeView>();
    }

    
    
    public Runtime.Group SaveAsset() {
      List<Node> nodes = Nodes.Select(nodeView => nodeView.Asset).ToList();

      Asset.Init(NodeTree, nodes);

      return Asset.Save(GetAssetName(), NodeTree.GetGroupsFolder());
    }
    
    
    public void Destroy() {
      var assetPath = SaveUtils.GetSaveAssetPath(GetAssetName(), NodeTree.GetGroupsFolder());
      
      NodeTree.Groups.Remove(Asset);
      
      AssetDatabase.DeleteAsset(assetPath);
      AssetDatabase.Refresh();
    }

    
    public string GetAssetName() {
      return title;
    }
    
    
    
    public void AddNode(NodeView nodeView) {
      if (Nodes.Contains(nodeView)) return;
      
      nodeView.Group = this;
      Nodes.Add(nodeView);
      
      if (!this.containedElements.Contains(nodeView))
        AddElement(nodeView);
    }
    
    public void RemoveNode(NodeView nodeView) {
      if (!Nodes.Contains(nodeView)) return;
      
      nodeView.Group = null;
      Nodes.Remove(nodeView);
      
      if (this.containedElements.Contains(nodeView))
        RemoveElement(nodeView);
    }
    
    
    
    private string MakeTitleUnique(string groupTitle) {
      var finalTitle        = groupTitle;
      var titleRepeatsCount = 0;
      var isUnique          = false;

      while (!isUnique) {
        isUnique = true;
        foreach (var group in NodeTree.Groups) {
          if (!group.name.Equals(finalTitle)) continue;

          finalTitle = $"{groupTitle}_{titleRepeatsCount++}";
          isUnique   = false;
          break;
        }
      }

      return finalTitle;
    }
  }
}
