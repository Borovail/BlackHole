using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UiManager : Singleton<UiManager>
    {
        [SerializeField] private Text _creditText;
        public Button AutomateUpgradeButton;
        public Text AutomateUpgradeButtonText;
        public Text CurrentLevel;

        private void Update()
        {
            _creditText.text = Coins.Credit.ToString();
        }
    }
}