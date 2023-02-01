using NodeEngine.Editor.View;

namespace NodeEngine.Editor.Graph {
  public partial class NodeGraph {
    public GroupView AddGroup(string title) {
      var groupView = new GroupView(title, EditorWindow.ActiveNodeTree);
      
      AddElement(groupView);
      return groupView;
    }
    
    public GroupView AddGroup(Runtime.Group group) {
      var groupView = new GroupView(group, EditorWindow.ActiveNodeTree);
      
      AddElement(groupView);
      return groupView;
    }
    

    private void SetAddToGroupAction() {
      elementsAddedToGroup = (group, elements) => {
        foreach (var element in elements) {
          if (group is GroupView groupView 
           && element is NodeView node) 
            groupView.AddNode(node);
        }
      };
    }
  }
}
