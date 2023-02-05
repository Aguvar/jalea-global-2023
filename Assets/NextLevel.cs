using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter (Collider coll)
     {
      // Moves camera to next level
        if (coll.gameObject.tag == "Player")
        {
            //Destroy any gameObject with tag "Enemy"
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            GameManager.Instance.CurrentStage++;
            GameManager.Instance.LoadEnemy(GameManager.Instance.CurrentStage);
            GameManager.Instance.Player.transform.position = new Vector3(0, 15, 0);

            
        }
     }
}
