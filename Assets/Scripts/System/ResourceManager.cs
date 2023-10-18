//using System.Collections.Generic;
//using UnityEngine;

//public class ResourceManager : MonoBehaviour
//{
//    // Luettelo pelaajahahmoista
//    public List<PlayerCharacterSO> availableCharacters = new List<PlayerCharacterSO>();

//    // Pelaajan valitsema hahmo ja taso
//    public PlayerCharacterSO selectedCharacter;
//    public int selectedCharacterLevel = 1;

//    // Singleton-instanssi ResourceManager-luokasta
//    public static ResourceManager Instance;

//    private void Awake()
//    {
//        // Singleton-instanenssin asettaminen
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }

//        // Lataa pelaajahahmot resursseista
//        LoadPlayerCharacters();
//    }

//    // Lataa pelaajahahmot resursseista
//    private void LoadPlayerCharacters()
//    {
//        PlayerCharacterSO[] characters = Resources.LoadAll<PlayerCharacterSO>("PlayerCharacters");

//        if (characters.Length > 0)
//        {
//            availableCharacters.AddRange(characters);
//        }
//    }

//    // Valitse pelaajahahmo
//    public void SelectCharacter(PlayerCharacterSO character)
//    {
//        selectedCharacter = character;
//        selectedCharacterLevel = 1;
//    }

//    // Päivitä pelaajahahmon tasoa
//    public void UpdateCharacterLevel(int newLevel)
//    {
//        selectedCharacterLevel = Mathf.Max(newLevel, 1);
//    }
//}
