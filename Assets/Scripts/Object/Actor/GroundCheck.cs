using ProjectW.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Object
{
    public class GroundCheck : MonoBehaviour
    {
        private Character character;

        private void Start()
        {
            character = GetComponentInParent<Character>();
            character.boCharacter.isGround = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
                character.boCharacter.isGround = true;

            if (other.CompareTag("Object"))
                character.boCharacter.isGround = true;
        }
    }
}