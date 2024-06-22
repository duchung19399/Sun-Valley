using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace FarmGame.Farming {
    public class FieldDetector : MonoBehaviour {
        private bool _isNearField = false;
        public bool IsNearField {
            get => _isNearField;
            set {
                _isNearField = value;
                if (_isNearField == false) {
                    OnFieldExited?.Invoke();
                }
            }
        }

        [SerializeField] private string _fieldTag = "Field";

        [SerializeField] private Transform _interactorCenter;

        private Vector2 _interactionDirection;
        [SerializeField] private FieldPositionValidator _fieldPositionValidator;

        private void Awake() {
            _fieldPositionValidator = FindObjectOfType<FieldPositionValidator>();
            if (_fieldPositionValidator == null) {
                Debug.LogError("FieldDetector: No FieldPositionValidator found in scene");
            }
        }

        public Vector2 PositionInFront => (Vector2)_interactorCenter.position + _interactionDirection * 0.5f;

        public event Action OnFieldExited;
        public event Action OnResetDetectedFields;
        public event Action<IEnumerable<Vector2>> OnPositionDectected;

        private float coroutineDelay = 0.1f;
        Coroutine oldCoroutine = null;
        private List<Vector2> validSelectionPositions = new();
        public List<Vector2> ValidSelectionPositions => validSelectionPositions;

        public void StartChecking(Vector2Int dectectRange) {
            StopChecking();
            oldCoroutine = StartCoroutine(CheckField(dectectRange));
        }

        private IEnumerator CheckField(Vector2Int dectectRange) {
            if (_isNearField && _fieldPositionValidator != null && _fieldPositionValidator.IsItFieldTile(PositionInFront)) {
                validSelectionPositions = DetectValidTiles(dectectRange);
                OnPositionDectected?.Invoke(validSelectionPositions);
            } else {
                validSelectionPositions.Clear();
                OnResetDetectedFields?.Invoke();
            }
            yield return new WaitForSeconds(coroutineDelay);
            oldCoroutine = StartCoroutine(CheckField(dectectRange));
        }

        public void StopChecking() {
            if (oldCoroutine != null) {
                StopCoroutine(oldCoroutine);
            }
        }



        public void SetInteractDirection(Vector2 direction) {
            if (direction != Vector2.zero) {
                _interactionDirection = direction;
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(_fieldTag)) {
                IsNearField = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag(_fieldTag)) {
                IsNearField = false;
            }
        }

        private void OnDrawGizmosSelected() {
            if (Application.isPlaying && _isNearField) {
                if (_fieldPositionValidator != null && _fieldPositionValidator.IsItFieldTile(PositionInFront)) {
                    Vector2 tilePosition = _fieldPositionValidator.GetValidFieldTile(PositionInFront);
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(tilePosition, Vector2.one);
                }
            }
        }


        public List<Vector2> DetectValidTiles(Vector2Int dectectRange) {
            if (_fieldPositionValidator == null) {
                return new List<Vector2>();
            }

            int halfX = dectectRange.x;
            int halfY = dectectRange.y;
            int xMax = halfX * 2 + 1;
            int yMax = halfY * 2 + 1;

            List<Vector2> tilesToCheck = new();
            Vector2 positionInFrontCached = PositionInFront;
            for (int x = 0; x < xMax; x++) {
                for(int y = 0; y < yMax; y++) {
                    tilesToCheck.Add(positionInFrontCached + new Vector2(x - halfX, y - halfY));
                }
            }

            return _fieldPositionValidator.GetValidFieldTiles(tilesToCheck);


            //return _fieldPositionValidator.GetValidFieldTiles(new List<Vector2> { PositionInFront });
        }
    }
}
