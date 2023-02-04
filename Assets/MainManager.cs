using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MainManager : MonoBehaviour
{
    public static MainManager Instance; 

    public GameObject EnemyPrefab;

    public GameObject Player;
    
    public List<GameObject> Enemies = new List<GameObject>();
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
