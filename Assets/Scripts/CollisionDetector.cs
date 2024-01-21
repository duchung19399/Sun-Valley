using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Agent {
    public class CollisionDetector : MonoBehaviour {
        [SerializeField] private ContactFilter2D contactFilter;

        private RaycastHit2D[] _hitObject = new RaycastHit2D[8];

        [SerializeField] private Collider2D movementCollider;
        [SerializeField] private float _safetyCollisionOffset = 0.01f;

        [SerializeField] private int collisionResultCount;

        private void Awake() {
            Debug.Assert(movementCollider != null, "Collider cant be null", gameObject);
        }

        public bool IsMovementValid(Vector2 direction, float distanceToMoveThisFrame) {
            collisionResultCount = movementCollider.Cast(direction, contactFilter, _hitObject, distanceToMoveThisFrame + _safetyCollisionOffset);
            Debug.DrawRay(
                transform.position + (Vector3)movementCollider.offset,
                direction * (distanceToMoveThisFrame + _safetyCollisionOffset),
                collisionResultCount > 0 ? Color.red : Color.green
            );
            return collisionResultCount == 0;
        }
    }
}
