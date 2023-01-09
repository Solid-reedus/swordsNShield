using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class playerInput : MonoBehaviour, Imelee
{
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;
    private float playerHealth;


    

    [SerializeField] private Transform Camera;
    //private Camera Camera;
    //[SerializeField] private GameObject playerModel;
    [SerializeField] private Collider groundCheck;
    private bool isGrounded = true;

    private Vector3 lookDir = new Vector3(15, 0, 0);

    private Vector3 movementValue;
    [SerializeField] private float jumpForce = 3;
    [SerializeField] private float WalkForce = 3;
    [SerializeField] private float lookSensitivity = 1.4f;

    // img
    //left Arrow
    [SerializeField] private GameObject arrowRight;
    [SerializeField] private GameObject arrowUp;
    [SerializeField] private GameObject arrowBottom;
    [SerializeField] private GameObject arrowLeft;

    [SerializeField] private float lookThreshold = 0.65f;
    private int lookValInt;

    //int Imelee.dir { get; set; }
    int Imelee.lookVal { get { return lookValInt; }}
    //public int dir;



    // Start is called before the first frame update
    void Start()
    {
        /*
        arrowUp     = GameObject.Find("top Arrow");
        arrowRight  = GameObject.Find("right Arrow");
        arrowBottom = GameObject.Find("bottom Arrow");
        arrowLeft   = GameObject.Find("left Arrow");
        */
        playerAnimator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    public void Update()
    {
        //Debug.Log("lookValInt = " + lookValInt);

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
        //Debug.Log(value);
        //Vector2 nn = new Vector2(0, 0);
        Vector2 movement = value.ReadValue<Vector2>();
        movementValue = new Vector3(movement.x, 0, movement.y);
        //playerTransform.rotation = Quaternion.LookRotation(dir);

        playerAnimator.SetFloat("inputX", movement.x);
        playerAnimator.SetFloat("inputY", movement.y);

        /*
        void CheckLegs()
        {
            if (movement.x == 0 && movement.y > 0)
            {
                playerAnimator.Play("forward");
                //forward
            }
            else if (movement.x > 0 && movement.y > 0)
            {
                playerAnimator.Play("forward right");
                //forward right
            }
            else if (movement.x > 0 && movement.y == 0)
            {
                playerAnimator.Play("right");
                //right
            }
            else if (movement.x > 0 && movement.y < 0)
            {
                playerAnimator.Play("backward right");
                //backward right
            }
            else if (movement.x == 0 && movement.y < 0)
            {
                playerAnimator.Play("backward");
                //backward
            }
            else if (movement.x < 0 && movement.y < 0)
            {
                playerAnimator.Play("backward left");
                //backward left
            }
            else if (movement.x < 0 && movement.y == 0)
            {
                playerAnimator.Play("left");
                //left
            }
            else if (movement.x < 0 && movement.y > 0)
            {
                playerAnimator.Play("forward left");
                //forward left
            }
            else
            {
                playerAnimator.Play("idle");
                //idle
            }
        }

        */

        if (movement.x != 0 || movement.y != 0)
        {

            //playerModel.transform.rotation = Quaternion.LookRotation(movementValue);
        }
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
        lookDir.y = Mathf.Clamp(lookDir.y, -15, 30);
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
