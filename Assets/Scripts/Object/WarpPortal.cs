using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectW
{
    public class WarpPortal : MonoBehaviour
    {
        [SerializeField] private Define.SceneType warpScene;
        [SerializeField] private int warpIndex; // SDStage¿« WarpPosition Array Index

        private bool isReady;

        private void Update()
        {
            if (!isReady)
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                StageManager.Instance.WarpStage(warpScene, warpIndex);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                isReady = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                isReady = false;
        }
    }
}