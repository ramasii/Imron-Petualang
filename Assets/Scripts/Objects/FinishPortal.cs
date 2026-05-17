using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishPortal : MonoBehaviour
{
    public string nextSceneName;
    private bool isLoading = false;
    public void GoToNextLevel()
    {
        Debug.Log("Go to next level: " + nextSceneName);

        // StartCoroutine(LoadAsync(nextSceneName));
        StartCoroutine(LoadAndHold(nextSceneName));
    }

    // Coroutine untuk memuat scene secara asynchronous
    // mencegah freeze saat loading
    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");
            
            yield return null; 
        }
    }

    IEnumerator LoadAndHold(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
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
            GoToNextLevel();
        }
    }
}
