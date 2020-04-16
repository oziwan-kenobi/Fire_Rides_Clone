using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RopeSwingScript : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject ball;




    [SerializeField] float initialSwingingSpeed = 10f;
    [SerializeField] float initialSwingingDistance = 16f;
    private float ballSpeed;
    Rigidbody rigidBodyOfBall;
    Vector3 origin;

    [Header("Rope")]
    private SpringJoint rope;
    private bool firstRopeConnected = false;

    [Header("Game")]
    [SerializeField] private bool gameStarted = false;
    public bool gameOver = false;
    public LineRenderer lineRenderer;
    int layerMask = 1 << 8;//wall layer

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rigidBodyOfBall = ball.GetComponent<Rigidbody>();
        ballSpeed = /*ball.GetComponent<Ball>().ballSpeed;*/125f;
        origin = ball.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {

                RaycastHit aim;
                Physics.Raycast(origin, transform.forward + transform.up, out aim, Mathf.Infinity);
                print(aim.collider.gameObject.name);
                if (aim.transform != null && aim.collider.gameObject.CompareTag("obstacle"))
                {
                    Sling(aim);
                }
            }
            else if (Input.GetMouseButton(0) && rope != null)
            {
                BallForce();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                GameObject.DestroyImmediate(rope);
                rigidBodyOfBall.useGravity = true;
                rigidBodyOfBall.drag = .5f;
            }
        }
        origin = ball.transform.position;

    }
    private void FixedUpdate()
    {
        if (!gameStarted)
        {
            if (!firstRopeConnected)
            {
                FirstRope();
            }
            else
            {
                Teeter();
            }
        }
    }

    void LateUpdate()
    {

        if (rope != null && !gameOver)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, ball.transform.position);
            lineRenderer.SetPosition(1, rope.connectedAnchor);
        }
        else
        {
            lineRenderer.enabled = false;
        }
        if (gameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RestartGame();
            }
            rigidBodyOfBall.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    /*
     * It connects first rope before starting
     */
    private void FirstRope()
    {
        rigidBodyOfBall.useGravity = false;
        rigidBodyOfBall.mass = .1f;
        RaycastHit hitForBeginning;
        Physics.Raycast(origin, ball.transform.up, out hitForBeginning);
        if (hitForBeginning.collider != null)
        {
            SpringJoint firstRope = ball.AddComponent<SpringJoint>();
            firstRope.autoConfigureConnectedAnchor = false;
            firstRope.damper = 30f;
            firstRope.enableCollision = true;
            firstRope.connectedAnchor = hitForBeginning.point;
            firstRope.spring = 1.5f;
            GameObject.DestroyImmediate(rope);
            rope = firstRope;
            firstRopeConnected = true;
        }
    }
    /*
     * It swings ball until game starts
     */
    private void Teeter()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameStarted = true;
        }
        else
        {
            rigidBodyOfBall.velocity = (Mathf.PingPong(Time.time * initialSwingingSpeed, initialSwingingDistance)
                                            - (initialSwingingDistance / 2))
                                            * Vector3.Cross(ball.transform.right, ball.transform.up);
        }

    }
    /*
     * When player touch on screen, this method is called and the rope is slinged to wall
     */
    private void Sling(RaycastHit aim)
    {
        Vector3 targetPos = new Vector3(aim.transform.position.x,
                                        aim.transform.position.y - aim.transform.localScale.y / 2,
                                        aim.transform.position.z);
        Vector3 directionOfRope = targetPos - origin;
        RaycastHit hit;
        Physics.Raycast(origin, directionOfRope, out hit, Mathf.Infinity);
        print(hit.collider);
        if (hit.collider != null)
        {
            rigidBodyOfBall.useGravity = false;
            rigidBodyOfBall.mass = 0.1f;
            rigidBodyOfBall.drag = 0f;
            NewRope(hit);
        }
    }
    /*
     * It creates a rope(SpringJoint)
     */
    private void NewRope(RaycastHit hit)
    {
        SpringJoint newRope = ball.AddComponent<SpringJoint>();
        newRope.autoConfigureConnectedAnchor = false;
        newRope.spring = 4.5f;//mesafe başına çekim gücü
        newRope.damper = 25f;//salınımı kesmeye yarar
        newRope.enableCollision = true;
        newRope.connectedAnchor = hit.point;
        GameObject.DestroyImmediate(rope);
        rope = newRope;
    }
    /*
     * During player touching on screen, this function is called to move player
     */
    private void BallForce()
    {
        rigidBodyOfBall.AddForce(Vector3.Cross(-transform.right, (origin - rope.connectedAnchor).normalized) * ballSpeed * Time.deltaTime);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
