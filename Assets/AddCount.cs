using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AddCount : MonoBehaviour
{
    private GameManager mango;
    private TextMeshProUGUI testo;

    private void Awake()
    {
        testo = GetComponent<TextMeshProUGUI>();
        mango = FindObjectOfType<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        testo.text = mango.familyTree.Count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
