using NodeEngine.Editor.Utils;
using UnityEngine;

namespace NodeEngine.Editor.Graph {
  public partial class NodeGraph {
    public Vector2 GetMousePos(Vector2 localMousePos) {
      return EditorUtils.GetLocalMousePos(localMousePos, this, EditorWindow);
    }
  }
}
