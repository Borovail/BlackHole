using UnityEngine;
using static Unity.Collections.AllocatorManager;

namespace Assets.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private Collider2D _blackHole;
        [SerializeField] private Collider2D _gravitationField;

        [SerializeField] private float _blackHoleAteAsteroidBeltThreshold;
        [SerializeField] private float _blackHoleAteLastPlanetThreshold;

        private void Start()
        {
            Invoke(nameof(EnableBlackHole), 1f);
        }

        private void EnableBlackHole()
        {
            _blackHole.enabled = true;
            _gravitationField.enabled = true;
        }

        private void Update()
        {
            if (_blackHole.transform.localScale.x >= _blackHoleAteAsteroidBeltThreshold && _blackHole.transform.localScale.x < _blackHoleAteLastPlanetThreshold)
            {
                Debug.Log("Increase camera size");
                AudioManager.Instance.PlaySfx(AudioManager.Instance.NextStage);
                //Increase Camera size
            }
            else if (_blackHole.transform.localScale.x >= _blackHoleAteLastPlanetThreshold)
            {
                Debug.Log("The end");
                //End game
            }
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _blackHoleAteAsteroidBeltThreshold);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _blackHoleAteLastPlanetThreshold);
        }

    }
}