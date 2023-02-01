using System.Linq;
using NodeEngine.Editor.View;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEngine.Editor.Graph {
  public partial class NodeGraph {
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
      base.BuildContextualMenu(evt);

      AddGroupUpAction(evt);
      AddUngroupAction(evt);
    }


    private void AddGroupUpAction(ContextualMenuPopulateEvent evt) {
      const string DEFAULT_NEW_GROUP_NAME = "new Group";
      
      var status = selection.Count > 0
        ? DropdownMenuAction.Status.Normal
        : DropdownMenuAction.Status.Disabled;

      evt.menu.AppendAction(
        "GroupUp", a => {
          if (status == DropdownMenuAction.Status.Disabled) return;

          var group = AddGroup(DEFAULT_NEW_GROUP_NAME);
          group.SetPosition(new Rect(GetMousePos(a.eventInfo.localMousePosition), default));
          
          foreach (var selected in selection) {
            if (selected is not NodeView node || !node.IsGroupable()) continue;
            group.AddNode(node);
          }
          
          EditorWindow.SaveGroup(group);
        }, status);
    }

    private void AddUngroupAction(ContextualMenuPopulateEvent evt) {
      var status = selection.Count > 0 && AllSelectedElementsHasGroups()
        ? DropdownMenuAction.Status.Normal
        : DropdownMenuAction.Status.Disabled;

      evt.menu.AppendAction(
        "Ungroup", _ => { 
          if (status == DropdownMenuAction.Status.Disabled) return;

          foreach (var selected in selection) {
            if (selected is NodeView node) 
              node.Group.RemoveNode(node);
          }
        }, status);
    }

    
    private bool AllSelectedElementsHasGroups() {
      return selection.All(selected => selected is not NodeView { Group: null });
    }
  }
}
