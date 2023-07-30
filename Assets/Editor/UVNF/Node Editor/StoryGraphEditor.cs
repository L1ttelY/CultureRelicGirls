﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

using XNodeEditor;
using XNode;

using UVNF.Core.Story;
using UVNF.Core.Story.Dialogue;
using UVNF.Entities.Containers;
using UVNF.Extensions;

namespace UVNF.Editor.Story.Nodes
{
    [CustomNodeGraphEditor(typeof(StoryGraph))]
    public class StoryGraphEditor : NodeGraphEditor
    {
        public override void OnOpen()
        {
            base.OnOpen();
        }

        public override string GetNodeMenuName(Type type)
        {
            if (type.BaseType == typeof(Node) || type.IsSubclassOf(typeof(Node)))
            {
                if (type.IsSubclassOf(typeof(StoryElement)))
                {
                    StoryElement element = ScriptableObject.CreateInstance(type) as StoryElement;
                    string returnString = element.Type.ToString() + "/" + type.Name.Replace("Element", "");
                    UnityEngine.Object.DestroyImmediate(element);
                    return returnString;
                }
                else
                    return base.GetNodeMenuName(type).Replace("Node", "");
            }
            else return null;
        }

        public override void OnGUI()
        {
            base.OnGUI();
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 2)
            {
                CreateNode(typeof(DialogueElement), window.WindowToGridPosition(Event.current.mousePosition).OffsetY(20));
            }
            else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
            {
                GenericMenu menu = new GenericMenu();
                AddContextMenuItems(menu);
                menu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
            }
        }
    }
}