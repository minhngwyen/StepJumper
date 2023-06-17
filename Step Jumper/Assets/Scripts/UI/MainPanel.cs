using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [SerializeField]
    private Button buttonStart;
    [SerializeField]
    private Button buttonHighScore;
    [SerializeField]
    private Button buttonExit;
    // Start is called before the first frame update
    void Start()
    {
        buttonStart.onClick.AddListener(OnButtonStartClick);
        buttonHighScore.onClick.AddListener(OnButtonHighScoreButton);
        buttonExit.onClick.AddListener(OnButtonExitButton);

    }

    private void OnButtonExitButton()
    {
        Application.Quit();
    }

    private void OnButtonHighScoreButton()
    {
        
    }

    private void OnButtonStartClick()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
