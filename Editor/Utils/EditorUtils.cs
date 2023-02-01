using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEngine.Editor.Utils {
  public class EditorUtils {
    public static Vector2 GetLocalMousePos(Vector2 mousePosition, GraphView graphView, EditorWindow editorWindow) {
      var root                = editorWindow.rootVisualElement;
      var windowMousePosition = root.ChangeCoordinatesTo(root.parent, mousePosition - editorWindow.position.position);
      var graphMousePosition  = graphView.contentViewContainer.WorldToLocal(windowMousePosition);

      return graphMousePosition;
    }
  }
}
