using UnityEngine.UI;
using UnityEngine;

public class ControlsScript : MonoBehaviour
{
    private float screenWidth;
    private float screenHeight;
    private Vector3 startPosition;
    private Vector3 endPosition;
    
    private PlayerMovement playerMovement;
    private DeathManager deathManager;
    private AudioManager audioManager;
    
    [SerializeField]private  LayerMask obstacleLayer;
    
    private int obstaclesHit;
    [SerializeField]private  float deadZone = 0.2f;
    
    [SerializeField] private GameObject gotHitScreen;


    private void Awake()
    {
        deathManager = FindObjectOfType<DeathManager>();
        audioManager = FindObjectOfType<AudioManager>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        startPosition = Vector3.zero;
    }

    private void Update()
    {
        if (Input.touchCount > 0 && GameManager.Instance.GamePaused == false)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startPosition = GetTouchPos(touch);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endPosition = GetTouchPos(touch);
            }
        }

        MouseMovement();

        if (gotHitScreen != null)
        {
            if (gotHitScreen.GetComponent<Image>().color.a > 0)
            {
                var color = gotHitScreen.GetComponent<Image>().color;
                color.a -= 0.01f;
                gotHitScreen.GetComponent<Image>().color = color;
            }
        }
    }

    private Vector3 GetTouchPos(Touch touch)
    {
        Vector2 pos = touch.position;
        pos.x = (pos.x - screenWidth) / screenWidth;
        pos.y = (pos.y - screenHeight) / screenHeight;
        return new Vector3(-pos.x, pos.y, 0.0f);
    }

    private Vector3 GetMousePos()
    {
        Vector2 pos = Input.mousePosition;
        pos.x = (pos.x - screenWidth) / screenWidth;
        pos.y = (pos.y - screenHeight) / screenHeight;
        return new Vector3(-pos.x, pos.y, 0.0f);
    }

    private void MouseMovement()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && GameManager.Instance.GamePaused == false)
        {
            startPosition = GetMousePos();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && GameManager.Instance.GamePaused == false)
        {
            endPosition = GetMousePos();

            float moveX = Vector3.Dot(Vector2.left, endPosition - startPosition);
            float moveY = Vector3.Dot(Vector2.up, endPosition - startPosition);
            Move(moveX, moveY);
        }
    }

    private void Move(float x, float y)
    {
        if (x > deadZone && GameManager.Instance.GamePaused == false)
        {
            if (CheckForObstacles(Vector3.right))
            {
                audioManager.Play("Swipe");
                playerMovement.MovePosition(1);
            }
            else
            {
                obstaclesHit++;
                playerMovement.IsSwiping = true;
            }
        }
        else if (x < -deadZone && GameManager.Instance.GamePaused == false)
        {
            if (CheckForObstacles(Vector3.left))
            {
                audioManager.Play("Swipe");
                playerMovement.MovePosition(-1);
            }
            else
            {
                obstaclesHit++;
                playerMovement.IsSwiping = true;
            }
        }

        if (obstaclesHit > 2)
        {
            deathManager.DeadState();
        }

        if (!playerMovement.IsSwiping && GameManager.Instance.GamePaused == false)
        {
            playerMovement.GroundCheck();
            if (y > 0 && playerMovement.IsGrounded)
            {
                audioManager.Play("SwipeUp");
                playerMovement.Jump();
            }
            else if (y < 0)
            {
                audioManager.Play("SwipeDown");
                playerMovement.Crouch();
            }
        }
        playerMovement.IsSwiping = false;
    }

    private bool CheckForObstacles(Vector3 direction)
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(playerMovement.transform.position + Vector3.up * 2, direction, out hit, 10f, obstacleLayer))
        {
            //Debug.Log("Did Hit");
            GotHit();
            return false;
        }
        else
        {
            //Debug.Log("Did not Hit");
            return true;
        }
    }

    private void GotHit()
    {
        audioManager.Play("Hit");
        var color = gotHitScreen.GetComponent<Image>().color;
        color.a = 0.8f;
        gotHitScreen.GetComponent<Image>().color = color;
        GameManager.Instance.DecreaseHealth();
    }
}