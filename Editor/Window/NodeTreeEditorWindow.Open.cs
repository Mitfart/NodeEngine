using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;

namespace NodeEngine.Editor.Window {
  public partial class NodeTreeEditorWindow {
    private const string OPEN_MENU_PATH     = "Mitfart/NodeTreeEditor";
    private const string NEW_NODE_TREE_NAME = "Undefined *";
    
    private static readonly Dictionary<Runtime.NodeTree, NodeTreeEditorWindow> Windows = new();



    [MenuItem(OPEN_MENU_PATH)]
    public static void Open() {
      var newNodeTree = CreateInstance<Runtime.NodeTree>();
      newNodeTree.name = NEW_NODE_TREE_NAME;
      
      OpenFor(newNodeTree);
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID) {
      if (EditorUtility.InstanceIDToObject(instanceID) is not Runtime.NodeTree nodeTree) return false;

      OpenFor(nodeTree);
      return true;
    }


    public static void OpenFor(Runtime.NodeTree nodeTree) {
      if (!Windows.TryGetValue(nodeTree, out var window) || window == null) {
        window                   = CreateWindow<NodeTreeEditorWindow>();
        window.titleContent.text = nodeTree.name;
        Windows[nodeTree]        = window;
      }

      window.ActiveNodeTree = nodeTree;
      window.Focus();
      window.LoadTree();
    }
  }
}
