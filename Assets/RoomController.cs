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

    private bool bossSpawned = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !bossSpawned) {
            Instantiate(BossToSpawm, bossSpawn.transform.position, bossSpawn.transform.rotation);
            bossSpawned = true;
        }
    }
}
