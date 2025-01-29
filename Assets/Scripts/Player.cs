using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Tile tile;
    [SerializeField] private float speed;
    
    private bool _moving = true;
    private float _progress;

    private void Update()
    {
        if (!_moving) return;

        _progress += (speed * Time.deltaTime) /
                     Vector3.Distance(tile.transform.position, tile.child.tile.transform.position);

        if (_progress > 1)
        {
            _progress -= 1;
            tile = tile.child.tile;
            transform.position = tile.transform.position;

            if (!tile.child.tile)
            {
                _moving = false;
                _progress = 0;
                return;
            }
        }

        transform.position = GetBezierPoint(_progress);
    }

    private Vector3 GetBezierPoint(float t)
    {
        var point = Mathf.Pow(1 - t, 3) * tile.transform.position +
                    3 * Mathf.Pow(1 - t, 2) * t * tile.child.startTangent +
                    3 * (1 - t) * Mathf.Pow(t, 2) * tile.child.endTangent +
                    Mathf.Pow(t, 3) * tile.child.tile.transform.position;

        return point;
    }
}