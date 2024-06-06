using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame {
    [RequireComponent(typeof(Animator))]
    public class ToolAnimation : MonoBehaviour {
        private Animator animator;
        private const string DIRECTION_X = "DirectionX";
        private const string DIRECTION_Y = "DirectionY";
        private const string USE = "Use";

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        public void SetAnimatorController(RuntimeAnimatorController controller) {
            animator.runtimeAnimatorController = controller;
        }

        public void ChangeDirection(Vector2 direction) {
            if (direction.magnitude < 0.1f) return;
            Vector2Int dir = Vector2Int.RoundToInt(direction);
            if (dir.x != 0) dir.y = 0;
            animator.SetFloat(DIRECTION_X, dir.x);
            animator.SetFloat(DIRECTION_Y, dir.y);
        }

        public void PlayAnimation() {
            animator.SetTrigger(USE);
        }
    }
}
