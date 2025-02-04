using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tile))]
[CanEditMultipleObjects]
public class TileHandle : Editor
{
    private int _selectedPath;

    public override void OnInspectorGUI()
    {
        var tile = (Tile)target;

        DrawDefaultInspector();

        if (!tile.child.tile)
        {
            if (GUILayout.Button("Add child"))
            {
                tile.CreateChild();
            }

            return;
        }

        if (tile.child.path == null)
        {
            if (!GUILayout.Button("Add path")) return;
            
            tile.child.path = new Tile.Straight();
            SceneView.RepaintAll();

            return;
        }

        _selectedPath = tile.child.path switch
        {
            Tile.Straight => 0,
            Tile.QBezier => 1,
            Tile.CBezier => 2,
            _ => 0
        };

        var oldSelectedPath = _selectedPath;

        string[] options = { "Straight", "Quadratic Bezier", "Cubic Bezier" };

        _selectedPath = EditorGUILayout.Popup(
            "Path Type",
            _selectedPath,
            options
        );

        if (oldSelectedPath != _selectedPath)
        {
            tile.child.path = _selectedPath switch
            {
                0 => new Tile.Straight(),
                1 => new Tile.QBezier()
                    { tangent = Vector3.Lerp(tile.transform.position, tile.child.tile.transform.position, 1f / 2f) },
                2 => new Tile.CBezier()
                {
                    startTangent = Vector3.Lerp(tile.transform.position, tile.child.tile.transform.position, 1f / 3f),
                    endTangent = Vector3.Lerp(tile.transform.position, tile.child.tile.transform.position,
                        2f / 3f)
                },
                _ => tile.child.path
            };
        }

        if (!tile.child.tile || tile.child.path is Tile.Straight || !GUILayout.Button("Reset Path")) return;
        
        switch (tile.child.path)
        {
            case Tile.QBezier path:
                path.tangent = Vector3.Lerp(tile.transform.position, tile.child.tile.transform.position, 1f / 2f);
                break;
            case Tile.CBezier path:
                path.startTangent =
                    Vector3.Lerp(tile.transform.position, tile.child.tile.transform.position, 1f / 3f);
                path.endTangent = Vector3.Lerp(tile.transform.position, tile.child.tile.transform.position,
                    2f / 3f);
                break;
        }

        SceneView.RepaintAll();
    }

    private void OnSceneGUI()
    {
        var tile = (Tile)target;

        tile.transform.position = Handles.PositionHandle(tile.transform.position, Quaternion.identity);

        if (!tile.child.tile) return;

        tile.child.tile.transform.position =
            Handles.PositionHandle(tile.child.tile.transform.position, Quaternion.identity);

        switch (tile.child.path)
        {
            case Tile.Straight _:
                Handles.DrawBezier(tile.transform.position, tile.child.tile.transform.position, tile.transform.position,
                    tile.child.tile.transform.position, Color.red, null, 5f);
                break;
            case Tile.QBezier path:
                path.tangent = Handles.PositionHandle(path.tangent, Quaternion.identity);

                Handles.DrawBezier(tile.transform.position, tile.child.tile.transform.position, path.tangent,
                    path.tangent, Color.red, null, 5f);
                break;
            case Tile.CBezier path:
                path.startTangent = Handles.PositionHandle(path.startTangent, Quaternion.identity);
                path.endTangent = Handles.PositionHandle(path.endTangent, Quaternion.identity);

                Handles.DrawBezier(tile.transform.position, tile.child.tile.transform.position, path.startTangent,
                    path.endTangent, Color.red, null, 5f);
                break;
        }
    }
}