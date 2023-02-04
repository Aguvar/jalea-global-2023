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

    public static void GenerateClothes(GameObject character, Ancestor ancestor)
    {
        SpriteRenderer[] spriteRenderers = character.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.name == "Armor")
            {
                int armorNum = Random.Range(1, 4);
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/armor" + armorNum);
                ancestor.ArmorNum = armorNum;
            }
            else if (spriteRenderer.name == "Body")
            {
                int bodyNum = Random.Range(1, 4);
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/body" + bodyNum);
                ancestor.BodyNum = bodyNum;
            }
            else if (spriteRenderer.name == "Helmet")
            {
                int helmetNum = Random.Range(1, 4);
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/helmet" + helmetNum);
                ancestor.HelmetNum = helmetNum;
            }
        }
    }

    

    
}
