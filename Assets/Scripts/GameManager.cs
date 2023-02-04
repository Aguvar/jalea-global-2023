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

    public GameObject EnemyPrefab;

    public GameObject Player;

    public Ancestor PlayerAncestor;
    
    public GameObject PlayerPrefab;
    public int CurrentStage = 0;
    public List<GameObject> Enemies = new List<GameObject>();

    public void CreatePlayer() {
        Player = Instantiate(PlayerPrefab, new Vector3(0, 5, 0), Quaternion.identity);
        PlayerAncestor = new Ancestor();
        PlayerAncestor.Name = Utils.GenerateJapaneseName();
        PlayerAncestor.Iteration = 0;
        PlayerAncestor.DifficultyModifier = 0;
        Debug.Log(PlayerAncestor.Name);
        Utils.GenerateClothes(Player,PlayerAncestor);
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
        familyTree = new List<Ancestor>();
        SceneManager.LoadScene("Main");

    }
    public void LoadEnemies() {
        for(int i = 0; i < 1; i++) {
            Ancestor newAncestor = GenerateEnemy(i);
            Debug.Log(newAncestor.Name);
            Instance.familyTree.Add(newAncestor);
        }
    }

    public Ancestor GenerateEnemy(int iteration=0)
    {
        GameObject enemy = Instantiate(EnemyPrefab);
        Ancestor enemyAnc;
        enemyAnc = new Ancestor();
        enemyAnc.Name = Utils.GenerateJapaneseName();
        enemyAnc.Iteration = iteration;
        enemyAnc.DifficultyModifier = 0*iteration;
        // Move enemy to random position near player
        enemy.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        enemy.GetComponent<EnemyAI>().Name = enemyAnc.Name;
        enemy.GetComponent<EnemyAI>().Player = Player.transform;
        Utils.GenerateClothes(enemy,enemyAnc);
        Enemies.Add(enemy);
        return enemyAnc;
    }

        public void OnDeath()
    {
        foreach (GameObject enemy in Enemies)
        {
            Destroy(enemy);
        }
        Player.name = "My ghost";
        Enemies.Insert(CurrentStage, Player);
        foreach (GameObject enemy in Enemies)
        {
            Instantiate(enemy);
        }
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


}
