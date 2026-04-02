using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class C_CombatSystem : MonoBehaviour
{
    [SerializeField] private float enemiesUpdateTime;
    [SerializeField] private Transform enemiesContainer;
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private Enemy clinchEnemy;
    [SerializeField] private Enemy closestEnemy;
    [SerializeField] private CombatState combatState;

    public CombatState CombatStateParameter
    {
        get => combatState;
        set
        {
            if (combatState == value) return;
            combatState = value;
        }
    }

    public enum CombatState
    {
        Free,
        Clinch
    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        var go = GameObject.Find("Enemies");
        if(go != null )
        {
            enemiesContainer = go.transform;
        }

        StartCoroutine(UpdateEnemiesList());
    }

    private void Update()
    {
        closestEnemy = FindClosestEnemy();
    }


    private IEnumerator UpdateEnemiesList()
    {
        yield return new WaitForSeconds(enemiesUpdateTime);

        var childCount = enemiesContainer.childCount;
        var newList = new List<Enemy>();
        for(int i = 0; i < childCount; i++)
        {
            newList.Add(enemiesContainer.GetChild(i).GetComponent<Enemy>());
        }
        
        enemies = newList;
    }

    private Enemy FindClosestEnemy()
    {
        int index = 0;
        float distance = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            var e = enemies[i];

            if(i == 0)
            {
                distance = e.distanceToTarget;
                index = i;
                continue;
            }

            if(e.distanceToTarget < distance)
            {
                distance = e.distanceToTarget;
                index = i;
                continue;
            }
            else
            {
                continue;
            }
        }

        if(enemies.Count > 0)
        {
            return enemies[index];
        }
        else
        {
            return null;
        }
    }
}
