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
    }
    //Score - High Score data

    //Map Create
    /// <summary>
    /// prefabı çek 
    /// renk bas
    /// z tek se açık
    /// z çift ise koyu bas
    /// math.abs(y - y)
    /// max range min range arası değerler ata
    /// </summary>
    void mapCreate(int level)//int level count alıp *2 ile çarpıp öyle renk bulunacak
    {
        print("burdayım");
        for(int i = (level)* levelLinecount; i < (level + 1) * levelLinecount; i++)
        {
            rangeTemp = UnityEngine.Random.Range(minRange, maxRange);
            top_ = Instantiate(top);
            top_.transform.position = new Vector3(0, 0 + rangeTemp / 2, i * 2);
            bot_ = Instantiate(bot);
            bot_.transform.position = new Vector3(0, 0 - rangeTemp / 2, i * 2);
            if (i % 2 == 0)
            {
                top_.GetComponent<MeshRenderer>().material = colorList[level * 2];
                bot_.GetComponent<MeshRenderer>().material = colorList[level * 2];
            }
            else
            {
                top_.GetComponent<MeshRenderer>().material = colorList[(level * 2)+1];
                bot_.GetComponent<MeshRenderer>().material = colorList[(level * 2)+1];
            }
            if(i % 30 == 0 && i != 0)
            {
                circleCreate(i);
            }
        }
    }

    void mapCreateCounter()
    {
        print(destroyObject * (levelNumber + 1) + " > " + levelLinecount * (levelNumber + 1));
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
        
        if(point == 1) { pointCount++; smallHexCombo = false; smallHexComboCount = 2; print(pointCount); }
        else if(point == 2) { pointCount += smallHexComboCount; smallHexCombo = true; if (smallHexCombo && smallHexComboCount <= 5) { smallHexComboCount++; } print(pointCount); } 
        
    }
    //save data
    public void saveScore() 
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/appLog.dat");

        SaveData data = new SaveData();
        if (highScoreTemp < pointCount + linePointCount) { data.highScore = pointCount + linePointCount; }
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
        SceneManager.LoadScene(0);
    }

    [Serializable]
    class SaveData
    {
        public float highScore;
    }
}
