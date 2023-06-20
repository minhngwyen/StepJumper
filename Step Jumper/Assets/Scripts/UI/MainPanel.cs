using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [SerializeField]
    private Button buttonStart;
    [SerializeField]
    private Button buttonExit;

    public int gameSceneNum;
    // Start is called before the first frame update
    void Start()
    {
        buttonStart.onClick.AddListener(OnButtonStartClick);
        buttonExit.onClick.AddListener(OnButtonExitButton);

    }

    private void OnButtonExitButton()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    private void OnButtonStartClick()
    {
        SceneManager.LoadScene(gameSceneNum);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
