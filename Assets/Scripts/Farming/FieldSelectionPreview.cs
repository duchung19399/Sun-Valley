using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using UnityEngine;

namespace FarmGame.Farming {
    public class FieldSelectionPreview : MonoBehaviour {
        [SerializeField]
        private GameObject selectionIndicatorPrefab;

        private List<GameObject> selectionIcons = new();

        private FieldDetector fieldDetector;

        private void Awake() {
            fieldDetector = FindObjectOfType<Player>().GetComponentInChildren<FieldDetector>();

            if (fieldDetector == null) {
                Debug.LogError("FieldSelectionPreview: FieldDetector not found.", gameObject);
                return;
            }


            fieldDetector.OnFieldExited += HideAllIcons;
            fieldDetector.OnResetDetectedFields += HideAllIcons;
            fieldDetector.OnPositionDectected += UpdateIcons;

        }

        public void HideAllIcons() {
            foreach (GameObject icon in selectionIcons) {
                icon.SetActive(false);
            }
        }

        public void UpdateIcons(IEnumerable<Vector2> positions) {
            HideAllIcons();
            ShowIcons(positions);
        }

        public void ShowIcons(IEnumerable<Vector2> positions) {
            int index = 0;
            foreach (Vector2 position in positions) {

                if(selectionIcons.Count <= index) {
                    selectionIcons.Add(Instantiate(selectionIndicatorPrefab));
                }

                selectionIcons[index].transform.position = position;
                selectionIcons[index].transform.rotation = Quaternion.identity;
                selectionIcons[index].SetActive(true);
                index++;
            }
        }

        private void OnDisable() {
            fieldDetector.OnFieldExited -= HideAllIcons;
            fieldDetector.OnResetDetectedFields -= HideAllIcons;
            fieldDetector.OnPositionDectected -= UpdateIcons;
        }
    }
}
