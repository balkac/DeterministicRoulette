using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WheelController))]
public class WheelControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WheelController wheelController = (WheelController)target;
        if (GUILayout.Button("Set Wheel Number Data"))
        {
            wheelController.SetWheelNumberData();
            EditorUtility.SetDirty(wheelController);
        }
    }
}