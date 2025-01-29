using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tile))]
[CanEditMultipleObjects]
public class TileHandle : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var tile = (Tile)target;

        if (!tile.child.tile && GUILayout.Button("+"))
        {
            tile.CreateChild();
        }

        if (tile.child.tile && GUILayout.Button("Reset Curves"))
        {
            tile.child.startTangent =
                Vector3.Lerp(tile.transform.position, tile.child.tile.transform.position, 1f / 3f);
            tile.child.endTangent = Vector3.Lerp(tile.transform.position, tile.child.tile.transform.position, 2f / 3f);
        }
    }

    private void OnSceneGUI()
    {
        var tile = (Tile)target;

        if (!tile.child.tile) return;

        tile.child.startTangent = Handles.PositionHandle(tile.child.startTangent, Quaternion.identity);
        tile.child.endTangent = Handles.PositionHandle(tile.child.endTangent, Quaternion.identity);

        Handles.DrawBezier(tile.transform.position, tile.child.tile.transform.position, tile.child.startTangent,
            tile.child.endTangent, Color.red, null, 5f);
    }
}