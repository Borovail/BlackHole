using UnityEngine;

public class AsteroidBelt : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var asteroid = other.gameObject.GetComponent<Asteroid>();
        asteroid.EnterAsteroidBelt(transform.position);
    }
}
