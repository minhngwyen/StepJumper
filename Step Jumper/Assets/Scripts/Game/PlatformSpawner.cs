using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformGroupType
{
    Grass,
    Winter
}
public class PlatformSpawner : MonoBehaviour
{
    public Vector3 startSpawnPos;
    /// <summary>
    /// </summary>
    public int milestoneCount = 10;
    public float fallTime;
    public float minFallTime;
    public float multiple;
    /// <summary>
    /// </summary>
    private int spawnPlatformCount;
    private ManagerVars vars;
    /// <summary>
    /// </summary>
    private Vector3 platformSpawnPosition;
    /// <summary>
    /// </summary>
    private bool isLeftSpawn = false;

    /// <summary>
    /// </summary>
    private Sprite selectPlatformSprite;
    /// <summary>
    /// </summary>
    private PlatformGroupType groupType;


    private void Awake()
    {
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
        vars = ManagerVars.GetManagerVars();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
    }
    private void Start()
    {
        RandomPlatformTheme();
        platformSpawnPosition = startSpawnPos;
        for (int i = 0; i < 5; i++)
        {
            spawnPlatformCount = 5;
            DecidePath();
        }

        GameObject go = Instantiate(vars.characterPre);
        go.transform.position = new Vector3(0, -1.8f, 0);
    }
    private void Update()
    {
        if (GameManager.Instance.IsGameStarted && GameManager.Instance.IsGameOver == false)
        {
            UpdateFallTime();
        }
    }
    /// <summary>
    /// </summary>
    private void UpdateFallTime()
    {
            milestoneCount *= 2;
            fallTime *= multiple;
            if (fallTime < minFallTime)
            {
                fallTime = minFallTime;
            }
    }
    /// <summary>
    /// </summary>
    private void RandomPlatformTheme()
    {
        int ran = Random.Range(0, vars.platformThemeSpriteList.Count);
        selectPlatformSprite = vars.platformThemeSpriteList[ran];

        if (ran == 2)
        {
            groupType = PlatformGroupType.Winter;
        }
        else
        {
            groupType = PlatformGroupType.Grass;
        }
    }
    /// <summary>
    /// </summary>
    private void DecidePath()
    {
        //if (isSpawnSpike)
        //{
        //    AfterSpawnSpike();
        //    return;
        //}
        if (spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }
    }
    /// <summary>
    /// </summary>
    private void SpawnPlatform()
    {
        int ranObstacleDir = Random.Range(0, 2);

        if (spawnPlatformCount >= 1)
        {
            SpawnNormalPlatform(ranObstacleDir);
        }
        else if (spawnPlatformCount == 0)
        {
            //int ran = Random.Range(0, 3);
            //if (ran == 0)
            //{
                SpawnCommonPlatformGroup(ranObstacleDir);
            //}
            //else if (ran == 1)
            //{
            //    switch (groupType)
            //    {
            //        case PlatformGroupType.Grass:
            //            SpawnGrassPlatformGroup(ranObstacleDir);
            //            break;
            //        case PlatformGroupType.Winter:
            //            SpawnWinterPlatformGroup(ranObstacleDir);
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else
            //{
            //    int value = -1;
            //    if (isLeftSpawn)
            //    {
            //        value = 0;
            //    }
            //    else
            //    {
            //        value = 1;
            //    }
            //}
        }

        //int ranSpawnDiamond = Random.Range(0, 8);
        //if (ranSpawnDiamond >= 6 && GameManager.Instance.PlayerIsMove)
        //{
        //    GameObject go = ObjectPool.Instance.GetDiamond();
        //    go.transform.position = new Vector3(platformSpawnPosition.x,
        //        platformSpawnPosition.y + 0.5f, 0);
        //    go.SetActive(true);
        //}
        if (isLeftSpawn)
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos,
                platformSpawnPosition.y + vars.nextYPos, 0);
        }
        else
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos,
                platformSpawnPosition.y + vars.nextYPos, 0);
        }
    }
    /// <summary>
    /// </summary>
    private void SpawnNormalPlatform(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetNormalPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }
    /// <summary>
    /// </summary>
    private void SpawnCommonPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetCommonPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }
    /// <summary>
    /// </summary>
    private void SpawnGrassPlatformGroup(int ranObstacleDir)
    {
        //GameObject go = ObjectPool.Instance.GetGrassPlatformGroup();
        //go.transform.position = platformSpawnPosition;
        //go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        //go.SetActive(true);
    }
    /// <summary>
    /// </summary>
    private void SpawnWinterPlatformGroup(int ranObstacleDir)
    {
        //GameObject go = ObjectPool.Instance.GetWinterPlatformGroup();
        //go.transform.position = platformSpawnPosition;
        //go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        //go.SetActive(true);
    }
   
    
}
