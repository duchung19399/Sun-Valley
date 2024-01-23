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

        public IEnumerable<PickUpInteraction> PerformDetection() {

            Collider2D colliderResult = Physics2D.OverlapCircle(
                (Vector2)transform.position + interactDirection * _interactionRadius,
                0.1f,
                _interactionLayerMask);

            if (colliderResult != null) {
                return colliderResult.GetComponents<PickUpInteraction>();
            }

            return new List<PickUpInteraction>();


        }
    }
}
