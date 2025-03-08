using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ClickManager : MonoBehaviour
    {
        [SerializeField] private LayerMask _asteroidLayer;
        [SerializeField] private Asteroid _asteroidPrefab;
        [SerializeField] private RectTransform _buttonUpgrade;

        private Planet selectedPlanet;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (selectedPlanet != null)
                {
                    Vector2 localPoint = _buttonUpgrade.InverseTransformPoint(worldPoint);

                    if (_buttonUpgrade.rect.Contains(localPoint))
                        return;
                }

                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, _asteroidLayer);


                selectedPlanet?.OnDeselected();

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out selectedPlanet))
                        selectedPlanet.OnSelected();
                    if (hit.collider.TryGetComponent<Asteroid>(out var asteroid))
                        asteroid.MoveCenter();

                    AudioManager.Instance.PlaySfx(AudioManager.Instance.Click);
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