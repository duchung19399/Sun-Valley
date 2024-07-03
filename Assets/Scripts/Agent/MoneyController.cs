using FarmGame.UI;
using UnityEngine;

namespace FarmGame.Agent {
    public class MoneyController : MonoBehaviour {
        [SerializeField]
        private AgentDataSO _agentData;

        [SerializeField]
        private MoneyUI _moneyUI;

        private void OnEnable() {
            if (_agentData == null || _moneyUI == null) return;
            _agentData.OnDataUpdated += UpdateMoneyUI;
        }

        private void Start() {
            if (_agentData == null || _moneyUI == null) return;
            UpdateMoneyUI(_agentData);
        }

        private void UpdateMoneyUI(AgentDataSO sO) {
            _moneyUI.UpdateMoney(sO.Money);
        }

        private void OnDisable() {
            if (_agentData == null || _moneyUI == null) return;
            _agentData.OnDataUpdated -= UpdateMoneyUI;
        }
    }
}
