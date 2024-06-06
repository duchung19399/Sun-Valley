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

        private void Awake() {
            Debug.Assert(preparedFieldTilemap != null, "FieldRenderer: preparedFieldTilemap is not set.");
        }

        public Vector3Int GetTilemapTilePosition(Vector3 worldPosition)
        => preparedFieldTilemap.WorldToCell(worldPosition);

        public void PrepareFieldAtPosition(Vector3Int tilePostion) {
            preparedFieldTilemap.SetTile(tilePostion, preparedFieldTile);
        }

    }
}
