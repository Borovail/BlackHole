using System.IO;
using UnityEditor;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class FolderPath : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(FolderPath))]
public class FolderPathDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            Rect textFieldPosition = new Rect(position.x, position.y, position.width - 30, position.height);
            property.stringValue = EditorGUI.TextField(textFieldPosition, label, property.stringValue);

            Rect buttonPosition = new Rect(position.x + position.width - 50, position.y, 50, position.height);
            if (GUI.Button(buttonPosition, "Select"))
            {
                string path = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");
                if (!string.IsNullOrEmpty(path))
                {
                    property.stringValue = Path.GetRelativePath(Application.dataPath, path);
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use [FolderPath] with string.");
        }
    }
}