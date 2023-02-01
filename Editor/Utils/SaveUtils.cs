using System.IO;
using UnityEditor;
using UnityEngine;

namespace NodeEngine.Editor.Utils {
  public static class SaveUtils {
    public static string GetSaveAssetPath(string assetName, string path) {
      return $"{path}/{assetName}.asset";
    }
    
    
    
    public static T Save<T>(this T asset, string like, string to) where T : Object{
      var savePath = GetSaveAssetPath(like, to);
      
      var assetPath = AssetDatabase.GetAssetPath(asset);
      if (!string.IsNullOrWhiteSpace(assetPath)) {
        AssetDatabase.MoveAsset(assetPath, to);
        return default;
      }
      
      var loadedAsset = AssetDatabase.LoadAssetAtPath<T>(savePath);
      if (loadedAsset != default) return loadedAsset;
      
      AssetDatabase.CreateAsset(asset, savePath);
      return asset;
    }

    
    public static string GetFolder(string path) {
      if (!Directory.Exists(path)) 
        Directory.CreateDirectory(path);

      return path;
    }

    public static string GetRootPath(this Object asset) {
      var assetPath = AssetDatabase.GetAssetPath(asset);
      var assetName = Path.GetFileName(assetPath);
      var rootPath  = assetPath.Replace(assetName, null);
      return rootPath;
    }
  }
}
