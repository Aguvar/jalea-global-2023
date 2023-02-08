using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 

    public List<Ancestor> familyTree;

    private string playerName;

    [SerializeField]
    private TMP_Text introText;
    [SerializeField]
    private GameObject introPanel;
    [SerializeField]
    private CanvasGroup blackDrape;

    public GameObject EnemyPrefab;

    public GameObject TombstonePrefab;

    public GameObject Player;
    public float PlayerHealth = 100f;
    public Ancestor PlayerAncestor;
    
    public GameObject PlayerPrefab;

    public Vector3 lastTombstonePosition = new Vector3(0,0,0);
    public int CurrentStage = 0;
    public List<Ancestor> Enemies = new List<Ancestor>();
    // Queue of stages to load
    public LifePanel lifePanel;

    public void CreatePlayerData() {
        //Player = Instantiate(PlayerPrefab, new Vector3(0, 15, 0), Quaternion.identity);
        PlayerAncestor = new Ancestor();
        PlayerAncestor.Name = Utils.GenerateJapaneseName();
        PlayerAncestor.Iteration = Enemies.Capacity;
        PlayerAncestor.DifficultyModifier = 0;
        //Debug.Log(PlayerAncestor.Name);
        //Utils.GenerateClothes(Player,PlayerAncestor);
    }
    private void Awake() {
        

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start() {

        lifePanel = GameObject.FindObjectOfType<LifePanel>(true);
        familyTree = new List<Ancestor>();
        CreatePlayerData();
        StartCoroutine(LoadSceneWithFade("Main"));
        //StartCoroutine(ShowIntro());
    }
    //public void LoadEnemy(int stage) {
    //    Ancestor newAncestor = GenerateEnemy(stage);
    //    Instance.familyTree.Add(newAncestor);
    //}

    //public Ancestor GenerateEnemy(int iteration=0)
    //{
    //    GameObject enemy = Instantiate(EnemyPrefab);
    //    Ancestor enemyAnc;
    //    enemyAnc = new Ancestor();
    //    enemyAnc.Name = Utils.GenerateJapaneseName();
    //    enemyAnc.Iteration = iteration;
    //    enemyAnc.DifficultyModifier = 0*iteration;
    //    // Move enemy to random position near player
    //    enemy.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    //    enemy.GetComponent<EnemyAI>().Name = enemyAnc.Name;
    //    enemy.GetComponent<EnemyAI>().Player = Player.transform;
    //    Utils.GenerateClothes(enemy,enemyAnc);
    //    Enemies.Add(enemyAnc);
    //    return enemyAnc;
    //}

    public void OnDeath()
    {
        //Spawn Tomb prefab where player died
        //GameObject tomb = Instantiate(TombstonePrefab);
        //tomb.transform.position = Player.transform.position;
        //tomb.transform.rotation = Player.transform.rotation;
        //lastTombstonePosition = tomb.transform.position;
        Enemies.Insert(CurrentStage, PlayerAncestor);
        CurrentStage = 0;


        StartCoroutine(LoadSceneWithFade("Main"));
    }

    // Update is called once per frame
    void Update() {

    }

    public IEnumerator ShowIntro() {
        string intro = $"Your name is {PlayerAncestor.Name}, and your family has been cursed for generations...\n\nIt's up to you to break the curse.";
        
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

    IEnumerator LoadSceneWithFade(string sceneName) {

        blackDrape.gameObject.SetActive(true);
        blackDrape.alpha = 0;

        while (blackDrape.alpha < 1) {
            yield return null;
            blackDrape.alpha += 0.01f;
        }

        SceneManager.LoadScene(sceneName);
        lifePanel.gameObject.SetActive(true);

        while (blackDrape.alpha > 0) {
            yield return null;
            blackDrape.alpha -= 0.01f;
        }

        blackDrape.gameObject.SetActive(false);
    }

    public void Win() {
        StartCoroutine(LoadSceneWithFade("Win"));
    }


}
