using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Player
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    public float playerHealth;

    // Checkpoint
    public int checkpointID;         // which checkpoint is active
    public float checkpointPosX;     // optionally store position
    public float checkpointPosY;
    public float checkpointPosZ;

    // Weapons unlocked
    public bool swordUnlocked;
    public bool magicUnlocked;

    // Add more fields if needed
}

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    private string saveFilePath;

    private void Awake()
    {
        // Basic singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.xml");
        Debug.Log("Save Path: " + saveFilePath);
    }

    // ------------------- SAVE -------------------
    public void SaveGame()
    {
        SaveData data = GatherGameData();

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            using (FileStream stream = new FileStream(saveFilePath, FileMode.Create))
            {
                serializer.Serialize(stream, data);
            }
            Debug.Log("Game saved successfully to " + saveFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving game: " + e.Message);
        }
    }

    // Build our SaveData from the current scene
    private SaveData GatherGameData()
    {
        SaveData data = new SaveData();

        // Example: find the player
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            Vector3 pPos = player.transform.position;
            data.playerPosX = pPos.x;
            data.playerPosY = pPos.y;
            data.playerPosZ = pPos.z;

            // If you have a PlayerHealth script
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            data.playerHealth = (ph != null) ? ph.GetCurrentHealth() : 100f;

            // If your PlayerController stores the weapon unlock states:
            data.swordUnlocked = player.swordUnlocked;
            data.magicUnlocked = player.magicUnlocked;
        }

        // Example: store current checkpoint
        // If using a GameManager storing activeCheckpointID & position
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            data.checkpointID = gm.activeCheckpointID;
            data.checkpointPosX = gm.activeCheckpointPos.x;
            data.checkpointPosY = gm.activeCheckpointPos.y;
            data.checkpointPosZ = gm.activeCheckpointPos.z;
        }

        return data;
    }

    // ------------------- LOAD -------------------
    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("No save file found at: " + saveFilePath);
            return;
        }

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            using (FileStream stream = new FileStream(saveFilePath, FileMode.Open))
            {
                SaveData data = (SaveData)serializer.Deserialize(stream);
                ApplyGameData(data);
            }
            Debug.Log("Game loaded successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading game: " + e.Message);
        }
    }

    private void ApplyGameData(SaveData data)
    {
        // Find the player
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            Vector3 loadedPos = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
            player.transform.position = loadedPos;
            Debug.Log("Setting Player Pos to:" + loadedPos);

            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.SetCurrentHealth(data.playerHealth); // you might need a setter
                Debug.Log("Setting Player health to " + data.playerHealth);
            }

            // Restore weapon unlock states
            player.swordUnlocked = data.swordUnlocked;
            player.magicUnlocked = data.magicUnlocked;
            Debug.Log("Setting swordUnlocked=" + data.swordUnlocked);
            Debug.Log("Setting magicUnlocked=" + data.magicUnlocked);
        }

        // Restore checkpoint
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.activeCheckpointID = data.checkpointID;
            gm.activeCheckpointPos = new Vector3(data.checkpointPosX, data.checkpointPosY, data.checkpointPosZ);
        }
    }
}
