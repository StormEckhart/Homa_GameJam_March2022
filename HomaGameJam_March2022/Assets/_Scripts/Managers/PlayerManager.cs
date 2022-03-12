using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonManager<PlayerManager>
{
    [Header("Components")]

    [ReadOnly]
    [Tooltip("The current player")]
    [SerializeField]
    private PlayerController m_CurrentPlayer;
    public PlayerController currentPlayer => m_CurrentPlayer;



    //To set which player is the current player
    private void SetCurrentPlayerController(PlayerController i_CurrentPlayerController)
    {
        m_CurrentPlayer = i_CurrentPlayerController;
    }


    //To disable the current player in the current level
    public void DisablePlayer()
    {
        currentPlayer.gameObject.SetActive(false);

        SetCurrentPlayerController(null);
    }
    //To "spawn" the current player in the current level
    public void SpawnPlayer()
    {
        LevelManager.Instance.currentLevel.Player.transform.position = LevelManager.Instance.currentLevel.SpawnPoint;
        LevelManager.Instance.currentLevel.Player.gameObject.SetActive(true);
        LevelManager.Instance.currentLevel.Player.ResetPlayer();

        SetCurrentPlayerController(LevelManager.Instance.currentLevel.Player);
    }
}
