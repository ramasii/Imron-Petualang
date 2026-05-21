using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource;

    public AudioClip[] footstepSFX;
    public AudioClip[] SFX;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // public void PlayFootstep(Tile.TileType type)
    // {
    //     sfxSource.PlayOneShot(footstepSFX[(int)type]);
    // }

    public void PlaySFX(int index)
    {
        sfxSource.PlayOneShot(SFX[index]);
    }
}