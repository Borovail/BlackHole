using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidBelt : MonoBehaviour
{
    [SerializeField, FolderPath] private string _pathToSpritesFolder;

    [SerializeField] private SpaceShip _spaceShipPrefab;
    [SerializeField] private Asteroid _asteroidPrefab;
    [SerializeField] private int _priceToAutomate;

    [SerializeField] private int _initialAsteroidsCount = 10;
    [SerializeField] private int _maxUpgradeLevel = 5;
    [SerializeField] private float _generateAsteroidCooldown = 2;
    [SerializeField, Tooltip("Blue sphere")] private float _minOrbitRadius = 3f;
    [SerializeField, Tooltip("Red sphere")] private float _maxOrbitRadius = 5f;

    private List<Sprite> _asteroidSprites;


    private float _generateAsteroidTimer;
    private int _currentLevel = 1;

    private void Awake()
    {
        _generateAsteroidTimer = _generateAsteroidCooldown;

        _asteroidSprites = AssetLoader<Sprite>.LoadAllAssets(_pathToSpritesFolder, FileExtensions.Sprite);
    }

    private void Start()
    {
        SpawnAsteroids(_initialAsteroidsCount);
    }

    private void SpawnAsteroids(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            float radius = Random.Range(_minOrbitRadius, _maxOrbitRadius);

            var spawnPosition = new Vector2(
                transform.position.x + Mathf.Cos(angle) * radius,
                transform.position.y + Mathf.Sin(angle) * radius
            );

            var asteroid = Instantiate(_asteroidPrefab, spawnPosition, Quaternion.identity);
            asteroid.GetComponent<SpriteRenderer>().sprite = _asteroidSprites[Random.Range(0, _asteroidSprites.Count - 1)];
            //asteroid.GetComponent<Transform>().localScale *= Random.Range(1f, 2.5f);
            asteroid.MinOrbitRadius = _minOrbitRadius;
            asteroid.MaxOrbitRadius = _maxOrbitRadius;

        }
    }


    private void Update()
    {
        _generateAsteroidTimer -= Time.deltaTime;
        if (!(_generateAsteroidTimer < 0)) return;
        _generateAsteroidTimer = _generateAsteroidCooldown;
        SpawnAsteroids(1);
    }


    public void OnSelected()
    {
        UiManager.Instance.CurrentLevel.gameObject.SetActive(true);
        UiManager.Instance.AutomateUpgradeButton.gameObject.SetActive(true);
        UiManager.Instance.CurrentLevel.text = "Current level: " + _currentLevel;

        if (_currentLevel >= _maxUpgradeLevel)
        {
            UiManager.Instance.AutomateUpgradeButtonText.text = "MAX!!!";
            UiManager.Instance.AutomateUpgradeButtonText.color = Color.yellow;
            UiManager.Instance.AutomateUpgradeButton.interactable = false;
            return;
        }

        UiManager.Instance.AutomateUpgradeButtonText.text = _currentLevel == 1
            ? $"Automate: {_priceToAutomate}" : $"Upgrade: {_priceToAutomate}";

        if (Coins.Credit - _priceToAutomate >= 0)
        {
            UiManager.Instance.AutomateUpgradeButtonText.color = Color.green;
            UiManager.Instance.AutomateUpgradeButton.interactable = true;
            UiManager.Instance.AutomateUpgradeButton.onClick.AddListener(OnAutomateButtonClicked);
        }
        else
        {
            UiManager.Instance.AutomateUpgradeButton.interactable = false;
            UiManager.Instance.AutomateUpgradeButtonText.color = Color.red;
        }
    }

    private void OnAutomateButtonClicked()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.Automation);
        Coins.Credit -= _priceToAutomate;
        _currentLevel++;
        _priceToAutomate *= _currentLevel;
        _generateAsteroidCooldown /= _currentLevel;

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        float radius = Random.Range(_minOrbitRadius, _maxOrbitRadius);

        var spawnPosition = new Vector2(
            transform.position.x + Mathf.Cos(angle) * radius,
            transform.position.y + Mathf.Sin(angle) * radius
        );

        for (int i = 0; i < _currentLevel; i++)
            Instantiate(_spaceShipPrefab, spawnPosition, Quaternion.identity).SetData(transform, new Vector2(_minOrbitRadius, _maxOrbitRadius));

        OnDeselected();
        OnSelected();
    }

    public void OnDeselected()
    {
        UiManager.Instance.AutomateUpgradeButton.gameObject.SetActive(false);
        UiManager.Instance.CurrentLevel.gameObject.SetActive(false);

        UiManager.Instance.AutomateUpgradeButton.onClick.RemoveListener(OnAutomateButtonClicked);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<Asteroid>().EnterAsteroidBelt(transform);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue; // ÷вет внутренней границы
        Gizmos.DrawWireSphere(transform.position, _minOrbitRadius);

        Gizmos.color = Color.red; // ÷вет внешней границы
        Gizmos.DrawWireSphere(transform.position, _maxOrbitRadius);
    }
}
