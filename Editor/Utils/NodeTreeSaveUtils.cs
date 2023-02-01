namespace NodeEngine.Editor.Utils {
  public static class NodeTreeSaveUtils {
    public static string GetTreeFolder(this Runtime.NodeTree nodeTree) {
      return $"{nodeTree.GetRootPath()}{nodeTree.name}";
    }
    public static string GetGroupsFolder(this Runtime.NodeTree nodeTree) {
      return SaveUtils.GetFolder($"{GetTreeFolder(nodeTree)}/Groups");
    }
    public static string GetNodesFolder(this Runtime.NodeTree nodeTree) {
      return SaveUtils.GetFolder($"{GetTreeFolder(nodeTree)}/Nodes");
    }
    public static string GetEdgesFolder(this Runtime.NodeTree nodeTree) {
      return SaveUtils.GetFolder($"{GetTreeFolder(nodeTree)}/Edges");
    }
  }
}
