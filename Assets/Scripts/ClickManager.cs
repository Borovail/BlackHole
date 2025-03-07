using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Assets.Scripts
{
    public class ClickManager : MonoBehaviour
    {
        [SerializeField] private LayerMask _asteroidLayer;
        [SerializeField] private Asteroid _asteroidPrefab;

        private AsteroidBelt selectedSAsteroidBelt;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, _asteroidLayer);

                selectedSAsteroidBelt?.OnDeselected();

                if (hit.collider != null)
                {

                    if (hit.collider.TryGetComponent(out selectedSAsteroidBelt))
                        selectedSAsteroidBelt.OnSelected();
                    if (hit.collider.TryGetComponent<Asteroid>(out var asteroid))
                        asteroid.MoveCenter();
                }

            }
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.W))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var asteroid = Instantiate(_asteroidPrefab, worldPoint, Quaternion.identity);
                if (Input.GetKeyDown(KeyCode.W))
                    asteroid.MoveCenter();
            }
#endif
        }
    }
}