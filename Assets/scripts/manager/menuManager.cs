using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    int allyCount = 1;
    int enemyCount = 1;

    [SerializeField] GameObject mainCanv;
    [SerializeField] GameObject SkirCanv;
    [SerializeField] GameObject howtCanv;

    [SerializeField] Button howtRetrunButton;
    [SerializeField] Button SkirRetrunButton;

    [SerializeField] Button howtOpenButton;
    [SerializeField] Button SkirOpenButton;

    [SerializeField] Button decreaseAllyButton;
    [SerializeField] Button increaseAllyButton;
    [SerializeField] Button decreaseEnemyButton;
    [SerializeField] Button increaseEnemyButton;

    [SerializeField] Button BattleButton;

    [SerializeField] TMP_InputField allyCountText;
    [SerializeField] TMP_InputField enemyCountText;

    [SerializeField] sceneManager sceneManager;

    [SerializeField] Button Exit;

    //in the start methods are a list of buttons that get a onclick Listener attached to it
    void Start()
    {
        howtRetrunButton.onClick.AddListener(RetrunButtonClicked);
        SkirRetrunButton.onClick.AddListener(RetrunButtonClicked);

        howtOpenButton.onClick.AddListener(OpenHowt);
        SkirOpenButton.onClick.AddListener(OpenSkir);
        Exit.onClick.AddListener(ExitGame);

        decreaseAllyButton.onClick.AddListener(decreaseAlly);
        increaseAllyButton.onClick.AddListener(increaseAlly);
        decreaseEnemyButton.onClick.AddListener(decreaseEnemy);
        increaseEnemyButton.onClick.AddListener(increaseEnemy);

        BattleButton.onClick.AddListener(Battle);
    }

    //when the menuManager is "awake" the inputfields gets new values defined
    private void Awake()
    {
        allyCountText.text = allyCount.ToString();
        enemyCountText.text = enemyCount.ToString();
    }

    /*
     in the update there is code that checks if allyCountText.text or enemyCountText.text goes
    under 1 or over 25.
    if it goes under 1 then the value is set to 1 and if it goes over 25 it goes to 25 
    */
    void Update()
    {
        if (int.Parse(allyCountText.text) < 1)
        {
            allyCount = 1;
            allyCountText.text = allyCount.ToString();
        }
        if (int.Parse(allyCountText.text) > 25)
        {
            allyCount = 25;
            allyCountText.text = allyCount.ToString();
        }
        if (int.Parse(enemyCountText.text) < 1)
        {
            enemyCount = 1;
            enemyCountText.text = enemyCount.ToString();
        }
        if (int.Parse(enemyCountText.text) > 25)
        {
            enemyCount = 25;
            enemyCountText.text = enemyCount.ToString();
        }
    }

    void RetrunButtonClicked()
    {
        UpdateMenu(1);
    }

    void OpenSkir()
    {
        UpdateMenu(2);
    }

    void OpenHowt()
    {
        UpdateMenu(3);
    }

    //this exits the game
    void ExitGame()
    {
        Application.Quit();
    }

    void decreaseAlly()
    {
        if (allyCount > 1)
        {
            allyCount--;
        }
        allyCountText.text = allyCount.ToString();
    }

    void increaseAlly()
    {
        if (allyCount < 25)
        {
            allyCount++;
        }
        allyCountText.text = allyCount.ToString();
    }

    void decreaseEnemy()
    {
        if (enemyCount > 1)
        {
            enemyCount--;
        }
        enemyCountText.text = enemyCount.ToString();
    }

    void increaseEnemy()
    {
        if (enemyCount < 25)
        {
            enemyCount++;
        }
        enemyCountText.text = enemyCount.ToString();
    }

    //this method sets the sceneManager allyCount and enemyCount to be used in the seconds scene
    //and afterwards loads the next scene
    void Battle()
    {
        sceneManager.allyCount = allyCount;
        sceneManager.enemyCount = enemyCount;
        SceneManager.LoadScene(1);
    }

    //this methods updates the menu's so only one is seen
    //the menu seen is based on menuIndex
    void UpdateMenu(int menuIndex)
    {
        mainCanv.SetActive(false);
        SkirCanv.SetActive(false);
        howtCanv.SetActive(false);

        switch (menuIndex)
        {
            case 1:
            {
                mainCanv.SetActive(true);
                break;
            }
            case 2:
            {
                SkirCanv.SetActive(true);
                break;
            }
            case 3:
            {
                howtCanv.SetActive(true);
                break;
            }
        }
    }

}
