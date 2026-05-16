using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private int nextSceneLoad;
    public GameObject win;

    void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        Time.timeScale = 1;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            win.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(nextSceneLoad);
        Time.timeScale = 1;

        //Setting Int for Index
        if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextSceneLoad);
        }
    }

}