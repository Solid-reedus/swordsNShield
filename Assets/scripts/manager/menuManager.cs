using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    [SerializeField] GameObject mainCanv;
    [SerializeField] GameObject SkirCanv;
    [SerializeField] GameObject howtCanv;

    [SerializeField] Button howtRetrunButton;
    [SerializeField] Button SkirRetrunButton;

    [SerializeField] Button howtOpenButton;
    [SerializeField] Button SkirOpenButton;

    [SerializeField] Button Exit;

    //private int menuIndex = 1;





    //pistolButton.onClick.AddListener(pistolButtonClicked);

    // Start is called before the first frame update
    void Start()
    {
        howtRetrunButton.onClick.AddListener(RetrunButtonClicked);
        SkirRetrunButton.onClick.AddListener(RetrunButtonClicked);

        howtOpenButton.onClick.AddListener(OpenHowt);
        SkirOpenButton.onClick.AddListener(OpenSkir);
        Exit.onClick.AddListener(Quit);


        //howtRetrunButton = howtCanv.GetComponentInChildren<Button>();
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

    void Quit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
