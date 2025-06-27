using System;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;


    private bool isPaused = false;

    private enum State
    {
        waitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    private State state;

    private float CountdownToStartTimer = 3;
    private float GamePlayingTimer;
    private float GamePlayingTimerMax = 20;

    private void Awake()
    {
        state = State.waitingToStart;
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.playerInputActions.Player.Interact.performed += GameInput_Interact;
    }

    private void GameInput_Interact(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (state == State.waitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.waitingToStart:

                break;
            case State.CountdownToStart:
                CountdownToStartTimer -= Time.deltaTime;
                if (CountdownToStartTimer < 0)
                {
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    GamePlayingTimer = GamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                GamePlayingTimer -= Time.deltaTime;
                if (GamePlayingTimer < 0)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
            default:
                break;
        }
    }


    public bool IsGamePlaying() => state == State.GamePlaying;

    public bool IsCountDownToStartActive() => state == State.CountdownToStart;

    public float GetCountDownToStartTimer() => CountdownToStartTimer;

    public bool IsGameOver() => state == State.GameOver;

    public float GetGamePlayTimerNormalized() => 1 - (GamePlayingTimer / GamePlayingTimerMax);

    public void TogglePauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
