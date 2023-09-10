using ProjectW;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadStateController : MonoBehaviour
{
    [SerializeField] private Image loadingBar;
    [SerializeField] private TextMeshProUGUI loadState;

    void Update()
    {
        loadingBar.fillAmount = GameManager.Instance.loadState;
        loadState.text = (GameManager.Instance.loadState * 100).ToString("F0") + "%";
    }
}
