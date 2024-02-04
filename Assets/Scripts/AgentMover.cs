using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Input;
using UnityEngine;

namespace FarmGame.Agent {
    public class AgentMover : MonoBehaviour {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float moveSpeed = 2f;
        public event Action<bool> OnMoveStateChanged;

        private Vector2 moveVector;
        [SerializeField]
        private CollisionDetector _collisionDetector;

        private bool _stopped;
        public bool Stopped {
            get => _stopped;
            set { _stopped = value; }
        }

        internal void SetMoveVector(Vector2 moveVector) {
            this.moveVector = moveVector;
        }

        private void FixedUpdate() {
            if (_stopped) return;

            Vector2 velocity = moveVector * moveSpeed;
            float distanceToMoveThisFrame = velocity.magnitude * Time.fixedDeltaTime;
            if (_collisionDetector.IsMovementValid(moveVector, distanceToMoveThisFrame) == false) {
                velocity = Vector2.zero;
            }
            OnMoveStateChanged?.Invoke(velocity.magnitude > 0.1f);
            _rigidbody.MovePosition(_rigidbody.position + velocity * Time.fixedDeltaTime);

        }
    }
}
