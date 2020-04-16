using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Gameobject")]
    public GameObject gameManager;

    [Header("Text")]
    public Text pointText;

    private void Update()
    {
        pointRender();
    }
    public void pointRender()
    {
        float point = gameManager.GetComponent<GameManager>().linePointCount + gameManager.GetComponent<GameManager>().pointCount;
        pointText.text =  point.ToString("0.#");
    }
}
