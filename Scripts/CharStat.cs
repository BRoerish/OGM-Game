using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharStat : MonoBehaviour
{

    //Character identifier
    public int index;
    public int TeamNum;

    //Used to roll attack damage
    public int AtkMax;
    public int AtkMin;

    // HP values
    public float HPCurrent;
    public float HPMax;
    public Image HealthBar;
    public TextMeshProUGUI HPText;

    //Speed values
    public float Rate;
    public Image AttackMeter;
    //In case of speed buff
    public int SpeedMod;

    //Talks to the GameManager
    public GameObject GM;

    //Keeps tabs of enemy stats
    public GameObject[] EnemyTeam;

    //Toggles if the character is attacking or not
    public bool Attacking;

    //Character is in play, meaning they can make actions or be targeted
    public bool Active;

    //Collection of elements
    public enum Element {Earth, Wind, Fire, Water, Electricity};

    //Determines the character's element
    public Element Elem;

    //Read the JSON file to collect data on the character
    public GameObject JSONR;

    // Start is called before the first frame update
    void Start()
    {
        
        //Read the JSON file to determine this character's stats
        for (int i = 0; i < JSONR.GetComponent<JSONReader>().MyCharacter.Character.Length; i++)
        {
            Debug.Log(gameObject.name);
            //If the character's Index value matches the Token ID, then match the character's stats
            if (index == JSONR.GetComponent<JSONReader>().MyCharacter.Character[i].TokenID)
            {
                //Debug.Log("Match found");
                AtkMax = JSONR.GetComponent<JSONReader>().MyCharacter.Character[i].ATKMax;
                AtkMin = JSONR.GetComponent<JSONReader>().MyCharacter.Character[i].ATKMin;
                HPMax = JSONR.GetComponent<JSONReader>().MyCharacter.Character[i].HP;
                HPMax = JSONR.GetComponent<JSONReader>().MyCharacter.Character[i].HP;
                Rate = JSONR.GetComponent<JSONReader>().MyCharacter.Character[i].Rate;
            }
        }
        
        //Set current HP to max
        HPCurrent = HPMax;
        //Keep them from attacking out of turn
        Attacking = false;
        //Set the character to active
        Active = true;
        //Normalize the meter's fill
        AttackMeter.fillAmount = 0;
        //Normalize speed value, can be adjusted with abilities
        SpeedMod = 1;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If this character is next in line to attack
        if (GM.GetComponent<GameManager>().TurnTable.Count > 0 && GM.GetComponent<GameManager>().TurnTable[0]==this.gameObject && Attacking==false)
        {
            //Debug.Log(this.gameObject + "'s Turn");

            //This character is attacking
            Attacking = true;
            Target();
        }

        //If HP is 0, the character is considered KOed
        if (HPCurrent<=0)
        {
            //Debug.Log(this.gameObject.name + " is dead");
            Active = false;
        }

        //Reduce HP fill value based on current HP. Fill can not go bellow 0
        if (HPCurrent / HPMax < HealthBar.fillAmount)
        {
            HealthBar.fillAmount -= 0.001f;
        }

        HPText.text = HPCurrent.ToString();

        
    }

    public void TurnFill()
    {
        //So long as the character is not KOed...
        if (Active == true)
        {
            //If the meter isn't full..
            if (AttackMeter.fillAmount != 1)
            {
                //Increase fill based on the character's speed (include any speed modifications) divided by a flate rate
                AttackMeter.fillAmount += (Rate * SpeedMod) / 180.0f;
            }
            else
            {
                //Once the meter is full, add this character to the TurnTable, then reset the fill amount
                GM.GetComponent<GameManager>().TurnTable.Add(this.gameObject);
                AttackMeter.fillAmount = 0;
            }
        }
        else
        {
            //Default the fill to 0, if the character is not active
            AttackMeter.fillAmount = 0;

        }


    }

    void Target()
    {
        //Defaults target to the 1st character on the enemy list
        GameObject AttackTarget=EnemyTeam[0];
        
        //Check enemies CurrentHP value, target the character with the lowest value
        for(int i = 0; i < EnemyTeam.Length; i++)
        {
            if (EnemyTeam[i].GetComponent<CharStat>().HPCurrent >= AttackTarget.GetComponent<CharStat>().HPCurrent)
            {
                AttackTarget = EnemyTeam[i];
            }

        }

        //Debug.Log("Target:" + AttackTarget);

        //Run the Attack function, targeting the designated opponent
        Attack(AttackTarget);
    }

    void Attack(GameObject FinalTarget)
    {
        //Roll damage here!!
        int Damage = Random.Range(AtkMin, AtkMax);

        //Target takes damage based on damage roll
        FinalTarget.GetComponent<CharStat>().HPCurrent -= Damage;

        //Debug.Log(this.gameObject.name + " did " + Damage + " to " + FinalTarget.name);
        Debug.Log(this.gameObject.name + " is attacking");

        //Increase the number of turns pased
        GM.GetComponent<GameManager>().TurnIndex++;

        //Switch condition to check elements of this character and the opponent

        /*
        switch (FinalTarget.GetComponent<CharStat>().Elem)
        { 
            case Element.Earth:
                Debug.Log(FinalTarget.name + " is Earth aligned");
                break;

            case Element.Electricity:
                Debug.Log(FinalTarget.name + " is Electricity aligned");
                break;

            case Element.Fire:
                Debug.Log(FinalTarget.name + " is Fire aligned");
                break;

            case Element.Water:
                Debug.Log(FinalTarget.name + " is Water aligned");
                break;

            case Element.Wind:
                Debug.Log(FinalTarget.name + " is Wind aligned");
                break;

        }
        */

    }
}
