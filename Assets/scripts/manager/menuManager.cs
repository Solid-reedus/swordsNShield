using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    GameObject mainCanv;
    GameObject SkirCanv;
    GameObject howtCanv;

    public Button howtRetrunButton;
    public Button SkirRetrunButton;





    //pistolButton.onClick.AddListener(pistolButtonClicked);

    // Start is called before the first frame update
    void Start()
    {
        howtRetrunButton = howtCanv.GetComponentInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
