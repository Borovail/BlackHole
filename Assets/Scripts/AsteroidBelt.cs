using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class AsteroidBelt : MonoBehaviour
{
    [SerializeField] private Asteroid _asteroidPrefab;
    [SerializeField] private Button _automateButton;
    [SerializeField] private Text _automatePriceText;
    [SerializeField] private int _priceToAutomate;
    [SerializeField] private float _generateAsteroidCooldown;
    [SerializeField] private float _shootAsteroidCooldown;


    private List<Asteroid> _asteroids;
    private RectTransform _automateButtonRectTransform;
    private RectTransform _automateTextRectTransform;

    private SpriteRenderer _renderer;
    private float _initialTransparency;
    private bool _auto;
    private float _generateAsteroidTimer;
    private float _shootAsteroidTimer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _automateButtonRectTransform = _automateButton.GetComponent<RectTransform>();
        _automateTextRectTransform = _automatePriceText.GetComponent<RectTransform>();
        _asteroids = new List<Asteroid>();

        _initialTransparency = _renderer.color.a;
        _generateAsteroidTimer = _generateAsteroidCooldown;
        _shootAsteroidTimer = _shootAsteroidCooldown;
    }


    private void Update()
    {
        _generateAsteroidTimer -= Time.deltaTime;
        if (_generateAsteroidTimer < 0)
        {
            _generateAsteroidTimer = _generateAsteroidCooldown;
            Instantiate(_asteroidPrefab, transform.position + Vector3.right * 2, Quaternion.identity);
        }

        if (_auto)
        {
            _shootAsteroidTimer -= Time.deltaTime;

            if (_shootAsteroidTimer < 0 && _asteroids.Count > 0)
            {
                _shootAsteroidTimer = _shootAsteroidCooldown;
                var asteroid = _asteroids.FirstOrDefault(asteroid => asteroid.enteredBelt);
                if (asteroid != null)
                {
                    asteroid.MoveCenter();
                    _asteroids.Remove(asteroid); 
                }
            }
        }

    }

    private void ChangeUiObjectPositionToLocalObjectPosition(RectTransform uiObject, Vector3 offset)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + offset);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiObject.parent.GetComponent<RectTransform>(), screenPosition, Camera.main, out var localPosition);
        uiObject.localPosition = localPosition;
    }

    public void OnSelected()
    {
        if (_auto) return;

        ChangeUiObjectPositionToLocalObjectPosition(_automateButtonRectTransform, Vector2.up * 3f);
        ChangeUiObjectPositionToLocalObjectPosition(_automateTextRectTransform, Vector2.up * 4f);

        _automateButton.gameObject.SetActive(true);
        _automatePriceText.text = _priceToAutomate.ToString();

        var color = _renderer.color;
        color.a = 100f / 255f;
        _renderer.color = color;

        if (Coins.Credit - _priceToAutomate >= 0)
        {
            _automateButton.interactable = true;
            _automatePriceText.color = new Color(Color.green.r, Color.green.g, Color.green.b, _automatePriceText.color.a);
            _automateButton.onClick.AddListener(OnAutomateButtonClicked);
        }
        else
        {
            _automateButton.interactable = false;
            _automatePriceText.color = new Color(Color.red.r, Color.red.g, Color.red.b, _automatePriceText.color.a);
        }
    }

    private void OnAutomateButtonClicked()
    {
        _automateButton.gameObject.SetActive(false);
        Coins.Credit -= _priceToAutomate;
        _auto = true;
        var color = _renderer.color;
        color.a = 200f / 255f;
        _renderer.color = color;
    }

    public void OnDeselected()
    {
        if (!_auto)
        {
            var color = _renderer.color;
            color.a = _initialTransparency;
            _renderer.color = color;

            _automateButton.gameObject.SetActive(false);
        }

        _automateButton.onClick.RemoveListener(OnAutomateButtonClicked);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        var asteroid = other.gameObject.GetComponent<Asteroid>();
        if(asteroid.enteredBelt) return;
        asteroid.EnterAsteroidBelt(transform);
        asteroid.enteredBelt = true;
        _asteroids.Add(asteroid);
    }

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    var asteroid = other.gameObject.GetComponent<Asteroid>();
    //    _asteroids.Add(asteroid);
    //    Debug.Log(asteroid.name + "Exit");
    //}
}
