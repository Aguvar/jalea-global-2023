using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {
    public int RoomNumber = 0;

    [SerializeField]
    private GameObject BlockingTomb;
    [SerializeField]
    private Transform bossSpawn;

    public GameObject BossToSpawm;
    public Ancestor ancestor;

    private bool bossSpawned = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !bossSpawned) {
            GameObject boss = Instantiate(BossToSpawm, bossSpawn.transform.position, bossSpawn.transform.rotation);
            EnemyAI ai = boss.GetComponent<EnemyAI>();
            ai.EnemyDied.AddListener(UnblockRoom);
            ai.Name = ancestor.Name;
            bossSpawned = true;
        }
    }

    void UnblockRoom() {
        BlockingTomb.SetActive(false);
    }
}
