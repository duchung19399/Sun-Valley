using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FarmGame.UI {
    public class GridLayoutScrollingUI : MonoBehaviour {
        [SerializeField]
        private RectTransform _gridLayoutTransform;
        [SerializeField]
        private ScrollRect _scrollRect;
        [SerializeField]
        int _numberOfRow = 0;

        private bool _gridReady = false;
        private float _movementStep = 0;

        [SerializeField]
        private InventoryRendererUI _inventoryRendererUI;

        private void PrepareScrolling() {
            DetectNumberOfRows();
            _movementStep = 1.0f / (_numberOfRow - 2.0f);
            _gridReady = true;
        }

        private void DetectNumberOfRows() {
            _numberOfRow = _gridLayoutTransform.childCount / _inventoryRendererUI.RowSize;
        }

        private Vector2Int GetGridPositionCooordinates(int index) {
            if (_gridReady == false) {
                PrepareScrolling();
            }
            return new Vector2Int(index % _inventoryRendererUI.RowSize, Mathf.FloorToInt(index / _inventoryRendererUI.RowSize));
        }

        public void OnSelectionChanged(int index) {
            if (_gridReady == false) {
                PrepareScrolling();
            }

            Vector2Int gridPos = GetGridPositionCooordinates(index);

            if (gridPos.y < 1) {
                _scrollRect.verticalNormalizedPosition = 1.0f;
            } else if (gridPos.y > _numberOfRow - 1) {
                _scrollRect.verticalNormalizedPosition = Mathf.Clamp01(1 - _movementStep * (_numberOfRow - 1));
            } else {
                _scrollRect.verticalNormalizedPosition = Mathf.Clamp01(1 - _movementStep * (gridPos.y - 1));
            }
        }
    }
}