using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Server;
using UnityEngine;

public class GameState
{
    private ConnectionHandler connHandler;
    private PlayerInfo currentPlayer;
    // Double-Buffer Pattern
    private PlayerInfo[] otherPlayer; // One for current 1 for buffering
    private bool isEnd;
    private bool isWin;

    private static Mutex _readMut = new Mutex();
    private static Mutex _writeMut = new Mutex();

    public GameState(ConnectionHandler connHandler, PlayerInfo currentPlayer, PlayerInfo otherPlayer, bool isEnd = false, bool isWin = false)
    {
        this.connHandler = connHandler;
        this.currentPlayer = currentPlayer;
        this.otherPlayer = new[]
        {
            otherPlayer,
            otherPlayer
        };
        this.isEnd = isEnd;
        this.isWin = isWin;
    }

    public void SendCurrentData()
    {
        var t = new Thread(() =>
        {
            _readMut.WaitOne();
            connHandler.SendBytes(currentPlayer.Serialize());
            _readMut.ReleaseMutex();
        });
        t.Start();
    }

    public void RecvOtherData()
    {
        var t = new Thread(() =>
        {
            _writeMut.WaitOne();
            otherPlayer[1] = PlayerInfo.Deserialize(connHandler.RecvBytes());
            SwapBuffer();
            _writeMut.ReleaseMutex();
        });
        t.Start();
    }

    private void SwapBuffer()
    {
        (otherPlayer[0], otherPlayer[1]) = (otherPlayer[1], otherPlayer[0]);
    }

    public Vector2 GetOtherPosition()
    {
        return otherPlayer[0].Position;
    }

    public void SetCurrentPosition(Vector2 position)
    {
        currentPlayer.Position = position;
    }
}
