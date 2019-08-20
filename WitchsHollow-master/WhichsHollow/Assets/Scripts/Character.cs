using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public enum CharacterClass
    {
        KNIGHT,
        ROGUE,
        WIZARD
    }

    public int level { get; set; }
    public int currentXP { get; set; }
    public int xpLevelCap { get; set; }
    public int maxHealth { get; set; }
    public int currentHealth { get; set; }
    public int bonusHealth { get; set; }
    public int strength { get; set; }
    public int bonusStrength { get; set; }
    public int armor { get; set; }
    public int bonusArmor { get; set; }
    public int xpPayout { get; set; }
    public List<GameObject> itemDrops = new List<GameObject>();
    public GameObject model { get; set; }
    public CharacterClass CharClass;

public void UpdateXP(int newXP)
    {
        currentXP += newXP;
        if (currentXP >= xpLevelCap)
        {
            currentXP -= xpLevelCap;
            level++;
            xpLevelCap = 15 * level;

            int newStat = Random.Range(0, 2);
            switch (newStat)
            {
                case 0:
                    maxHealth += 15;
                    currentHealth += 15;
                    break;
                case 1:
                    strength += 2;
                    break;
                case 2:
                    armor++;
                    break;
                default:
                    break;
            }
        }
    }
}
