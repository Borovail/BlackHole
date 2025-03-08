using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Planet : MonoBehaviour, IMoveable
    {
        [SerializeField] private float _gravityForce = 50f;
        [SerializeField] private float _orbitSpeed = 20f;
        [SerializeField] private float _selectedTransparency = 150f;
   

        private AsteroidBelt _asteroidBelt;
        private SpriteRenderer _renderer;

        private AsteroidState _state;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _asteroidBelt = GetComponentInChildren<AsteroidBelt>();

            _rigidbody.gravityScale = 0;
            _state = AsteroidState.Orbiting;

            float distanceToCenter = Vector2.Distance(transform.position, Vector3.zero);
            float distanceFactor = Mathf.Lerp(1f, 1.3f, Mathf.Clamp01(Mathf.InverseLerp(0, 10, distanceToCenter)));
            float randomSpeedIncrease = Random.Range(0, Mathf.Lerp(10, 20, distanceFactor));

            _orbitSpeed += randomSpeedIncrease;
        }

        public void EnterGravitationField()
        {
            _state = AsteroidState.GravitationalPull;
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

        private void OrbitPlanet()
        {
            transform.RotateAround(Vector3.zero, Vector3.forward, _orbitSpeed * Time.deltaTime);
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



        public void OnSelected()
        {
            var color = _renderer.color;
            color.a = _selectedTransparency / 255f;
            _renderer.color = color;
            _asteroidBelt.OnSelected();
        }

        public void OnDeselected()
        {
            var color = _renderer.color;
            color.a = 1;
            _renderer.color = color;

            _asteroidBelt.OnDeselected();
        }


    
    }
}