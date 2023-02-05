using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePanel : MonoBehaviour
{
    public void UpdateLifePanel(float Health)
    {
        // Make the panel shorter
        if(Health >0){
        transform.localScale = new Vector3(Health / 100, 1, 1);
        }
    }
    // Start is called before the first frame update
}
