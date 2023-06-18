using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    /// <summary>
    /// </summary>
    public static bool IsAgainGame = false;

    private bool isFirstGame;
    private int[] bestScoreArr;
    private int diamondCount;

    public void SetIsFirstGame(bool isFirstGame)
    {
        this.isFirstGame = isFirstGame;
    }
    public void SetBestScoreArr(int[] bestScoreArr)
    {
        this.bestScoreArr = bestScoreArr;
    }
    public void SetDiamondCount(int diamondCount)
    {
        this.diamondCount = diamondCount;
    }


    public bool GetIsFirstGame()
    {
        return isFirstGame;
    }
    public int[] GetBestScoreArr()
    {
        return bestScoreArr;
    }
    public int GetDiamondCount()
    {
        return diamondCount;
    }
}
