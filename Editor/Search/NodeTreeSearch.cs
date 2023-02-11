using System;
using System.Collections.Generic;
using System.Reflection;
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

    private static readonly StringBuilder        GroupsBuilder = new();
    private static          Texture2D            _indentationIcon;
    private                 NodeTreeEditorWindow _nodeTreeEditor;

    
    
    public void Init(NodeTreeEditorWindow nodeTreeEditor) {
      _nodeTreeEditor  = nodeTreeEditor;
      _indentationIcon = SearchWindowUtils.GetIndentationIcon();
    }
    
    
    
      public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
         var items     = new List<SearchTreeEntry>();
         var groups    = new HashSet<string>();
         var nodeTypes = TypeCache.GetTypesDerivedFrom<Node>();
      
         AddTitle(items);

         foreach (var nodeType in nodeTypes) {
            if (nodeType.IsAbstract) continue;
            if (nodeType.GetCustomAttribute(typeof(NonSearchableNode), false) != null) continue;

            GroupsBuilder.Clear();

            AddGroupsByInheritance(items, groups, nodeType, out var indentLevel);
            AddItem(items, nodeType.Name, indentLevel, nodeType);
         }

         return items;
      }

      
      private static void AddTitle(ICollection<SearchTreeEntry> items) {
         items.Add(new SearchTreeGroupEntry(new GUIContent(TITLE)));
      }
      
      private static void AddGroupsByInheritance(
         ICollection<SearchTreeEntry> items, 
         ICollection<string>          groups, 
         Type                         type, 
         out int                      indentLevel) 
      {
         indentLevel = 0;
         while (type != null && type != typeof(Node)) {
            AddGroup(
               items, 
               groups, 
               type.Name, 
               indentLevel++);
            
            type = type.BaseType;
         }
      }
      
      private static void AddGroup(
         ICollection<SearchTreeEntry> items, 
         ICollection<string>          groups, 
         string                       groupName,
         int                          indentLevel) 
      {
         var groupFullName = 
            GroupsBuilder
              .Append(groupName)
              .Append(HIERARCHY_SEPARATOR)
              .ToString();
         
         if (groups.Contains(groupFullName)) return;
            
         items.Add(
            new SearchTreeGroupEntry(
               new GUIContent(groupName), 
               indentLevel
               )
            );
         groups.Add(groupFullName);
      }
      
      private static void AddItem(
         ICollection<SearchTreeEntry> items,
         string                       name,
         int                          indentLevel,
         object                       data
      ) {
         items.Add(
            new SearchTreeEntry(new GUIContent(name, _indentationIcon)){
               level    = indentLevel, 
               userData = data
            });
      }
    
    
    
    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
      var nodeType = (Type) searchTreeEntry.userData;
      var position = _nodeTreeEditor.Graph.GetMousePos(context.screenMousePosition);
      var node     = _nodeTreeEditor.Graph.AddNode(position, nodeType);

      _nodeTreeEditor.SaveNode(node);
      return true;
    }
  }
}
