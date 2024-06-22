using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Farming {
    public class CropRenderer : MonoBehaviour {
        SpriteRenderer _spriteRenderer;
        [SerializeField]
        private Color _wiltedColor = new Color(0.5f, 0.5f, 0.5f, 1f);

        private void Awake() {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void ChangeLayerOrder() {
            _spriteRenderer.sortingOrder = 0;
        }

        public void SetSprite(Sprite cropSprite) {
            _spriteRenderer.sprite = cropSprite;
        }

        public void WiltCrop() {
            _spriteRenderer.color = _wiltedColor;
        }

    }
}
