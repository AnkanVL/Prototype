using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public int sceneToLoad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        SceneManager.LoadScene(sceneToLoad);
    }
}
