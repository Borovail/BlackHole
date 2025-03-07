using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Planet : MonoBehaviour, IMoveable
    {
        [SerializeField] private float _gravityForce = 1f;
        [SerializeField] private float _orbitSpeed = 50f;
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