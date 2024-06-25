using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.DataStorage;
using FarmGame.Interact;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FarmGame.Farming {
    public class FieldRenderer : MonoBehaviour {
        [SerializeField]
        private Tilemap _preparedFieldTilemap;


        [SerializeField]
        private TileBase _preparedFieldTile, _wateredFieldTile;

        Dictionary<Vector3Int, GameObject> _cropVisualRepresentation = new();
        [SerializeField]
        private GameObject _cropPrefab;

        private void Awake() {
            Debug.Assert(_preparedFieldTilemap != null, "FieldRenderer: preparedFieldTilemap is not set.");
        }

        public Vector3Int GetTilemapTilePosition(Vector3 worldPosition)
        => _preparedFieldTilemap.WorldToCell(worldPosition);

        public void PrepareFieldAt(Vector3Int tilePostion, bool watered = false) {
            TileBase tile = watered ? _wateredFieldTile : _preparedFieldTile;
            _preparedFieldTilemap.SetTile(tilePostion, tile);
        }

        public void CreateCropVisualization(Vector3Int tilePosition, Sprite cropSprite, bool changeLayerOrder = false) {
            _cropVisualRepresentation[tilePosition] = Instantiate(_cropPrefab);
            _cropVisualRepresentation[tilePosition].transform.position = tilePosition + new Vector3(0.5f, 0.5f, 0);
            UpdateCropVisualization(tilePosition, cropSprite, changeLayerOrder);
        }

        public void UpdateCropVisualization(Vector3Int tilePosition, Sprite cropSprite, bool changeLayerOrder) {
            CropRenderer cropRenderer = _cropVisualRepresentation[tilePosition].GetComponent<CropRenderer>();
            cropRenderer.SetSprite(cropSprite);
            if (changeLayerOrder) {
                cropRenderer.ChangeLayerOrder();
            }
        }

        public void WiltCropVisualization(Vector3Int position) {
            if (_cropVisualRepresentation[position] != null) {
                _cropVisualRepresentation[position].GetComponent<CropRenderer>().WiltCrop();
                if (_cropVisualRepresentation[position].TryGetComponent<PickUpInteraction>(out PickUpInteraction pickUpInteraction)) {
                    Destroy(pickUpInteraction.gameObject);
                }
            } else {
                Debug.LogError("No crop visual representation found at position " + position, gameObject);
            }
        }

        public PickUpInteraction MakeCropCollectable(Vector3Int position, CropData data, int quantity, ItemDatabaseSO itemDatabase) {
            GameObject cropObject = _cropVisualRepresentation[position];
            ItemInfo itemInfo = cropObject.AddComponent<ItemInfo>();
            itemInfo.itemDatabaseIndex = data.ProducedItemId;
            itemInfo.itemCount = data.ProducedCount;
            itemInfo.itemQuality = quantity;

            PickUpInteraction pickUpInteraction = cropObject.AddComponent<PickUpInteraction>();
            pickUpInteraction.ItemDatabase = itemDatabase;
            pickUpInteraction.UsableTools = data.GetCollectTools;
            pickUpInteraction.OnPickUp = new();
            return pickUpInteraction;
        }

        public void ClearPreparedField() {
            _preparedFieldTilemap.ClearAllTiles();
        }

        public void RemoveCropAt(Vector3Int position) {
            if (_cropVisualRepresentation.ContainsKey(position)) {
                Destroy(_cropVisualRepresentation[position]);
            }
            _cropVisualRepresentation.Remove(position);
        }
    }

}
