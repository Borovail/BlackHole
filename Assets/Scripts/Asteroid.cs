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
    [SerializeField] private float _minOrbitSpeed = 1f;
    [SerializeField] private float _rotationSpeed = 200f;

    private Rigidbody2D _rigidbody;
    private AsteroidState _state;
    private Vector3 _asteroidBeltCenter;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0;
        _state = AsteroidState.None;

        _orbitSpeed += Random.Range(1, 5);
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
        float gravity = _gravityForce / (distance * distance);

        Vector2 gravityForceVector = directionToCenter * gravity;
        _rigidbody.linearVelocity = Vector2.Lerp(_rigidbody.linearVelocity, _rigidbody.linearVelocity + gravityForceVector, Time.deltaTime);
    }

    private void InAsteroidBelt()
    {
        Vector2 directionToCenter = (_asteroidBeltCenter - transform.position).normalized;
        Vector2 desiredVelocity = Vector2.Perpendicular(directionToCenter) * _orbitSpeed;

        _rigidbody.linearVelocity = Vector2.Lerp(_rigidbody.linearVelocity, desiredVelocity, _transitionSpeed * Time.deltaTime);

        if (Vector2.Angle(_rigidbody.linearVelocity, desiredVelocity) < 5f)
        {
            _state = AsteroidState.Orbiting;
        }
    }

    private void OrbitPlanet()
    {
        Vector2 directionToCenter = (_asteroidBeltCenter - transform.position).normalized;
        Vector2 orbitDirection = Vector2.Perpendicular(directionToCenter);
        _rigidbody.linearVelocity = orbitDirection * _orbitSpeed ;
    }

    public void MoveCenter()
    {
        _state = AsteroidState.MovingToCenter;
    }

    public void EnterGravitationField()
    {
        _state = AsteroidState.GravitationalPull;
    }

    public void EnterAsteroidBelt(Vector3 center)
    {
        _state = AsteroidState.InAsteroidBelt;
        _asteroidBeltCenter = center;
    }
}
