using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    void Update()
    {
        string current = SceneManager.GetActiveScene().name;

        if (current == "Title")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(LoadLevelByName("Play"));   // gameplay scene
            }
        }

        else if (current == "GameOver")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(LoadLevelByName("Title"));
            }
        }

        
    }

    
    public void LoadGameOver()
    {
        StartCoroutine(LoadLevelByName("GameOver"));
    }

    IEnumerator LoadLevelByName(string sceneName)
    {
        if (transition != null)
            transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
}
