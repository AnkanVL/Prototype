// ================================================
// YOUR UPDATED CheckpointManager.cs
// (Copy-paste the entire script below)
// ================================================

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [Header("References")]
    public Transform player;                    // Drag your Player here (or it auto-finds)
    public Transform monster;                   // Drag your Monster here (optional but recommended)

    [Header("Spawn Settings")]
    public Vector3 defaultSpawnPosition = new Vector3(0, 1, 0);
    public Quaternion defaultSpawnRotation = Quaternion.identity;

    // Saved checkpoint data
    private Vector3 lastCheckpointPos;
    private Quaternion lastCheckpointRot;
    private bool hasCheckpoint = false;

    private Vector3 monsterStartPosition;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (monster == null)
            monster = GameObject.FindGameObjectWithTag("Monster")?.transform;

        if (monster != null)
            monsterStartPosition = monster.position;

        lastCheckpointPos = defaultSpawnPosition;
        lastCheckpointRot = defaultSpawnRotation;
    }

    public void SaveCheckpoint(Vector3 position, Quaternion rotation)
    {
        lastCheckpointPos = position;
        lastCheckpointRot = rotation;
        hasCheckpoint = true;
        Debug.Log($"<color=green>✓ CHECKPOINT SAVED at {position}</color>");
    }

    public void LoadLastCheckpoint()
    {
        if (player == null) return;
        StartCoroutine(DeathFlashAndRespawn());
    }

    // ================================================
    // THIS IS THE ONLY PART YOU NEED TO REPLACE
    // (I already did the replacement for you below)
    // ================================================
    private IEnumerator DeathFlashAndRespawn()
    {
        // Quick death slow-mo effect (feels terrifying)
        Time.timeScale = 0.1f;

        yield return new WaitForSecondsRealtime(0.8f);

        Time.timeScale = 1f;

        // === RESPAWN PLAYER (UPDATED BLOCK) ===
        player.position = lastCheckpointPos;
        player.rotation = lastCheckpointRot;

        // Get the PlayerController and hook the new horror features
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.ResetStamina();                    // Fresh stamina on respawn
            // pc.FreezePlayerForJumpscare();     // (Optional: uncomment if you want extra freeze on wake-up)
        }

        // === RESET MONSTER ===
        if (monster != null)
        {
            monster.position = monsterStartPosition;
            monster.rotation = Quaternion.identity;

            MonsterScript ms = monster.GetComponent<MonsterScript>();
            if (ms != null)
            {
                ms.resetMonster();
            }
        }

        // Unfreeze the player (so you can move again)
        pc?.UnfreezePlayer();

        Debug.Log("<color=cyan>♻️ Respawned at last checkpoint</color>");
    }

    // Optional: Full level restart
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}