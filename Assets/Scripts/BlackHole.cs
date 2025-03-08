using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.localScale += new Vector3(0.02f, 0.02f);
        Destroy(collision.gameObject);
        Coins.Credit++;
        AudioManager.Instance.PlaySfx(AudioManager.Instance.BlackHole);
    }
}
