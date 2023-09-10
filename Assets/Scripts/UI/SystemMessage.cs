using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectW
{
    public class SystemMessage : MonoBehaviour
    {
        private TextMeshProUGUI textMeshPro;
        private Vector3 originPos;

        private float maxEnableTime = 1.5f;

        private float currentTime = 0f;
        private float waitTime = 1f;

        private bool isReady = true;
        public bool IsReady { get { return isReady; } set { isReady = value; } }

        private Coroutine coroutine;

        private void Start()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            originPos = GetComponent<RectTransform>().position;
        }

        public void ShowSystemMessage(string text)
        {
            textMeshPro.alpha = 0f;
            transform.position += new Vector3(30f, 0, 0);
            textMeshPro.text = text;

            if (coroutine != null)
                StopCoroutine(coroutine);

            coroutine = StartCoroutine(SystemMessageTween());
        }

        public IEnumerator SystemMessageTween()
        {
            IsReady = false;

            while (currentTime < waitTime)
            {
                transform.position = Vector3.Lerp(transform.position, originPos, currentTime / waitTime);
                textMeshPro.alpha = Mathf.Lerp(0, 1, currentTime / waitTime);

                currentTime += Time.deltaTime;

                yield return null;
            }
            currentTime = 0f;

            yield return new WaitForSeconds(maxEnableTime);

            while (currentTime < waitTime)
            {
                textMeshPro.alpha = Mathf.Lerp(1, 0, currentTime / waitTime);

                currentTime += Time.deltaTime;

                yield return null;
            }
            currentTime = 0f;

            IsReady = true;
        }
    }
}