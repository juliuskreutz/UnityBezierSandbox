using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] public Child child;

    public void CreateChild()
    {
        var childGameObject = Instantiate(tilePrefab, transform.position, Quaternion.identity);
        childGameObject.name = "Tile";
        childGameObject.transform.parent = transform.parent;
        var tile = childGameObject.GetComponent<Tile>();

        child = new Child { tile = tile, startTangent = transform.position, endTangent = transform.position };
    }

    [Serializable]
    public class Child
    {
        public Tile tile;
        public Vector3 startTangent;
        public Vector3 endTangent;
    }
}