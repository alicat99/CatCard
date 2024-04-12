using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridCardTemplate))]
public class GridCardTemplateEditor : Editor
{
    private SerializedProperty dataProperty;

    private void OnEnable()
    {
        dataProperty = serializedObject.FindProperty("data");
        for (int i = 9 - dataProperty.arraySize; i > 0; i--)
        {
            dataProperty.InsertArrayElementAtIndex(0);
        }
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw the grid for datas
        int rows = 3;
        int columns = 3;
        for (int row = 0; row < rows; row++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int col = 0; col < columns; col++)
            {
                EditorGUILayout.PropertyField(dataProperty.GetArrayElementAtIndex(row * columns + col), GUIContent.none);
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
