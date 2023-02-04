using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject roomPrefab;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.CreatePlayer();
        StartCoroutine(gameManager.ShowIntro());

        Debug.Log("Player created");
        Debug.Log(gameManager.PlayerAncestor.Name);
                gameManager.LoadEnemies();

        int offset = 300;

        foreach (var ancestor in gameManager.familyTree) {
            Instantiate(roomPrefab, new Vector3(0, 0, offset), Quaternion.identity);
            offset += 300;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
