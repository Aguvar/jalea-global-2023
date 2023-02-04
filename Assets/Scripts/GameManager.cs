using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<(string, string, string)> familyTree;

    private string playerName;

    [SerializeField]
    private TMP_Text introText;
    [SerializeField]
    private GameObject introPanel;

    

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        familyTree = new List<(string, string, string)>();
        playerName = Utils.GenerateJapaneseName();

        //Generate level

        StartCoroutine(ShowIntro());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ShowIntro() {
        string intro = $"Your name is {playerName}, and your family has been cursed for generations...\n\nIt's up to you to break the curse.";
        
        introText.text = intro;

        introPanel.SetActive(true);

        yield return new WaitForSeconds(5);

        introPanel.SetActive(false);

        yield return null;
    }

    
}
