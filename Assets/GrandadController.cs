using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandadController : MonoBehaviour
{
    private EnemyAI ai;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<EnemyAI>();
        ai.EnemyDied.AddListener(WinSequence);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WinSequence() {
        GameManager.Instance.Win();
    }
}
