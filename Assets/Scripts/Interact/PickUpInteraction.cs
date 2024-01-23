using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using UnityEngine;

namespace FarmGame.Interact {
    public class PickUpInteraction : MonoBehaviour {

        public bool CanInteract() => true;
        public void Interact(Player agent) {
            Debug.Log("PickUpInteraction");
            Destroy(gameObject);
        }
    }
}
