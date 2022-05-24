using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Priority list, used to determine who gets priority in the event of a speed tie
    public GameObject[] Priority;

    //Over all list of characters in the game. Can be adjusted for 2V2, 1V1, or 3V1 battles
    public GameObject[] Characters;

    //Table of characters and when
    public List<GameObject> TurnTable;

    //Used to keep track of how long the game has been going
    public int TurnIndex;

    //Spaces out the attacks, can be adjusted based on animation speed
    public float AttackTimer;

    //Tells who was the last character to attack the previous turn
    public GameObject LastAttack;

    // Start is called before the first frame update
    void Start()
    {

        //All meters start at 0
        TurnIndex = 0;
        AttackTimer = 3;

        
    }

    //Sets up the priority list, then rolls character stats
    public void SetPriority() 
    {
        //Randomize priority table
        for (int i = 0; i < Characters.Length; i++)
        {
            //Values used to determine priority table
            bool Placement = false;
            int pos = 0;

            while (Placement == false)
            {
                //Gives the character a random value between 0 and the length of the character list
                pos = Random.Range(0, Characters.Length);
                //If the randmized spot on the priority list is empty...
                if (Priority[pos] == null)
                {
                    //Place that character on that section of the priority list, then break out of the while loop
                    Priority[pos] = Characters[i];
                    Placement = true;

                    //If that spot has already been taken, repeat until an empty spot is found
                }

            }

            for (int r = 0; r < Characters.Length; r++)
            {
                Characters[r].GetComponent<CharStat>().StatRoll();
            }

        }
    }

    void FixedUpdate()
    {

        

        //Each mater grows based on the Speed value (will use character data)
        for (int i = 0; i < Priority.Length; i++)
        {
            //Fill a character's attack meter, in order of priority
            Priority[i].GetComponent<CharStat>().TurnFill();


            
            //After the Turn Table is first filled...
            if (TurnTable.Count > 0)
            {
                //Attack playing
                AttackTimer -= 0.01f;
                
                //After the attack is finished
                if (AttackTimer <= 0)
                {
                    //Reset attack timer
                    AttackTimer = 2.0f;

                    //Place the character who attacked in the Last Attack value
                    LastAttack = TurnTable[0];
                    
                    //Replace the character who finished attacking with the next character
                    TurnTable.RemoveAt(0);

                    //Indicate that character is no longer attacking
                    LastAttack.GetComponent<CharStat>().Attacking = false;
                }
                
                
            }
            

        }

        
    }
}
