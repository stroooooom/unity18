using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanksGameManager : Singleton<TanksGameManager> {

    public enum GameMode
    {
        SINGLEPLAYER,
        MULTIPLAYER
    }

    public GameMode gameMode = GameMode.MULTIPLAYER;

    public bool multiplayerEnabled()
    {
        return gameMode == GameMode.MULTIPLAYER;
    }
}
