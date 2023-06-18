using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameData data;
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
    /// <summary>
    /// </summary>
    private int gameScore;
    private int gameDiamond;


    private bool isFirstGame;
    private bool isMusicOn;
    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        Instance = this;
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);

        if (GameData.IsAgainGame)
        {
            IsGameStarted = true;
        }
        InitGameData();
    }
    private void Start()
    {

    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
    }

    /// <summary>
    /// </summary>
    private void PlayerMove()
    {
        PlayerIsMove = true;
    }
    /// <summary>
    /// </summary>
    private void InitGameData()
    {
        Read();
        if (data != null)
        {
            isFirstGame = data.GetIsFirstGame();
        }
        else
        {
            isFirstGame = true;
        }

        if (isFirstGame)
        {
            isFirstGame = false;           

            data = new GameData();
            Save();
        }
    }
    /// <summary>
    /// </summary>
    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                data.SetIsFirstGame(isFirstGame);
                bf.Serialize(fs, data);
            }

        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// </summary>
    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data", FileMode.Open))
            {
                data = (GameData)bf.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// </summary>
    public void ResetData()
    {
        isFirstGame = false;      

        Save();
    }
}
