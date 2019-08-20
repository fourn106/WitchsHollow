using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    public enum CombatStates
    {
        START,
        PLAYERTURN,
        ENEMYTURN,
        WIN,
        LOSE
    }

    public bool enemyAttack = true;
    public Dropdown combatDrop;
    public Dropdown itemDrop;
    public GameObject currentUnitArrow;

    public int currentParty;
    public int currentEnemy;

    public List<GameObject> Party = new List<GameObject>();
    public List<GameObject> PartyLeft = new List<GameObject>();
    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> EnemiesLeft = new List<GameObject>();


    public CombatStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = CombatStates.START;
        currentUnitArrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (CombatStates.START):
                // Start of combat
                currentUnitArrow.SetActive(true);
                currentState = CombatStates.PLAYERTURN;
                break;
            case (CombatStates.PLAYERTURN):
                // Waiting for player input

                // Sets the currently active unit arrow above the player unit
                if (currentParty < PartyLeft.Count && currentUnitArrow.transform.parent != PartyLeft[currentParty].transform)
                {
                    currentUnitArrow.transform.SetParent(PartyLeft[currentParty].transform);
                    currentUnitArrow.transform.position = currentUnitArrow.transform.parent.position;
                }
                
                // If COMBAT is not selected and the player clicks an enemy, the player attacks
                if (combatDrop.value != 0)
                {
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        foreach (GameObject enem in EnemiesLeft)
                        {
                            if (enem.GetComponent<Collider2D>().OverlapPoint(worldPos))
                            {
                                //Attack
                                Debug.Log("attacking");
                                GameObject mem = Party[currentParty];
                                StartCoroutine(AnimateAttack(true, mem, enem));

                                // Call function that calculates if attack hits or not, calculates damage, and outputs true or false which determines if enemy takes damage

                                
                                itemDrop.value = 0;
                                combatDrop.value = 0;
                            }
                        }
                    }
                }

                if (itemDrop.value != 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        foreach (GameObject mem in PartyLeft)
                        {
                            if (mem.GetComponent<Collider2D>().OverlapPoint(worldPos))
                            {
                                //Using an item
                                Debug.Log("using an item");
                                Animator pAnim = mem.GetComponent<Animator>();
                                StartCoroutine(AnimateReceiveSupport(pAnim));

                                itemDrop.value = 0;
                                combatDrop.value = 0;
                            }
                        }
                    }
                }

                if (combatDrop.transform.Find("Dropdown List") != null)
                {
                    itemDrop.value = 0;
                }
                if(itemDrop.transform.Find("Dropdown List") != null)
                {
                    combatDrop.value = 0;
                }

                if (currentParty >= PartyLeft.Count)
                {
                    currentEnemy = 0;
                    currentState = CombatStates.ENEMYTURN;
                }


                break;
            case (CombatStates.ENEMYTURN):
                // Waiting for enemy to attack

                // Sets the currently active unit arrow above the enemy unit
                if (currentEnemy < EnemiesLeft.Count && currentUnitArrow.transform.parent != EnemiesLeft[currentEnemy].transform)
                {
                    currentUnitArrow.transform.SetParent(EnemiesLeft[currentEnemy].transform);
                    currentUnitArrow.transform.position = currentUnitArrow.transform.parent.position;
                }



                
                if (currentEnemy >= EnemiesLeft.Count)
                {
                    currentParty = 0;
                    currentState = CombatStates.PLAYERTURN;
                }
                else
                {
                    GameObject partyMem = PartyLeft[Random.Range(0, PartyLeft.Count)];
                    GameObject enemy = EnemiesLeft[currentEnemy];
                    if (enemyAttack)
                    {
                        StartCoroutine(AnimateAttack(false, enemy, partyMem));
                    }
                }

                break;
            case (CombatStates.WIN):
                // If the enemies are all at ZERO health
                currentUnitArrow.SetActive(false);
                break;
            case (CombatStates.LOSE):
                // If the player's party is all at ZERO health
                currentUnitArrow.SetActive(false);
                break;
            default:
                break;
        }
    }

    // Animates the attack for the player
    public IEnumerator AnimateAttack(bool playerCombat, GameObject attacker, GameObject defender)
    {
        Animator attackerAnim = attacker.GetComponent<Animator>();
        Animator defenderAnim = defender.GetComponent<Animator>();

        Character defenderChar = defender.GetComponent<Character>();
        float chanceToHit;

        if (playerCombat)
        {
            switch (combatDrop.value)
            {
                case 1:
                    chanceToHit = 100;
                    break;
                case 2:
                    //strength += 2;
                    break;
                case 3:
                    //
                    break;
                default:
                    break;
            }
        }

        enemyAttack = false;

        attackerAnim.SetBool("Attacking", true);

        yield return new WaitForSeconds(0.4f);

        // If damage is dealt
        defenderAnim.SetBool("TakingDamage", true);

        yield return new WaitForSeconds(0.1f);

        attackerAnim.SetBool("Attacking", false);
        defenderAnim.SetBool("TakingDamage", false);

        yield return new WaitForSeconds(0.5f);

        enemyAttack = true;

        currentParty += 1;
        currentEnemy += 1;
    }

    // Animates when a character uses/receives healing or an item
    public IEnumerator AnimateReceiveSupport(Animator receiver)
    {
        receiver.SetBool("GettingSupport", true);

        yield return new WaitForSeconds(0.1f);

        receiver.SetBool("GettingSupport", false);

        yield return new WaitForSeconds(0.9f);

        currentParty += 1;
        currentEnemy += 1;
    }

    public void Flee()
    {
        SceneManager.LoadScene("World");
    }
}
