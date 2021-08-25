using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CustomEditor(typeof(OffMeshLinkBox))]
public class OffMeshLinBoxEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Links"))
        {
            OffMeshLinkBox offMeshBox = target as OffMeshLinkBox;
            offMeshBox.GenerateOffMeshLinks();

            Debug.Log("Created OffMeshLinks");
        }
        var labelStyle = EditorStyles.label;
        labelStyle.wordWrap = true;
        GUILayout.Label("Be careful with this Clear, it destroys all children object attached to this object", labelStyle);

        if (GUILayout.Button("Clear Links"))
        {
            OffMeshLinkBox offMeshBox = target as OffMeshLinkBox;
            Transform transform = offMeshBox.transform;

            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            Debug.Log("Deleted Children objects");
        }
    }
}
