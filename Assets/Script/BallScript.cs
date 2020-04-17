using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [Header("GameManager")]
    GameObject gameManager;
    //Ball fly
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
    }

    private void FixedUpdate()
    {
        gameManager.GetComponent<GameManager>().linePointCount = transform.position.z * 0.1f;
        
    }
    //Ball Line
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bigHex")) {  gameManager.GetComponent<GameManager>().pointSystem(1); Destroy(other.transform.parent.gameObject); }
        else if (other.gameObject.CompareTag("smallHex")) { gameManager.GetComponent<GameManager>().pointSystem(2); Destroy(other.transform.parent.gameObject); }
    }
        

    private void OnCollisionEnter(Collision collision)
    {
        this.gameObject.SetActive(false);
        if (collision.gameObject.CompareTag("obstacle")) { gameManager.GetComponent<GameManager>().gameOver(); }
    }

}
