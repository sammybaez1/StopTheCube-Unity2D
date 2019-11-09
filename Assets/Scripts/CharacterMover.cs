using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMover : MonoBehaviour
{
    public Camera gameCamera;
    public Text scoreText;
    public AudioSource tapSound;
    public float moveSpeed = 0f;
    public SpriteRenderer spriteRenderer;
    public Sprite greenFace;
    public Sprite redFace;
    public BoxCollider2D boxCollider;
    public GameObject retryButton;
    public GameObject quitButton;

    private Vector3 direction;
    private Vector2 screenToVector;
    /* Will be used to store the camera
     * width and height as a vector, so that 
     * we can easily decide what quadrant the 
     * cube is currently in. 
    */
    private float xMin = 0; // Define left side of axis
    private float xMax = 0; // Define right side of axis
    private float yMin = 0; // Define bottom of axis
    private float yMax = 0; // Define top of axis
    private bool isClickable = true;
    private int score = 0;
    private static GameObject[] clones;
    /* Store all clones in this array 
     * so that if the player fails to keep a cube
     * in the screen, we can access all clones and 
     * delete them.
    */

    void Start()
    {
        screenToVector = gameCamera.ScreenToWorldPoint(new Vector3(gameCamera.pixelWidth, gameCamera.pixelHeight));
        xMin = -screenToVector.x;
        xMax = screenToVector.x;
        yMin = -screenToVector.y;
        yMax = screenToVector.y;
        direction = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0f).normalized; // Start moving cube in a random direction
        StartCoroutine(IncreaseMoveSpeed());
    }

    void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (isClickable)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (boxCollider == Physics2D.OverlapPoint(touchPosition))
                {
                    ChooseDirection();
                }
            }
        }


#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            if (isClickable)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                if (boxCollider == Physics2D.OverlapPoint(touchPosition))
                {
                    ChooseDirection();
                }
            }

        }
#endif
    }

    IEnumerator IncreaseMoveSpeed()
    {
        while (true)
        {
            moveSpeed += 0.1f;
            yield return new WaitForSeconds(5f);
        }
    }

    void ChooseDirection()
    {
        tapSound.Play();
        int roll = Random.Range(1, 6);
        score = int.Parse(scoreText.text);
        score++;
        scoreText.text = score.ToString();

        if (roll == 2)//Generate a clone
        {
            Instantiate(gameObject);
        }

        float currentXPosition = transform.position.x;
        float currentYPosition = transform.position.y;
        //Quadrant1
        if (0f >= currentXPosition && currentXPosition > xMin && 0f <= currentYPosition && currentYPosition <= yMax)
        {
            direction = new Vector3(Random.Range(0f, xMax), Random.Range(yMin, 0f), 0f).normalized;
        }
        //Quadrant2
        else if (0f <= currentXPosition && currentXPosition <= xMax && 0f <= currentYPosition && currentYPosition < yMax)
        {
            direction = new Vector3(Random.Range(xMin, 0f), Random.Range(yMin, 0f), 0f).normalized;
        }
        //Quadrant3
        else if (0f >= currentXPosition && currentXPosition > xMin && 0f > currentYPosition && currentYPosition > yMin)
        {
            direction = new Vector3(Random.Range(0f, xMax), Random.Range(0, yMax), 0).normalized;
        }
        //Quadrant4
        else if (0f < currentXPosition && currentXPosition <= xMax && 0f > currentYPosition && currentYPosition > yMin)
        {
            direction = new Vector3(Random.Range(xMin, 0f), Random.Range(0f, yMax), 0).normalized;
        }

        StartCoroutine(ClickableTimer());
    }

    IEnumerator ClickableTimer()
    {
        spriteRenderer.sprite = redFace;
        isClickable = false;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = greenFace;
        isClickable = true;
    }

    void OnBecameInvisible()
    {
        retryButton.SetActive(true);
        quitButton.SetActive(true);
        clones = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject clone in clones)
        {
            Destroy(clone);
        }
    }


}


