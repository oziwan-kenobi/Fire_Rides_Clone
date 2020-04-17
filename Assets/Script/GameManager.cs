using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("List")]
    public List<Material> colorList =  new List<Material>();

    [Header("GameObject")]
    [SerializeField] GameObject UIManager;
    [SerializeField] GameObject rope;
    [SerializeField] GameObject top;
    [SerializeField] GameObject bot;
    [SerializeField] GameObject hex;
    [SerializeField] GameObject ball;
    GameObject top_, bot_, hex_;

    [Header("Vector3")]
    [SerializeField] Vector3 camera;

    [Header("Int")]
    [SerializeField] int minRange, maxRange;
    public int destroyObject;
    public int smallHexComboCount = 2;
    int levelLinecount = 300;
    int rangeTemp;
    int levelNumber = 0;

    [Header("float")]
    public float pointCount = 0;
    public float linePointCount;
    float highScoreTemp;

    [Header("bool")]
    public bool smallHexCombo = false;
    private void Start()
    {
        mapCreate(levelNumber);
        camera = Camera.main.transform.position;
        loadScore();
        UIManager.GetComponent<UIManager>().highScoreRender(highScoreTemp);
    }

    private void FixedUpdate()
    {
        cameraFollow();
        mapCreateCounter();
    }

    //Camera Follow - Floow Ball
    void cameraFollow()
    {
        Camera.main.transform.position = ball.transform.position + camera;
        this.transform.position = ball.transform.position;
    }

    //Map Create
    void mapCreate(int level)
    {
        for(int i = (level)* levelLinecount; i < (level + 1) * levelLinecount; i++)
        {
            rangeTemp = UnityEngine.Random.Range(minRange, maxRange);
            top_ = Instantiate(top);
            top_.transform.position = new Vector3(0, 0 + rangeTemp / 2, i * 2);
            bot_ = Instantiate(bot);
            bot_.transform.position = new Vector3(0, 0 - rangeTemp / 2, i * 2);
            GameObject child = bot_.transform.GetChild(0).gameObject;
            if (i % 2 == 0)
            {
                top_.GetComponent<MeshRenderer>().material = colorList[level * 2];
                bot_.GetComponent<MeshRenderer>().material = colorList[level * 2];
                child.GetComponent<MeshRenderer>().material = colorList[level * 2];
            }
            else
            {
                top_.GetComponent<MeshRenderer>().material = colorList[(level * 2)+1];
                bot_.GetComponent<MeshRenderer>().material = colorList[(level * 2)+1];
                child.GetComponent<MeshRenderer>().material = colorList[(level * 2) + 1];
            }
            if(i % 30 == 0 && i != 0)
            {
                circleCreate(i);
            }
        }
    }

    void mapCreateCounter()
    {
        if(destroyObject * (levelNumber+1) > levelLinecount * (levelNumber + 1)) { levelNumber++; mapCreate(levelNumber); if (levelNumber == 3) { levelNumber = 0; } destroyObject = 0; }
    }

    //Circle Create
    void circleCreate(int i )
    {
        hex_ = Instantiate(hex);
        hex_.transform.position = new Vector3(1.15f, 0 + hex.GetComponentInChildren<SpriteRenderer>().bounds.size.y , i*2);
    }

    //High Score- Score
    public void pointSystem(int point)
    {
        
        if(point == 1) { pointCount++; smallHexCombo = false; smallHexComboCount = 2; StartCoroutine(UIManager.GetComponent<UIManager>().hexPointRender(point)); }
        else if(point == 2) { pointCount += smallHexComboCount; smallHexCombo = true; StartCoroutine(UIManager.GetComponent<UIManager>().hexPointRender(smallHexComboCount)); if (smallHexCombo && smallHexComboCount <= 5) { smallHexComboCount++;  } } 
        
    }

    //save data
    public void saveScore() 
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/appLog.dat");

        SaveData data = new SaveData();
        if (highScoreTemp < pointCount + linePointCount) { data.highScore = pointCount + linePointCount; highScoreTemp = pointCount + linePointCount; }
        else { data.highScore = highScoreTemp; }
        bf.Serialize(file, data);
        file.Close();
    }

    //load data
    public void loadScore()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/appLog.dat", FileMode.Open);

        SaveData data = (SaveData)bf.Deserialize(file);
        file.Close();
        highScoreTemp = data.highScore;
    }

    //Game Over
    public void gameOver()
    {
        saveScore();
        UIManager.GetComponent<UIManager>().highScoreRender(highScoreTemp);
        UIManager.GetComponent<UIManager>().gameOverButtonRender();
    }

    //Restart
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Contiune()
    {
        ball.transform.position = new Vector3(ball.transform.position.x, 0, ball.transform.position.z);
        ball.SetActive(true);
        rope.GetComponent<RopeSwingScript>().gameStarted = false;
        rope.GetComponent<RopeSwingScript>().firstRopeConnected = false;
        UIManager.GetComponent<UIManager>().gameOverButtonClosed();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bigHex")) { smallHexCombo = false; smallHexComboCount = 2; }
        else if (other.gameObject.CompareTag("smallHex")) { smallHexCombo = false; smallHexComboCount = 2; }
        Destroy(other.gameObject); destroyObject++; 
    }

    [Serializable]
    class SaveData
    {
        public float highScore;
    }
}
