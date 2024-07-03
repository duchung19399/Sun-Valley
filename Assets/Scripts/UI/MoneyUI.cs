using UnityEngine;

namespace FarmGame.UI {
    public class MoneyUI : MonoBehaviour {
        [SerializeField]
        private TMPro.TextMeshProUGUI _moneyText;
        public void UpdateMoney(int money) {
            _moneyText.text = money.ToString();
        }
    }
}
