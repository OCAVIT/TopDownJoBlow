using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMananger : MonoBehaviour
{
    [SerializeField] EnemyAI[] enemies;
    int enemy = 0;
    void Start()
    {
        enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        enemy = enemies.Length;
    }
    public void MinusEnemy()
    {
        enemy -= 1;
        if(enemy <= 0)
        {
            //своё нечто
            print("гойда ZOV");
        }
    }
}
