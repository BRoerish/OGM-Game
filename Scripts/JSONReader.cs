using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    //Place JSON file here!!
    public TextAsset jsonfile;

    public GameObject GM;

    //Make sure variables here matches the variables in the JSON file
    [System.Serializable]
    public class JSONData
    {
        public int TokenID;
        public string Sex;
        public string Class;
        public int HP;
        public int ATKMax;
        public int ATKMin;
        public int Rate;
    }

    //Make a list of characters, using the JSON data
    [System.Serializable]
    public class CharacterData
    {
        public JSONData[] Character;
    }

    //Houses the list of characters
    public CharacterData MyCharacter = new CharacterData();

    JSONData jsoninfo = new JSONData();

    // Start is called before the first frame update
    void Start()
    {
        //Fills the character list with data from the JSON file
        MyCharacter = JsonUtility.FromJson<CharacterData>(jsonfile.text);
        GM.GetComponent<GameManager>().SetPriority();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
