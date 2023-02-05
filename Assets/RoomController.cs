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

    public GameObject[] buffs;

    private bool bossSpawned = false;

    // Start is called before the first frame update
    void Start() {
        foreach (var buff in buffs) {
            var buffScript = buff.GetComponent<Bufos>();
            buffScript.grabbedEvent.AddListener(HideBuffs);
        }
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

        foreach (var buff in buffs) {
            buff.SetActive(true);
        }
    }

    void HideBuffs() {
        foreach (var buff in buffs) {
            buff.SetActive(false);
        }
    }
}
