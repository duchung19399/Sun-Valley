using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FarmGame.Farming {
    public class FieldRenderer : MonoBehaviour {
        [SerializeField]
        private Tilemap preparedFieldTilemap;

        [SerializeField]
        private TileBase preparedFieldTile;

        Dictionary<Vector3Int, GameObject> _cropVisualRepresentation = new();
        [SerializeField]
        private GameObject _cropPrefab;

        private void Awake() {
            Debug.Assert(preparedFieldTilemap != null, "FieldRenderer: preparedFieldTilemap is not set.");
        }

        public Vector3Int GetTilemapTilePosition(Vector3 worldPosition)
        => preparedFieldTilemap.WorldToCell(worldPosition);

        public void PrepareFieldAtPosition(Vector3Int tilePostion) {
            preparedFieldTilemap.SetTile(tilePostion, preparedFieldTile);
        }

        public void CreateCropVisualization(Vector3Int tilePosition, Sprite cropSprite, bool changeLayerOrder = false) {
            _cropVisualRepresentation[tilePosition] = Instantiate(_cropPrefab);
            _cropVisualRepresentation[tilePosition].transform.position = tilePosition + new Vector3(0.5f, 0.5f, 0);
            UpdateCropVisualization(tilePosition, cropSprite, changeLayerOrder);
        }

        private void UpdateCropVisualization(Vector3Int tilePosition, Sprite cropSprite, bool changeLayerOrder) {
            CropRenderer cropRenderer = _cropVisualRepresentation[tilePosition].GetComponent<CropRenderer>();
            cropRenderer.SetSprite(cropSprite);
            if(changeLayerOrder) {
                cropRenderer.ChangeLayerOrder();
            }
        }
    }

}
