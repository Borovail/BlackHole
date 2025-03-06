using UnityEngine;

namespace Assets.Scripts
{
    public class GravitationField : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IMoveable>(out var movable))
                movable.EnterGravitationField();
        }
    }
}