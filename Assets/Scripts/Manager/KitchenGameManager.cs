using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager Instance;

    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameUnPaused;
    public event EventHandler OnMultiplayerGamePaused;
    public event EventHandler OnMultiplayerGameUnPaused;
    public event EventHandler OnLocalPlayerReadyChanged;

    [SerializeField] private Transform playerPrefab;

    private enum State
    {
        waitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    private bool isLocalGamePaused = false;
    private bool autoTestGamePausedState;
    private bool isLocalPlayerReady;
    private float GamePlayingTimerMax = 10;
    private NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);
    private NetworkVariable<State> state = new NetworkVariable<State>(State.waitingToStart);
    private NetworkVariable<float> CountdownToStartTimer = new NetworkVariable<float>(3);
    private NetworkVariable<float> GamePlayingTimer = new NetworkVariable<float>(0);
    private Dictionary<ulong, bool> playerReadyDictionary;
    private Dictionary<ulong, bool> playerPausedDictionary;

    //----------------------------------------------------

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerPausedDictionary = new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.playerInputActions.Player.Interact.performed += GameInput_Interact;
    }

    private void Update()
    {
        if (!IsServer) return;

        switch (state.Value)
        {
            case State.waitingToStart:

                break;
            case State.CountdownToStart:
                CountdownToStartTimer.Value -= Time.deltaTime;
                if (CountdownToStartTimer.Value < 0)
                {
                    state.Value = State.GamePlaying;
                    GamePlayingTimer.Value = GamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                GamePlayingTimer.Value -= Time.deltaTime;
                if (GamePlayingTimer.Value < 0)
                {
                    state.Value = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
            default:
                break;
        }
    }


    private void LateUpdate()
    {
        if (autoTestGamePausedState)
        {
            autoTestGamePausedState = false;
            TestGamePausedState();
        }
    }

    //-----------------------------------------------------

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        isGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, false);
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong cliendId)
    {
        autoTestGamePausedState = true;
    }

    private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        IsGamePaused_OnValueChanged(previousValue, newValue, isGamePaused);
    }

    private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue, NetworkVariable<bool> isGamePaused)
    {
        if (isGamePaused.Value)
        {
            Time.timeScale = 0f;
            OnMultiplayerGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnMultiplayerGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void State_OnValueChanged(State previousValue, State newValue) => OnStateChanged?.Invoke(this, EventArgs.Empty);

    private void GameInput_Interact(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (state.Value == State.waitingToStart)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allPlayerReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                // Player is not ready
                allPlayerReady = false;
                break;
            }
        }

        if (allPlayerReady)
            state.Value = State.CountdownToStart;
    }


    private void GameInput_OnPauseAction(object sender, EventArgs e) => TogglePauseGame();



    public bool IsGamePlaying() => state.Value == State.GamePlaying;
    public bool IsLocalPlayerReady() => isLocalPlayerReady;
    public bool IsCountDownToStartActive() => state.Value == State.CountdownToStart;
    public bool IsGameOver() => state.Value == State.GameOver;


    public float GetCountDownToStartTimer() => CountdownToStartTimer.Value;

    public float GetGamePlayTimerNormalized() => 1 - (GamePlayingTimer.Value / GamePlayingTimerMax);

    public void TogglePauseGame()
    {
        isLocalGamePaused = !isLocalGamePaused;
        if (isLocalGamePaused)
        {
            PauseGameServerRpc();
            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnpauseGameServerRpc();
            OnLocalGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;
        TestGamePausedState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;
        TestGamePausedState();
    }

    public bool IsWaitingToStart()
    {
        return state.Value == State.waitingToStart;
    }

    private void TestGamePausedState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId])
            {
                // This player is paused
                isGamePaused.Value = true;
                return;
            }
        }

        // All players are unpaused
        isGamePaused.Value = false;
    }

}
