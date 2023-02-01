using NodeEngine.Editor.Search;
using NodeEngine.Editor.Style;
using NodeEngine.Editor.Window;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEngine.Editor.Graph {
  public partial class NodeGraph : GraphView {
    public  NodeTreeEditorWindow EditorWindow { get; }
    
    private GridBackground _gridBackground;
    private NodeTreeSearch _search;
    

    
    public NodeGraph(NodeTreeEditorWindow editorWindow) {
      EditorWindow = editorWindow;

      CreateElements();
      AddElements();
      InitElements();

      AddManipulators();

      SetActions();
    }
    

    private void CreateElements() {
      _gridBackground = new GridBackground();
      _search         = ScriptableObject.CreateInstance<NodeTreeSearch>();
    }

    private void AddElements() {
      Insert(0, _gridBackground);
    }

    private void InitElements() {
      this.StretchToParentSize();
      styleSheets.AddAllNodeTreeStyles();

      _gridBackground.StretchToParentSize();
      _search.Init(EditorWindow);


      nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _search);
      graphViewChanged    = OnChange;
    }


    private void AddManipulators() {
      SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

      this.AddManipulator(new ContentDragger());
      this.AddManipulator(new SelectionDragger());
      this.AddManipulator(new RectangleSelector());
    }


    private void SetActions() {
      SetAddToGroupAction();
    }


    public void ClearElements() {
      foreach (var graphElement in graphElements)
        RemoveElement(graphElement);
    }
  }
}
