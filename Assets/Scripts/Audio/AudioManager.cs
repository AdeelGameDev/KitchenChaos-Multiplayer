using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClipsRefsSO audioClipsRefs;

    public static AudioManager Instance { get; private set; }

    private float volume;
    private const string PLAYER_PREF_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    private void Awake()
    {
        Instance = this;//

        volume = PlayerPrefs.GetFloat(PLAYER_PREF_SOUND_EFFECTS_VOLUME, 1);
    }

    private void Start()
    {
        Player.OnAnyPickedSomething += Player_OnPickedSomething;

        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFail += DeliveryManager_OnRecipeFail;

        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.onAnyObjectTrashed += TrashCounter_onAnyObjectTrashed;
    }

    private void TrashCounter_onAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipsRefs.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipsRefs.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(audioClipsRefs.objectPickup, player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipsRefs.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFail(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefs.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefs.deliverySuccess, deliveryCounter.transform.position);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultipliyer = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultipliyer * volume);
    }
    public void PlaySound(AudioClip[] audioClips, Vector3 position, float volumeMultipliyer = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Length)], position, volumeMultipliyer * volume);
    }

    public void PlayFootStepSound(Vector3 position, float volume)
    {
        PlaySound(audioClipsRefs.footSteps, position, volume);
    }

    public void PlayPopupSound()
    {
        PlaySound(audioClipsRefs.warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position, float volume = 1)
    {
        PlaySound(audioClipsRefs.warning, position, volume);
    }


    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1)
        {
            volume = 0;
        }
        PlayerPrefs.SetFloat(PLAYER_PREF_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
