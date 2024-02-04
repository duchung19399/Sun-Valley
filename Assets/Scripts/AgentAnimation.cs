using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Agent {
    [RequireComponent(typeof(Animator))]
    public class AgentAnimation : MonoBehaviour {
        private Animator animator;
        private const string DIRECTION_X = "DirectionX";
        private const string DIRECTION_Y = "DirectionY";
        private const string IS_MOVING = "IsMoving";
        private const string PICK_UP = "PickUp";

        [HideInInspector] public UnityEvent OnAnimationEnd;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        public void PlayerActionAnimationEnd() {
            OnAnimationEnd?.Invoke();
            OnAnimationEnd.RemoveAllListeners();
        }

        public void SetMoving(bool val) => animator.SetBool(IS_MOVING, val);
        public void ChangeDirection(Vector2 direction) {
            if (direction.magnitude < 0.1f) return;
            Vector2Int dir = Vector2Int.RoundToInt(direction);
            animator.SetFloat(DIRECTION_X, dir.x);
            animator.SetFloat(DIRECTION_Y, dir.y);
        }

        public void PlayAnimation(AnimationType animationType) {
            if (animationType == AnimationType.PickUp) {
                animator.SetTrigger(PICK_UP);
            }
        }
    }

    public enum AnimationType {
        None,
        Idle,
        Walk,
        PickUp,
        Drop
    }
}
