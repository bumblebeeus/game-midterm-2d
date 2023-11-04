// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.21
// 

using Colyseus.Schema;
using Action = System.Action;

public partial class GameStateSchema : Schema {
	[Type(0, "map", typeof(MapSchema<PlayerSchema>))]
	public MapSchema<PlayerSchema> players = new MapSchema<PlayerSchema>();

	[Type(1, "boolean")]
	public bool isStart = default(bool);

	[Type(2, "uint32")]
	public uint mapId = default(uint);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<MapSchema<PlayerSchema>> __playersChange;
	public Action OnPlayersChange(PropertyChangeHandler<MapSchema<PlayerSchema>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.players));
		__playersChange += __handler;
		if (__immediate && this.players != null) { __handler(this.players, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(players));
			__playersChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<bool> __isStartChange;
	public Action OnIsStartChange(PropertyChangeHandler<bool> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.isStart));
		__isStartChange += __handler;
		if (__immediate && this.isStart != default(bool)) { __handler(this.isStart, default(bool)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(isStart));
			__isStartChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<uint> __mapIdChange;
	public Action OnMapIdChange(PropertyChangeHandler<uint> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.mapId));
		__mapIdChange += __handler;
		if (__immediate && this.mapId != default(uint)) { __handler(this.mapId, default(uint)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(mapId));
			__mapIdChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(players): __playersChange?.Invoke((MapSchema<PlayerSchema>) change.Value, (MapSchema<PlayerSchema>) change.PreviousValue); break;
			case nameof(isStart): __isStartChange?.Invoke((bool) change.Value, (bool) change.PreviousValue); break;
			case nameof(mapId): __mapIdChange?.Invoke((uint) change.Value, (uint) change.PreviousValue); break;
			default: break;
		}
	}
}

