using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnTouch : MonoBehaviour
{
    ///// restart the scene when the player touches the object with this script attached
    private void OnTriggerEnter2D(Collider2D other)
    {
        ///// player need this tag to trigger the restart
        if (other.CompareTag("Player"))
        {
            RestartScene();
        }
    }

    void RestartScene()
    {
        ///// the scene has to be in the build profiles to be able to load it
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}