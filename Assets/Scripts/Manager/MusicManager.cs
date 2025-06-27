using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{

    public static MusicManager Instance;

    private AudioSource audioSource;
    private float volume;
    private const string PLAYER_PREF_MUSIC_VOLUME = "MusicVolume";

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        volume =  PlayerPrefs.GetFloat(PLAYER_PREF_MUSIC_VOLUME, 1);
        audioSource.volume = volume;
    }

    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1)
        {
            volume = 0;
        }
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREF_MUSIC_VOLUME,volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
