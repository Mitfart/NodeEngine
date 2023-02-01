using System;
using System.Collections.Generic;
using System.Text;
using NodeEngine.Attributes;
using NodeEngine.Editor.Window;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Node = NodeEngine.Runtime.Node;

namespace NodeEngine.Editor.Search {
  public class NodeTreeSearch : ScriptableObject, ISearchWindowProvider {
    private const string TITLE               = "Nodes";
    private const string HIERARCHY_SEPARATOR = "/";

    private static readonly StringBuilder        Groups_Builder = new();
    private static          Texture2D            _indentationIcon;
    private                 NodeTreeEditorWindow _nodeTreeEditor;

    
    
    public void Init(NodeTreeEditorWindow nodeTreeEditor) {
      _nodeTreeEditor  = nodeTreeEditor;
      _indentationIcon = GetIndentationIcon();
    }
    
    
    
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
      var items     = new List<SearchTreeEntry>();
      var groups    = new HashSet<string>();
      var nodeTypes = TypeCache.GetTypesDerivedFrom<Node>();
      int indentLevel;
      
      AddTitle();

      foreach (var nodeType in nodeTypes) {
        
        if (nodeType.IsAbstract) continue;
        if (nodeType.GetCustomAttributes(typeof(NonSearchableNode), false).Length > 0) continue;

          indentLevel = 1;
        Groups_Builder.Clear();

        AddGroups(nodeType.BaseType);
        AddItem(nodeType);
      }

      return items;


      void AddTitle() {
        items.Add(new SearchTreeGroupEntry(new GUIContent(TITLE)));
      }

      void AddGroups(Type baseType) {
        while (baseType != null && baseType != typeof(Node)) {
          Groups_Builder.Append(baseType);

          var curGroup = Groups_Builder.ToString();

          if (!groups.Contains(curGroup)) { AddGroup(baseType, curGroup); }

          Groups_Builder.Append(HIERARCHY_SEPARATOR);

          baseType = baseType.BaseType;
          indentLevel++;
        }
      }

      void AddGroup(Type baseType, string curGroup) {
        items.Add(new SearchTreeGroupEntry(new GUIContent(baseType.Name), indentLevel));
        groups.Add(curGroup);
      }

      void AddItem(Type nodeType) {
        items.Add(
          new SearchTreeEntry(new GUIContent(nodeType.Name, _indentationIcon)) {
            level = indentLevel, userData = nodeType
          });
      }
    }
    
    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
      var nodeType = (Type) searchTreeEntry.userData;
      var position = _nodeTreeEditor.Graph.GetMousePos(context.screenMousePosition);
      var node     = _nodeTreeEditor.Graph.AddNode(position, nodeType);

      _nodeTreeEditor.SaveNode(node);
      
      return true;
    }

    

    private static Texture2D GetIndentationIcon() {
      var icon = new Texture2D(1, 1);
      icon.SetPixel(0, 0, Color.clear);
      icon.Apply();
      return icon;
    }
  }
}
