using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager.Instance.SaveCheckpoint(
                other.transform.position,
                other.transform.rotation
                );
        }
    }
}