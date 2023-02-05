using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject roomPrefab;
    private GameManager gameManager;
    [SerializeField]
    public GameObject cinematicvirtualcam;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.CreatePlayer();     
        cinematicvirtualcam.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = gameManager.Player.transform;
        cinematicvirtualcam.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = gameManager.Player.transform; 
        //StartCoroutine(gameManager.ShowIntro());

        Debug.Log("Player created");
        Debug.Log(gameManager.PlayerAncestor.Name);
        gameManager.LoadEnemy(gameManager.CurrentStage);
        // Spawn tombstone at lastTombsonePosition
        if(gameManager.lastTombstonePosition != new Vector3(0,0,0)) Instantiate(gameManager.TombstonePrefab, gameManager.lastTombstonePosition, Quaternion.identity);
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
