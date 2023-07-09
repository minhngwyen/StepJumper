using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Playables;

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
        EventCenter.AddListener(EventDefine.AddScore, AddGameScore);
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond, AddGameDiamond);

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
        EventCenter.RemoveListener(EventDefine.AddScore, AddGameScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddGameDiamond);
    }
    /// <summary>
    /// </summary>
    /// <param name="score"></param>
    public void SaveScore(int score)//60
    {
        List<int> list = bestScoreArr.ToList();

        list.Sort((x, y) => (-x.CompareTo(y)));
        bestScoreArr = list.ToArray();

        //50 20 10
        int index = -1;
        for (int i = 0; i < bestScoreArr.Length; i++)
        {
            if (score > bestScoreArr[i])
            {
                index = i;
            }
        }
        if (index == -1) return;

        for (int i = bestScoreArr.Length - 1; i > index; i--)
        {
            bestScoreArr[i] = bestScoreArr[i - 1];
        }
        bestScoreArr[index] = score;

        Save();
    }
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int GetBestScore()
    {
        return bestScoreArr.Max();
    }
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int[] GetScoreArr()
    {
        List<int> list = bestScoreArr.ToList();
        list.Sort((x, y) => (-x.CompareTo(y)));
        bestScoreArr = list.ToArray();

        return bestScoreArr;
    }
    /// <summary>
    /// </summary>
    private void PlayerMove()
    {
        PlayerIsMove = true;
    }
    /// <summary>
    /// </summary>
    private void AddGameScore()
    {
        if (IsGameStarted == false || IsGameOver || IsPause) return;

        gameScore++;
        EventCenter.Broadcast(EventDefine.UpdateScoreText, gameScore);
    }
    /// <summary>
    /// </summary>
    public int GetGameScore()
    {
        return gameScore;
    }
    /// <summary>
    /// </summary>
    private void AddGameDiamond()
    {
        gameDiamond++;
        EventCenter.Broadcast(EventDefine.UpdateDiamondText, gameDiamond);
    }
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int GetGameDiamond()
    {
        return gameDiamond;
    }
    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetSkinUnlocked(int index)
    {
        return skinUnlocked[index];
    }
    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    public void SetSkinUnloacked(int index)
    {
        skinUnlocked[index] = true;
        Save();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int GetAllDiamond()
    {
        return diamondCount;
    }
    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    public void UpdateAllDiamond(int value)
    {
        diamondCount += value;
        Save();
    }
    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectedSkin(int index)
    {
        selectSkin = index;
        Save();
    }
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int GetCurrentSelectedSkin()
    {
        return selectSkin;
    }
    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    public void SetIsMusicOn(bool value)
    {
        isMusicOn = value;
        Save();
    }
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public bool GetIsMusicOn()
    {
        return isMusicOn;
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
            isMusicOn = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            skinUnlocked[0] = true;
            diamondCount = 10;

            data = new GameData();
            Save();
        }
        else
        {
            isMusicOn = data.GetIsMusicOn();
            bestScoreArr = data.GetBestScoreArr();D
            selectSkin = data.GetSelectSkin();
            skinUnlocked = data.GetSkinUnlocked();
            diamondCount = data.GetDiamondCount();
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
                data.SetBestScoreArr(bestScoreArr);
                data.SetDiamondCount(diamondCount);
                data.SetIsFirstGame(isFirstGame);
                data.SetIsMusicOn(isMusicOn);
                data.SetSelectSkin(selectSkin);
                data.SetSkinUnlocked(skinUnlocked);
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
        isMusicOn = true;
        bestScoreArr = new int[3];
        selectSkin = 0;
        skinUnlocked = new bool[vars.skinSpriteList.Count];
        skinUnlocked[0] = true;
        diamondCount = 10;

        Save();
    }
}
