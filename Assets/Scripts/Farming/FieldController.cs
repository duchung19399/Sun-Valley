using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Farming {
    public class FieldController : MonoBehaviour {
        private FieldRenderer fieldRenderer;
        private List<Vector3Int> preparedFieldTiles = new();

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
    private AudioClip prepareFieldSound;

        private void Awake() {
            fieldRenderer = FindObjectOfType<FieldRenderer>(true);
        }

        public void PrepareField(Vector3 worldPosition) {
            if (fieldRenderer == null) return;
            Vector3Int tilePosition = fieldRenderer.GetTilemapTilePosition(worldPosition);

            if (preparedFieldTiles.Contains(tilePosition)) return;

            fieldRenderer.PrepareFieldAtPosition(tilePosition);
            audioSource.PlayOneShot(prepareFieldSound);
            preparedFieldTiles.Add(tilePosition);

        }
    }
}
