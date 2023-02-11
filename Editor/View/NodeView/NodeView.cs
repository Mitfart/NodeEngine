using System;
using System.Collections.Generic;
using System.Reflection;
using NodeEngine.Editor.Utils;
using NodeEngine.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace NodeEngine.Editor.View {
  public class NodeView : Node, IDestroyable {
    public NodeTree     NodeTree { get; }
    public Runtime.Node Asset    { get; }

    public Dictionary<Port, PropertyInfo> OutputPorts { get; }
    public Dictionary<Port, PropertyInfo> InputPorts  { get; }

    public event Action<Port, Port> OnConnectPort;
    public event Action<Port, Port> OnDisconnectPort;
    
    public GroupView Group { get; set; }

    

    public NodeView(Runtime.Node asset, NodeTree nodeTree) {
      NodeTree = nodeTree;
      Asset    = asset;
      title    = asset.Title;
      
      OutputPorts = new Dictionary<Port, PropertyInfo>();
      InputPorts  = new Dictionary<Port, PropertyInfo>();

      this.AddNodePorts();
      this.AddNodeFieldsAndProperties();
    }
    
    
    
    public virtual void InvokeConnectPort(Port curPort, Port connectedPort) { OnConnectPort?.Invoke(curPort, connectedPort); }
    public virtual void InvokeDisconnectPort(Port curPort, Port disconnectedPort) { OnDisconnectPort?.Invoke(curPort, disconnectedPort); }
    
    
    
    public Runtime.Node SaveAsset() {
      Asset.Init(NodeTree);
      Asset.savePosition = GetPosition().position;
      return Asset.Save(GetAssetName(), NodeTree.GetNodesFolder());
    }
    
    
    public void Destroy() {
      var assetPath = SaveUtils.GetSaveAssetPath(GetAssetName(), NodeTree.GetNodesFolder());
      
      NodeTree.Nodes.Remove(Asset);
      Group?.RemoveNode(this);

      AssetDatabase.DeleteAsset(assetPath);
      AssetDatabase.Refresh();
    }
    
    
    public string GetAssetName() {
      return $"{title}_{NodeTree.Nodes.IndexOf(Asset)}";
    }
    
    
    
    public Port AddInputPort(Type type, string portName, PropertyInfo propertyInfo, Port.Capacity capacity = Port.Capacity.Single) {
      var port = CreatePort(type, Direction.Input, portName, capacity);
      InputPorts.Add(port, propertyInfo);
      inputContainer.Add(port);
      return port;
    }
    
    public Port AddInputPort<T>(string portName, PropertyInfo propertyInfo, Port.Capacity capacity = Port.Capacity.Single) {
      return AddInputPort(typeof(T), portName, propertyInfo, capacity);
    }
    
    
    public Port AddOutputPort(Type type, string portName, PropertyInfo propertyInfo, Port.Capacity capacity = Port.Capacity.Single) {
      var port = CreatePort(type, Direction.Output, portName, capacity);
      OutputPorts.Add(port, propertyInfo);
      outputContainer.Add(port);
      return port;
    }
    
    public Port AddOutputPort<T>(string portName, PropertyInfo propertyInfo, Port.Capacity capacity = Port.Capacity.Single) {
      return AddOutputPort(typeof(T), portName, propertyInfo, capacity);
    }
    
    
    private Port CreatePort(
      Type          type,
      Direction     direction,
      string        portName = null,
      Port.Capacity capacity = Port.Capacity.Single
    ) {
      var port = InstantiatePort(Orientation.Horizontal, direction, capacity, type);
      port.portName = portName;
      return port;
    }



    public override void SetPosition(Rect newPos) {
      base.SetPosition(newPos);
      Asset.savePosition = newPos.position;
      EditorUtility.SetDirty(Asset);
    }
  }
}
