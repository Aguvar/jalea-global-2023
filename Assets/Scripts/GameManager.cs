using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public List<Ancestor> familyTree;

    private string playerName;

    [SerializeField]
    private TMP_Text introText;
    [SerializeField]
    private GameObject introPanel;



    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        familyTree = new List<Ancestor>();
        playerName = Utils.GenerateJapaneseName();


        //Generate level

        SceneManager.LoadScene("Main");

        StartCoroutine(ShowIntro());
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator ShowIntro() {
        string intro = $"Your name is {playerName}, and your family has been cursed for generations...\n\nIt's up to you to break the curse.";

        introText.text = intro;

        CanvasGroup canvasGroup = introPanel.GetComponent<CanvasGroup>();

        introPanel.SetActive(true);
        canvasGroup.alpha = 0;

        while (canvasGroup.alpha < 1) {
            canvasGroup.alpha += 0.01f;
            yield return null;
        }

        yield return new WaitForSeconds(4);

        while (canvasGroup.alpha > 0) {
            canvasGroup.alpha -= 0.01f;
            yield return null;

        }

        introPanel.SetActive(false);

        yield return null;
    }

    void GameOver() {
        var ancestor = new Ancestor() {  };

    }


}
