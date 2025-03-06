using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Planet : MonoBehaviour, IMoveable
    {
        [SerializeField] private float _gravityForce = 1f;

        private Rigidbody2D _rigidbody;
        private bool flag;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
        }

        public void EnterGravitationField()
        {
            flag = true;
        }

        private void Update()
        {
            if (flag)
            {
                Vector2 directionToCenter = (Vector3.zero - transform.position).normalized;
                float distance = Vector2.Distance(transform.position, Vector3.zero);
                float gravity = _gravityForce / (distance * distance);

                Vector2 gravityForceVector = directionToCenter * gravity;
                _rigidbody.linearVelocity = Vector2.Lerp(_rigidbody.linearVelocity,
                    _rigidbody.linearVelocity + gravityForceVector, Time.deltaTime);
            }
        }
    }
}