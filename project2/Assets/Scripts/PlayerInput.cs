using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour
{
    public GunControl gun;
    private CharacterController cc;
    public Transform camTrans;


    public float useDistance = 2.0f;
    int intEnvMask;

    public bool isMovingBox = false;
    public Rigidbody moveBox = null;
    Vector3 boxMoveDir = Vector3.zero;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float boxMoveSpeed = 2.0f;
    [SerializeField] private float boxMoveForce = 20.0f;

    Vector3 moveDir;

    public Text tooltipText;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        intEnvMask = LayerMask.GetMask("InteractiveEnvironment");
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        

        gun.gunDown = isMovingBox;

        moveDir = transform.forward * vertical + transform.right * horizontal;


        cc.SimpleMove(moveDir * (isMovingBox ? boxMoveSpeed : moveSpeed));

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            StartCoroutine(Jump());
        }


    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        bool didHit = Physics.Raycast(camTrans.position, camTrans.forward, out hit, 2.0f, intEnvMask);

        if (didHit && !isJumping)
        {
            string ttt = "";
            if (hit.transform.CompareTag("SlidingBox"))
                ttt = "Press 'E' to push/pull box.";
            else if (hit.transform.CompareTag("BoxLift"))
                ttt = "Press 'E' to use box lift.";
            else if (hit.transform.CompareTag("Door"))
                ttt = "Press 'E' to exit this room.";

            tooltipText.text = ttt;
        }
        else tooltipText.text = "";

        if (Input.GetButton("Use") && didHit)
        {
            if (hit.collider.CompareTag("SlidingBox"))
            {
                StopCoroutine(ResetBox());
                Vector3 forcePoint = hit.point - (hit.point.y >= hit.transform.position.y ? Vector3.up : Vector3.zero);
                Rigidbody box = hit.rigidbody;
                
                box.AddForceAtPosition(moveDir * boxMoveForce * Time.fixedDeltaTime, forcePoint);
                isMovingBox = true;
            }
            
        }
        else
        {
            isMovingBox = false;
        }

        if (Input.GetButtonDown("Use") && didHit)
        {
            if (hit.collider.CompareTag("BoxLift"))
            {
                hit.collider.GetComponent<BoxLift>().Toggle();
            }
        }
    }

    private IEnumerator ResetBox()
    {
        
        yield return new WaitForSeconds(0.1f);
        isMovingBox = false;
    }

    [SerializeField] private AnimationCurve jumpCurve; // used to control upward force
    [SerializeField] private float jumpFactor = 5.0f;

    private bool isJumping;


    private IEnumerator Jump()
    {
        // avoid crawling up slopes during the jump
        cc.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        // if we are grounded and our head is not colliding with the ceiling, move upward
        do
        {
            // at some point, the jump amount will be less than the effects of gravity
            float jumpAmount = jumpCurve.Evaluate(timeInAir) * jumpFactor * Time.deltaTime;
            cc.Move(Vector3.up * jumpAmount);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!cc.isGrounded && cc.collisionFlags != CollisionFlags.Above);

        // reset slope limit so we can go up slopes when not jumping
        cc.slopeLimit = 45.0f;
        isJumping = false;
    }

}
