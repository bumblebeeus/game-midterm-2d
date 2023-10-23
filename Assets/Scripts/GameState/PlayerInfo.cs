using FlexBuffers;
using UnityEngine;

public struct PlayerInfo
{
    public Vector2 Position;
    public string Name;
    public uint SkinId;

    public byte[] Serialize()
    {
        var currentPos = this.Position;
        var currentName = this.Name;
        var currentSkinId = this.SkinId;
        return FlexBufferBuilder.Map(root =>
        {
            root.Add("pX", currentPos.x);
            root.Add("pY", currentPos.y);
            root.Add("n", currentName);
            root.Add("sk", currentSkinId);
        });
    }

    public static PlayerInfo Deserialize(byte[] data)
    {
        var buffer = FlxValue.FromBytes(data);
        var info = new PlayerInfo();
        info.Position = new Vector2();
        info.Position.x = (float)buffer["pX"].AsDouble;
        info.Position.y = (float)buffer["pY"].AsDouble;
        info.Name = buffer["n"].AsString;
        info.SkinId = (uint)buffer["sk"].AsULong;
        return info;
    }
}