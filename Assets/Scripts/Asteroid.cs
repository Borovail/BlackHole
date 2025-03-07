using Assets.Scripts;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public enum AsteroidState { MovingToCenter, GravitationalPull, InAsteroidBelt, Orbiting, None }

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour,IMoveable
{
    [SerializeField] private float _orbitSpeed = 2;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _gravityForce = 1f;
    [SerializeField] private float _transitionSpeed = 2f;
    [SerializeField] private float _rotationSpeed = 200f;

    private Rigidbody2D _rigidbody;
    private AsteroidState _state;
    private Transform _asteroidBelt;

    private float _orbitAngle;
    public float MinOrbitRadius { private get; set; } = 3f;// Минимальный радиус орбиты
    public float MaxOrbitRadius { private get; set; } = 5f; // Максимальный радиус орбиты
    private float _randomRadius;          // Радиус орбиты для каждого астероида

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0;
        _state = AsteroidState.None;
    }

    private void Start()
    {
        _randomRadius = Random.Range(MinOrbitRadius, MaxOrbitRadius);
        _orbitAngle = Random.Range(0f, 360f);  // Случайный начальный угол

        _orbitSpeed += Random.Range(0.5f, 1.5f); 
    }

    private void Update()
    {
        switch (_state)
        {
            case AsteroidState.MovingToCenter:
                MoveTowardsCenter();
                break;
            case AsteroidState.GravitationalPull:
                ApplyGravitationalPull();
                break;
            case AsteroidState.InAsteroidBelt:
                InAsteroidBelt();
                break;
            case AsteroidState.Orbiting:
                OrbitPlanet();
                break;
        }

        _rigidbody.angularVelocity = _rotationSpeed;
    }

    private void MoveTowardsCenter()
    {
        Vector2 direction = (Vector3.zero - transform.position).normalized;
        _rigidbody.linearVelocity = direction * _moveSpeed;
    }

    private void ApplyGravitationalPull()
    {
        Vector2 directionToCenter = (Vector3.zero - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, Vector3.zero);

        // Рассчитываем силу притяжения, пропорциональную квадрату расстояния
        float gravity = _gravityForce / (distance);

        // Гравитационное ускорение
        Vector2 gravityForceVector = directionToCenter * gravity;

        // Обновляем скорость непосредственно, без использования Lerp
        _rigidbody.linearVelocity += gravityForceVector * Time.deltaTime;
    }

    private void InAsteroidBelt()
    {
        Vector2 directionToCenter = (_asteroidBelt.position - transform.position).normalized;
        Vector2 desiredVelocity = Vector2.Perpendicular(directionToCenter) * _orbitSpeed;

        _rigidbody.linearVelocity = Vector2.Lerp(_rigidbody.linearVelocity, desiredVelocity, _transitionSpeed * Time.deltaTime);

        if (Vector2.Angle(_rigidbody.linearVelocity, desiredVelocity) < 5f)
        {
            _state = AsteroidState.Orbiting;
        }
    }

    private void OrbitPlanet()
    {
        _orbitAngle += _orbitSpeed * Time.deltaTime;

        float x = _asteroidBelt.position.x + Mathf.Cos(_orbitAngle) * _randomRadius;
        float y = _asteroidBelt.position.y + Mathf.Sin(_orbitAngle) * _randomRadius;

        transform.position = new Vector3(x, y, transform.position.z);
    }

    public void MoveCenter()
    {
        _state = AsteroidState.MovingToCenter;
    }

    public void EnterGravitationField()
    {
        _state = AsteroidState.GravitationalPull;
    }

    public void EnterAsteroidBelt(Transform asteroidBelt)
    {
        _state = AsteroidState.InAsteroidBelt;
        _asteroidBelt = asteroidBelt;
    }
}
