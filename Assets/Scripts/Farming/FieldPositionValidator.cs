using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FarmGame.Farming {
    public class FieldPositionValidator : MonoBehaviour {
        [SerializeField]
        private Tilemap _fieldTilemap;
        [SerializeField]
        private string _fieldTileTag = "Field";

        private void Awake() {
            if (_fieldTilemap == null) {
                _fieldTilemap = FindObjectsOfType<Tilemap>().FirstOrDefault(tilemap => tilemap.CompareTag(_fieldTileTag));
            }
            Debug.Assert(_fieldTilemap != null, "FieldPositionValidator: No field tilemap found");
        }

        public bool IsItFieldTile(Vector2 position) {
            return _fieldTilemap.HasTile(_fieldTilemap.WorldToCell(position));
        }

        public List<Vector2> GetValidFieldTiles(List<Vector2> rawPositions) {
            List<Vector2> validPositions = new();
            foreach (var position in rawPositions) {
                Vector3Int tilemapPosition = _fieldTilemap.WorldToCell(position);
                if (_fieldTilemap.HasTile(tilemapPosition) != false)
                    validPositions.Add(_fieldTilemap.GetCellCenterWorld(tilemapPosition));
            }

            return validPositions;
        }

        public Vector2 GetValidFieldTile(Vector2 rawPosition) {
            return _fieldTilemap.GetCellCenterWorld(_fieldTilemap.WorldToCell(rawPosition));
        }
    }
}
