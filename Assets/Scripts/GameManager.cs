using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

namespace Assets.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private Collider2D _blackHole;
        [SerializeField] private Collider2D _gravitationField;

        [SerializeField] private float _blackHoleAteAsteroidBeltThreshold;
        [SerializeField] private float _blackHoleAteLastPlanetThreshold;

        [SerializeField] private GameObject _gameOverCanvas;
        [SerializeField] private Button _exitGameButton;


        private bool _secondStage;

        private void OnEnable()
        {
            _exitGameButton.onClick.AddListener(Application.Quit);
        }
        private void OnDisable()
        {
            _exitGameButton.onClick.RemoveListener(Application.Quit);
        }


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
            if (_blackHole.transform.localScale.x >= _blackHoleAteAsteroidBeltThreshold && !_secondStage && _blackHole.transform.localScale.x < _blackHoleAteLastPlanetThreshold)
            {
                AudioManager.Instance.PlaySfx(AudioManager.Instance.NextStage);
                CameraScroll.Instance.CurrentMaxSize = CameraScroll.Instance.SecondStageMax;
                _secondStage = true;
            }
            else if (_blackHole.transform.localScale.x >= _blackHoleAteLastPlanetThreshold && _secondStage)
            {
                Time.timeScale = 0f;
                _gameOverCanvas.SetActive(true);
                AudioManager.Instance.PlaySfx(AudioManager.Instance.WinSound);
                _secondStage = false;
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