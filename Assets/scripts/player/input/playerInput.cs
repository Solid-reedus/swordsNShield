using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerInput : MonoBehaviour, Imelee, IBlock, IdamageAble
{
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;

    [SerializeField] private float playerHealth = 100;
    SpawnManager SpawnManager;

    [SerializeField] private Transform Camera;
    [SerializeField] private Collider groundCheck;

    private Vector3 lookDir = new Vector3(15, 0, 0);

    private Vector3 movementValue;
    [SerializeField] private float jumpForce = 3;
    [SerializeField] private float WalkForce = 3;
    [SerializeField] private float lookSensitivity = 1.4f;

    [SerializeField] private GameObject arrowRight;
    [SerializeField] private GameObject arrowUp;
    [SerializeField] private GameObject arrowBottom;
    [SerializeField] private GameObject arrowLeft;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    [SerializeField] private Collider swordTrigger;
    [SerializeField] private Collider shieldTrigger;

    [SerializeField] private float lookThreshold = 0.65f;
    private int lookValInt;

    public bool isDead = false;

    [SerializeField] private bool isBlocking = false;
    [SerializeField] private bool isSwining = false;

    private string enemyTag = "enemy";

    int IdirectionalInput.lookVal { get { return lookValInt; }}
    bool IBlock.isBlocking { get { return isBlocking; } set { this.isBlocking = value; } }
    bool Imelee.isSwinging { get { return isSwining; } set { this.isSwining = value; } }
    string Imelee.enemyTag { get { return enemyTag; } set { this.enemyTag = value; } }

    Collider IBlock.shieldTrigger { get { return shieldTrigger; } }

    void Start()
    {
        SpawnManager = FindObjectOfType<SpawnManager>();
        playerAnimator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    //this updates the movement force and checks if the battle is over
    public void Update()
    {
        SpawnManager.BattleIsOver(enemyTag);
        playerRigidbody.AddRelativeForce(Time.deltaTime * movementValue * WalkForce, ForceMode.VelocityChange);
        updateBattle(SpawnManager.WhoWon);
    }

    //this coroutine will go back to the menu
    IEnumerator goToMenu()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
        yield break;
    }

    //this method sets a new value for movementValue based 
    //on the context from the new input system
    public void Move(InputAction.CallbackContext value)
    {
        Vector2 movement = value.ReadValue<Vector2>();
        movementValue = new Vector3(movement.x, 0, movement.y);

        playerAnimator.SetFloat("inputX", movement.x);
        playerAnimator.SetFloat("inputY", movement.y);
    }

    //this method displays the correct arrow based on the lookValInt
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

    /*
    this method will
    1 - rotate the camera (vertically) and rotate the players (horizontally) based on the 
        context of the new input system
    2 - it will update the value and arrow of the dominant direction of where your move is moving
        in 1 of 4 directions (up, left, down and right)
    */
    public void Look(InputAction.CallbackContext value)
    {
        Vector2 lookVal = value.ReadValue<Vector2>();
        Vector2 lookVal2 = lookVal * Time.deltaTime * lookSensitivity * 10;

        float result = Mathf.Max(lookVal2.x, lookVal2.y, -lookVal2.x, -lookVal2.y);

        if (result == lookVal2.x && result > lookThreshold)
        {
            lookValInt = 1;
        }
        else if (result == lookVal2.y && result > lookThreshold)
        {
            lookValInt = 2;
        }
        else if (result == -lookVal2.x && result > lookThreshold)
        {
            lookValInt = 3;
        }
        else if (result == -lookVal2.y && result > lookThreshold)
        {
            lookValInt = 4;
        }
        else
        {

        }

        lookDir.y -= lookVal.y * Time.deltaTime * lookSensitivity;
        lookDir.y = Mathf.Clamp(lookDir.y, -5, 30);

        transform.Rotate(0, lookVal.x * Time.deltaTime * lookSensitivity, 0);
        UpdateArrow();

        Camera.transform.eulerAngles = new Vector3(
            lookDir.y,
            Camera.transform.eulerAngles.y,
            Camera.transform.eulerAngles.z);
    }

    /*
    this is a implementation of the IdamageAble interface and
    allows this method to be called as a interface instead the whole script
    this script damages the player
    */
    public void damage(float dmg)
    {
        playerHealth -= dmg;
        playerAnimator.Play("hit");
        if (playerHealth < 1)
        {
            Die();
        }
    }

    //if the player loses to many hit points he will die and will play a death animation
    //and controls wil be disabled
    void Die()
    {
        playerAnimator.applyRootMotion = true;
        playerAnimator.Play("no more walking");
        playerAnimator.Play("die");
        GetComponent<PlayerInput>().enabled = false;
        lookSensitivity = 0;
        isDead = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /*
    if the state is 1 or 2 (allies won and enemy won) 
    then it will disable controls and active the cursor
    after that it will show the correct win or lose text
    */
    void updateBattle(int state)
    {
        if (state == 1 || state == 2)
        {
            Cursor.lockState = CursorLockMode.None;
            lookSensitivity = 0;
            GetComponent<PlayerInput>().enabled = false;
            StartCoroutine(goToMenu());
        }
        switch (state)
        {
            case 0:
            {
                // nothing
                break;
            }
            case 1:
            {
                // we won
                winScreen.SetActive(true);
                break;
            }
            case 2:
            {
                // enemy won
                loseScreen.SetActive(true);
                break;
            }
            default:
            {
                Debug.LogError("winstate is incorect");
                break;
            }
        }
    }

}
