using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.localScale += new Vector3(0.1f, 0.1f);
        Destroy(collision.gameObject);
    }
}
