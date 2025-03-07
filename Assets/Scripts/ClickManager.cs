﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

namespace Assets.Scripts
{
    public class ClickManager : MonoBehaviour
    {
        [SerializeField] private LayerMask _asteroidLayer;
        [SerializeField] private Asteroid _asteroidPrefab;

        private Planet selectedPlanet;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, _asteroidLayer);

                selectedPlanet?.OnDeselected();

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out selectedPlanet))
                        selectedPlanet.OnSelected();
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