using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : MonoBehaviour, IMoveable
    {
        [SerializeField] private float _orbitSpeed = 2;
        [SerializeField] private float _shootAsteroidCooldown = 2f;   // Охлаждение для стрельбы астероидом
        [SerializeField] private float _gravityForce = 10f;

        private float _orbitAngle;
        private float _shootAsteroidTimer;

        private Transform _asteroidBelt;
        private Vector2 _minMaxRadius;
        private AsteroidState _state;

        // Сохранение уникального отклонения для каждого корабля
        private float _randomAngleOffset;
        private float _randomPatternSpeed;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _state = AsteroidState.Orbiting;
            _shootAsteroidTimer = _shootAsteroidCooldown;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetData(Transform asteroidBelt, Vector2 minMaxRadius)
        {
      
            _asteroidBelt = asteroidBelt;
            _minMaxRadius = minMaxRadius;

            // Генерируем случайное отклонение для этого корабля
            _randomAngleOffset = Random.Range(-Mathf.PI, Mathf.PI); // Уникальный угол для каждого корабля
            _randomPatternSpeed = Random.Range(0.5f, 2f); // Разная скорость паттерна для каждого корабля
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_shootAsteroidTimer > 0) return;

            other.GetComponent<Asteroid>().MoveCenter();
            _shootAsteroidTimer = _shootAsteroidCooldown;
        }

        private void Update()
        {
            switch (_state)
            {
                case AsteroidState.GravitationalPull:
                    InGravitationField();
                    break;
                case AsteroidState.Orbiting:
                    OrbitPlanet();
                    break;
            }

        }

        private void InGravitationField()
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

        private void OrbitPlanet()
        {
            _shootAsteroidTimer -= Time.deltaTime;
            _orbitAngle += _orbitSpeed * Time.deltaTime;

            // Добавляем случайное отклонение для каждого корабля
            // Визуализация случайного паттерна движения
            float angleOffset = Mathf.Sin(_orbitAngle * _randomPatternSpeed + _randomAngleOffset) * 0.5f;

            // Рассчитываем радиус в зависимости от расстояния от центра
            float currentRadius = Mathf.Lerp(_minMaxRadius.x, _minMaxRadius.y, Mathf.PingPong(Time.time * 0.2f, 1));

            // Вычисляем новые X и Y координаты с учетом углового отклонения
            float x = _asteroidBelt.position.x + Mathf.Cos(_orbitAngle + angleOffset) * currentRadius;
            float y = _asteroidBelt.position.y + Mathf.Sin(_orbitAngle + angleOffset) * currentRadius;

            // Устанавливаем новую позицию объекта
            transform.position = new Vector3(x, y, transform.position.z);
        }


        public void EnterGravitationField()
        {
            _state = AsteroidState.GravitationalPull;
        }
    }
}
