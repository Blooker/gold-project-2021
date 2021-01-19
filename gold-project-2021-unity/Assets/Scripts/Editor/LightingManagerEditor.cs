using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Code here is a modified version of code written here:
// Live: https://gist.github.com/bzgeb/3800350
// Archived: https://archive.is/Sc6kY

[CustomEditor(typeof(LightingManager))]
public class LightingManagerEditor : Editor
{
    private SerializedObject obj;

    public void OnEnable ()
    {
        obj = new SerializedObject (target);
    }
 
    public override void OnInspectorGUI ()
    {
        if (DropAreaGUI("Drop Lit Materials Here", out var litMaterials))
        {
            // Populate lit materials
            PopulateMaterials(litMaterials, isLit: true);
        }
        
        EditorGUILayout.Space();
        
        if (DropAreaGUI("Drop Unlit Materials Here", out var unlitMaterials))
        {
            // Populate unlit materials
            PopulateMaterials(unlitMaterials, isLit: false);
        }
        
        EditorGUILayout.Space ();

        DrawDefaultInspector ();
    }

    void PopulateMaterials(Object[] materialObjects, bool isLit)
    {
        var lighting = target as LightingManager;
        if (lighting == null)
        {
            return;
        }
        
        var materials = new List<Material>();
        for (int i = 0; i < materialObjects.Length; i++)
        {
            var material = materialObjects[i] as Material;
            if (material == null)
            {
                continue;
            }
            
            materials.Add(material);
        }
        
        lighting.SetMaterials(materials.ToArray(), isLit);
    }
    
    public bool DropAreaGUI (string text, out Object[] droppedObjs)
    {
        droppedObjs = null;
        
        Event evt = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect (0.0f, 50.0f, GUILayout.ExpandWidth (true));
        GUI.Box (drop_area, text);
     
        switch (evt.type) {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains (evt.mousePosition))
                    return false;
             
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
         
                if (evt.type == EventType.DragPerform) {
                    DragAndDrop.AcceptDrag ();
                    droppedObjs = DragAndDrop.objectReferences;
                    return true;
                }
                break;
        }

        return false;
    }
}
