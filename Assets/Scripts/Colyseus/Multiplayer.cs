using System;
using System.Threading.Tasks;
using Colyseus;


// Singleton
public class Multiplayer
{
    private readonly ColyseusClient client = new("wss://hcmusproject.live");
    private ColyseusRoom<GameStateSchema> cachedRoom;
    private string cachedSessionId;

    public async Task<string> CreateRoom()
    {
        cachedRoom ??= await client.Create<GameStateSchema>("game");
        return cachedRoom.RoomId;
    }

    public async Task JoinRoom(string roomId)
    {
        cachedRoom ??= await client.JoinById<GameStateSchema>(roomId);
    }

    public void Disconnect()
    {
        cachedRoom?.Leave();
    }

    public async Task SendData(string message, object data)
    {
        if (cachedRoom != null)
        {
            await cachedRoom.Send(message, data);
        }
    }

    public void OnStart(Action callback)
    {
        cachedRoom?.State.OnIsStartChange((newVal, _) =>
        {
            if (newVal)
                callback();
        });
    }

    public void OnMapChange(Action<uint> callback)
    {
        cachedRoom?.State.OnMapIdChange((newVal, _) =>
        {
            callback(newVal);
        });
    }

    public void OnChange(Action<GameStateSchema, bool> callback)
    {
        if (cachedRoom != null)
        {
            cachedRoom.OnStateChange += (state, firstState) =>
            {
                callback(state, firstState);
            };
        }
    }

    public void LoopThrough(Action<string, PlayerSchema> callback)
    {
        if (cachedRoom != null)
        {
            cachedRoom.State.players.ForEach(callback);
        }
    }

    public void OnWin(Action<string> callback)
    {
        if (cachedRoom != null)
        {
            cachedRoom.State.OnSessionIdWinChange((newVal, _) =>
            {
                callback(newVal);
            });
        }
    }

    public int GetNumberOfPlayer()
    {
        return cachedRoom != null ? cachedRoom.State.players.Count-1 : 0;
    }

    public string GetCurrentSessionId()
    {
        return cachedSessionId ??= cachedRoom.SessionId;
    }
}