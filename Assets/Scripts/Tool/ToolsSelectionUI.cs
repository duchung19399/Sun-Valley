using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarmGame.Tools {
    public class ToolsSelectionUI : MonoBehaviour {
        [SerializeField] private Image _toolImage;
        [SerializeField] private Image _toolTipImage;
        [SerializeField] private List<Image> _toolImages;
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private float _alphaOfEmptyImage = 0.04f, _alphaOfFilledImage = 0.5f;


        public void UpdateUI(int selectedImageIndex, List<Sprite> images, int? count) {
            ClearToolsList();
            Color filledImageColor = Color.white;
            filledImageColor.a = _alphaOfFilledImage;
            ToggleToolSwapTip(images.Count > 1);

            for (int i = 0; i < images.Count; i++) {
                if (i >= _toolImages.Count) {
                    Debug.LogError("Tool images count is less than the number of tools available");
                    break;
                }
                if (i == selectedImageIndex) {
                    _toolImage.sprite = images[i];
                    _toolImage.color = filledImageColor;
                    if (count.HasValue) {
                        _countText.gameObject.SetActive(true);
                        _countText.text = count.Value.ToString();
                    } else {
                        _countText.gameObject.SetActive(false);
                    }
                }

                if(_toolImages.Count > i && images[i] != null) {
                    _toolImages[i].sprite = images[i];
                    _toolImages[i].color = filledImageColor;
                }
            }
        }

        private void ToggleToolSwapTip(bool v) {
            _toolTipImage.gameObject.SetActive(v);
        }

        private void ClearToolsList() {
            Color emptyImageColor = Color.white;
            emptyImageColor.a = _alphaOfEmptyImage;
            foreach (Image image in _toolImages) {
                image.sprite = null;
                image.color = emptyImageColor;
            }
        }
    }
}
