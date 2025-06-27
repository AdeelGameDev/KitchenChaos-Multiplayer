using UnityEngine;

public class StoveCounterAudioSource : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;
    private float warningSountTimer;
    private bool playWarningSound;


    private void Awake() => audioSource = GetComponent<AudioSource>();

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void Update()
    {
        if(playWarningSound)
        {
            warningSountTimer -= Time.deltaTime;
            float warningSoundTimerMax = .5f;
            if (warningSountTimer < 0)
            {
                warningSountTimer = warningSoundTimerMax;
                AudioManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
        
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized > burnShowProgressAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Cooked;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }


    }
}
