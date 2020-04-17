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
    public Text highScoreText;
    public Text pointTextStr;
    public Text pointTextInt;

    [Header("Button")]
    public Button startButton;
    public Button restartButton;
    public Button continueButton;

    private void Update()
    {
        pointRender();
    }
    public void pointRender()
    {
        pointText.gameObject.SetActive(true);
        float point = gameManager.GetComponent<GameManager>().linePointCount + gameManager.GetComponent<GameManager>().pointCount;
        pointText.text = Mathf.Abs(point).ToString("0.#");
    }
    public void highScoreRender(float highScore)
    {
        highScoreText.text = "Best : "+ highScore.ToString("0.#");
        highScoreText.gameObject.SetActive(true);
    }
   
    public void gameOverButtonRender()
    {
        restartButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
    }

    public void gameOverButtonClosed()
    {
        restartButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
    }

    public void tapToStartButtonClosed() 
    {
        startButton.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
    }

    public IEnumerator hexPointRender(int point)
    {
        switch (point)
        {
            case 1:
                pointTextInt.text = "+"+point.ToString();
                break;
            case 2:
                pointTextStr.text = "WOW";
                pointTextInt.text = "+" + point.ToString();
                pointTextStr.gameObject.SetActive(true);
                break;
            case 3:
                pointTextStr.text = "COOL";
                pointTextInt.text = "+" + point.ToString();
                pointTextStr.gameObject.SetActive(true);
                break;
            case 4:
                pointTextStr.text = "AWESOME";
                pointTextInt.text = "+" + point.ToString();
                pointTextStr.gameObject.SetActive(true);
                break;
            case 5:
                pointTextStr.text = "MIND-BLOWING";
                pointTextInt.text = "+" + point.ToString();
                pointTextStr.gameObject.SetActive(true);
                break;
        }
        pointTextInt.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        pointTextInt.gameObject.SetActive(false);
        pointTextStr.gameObject.SetActive(false);
    }

}
