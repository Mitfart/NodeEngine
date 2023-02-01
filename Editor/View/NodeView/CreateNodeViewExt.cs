using System;
using System.Reflection;
using NodeEngine.Attributes;
using UnityEditor;
using UnityEditor.UIElements;

namespace NodeEngine.Editor.View {
  public static class CreateNodeViewExt {
    
    public static void AddNodePorts(this NodeView nodeView) {
      var nodeType = nodeView.Asset.GetType();
      
      foreach (var property in nodeType.GetProperties()) {
        AddInputPort(nodeView, property);
        AddOutputPort(nodeView, property);
      }
    }

    
    private static void AddInputPort(this NodeView nodeView, PropertyInfo property) {
      var input = property.GetCustomAttribute<NodeInput>();
      if (input != null)
        nodeView.AddInputPort(property.PropertyType, input.Name, input.Capacity);
    }
    
    private static void AddOutputPort(this NodeView nodeView, PropertyInfo property) {
      var output = property.GetCustomAttribute<NodeOutput>();
      if (output != null)
        nodeView.AddOutputPort(property.PropertyType, output.Name, output.Capacity);
    }
    
    
    
    public static void AddNodeFieldsAndProperties(this NodeView nodeView) {
      var node     = nodeView.Asset;
      var nodeType = node.GetType();
      var sObject  = new SerializedObject(node);

      foreach (var field in nodeType.GetFields()) {
        if (Attribute.IsDefined(field, typeof(NodeField))) 
          nodeView.AddField(field.Name, sObject);
      }

      foreach (var property in nodeType.GetProperties()) {
        if (Attribute.IsDefined(property, typeof(NodeField))) 
          nodeView.AddField(property.Name, sObject);
      }

      nodeView.RefreshExpandedState();
    }

    private static void AddField(this NodeView nodeView, string fieldName, SerializedObject sObject) {
      var sProperty      = sObject.FindProperty(fieldName);
      var sPropertyField = new PropertyField(sProperty);

      nodeView.extensionContainer.Add(sPropertyField);
      sPropertyField.bindingPath = fieldName;
      sPropertyField.Bind(sObject);
    }
  }
}
