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

        _progress += speed * Time.deltaTime;

        // wrap progress back or stop moving depending on if the tile has a child
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

        transform.position = tile.Position(_progress);
    }
}