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

    private static Mutex _readCountMut = new Mutex();
    private static Mutex _writeCountMut = new Mutex();
    private uint writeCount;
    private uint readCount;

    public const uint MaxThread = 5;

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
        writeCount = 0;
        readCount = 0;
    }

    public void SendCurrentData()
    {
        var t = new Thread(() =>
        {
            _readCountMut.WaitOne();
            var result = readCount < MaxThread;
            if (result)
            {
                readCount += 1;
            }
            _readCountMut.ReleaseMutex();
            if (!result)
            {
                return;
            }
            
            _readMut.WaitOne();
            connHandler.SendBytes(currentPlayer.Serialize());
            _readMut.ReleaseMutex();
            
            _readCountMut.WaitOne();
            readCount -= 1;
            _readCountMut.ReleaseMutex();
        });
        t.Start();
    }

    public void RecvOtherData()
    {
        var t = new Thread(() =>
        {
            _writeCountMut.WaitOne();
            var result = writeCount < MaxThread;
            if (result)
            {
                writeCount += 1;
            }
            _writeCountMut.ReleaseMutex();
            if (!result)
            {
                return;
            }
            
            _writeMut.WaitOne();
            otherPlayer[1] = PlayerInfo.Deserialize(connHandler.RecvBytes());
            SwapBuffer();
            _writeMut.ReleaseMutex();
            
            _writeCountMut.WaitOne();
            writeCount -= 1;
            _writeCountMut.ReleaseMutex();
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
