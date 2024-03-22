using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int currentPosition;
    public List<Transform> TrailsList = new List<Transform>();
    [SerializeField]private GameObject groundCheckObject;
    
    [HideInInspector]
    public bool IsGrounded, IsSwiping;
    
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    private DeathManager deathManager;
    
    [SerializeField]private float velocity = 10f;
    [SerializeField]private float jumpHeight = 10f;
    [SerializeField]private float barrelRollLength = 1f;
    [SerializeField]private float changeTrailTime = 0.5f;
    [SerializeField]private float fallGravity = 5.0f;
    [SerializeField]private float maxVelocity;
    
    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private LayerMask obstacle;
    
    private readonly Vector3 barrelRollCenter = new Vector3(0, 0.22f, 0);
    private readonly Vector3 normalCenter = new Vector3(0.05f, 0.50f, 0.02f);
    private const float crouchShrink = 0.5f;
    private const float normalShrink = 1f;

    private void Start()
    {
        currentPosition = 1;
        StartCoroutine(IncreaseSpeed());
        deathManager = FindObjectOfType<DeathManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float speedFactor = (maxVelocity - playerRigidbody.velocity.magnitude) / maxVelocity;
        playerRigidbody.AddForce(speedFactor * velocity * Vector3.forward, ForceMode.Impulse);
        GroundCheck();
    }

    bool highPt = false;

    private void Update()
    {
        if (playerRigidbody.velocity.y < 0 && this.highPt == false)
        {
            this.highPt = true;
            playerRigidbody.AddForce(Vector3.down * fallGravity, ForceMode.Impulse);
        }

        if (IsGrounded)
        {
            this.highPt = false;
        }
        HandleGravity();
    }

    public void Crouch()
    {
        StartCoroutine(BarrelRoll());
    }

    private IEnumerator BarrelRoll()
    {
        playerAnimator.SetBool("isJumping", false);
        CapsuleCollider capsuleCollider = transform.GetComponent<CapsuleCollider>();
        playerAnimator.SetBool("isScrolling", true);
        if (!IsGrounded)
        {
            playerRigidbody.AddForce(Vector3.down * fallGravity, ForceMode.Impulse);
        }

        capsuleCollider.height = crouchShrink;
        capsuleCollider.center = barrelRollCenter;
        yield return new WaitForSeconds(barrelRollLength);
        capsuleCollider.height = normalShrink;
        capsuleCollider.center = normalCenter;
        playerAnimator.SetBool("isScrolling", false);
    }

    public void Jump()
    {
        GroundCheck();
        if (IsGrounded)
        {
            StartCoroutine(JumpAnimation());
        }
    }

    private IEnumerator JumpAnimation()
    {
        playerAnimator.SetBool("isJumping", true);
        playerRigidbody.AddForce(Vector3.up * jumpHeight);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => IsGrounded);
        playerAnimator.SetBool("isJumping", false);
    }


    public void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheckObject.transform.position, Vector3.down, out hit, 0.2f, groundLayer))
        {
            IsGrounded = true;
        }
        else if (Physics.Raycast(groundCheckObject.transform.position, Vector3.down, out hit, 0.2f, obstacle))
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }
    
    private void HandleGravity()
    {
        float currentVerticalSpeed = playerRigidbody.velocity.y;
     
        if(IsGrounded)
        {
            if(currentVerticalSpeed < 0f)
                currentVerticalSpeed = 0f;
        }
        else if(!IsGrounded)
        {
            currentVerticalSpeed -= fallGravity * Time.deltaTime;
        }
     
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x,currentVerticalSpeed,playerRigidbody.velocity.z);
    }


    public void MovePosition(int move)
    {
        if (currentPosition + move < 0)
        {
            //Debug.Log("far left");
        }
        else if (currentPosition + move > TrailsList.Count - 1)
        {
            //Debug.Log("far right");
        }
        else
        {
            StartCoroutine(VerticalMove(move));
        }
    }

    private IEnumerator VerticalMove(int move)
    {
        if (move == 1)
        {
            playerAnimator.SetBool("goingRight", true);
        }
        else if (move == -1)
        {
            playerAnimator.SetBool("goingLeft", true);
        }

        IsSwiping = true;
        transform.DOMoveX(TrailsList[currentPosition + move].position.x, changeTrailTime);
        currentPosition += move;
        yield return new WaitForSeconds(0.5f);
        IsSwiping = false;
        playerAnimator.SetBool("goingLeft", false);
        playerAnimator.SetBool("goingRight", false);
    }

    private IEnumerator IncreaseSpeed()
    {
        yield return new WaitForSeconds(10f);
        maxVelocity += 2f;
        fallGravity += 0.5f;
        StartCoroutine(IncreaseSpeed());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 6) return;
        Vector3 normal = collision.contacts[0].normal;
        normal = new Vector3(Mathf.Round(normal.x), Mathf.Round(normal.y), Mathf.Round(normal.z));
        //Debug.Log(normal);
        if (normal == -(transform.forward))
        {
            //Debug.Log("DEAD");
            deathManager.DeadState();
            AudioManager.Instance.Play("Hit");
        }

        else if (normal == transform.up)
        {
            //Debug.Log("Not dead");
        }
        else if (normal == new Vector3(0, -1, -1))
        {
            deathManager.DeadState();
        }
            
        else
        {
            return;
        }
    }
}