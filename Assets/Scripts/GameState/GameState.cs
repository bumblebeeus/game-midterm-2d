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

    private static Mutex _rwMut = new Mutex();

    private static Mutex _rwCountMut = new Mutex();
    private uint rwCount;

    public const uint MaxThread = 1;

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
        rwCount = 0;
    }

    public void SendRecvData()
    {
        var t = new Thread(() =>
        {
            _rwCountMut.WaitOne();
            var result = rwCount < MaxThread;
            if (result)
            {
                rwCount += 1;
            }
            _rwCountMut.ReleaseMutex();
            if (!result)
            {
                return;
            }

            _rwMut.WaitOne();
            connHandler.SendBytes(currentPlayer.Serialize());
            otherPlayer[1] = PlayerInfo.Deserialize(connHandler.RecvBytes());
            SwapBuffer();
            _rwMut.ReleaseMutex();
            
            _rwCountMut.WaitOne();
            rwCount -= 1;
            _rwCountMut.ReleaseMutex();
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
