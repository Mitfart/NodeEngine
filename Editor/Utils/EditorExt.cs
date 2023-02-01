using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NodeEngine.Editor.Utils {
  public static class Extentions {
    public static T AddChild<T, TChild>(this T root, TChild visualElement)
      where T : VisualElement where TChild : VisualElement {
      root.Add(visualElement);
      return root;
    }
    
    
    public static VisualElement AddPropertyVisualElement(
      this VisualElement root,
      SerializedProperty property,
      int                skipFoldoutsCount = 0) {
      var lastPropPath = string.Empty;

      foreach (SerializedProperty prop in property)
        if (prop.isArray && prop.propertyType == SerializedPropertyType.Generic){
          if (skipFoldoutsCount <= 0){
            var foldout = new Foldout{ text = prop.displayName };
            foldout.AddPropertyVisualElement(prop);
            root.Add(foldout);
          }
          else{
            root.AddPropertyVisualElement(prop);
            skipFoldoutsCount--;
          }
        }
        else{
          if (!string.IsNullOrWhiteSpace(lastPropPath) && prop.propertyPath.Contains(lastPropPath)) continue;

          lastPropPath = prop.propertyPath;
          root.Add(new PropertyField(prop));
        }

      return root;
    }
  }
}
