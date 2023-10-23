using System.Collections;
using System.Collections.Generic;
using Server;
using UnityEngine;

public class GameState
{
    private ConnectionHandler connHandler;
    private PlayerInfo currentPlayer;
    private PlayerInfo otherPlayer;
    private bool isEnd;
    private bool isWin;

    public GameState(ConnectionHandler connHandler, PlayerInfo currentPlayer, PlayerInfo otherPlayer, bool isEnd = false, bool isWin = false)
    {
        this.connHandler = connHandler;
        this.currentPlayer = currentPlayer;
        this.otherPlayer = otherPlayer;
        this.isEnd = isEnd;
        this.isWin = isWin;
    }

    public void SendCurrentData()
    {
        connHandler.SendBytes(currentPlayer.Serialize());
    }

    public void RecvOtherData()
    {
        otherPlayer = PlayerInfo.Deserialize(connHandler.RecvBytes());
    }

}
