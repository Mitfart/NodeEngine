using NodeEngine.Editor.Graph;
using NodeEngine.Editor.Style;
using NodeEngine.Editor.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEngine.Editor.Window {
  public partial class NodeTreeEditorWindow : EditorWindow {
    [field: SerializeField] public Runtime.NodeTree ActiveNodeTree { get; private set; }
    
    public NodeGraph Graph { get; private set; }

    
    public void CreateGUI() {
      var root = rootVisualElement;

      root
       .AddChild(new Toolbar()
                .AddChild(new ToolbarButton(SaveTree) { text = nameof(SaveTree) })
                .AddChild(new ToolbarButton(LoadTree) { text = nameof(LoadTree) }))
       .AddChild(new VisualElement { style = { flexGrow = 1 } }
                  .AddChild(Graph          = new NodeGraph(this)));


      root.styleSheets.AddAllNodeTreeStyles();


      if (ActiveNodeTree == null) return;
      
      Windows[ActiveNodeTree] = this;
      LoadTree();
    }
  }
}
