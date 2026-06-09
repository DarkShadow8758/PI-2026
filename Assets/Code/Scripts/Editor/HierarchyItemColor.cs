using UnityEngine;
using UnityEditor;
 
[InitializeOnLoad]
public static class HierarchyItemColor
{
    static HierarchyItemColor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarcheWindowItemOnGUI;
    }
 
    static void HierarcheWindowItemOnGUI(int instanceID, Rect selectRect)
    {
        var gameobject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if(gameobject != null && gameobject.name.StartsWith("/",System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectRect, Color.black);
            EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/", "").ToUpperInvariant());
        }
    }
}
 