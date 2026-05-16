using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Button[] lvlButtons;

    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 2);

        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i + 2 > levelAt)
            {
                lvlButtons[i].interactable = false;
            }
        }
    }
    public void resetLevel()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
}
