using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private Text _creditText;

        private void Update()
        {
            _creditText.text = Coins.Credit.ToString();
        }
    }
}