using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private ManagerVars vars;

    /// <summary>
    /// </summary>
    public bool IsGameStarted { get; set; }
    /// <summary>
    /// </summary>
    public bool IsGameOver { get; set; }
    public bool IsPause { get; set; }
    /// <summary>
    /// </summary>
    public bool PlayerIsMove { get; set; }
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        Instance = this;
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);

        
    }
    private void PlayerMove()
    {
        PlayerIsMove = true;
    }
}
