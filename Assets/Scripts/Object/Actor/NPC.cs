using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // 퀘스트 매니저를 만들까?

    public GameObject speechBubble;
    public Transform canvas;
    private TextMeshProUGUI textMesh;


    private void Start()
    {
        speechBubble.SetActive(false);
        textMesh = speechBubble.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        PlayerEnterSpeechArea();
    }

    private void PlayerEnterSpeechArea()
    {
        if (Physics.CheckSphere(transform.position, 2f, 1 << LayerMask.NameToLayer("Player")))
        {
            canvas.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward);

            textMesh.text = "인생은 힘든거야.. 개굴...\n그만하고 싶다..";
            speechBubble.SetActive(true);
        }
        else
        {
            speechBubble.SetActive(false);
        }
    }
}
