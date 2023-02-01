using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEngine.Editor.Style {
  public static class NodeTreeStyle {
    private const string ROOT_PATH = "Mitfart/NodeTree";
    
    public static readonly StyleSheet Vars      = GetStyle("_vars");
    public static readonly StyleSheet Node_Tree = GetStyle("NodeTreeStyle");

    
    public static void AddAllNodeTreeStyles(this VisualElementStyleSheetSet styleSheet) {
      styleSheet.Add(Vars);
      styleSheet.Add(Node_Tree);
    }

    private static StyleSheet GetStyle(string name) => Resources.Load<StyleSheet>($"{ROOT_PATH}/{name}");
  }
}
