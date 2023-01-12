using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class playerInput : MonoBehaviour, Imelee, IBlock
{
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;
    private float playerHealth;

    [SerializeField] private Transform Camera;
    [SerializeField] private Collider groundCheck;
    private bool isGrounded = true;

    private Vector3 lookDir = new Vector3(15, 0, 0);

    private Vector3 movementValue;
    [SerializeField] private float jumpForce = 3;
    [SerializeField] private float WalkForce = 3;
    [SerializeField] private float lookSensitivity = 1.4f;

    [SerializeField] private GameObject arrowRight;
    [SerializeField] private GameObject arrowUp;
    [SerializeField] private GameObject arrowBottom;
    [SerializeField] private GameObject arrowLeft;


    [SerializeField] private Collider swordTrigger;
    [SerializeField] private Collider shieldTrigger;

    [SerializeField] private float lookThreshold = 0.65f;
    private int lookValInt;

    [SerializeField] private bool isBlocking = false;

    int IdirectionalInput.lookVal { get { return lookValInt; }}
    bool IdirectionalInput.movingshield { get { return isBlocking; } set { this.isBlocking = value; } }

    Collider IBlock.shieldTrigger { get { return shieldTrigger; } }

    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        playerRigidbody.AddRelativeForce(Time.deltaTime * movementValue * WalkForce, ForceMode.Impulse);
    }

    public void Jump(InputAction.CallbackContext context)
    {

        /*
        Debug.Log("context =" + context);
        if (isGrounded == true)
        {
            playerRigidbody.AddForce(Time.deltaTime * Vector3.up * jumpForce, ForceMode.Impulse);
        }
        */
    }

    public void Move(InputAction.CallbackContext value)
    {
        Vector2 movement = value.ReadValue<Vector2>();
        movementValue = new Vector3(movement.x, 0, movement.y);

        playerAnimator.SetFloat("inputX", movement.x);
        playerAnimator.SetFloat("inputY", movement.y);
    }

    void UpdateArrow()
    {
        arrowRight.SetActive(false);
        arrowBottom.SetActive(false);
        arrowLeft.SetActive(false);
        arrowUp.SetActive(false);

        switch (lookValInt)
        {
            case 1:
            {
                arrowRight.SetActive(true);
                break;
            }
            case 2:
            {
                arrowUp.SetActive(true);
                break;
            }
            case 3:
            {
                arrowLeft.SetActive(true);
                break;
            }
            case 4:
            {
                arrowBottom.SetActive(true);
                break;
            }
        }

    }

    public void Look(InputAction.CallbackContext value)
    {
        
        Vector2 lookVal = value.ReadValue<Vector2>();

        Vector2 lookVal2 = lookVal * Time.deltaTime * lookSensitivity * 10;

        float result = Mathf.Max(lookVal2.x, lookVal2.y, -lookVal2.x, -lookVal2.y);

        if (result == lookVal2.x && result > lookThreshold)
        {
            lookValInt = 1;
            ////Debug.Log("player is looking right");
        }
        else if (result == lookVal2.y && result > lookThreshold)
        {
            lookValInt = 2;
            //Debug.Log("player is looking up");
        }
        else if (result == -lookVal2.x && result > lookThreshold)
        {
            lookValInt = 3;
            //Debug.Log("player is looking left");
        }
        else if (result == -lookVal2.y && result > lookThreshold)
        {
            lookValInt = 4;
            //Debug.Log("player is looking down");
        }
        else
        {
            //Debug.Log("player is looking n/a");
        }


        //Debug.Log("this the result = " + result + "  - vec2 = " + lookVal2);
        //bool lookingLeft => { lookVal.x < 0};


        //Debug.Log("lookVal is = " + lookVal);
        lookDir.y -= lookVal.y * Time.deltaTime * lookSensitivity;
        //lookDir.y = Mathf.Clamp(lookDir.y, -15, 30);
        lookDir.y = Mathf.Clamp(lookDir.y, -5, 30);
        //Debug.Log("Camera.transform.eulerAngles = " + Camera.transform.eulerAngles);
        //Camera.transform.Rotate(lookDir.y, 0, 0);

        transform.Rotate(0, lookVal.x * Time.deltaTime * lookSensitivity, 0);
        UpdateArrow();

        Camera.transform.eulerAngles = new Vector3(
            lookDir.y,
            Camera.transform.eulerAngles.y,
            Camera.transform.eulerAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }

}
