using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
   
    private static string[] prefixes = new string[] { "Aki", "Hiro", "Kai", "Ryo", "Sakura", "Taka" };
    private static string[] suffixes = new string[] { "hime", "ki", "ko", "masa", "nori", "ya" };
    


    public static string GenerateJapaneseName()
    {
        return prefixes[Random.Range(0, prefixes.Length)] + suffixes[Random.Range(0, suffixes.Length)];
    }

// We need to name the sprites in the folder "Sprites" as "Armor_n", "Weapon_n", "Helmet_n" where n is the number of the sprite or make folders for each part

    public static void GenerateClothes(GameObject character)
    {
        SpriteRenderer[] spriteRenderers = character.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.name == "Armor")
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Armor_" + Random.Range(1, 4));
            }
            else if (spriteRenderer.name == "Weapon")
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Weapon_" + Random.Range(1, 4));
            }
            else if (spriteRenderer.name == "Helmet")
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Helmet_" + Random.Range(1, 4));
            }
        }
    }

    public static GameObject GenerateEnemy()
    {
        GameObject enemy = Instantiate(MainManager.Instance.EnemyPrefab);
        // Move enemy to random position near player
        enemy.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        enemy.GetComponent<EnemyAI>().Name = GenerateJapaneseName();
        enemy.GetComponent<EnemyAI>().Player = MainManager.Instance.Player.transform;
        GenerateClothes(enemy);
        MainManager.Instance.Enemies.Add(enemy);
        return enemy;
    }

    
}
