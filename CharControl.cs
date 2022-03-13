using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;



public enum SIDE { Left = -1, Mid = 0, Right = 1 }

public class CharControl : MonoBehaviour
{
    public SIDE m_Side = SIDE.Mid;
    float newXPos = 0f;
    [HideInInspector]
    public bool SwipeLeft, SwipeRight, Tap;
    public float XValue = 1;
    private float x;
    private CharacterController charControl;
    private Animator animatör;
    private int startSpeed;
    private int forwardSpeed;
    public int gameSpeed;
    //public float jumpPower = 5f;
    //private float y;
    public float speedDodge = 10;

    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject confetti;
    public Transform confettiSpawnPoint;
    public GameObject NextLevelButton;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        charControl = GetComponent<CharacterController>();
        transform.position = Vector3.zero;
        animatör = GetComponent<Animator>();
        forwardSpeed = startSpeed;
        NextLevelButton.SetActive(false);
    }



    // Update is called once per frame
    void Update()

    {
        SwipeLeft = Swipe.swipeLeft;
        SwipeRight = Swipe.swipeRight;
        Tap = Swipe.tap;

        if (Tap)
        {
            forwardSpeed = gameSpeed ;
            animatör.SetBool("Run",true);
        }

        if (SwipeLeft)
        {
            if (m_Side == SIDE.Mid)
            {
                newXPos = -XValue;
                m_Side = SIDE.Left;

            }
            else if (m_Side == SIDE.Right)
            {
                newXPos = 0;
                m_Side = SIDE.Mid;

            }
        }
        else if (SwipeRight)
        {
            if (m_Side == SIDE.Mid)
            {
                newXPos = XValue;
                m_Side = SIDE.Right;

            }
            else if (m_Side == SIDE.Left)
            {
                newXPos = 0;
                m_Side = SIDE.Mid;

            }
        }

        if (true)
        {

        }

        Vector3 moveVector = new Vector3(x - transform.position.x, 0 * Time.deltaTime, forwardSpeed * Time.deltaTime);
        x = Mathf.Lerp(x, newXPos, Time.deltaTime * speedDodge);
        charControl.Move(moveVector);


        scoreText.text = "Score : " + score.ToString();
    }


    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("coins"))
        {
            score++;
        }


        if (other.CompareTag("Obstacles"))
        {
            animatör.CrossFadeInFixedTime("Jump", 0.3f);

        }

        if (other.CompareTag("Finish"))
        {
            newXPos = 0f;
            transform.eulerAngles = new Vector3(0, -180, 0);
            forwardSpeed = 0;
            animatör.SetBool("Run", false);
            animatör.CrossFadeInFixedTime("dans", 10f);
            Instantiate(confetti, confettiSpawnPoint.transform.position , transform.rotation);
            NextLevelButton.SetActive(true);
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(0);
    }
}



