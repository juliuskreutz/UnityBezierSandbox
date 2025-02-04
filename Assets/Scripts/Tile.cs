using System;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] public Child child;

    /// <summary>
    /// Should be called from an Editor Script. Creates a child tile with default values
    /// </summary>
    public void CreateChild()
    {
        var childGameObject = Instantiate(tilePrefab, transform.position, Quaternion.identity);
        childGameObject.name = "Tile";
        childGameObject.transform.parent = transform.parent;
        var tile = childGameObject.GetComponent<Tile>();

        child = new Child
        {
            tile = tile,
            path = new Straight()
        };
    }

    /// <summary>
    /// Evaluates the path position based on t
    /// </summary>
    /// <param name="t">Progress (0.0, 1.0)</param>
    /// <returns></returns>
    public Vector3 Position(float t)
    {
        return child.path.Position(transform.position, child.tile.transform.position, t);
    }

    [Serializable]
    public class Child
    {
        public Tile tile;
        [SerializeReference] public IPath path;
    }

    public interface IPath
    {
        public Vector3 Position(Vector3 start, Vector3 end, float t);
    }

    [Serializable]
    public class Straight : IPath
    {
        public Vector3 Position(Vector3 start, Vector3 end, float t)
        {
            return Vector3.Lerp(start, end, t);
        }
    }

    [Serializable]
    public class QBezier : IPath
    {
        public Vector3 tangent;

        public Vector3 Position(Vector3 start, Vector3 end, float t)
        {
            return Mathf.Pow(1 - t, 2) * start + 2 * (1 - t) * t * tangent + Mathf.Pow(t, 2) * end;
        }
    }

    [Serializable]
    public class CBezier : IPath
    {
        public Vector3 startTangent;
        public Vector3 endTangent;

        public Vector3 Position(Vector3 start, Vector3 end, float t)
        {
            return Mathf.Pow(1 - t, 3) * start +
                   3 * Mathf.Pow(1 - t, 2) * t * startTangent +
                   3 * (1 - t) * Mathf.Pow(t, 2) * endTangent +
                   Mathf.Pow(t, 3) * end;
        }
    }
}