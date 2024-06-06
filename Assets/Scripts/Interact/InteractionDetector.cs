using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Interact {
    public class InteractionDetector : MonoBehaviour {
        [SerializeField] LayerMask _interactionLayerMask;
        [SerializeField, Range(0.1f, 1)]
        float _interactionRadius = 0.5f;
        private Vector2 interactDirection;

        public void SetInteractDirection(Vector2 direction) {
            if (direction.magnitude > 0.1f) {
                interactDirection = direction.normalized;
            }
        }

        public IEnumerable<IInteractable> PerformDetection() {

            Collider2D colliderResult = Physics2D.OverlapCircle(
                (Vector2)transform.position + interactDirection * _interactionRadius,
                0.1f,
                _interactionLayerMask);

            if (colliderResult != null) {
                return colliderResult.GetComponents<IInteractable>();
            }

            return new List<IInteractable>();
        }

        public IEnumerable<IInteractable> PerformDetection(Vector2 position) {

            Collider2D colliderResult = Physics2D.OverlapCircle(
                position,
                0.1f,
                _interactionLayerMask);

            if (colliderResult != null) {
                return colliderResult.GetComponents<IInteractable>();
            }

            return new List<IInteractable>();
        }
    }
}
