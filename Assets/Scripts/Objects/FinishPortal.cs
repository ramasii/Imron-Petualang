using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishPortal : MonoBehaviour
{
    private int nextSceneLoad;
    public Transition transition;
    private bool isLoading = false;
    private Gameflow gameflow;
    void Awake()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        gameflow = FindAnyObjectByType<Gameflow>();
        if (gameflow)
        {
            gameflow.onLevelFinish += GoToNextLevel;
        }
    }
    public void GoToNextLevel()
    {
        // Debug.Log("Go to next level: " + nextSceneName);

        transition.StartTransition();
        StartCoroutine(LoadAndHold(nextSceneLoad));
        if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextSceneLoad);
        }
    }

    // Coroutine untuk memuat scene secara asynchronous
    // mencegah freeze saat loading
    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");
            
            yield return null; 
        }
    }

    IEnumerator LoadAndHold(int sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);

        Instantiate(transition, Vector3.zero, Quaternion.identity); // Buat instance transisi

        op.allowSceneActivation = false; // Tahan scene agar tidak langsung pindah

        while (op.progress < 0.9f)
        {
            yield return null;
        }

        // Tunggu sampai animasi Fade Out kamu selesai
        yield return new WaitForSeconds(1f); 

        op.allowSceneActivation = true; // Sekarang baru pindah
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isLoading)
        {
            isLoading = true;
            collision.gameObject.GetComponent<Player>().canMove = false; // Disable player movement

            if(gameflow) gameflow.FinishLevel(); // Trigger event level finish, yang akan memanggil method GoToNextLevel() pada Gameflow
        }
    }
}
