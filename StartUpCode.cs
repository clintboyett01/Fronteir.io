using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpCode : MonoBehaviour
{
    public GameObject enemyPlayer;
    public float xMaxRange;
    public float xMinRange;
    public float yMaxRange;
    public float yMinRange;

    void Start()
    {
        // max enemy count that current color system can handle is 27
        for (int v = 0; v < 27; v++)
        {
           summonEnemy();
        }
    }

    void Update()
    {
        if (GetEnemyNum() < 27)
        {
           summonEnemy();
        }
    }

    void summonEnemy()
    {
        Vector2 pos = new Vector2(Random.Range(xMinRange, xMaxRange), Random.Range(yMinRange, yMaxRange));
        Instantiate(enemyPlayer, pos, Quaternion.identity);
    }

    int GetEnemyNum()
    {
        Object[] g = FindObjectsOfType(typeof(EnemyCode));
        return g.Length;
    }
    
}
