using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource;

    public AudioClip[] footstepSFX;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayFootstep(Tile.TileType type)
    {
        sfxSource.PlayOneShot(footstepSFX[(int)type]);
    }
}