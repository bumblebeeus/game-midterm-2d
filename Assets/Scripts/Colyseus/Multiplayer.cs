using System;
using System.Threading.Tasks;
using Colyseus;


// Singleton
public class Multiplayer
{
    private readonly ColyseusClient client = new("ws://localhost:9999");
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

    public int GetNumberOfPlayer()
    {
        return cachedRoom != null ? cachedRoom.State.players.Count-1 : 0;
    }

    public string GetCurrentSessionId()
    {
        return cachedSessionId ??= cachedRoom.SessionId;
    }
}