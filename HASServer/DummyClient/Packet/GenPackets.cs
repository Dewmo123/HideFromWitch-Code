using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

public enum PacketID
{
	C_RoomEnter = 1,
	C_GameStart = 2,
	C_SetName = 3,
	S_RoomEnter = 4,
	S_RoomEnterFirst = 5,
	C_RoomExit = 6,
	S_RoomExit = 7,
	C_CreateRoom = 8,
	S_RoomList = 9,
	S_PacketResponse = 10,
	C_RoomList = 11,
	S_Chat = 12,
	C_Chat = 13,
	C_Move = 14,
	C_Rotate = 15,
	S_Move = 16,
	S_SyncTimer = 17,
	S_UpdateRoomState = 18,
	S_RoundStart = 19,
	S_ResetGame = 20,
	C_Shoot = 21,
	S_PlayAnimation = 22,
	C_Attack = 23,
	S_Shoot = 24,
	C_ResourceLoadComplete = 25,
	S_Dead = 26,
	S_ChangeModel = 27,
	S_Teleport = 28,
	C_UseSkill = 29,
	S_UseSkill = 30,
	C_Scanned = 31,
	S_Scanned = 32,
	S_MoveLock = 33,
	C_MoveLock = 34,
	C_Fall = 35,
	S_RoomStateChange = 36,
	S_HostChange = 37,
	S_SetSkill = 38,
	S_GameEnd = 39,
	
}

public struct VectorPacket : IDataPacket
{
	public float x;
	public float y;
	public float z;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadFloatData(segment, count, out x);
		count += PacketUtility.ReadFloatData(segment, count, out y);
		count += PacketUtility.ReadFloatData(segment, count, out z);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendFloatData(this.x, segment, count);
		count += PacketUtility.AppendFloatData(this.y, segment, count);
		count += PacketUtility.AppendFloatData(this.z, segment, count);
		return (ushort)(count - offset);
	}
}

public struct QuaternionPacket : IDataPacket
{
	public float x;
	public float y;
	public float z;
	public float w;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadFloatData(segment, count, out x);
		count += PacketUtility.ReadFloatData(segment, count, out y);
		count += PacketUtility.ReadFloatData(segment, count, out z);
		count += PacketUtility.ReadFloatData(segment, count, out w);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendFloatData(this.x, segment, count);
		count += PacketUtility.AppendFloatData(this.y, segment, count);
		count += PacketUtility.AppendFloatData(this.z, segment, count);
		count += PacketUtility.AppendFloatData(this.w, segment, count);
		return (ushort)(count - offset);
	}
}

public struct RoomInfoPacket : IDataPacket
{
	public int roomId;
	public int maxCount;
	public int currentCount;
	public string roomName;
	public string hostName;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out roomId);
		count += PacketUtility.ReadIntData(segment, count, out maxCount);
		count += PacketUtility.ReadIntData(segment, count, out currentCount);
		count += PacketUtility.ReadStringData(segment, count, out roomName);
		count += PacketUtility.ReadStringData(segment, count, out hostName);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.roomId, segment, count);
		count += PacketUtility.AppendIntData(this.maxCount, segment, count);
		count += PacketUtility.AppendIntData(this.currentCount, segment, count);
		count += PacketUtility.AppendStringData(this.roomName, segment, count);
		count += PacketUtility.AppendStringData(this.hostName, segment, count);
		return (ushort)(count - offset);
	}
}

public struct PlayerNamePacket : IDataPacket
{
	public string nickName;
	public int index;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadStringData(segment, count, out nickName);
		count += PacketUtility.ReadIntData(segment, count, out index);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendStringData(this.nickName, segment, count);
		count += PacketUtility.AppendIntData(this.index, segment, count);
		return (ushort)(count - offset);
	}
}

public struct PlayerInitPacket : IDataPacket
{
	public int index;
	public int modelIndex;
	public string name;
	public VectorPacket position;
	public QuaternionPacket rotation;
	public ushort role;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out index);
		count += PacketUtility.ReadIntData(segment, count, out modelIndex);
		count += PacketUtility.ReadStringData(segment, count, out name);
		count += PacketUtility.ReadDataPacketData(segment, count, out position);
		count += PacketUtility.ReadDataPacketData(segment, count, out rotation);
		count += PacketUtility.ReadUshortData(segment, count, out role);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.index, segment, count);
		count += PacketUtility.AppendIntData(this.modelIndex, segment, count);
		count += PacketUtility.AppendStringData(this.name, segment, count);
		count += PacketUtility.AppendDataPacketData(this.position, segment, count);
		count += PacketUtility.AppendDataPacketData(this.rotation, segment, count);
		count += PacketUtility.AppendUshortData(this.role, segment, count);
		return (ushort)(count - offset);
	}
}

public struct PlayerMovePacket : IDataPacket
{
	public int index;
	public float speed;
	public VectorPacket position;
	public VectorPacket direction;
	public QuaternionPacket rotation;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out index);
		count += PacketUtility.ReadFloatData(segment, count, out speed);
		count += PacketUtility.ReadDataPacketData(segment, count, out position);
		count += PacketUtility.ReadDataPacketData(segment, count, out direction);
		count += PacketUtility.ReadDataPacketData(segment, count, out rotation);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.index, segment, count);
		count += PacketUtility.AppendFloatData(this.speed, segment, count);
		count += PacketUtility.AppendDataPacketData(this.position, segment, count);
		count += PacketUtility.AppendDataPacketData(this.direction, segment, count);
		count += PacketUtility.AppendDataPacketData(this.rotation, segment, count);
		return (ushort)(count - offset);
	}
}

public struct PlayerModelPacket : IDataPacket
{
	public int index;
	public int modelIndex;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out index);
		count += PacketUtility.ReadIntData(segment, count, out modelIndex);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.index, segment, count);
		count += PacketUtility.AppendIntData(this.modelIndex, segment, count);
		return (ushort)(count - offset);
	}
}

public struct ScannedInfo : IDataPacket
{
	public int index;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out index);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.index, segment, count);
		return (ushort)(count - offset);
	}
}

public struct C_RoomEnter : IPacket
{
	public int roomId;

	public ushort Protocol { get { return (ushort)PacketID.C_RoomEnter; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out roomId);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.roomId, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_GameStart : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.C_GameStart; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_SetName : IPacket
{
	public string name;

	public ushort Protocol { get { return (ushort)PacketID.C_SetName; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadStringData(segment, count, out name);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendStringData(this.name, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_RoomEnter : IPacket
{
	public PlayerInitPacket newPlayer;

	public ushort Protocol { get { return (ushort)PacketID.S_RoomEnter; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadDataPacketData(segment, count, out newPlayer);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendDataPacketData(this.newPlayer, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_RoomEnterFirst : IPacket
{
	public int myIndex;
	public List<PlayerInitPacket> inits;

	public ushort Protocol { get { return (ushort)PacketID.S_RoomEnterFirst; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out myIndex);
		count += PacketUtility.ReadListData(segment, count, out inits);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.myIndex, segment, count);
		count += PacketUtility.AppendListData(this.inits, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_RoomExit : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.C_RoomExit; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_RoomExit : IPacket
{
	public int Index;

	public ushort Protocol { get { return (ushort)PacketID.S_RoomExit; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out Index);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.Index, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_CreateRoom : IPacket
{
	public string roomName;
	public int maxCount;

	public ushort Protocol { get { return (ushort)PacketID.C_CreateRoom; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadStringData(segment, count, out roomName);
		count += PacketUtility.ReadIntData(segment, count, out maxCount);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendStringData(this.roomName, segment, count);
		count += PacketUtility.AppendIntData(this.maxCount, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_RoomList : IPacket
{
	public List<RoomInfoPacket> roomInfos;

	public ushort Protocol { get { return (ushort)PacketID.S_RoomList; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadListData(segment, count, out roomInfos);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendListData(this.roomInfos, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_PacketResponse : IPacket
{
	public ushort responsePacket;
	public bool success;

	public ushort Protocol { get { return (ushort)PacketID.S_PacketResponse; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadUshortData(segment, count, out responsePacket);
		count += PacketUtility.ReadBoolData(segment, count, out success);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendUshortData(this.responsePacket, segment, count);
		count += PacketUtility.AppendBoolData(this.success, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_RoomList : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.C_RoomList; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_Chat : IPacket
{
	public string pName;
	public string text;

	public ushort Protocol { get { return (ushort)PacketID.S_Chat; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadStringData(segment, count, out pName);
		count += PacketUtility.ReadStringData(segment, count, out text);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendStringData(this.pName, segment, count);
		count += PacketUtility.AppendStringData(this.text, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_Chat : IPacket
{
	public string text;

	public ushort Protocol { get { return (ushort)PacketID.C_Chat; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadStringData(segment, count, out text);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendStringData(this.text, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_Move : IPacket
{
	public VectorPacket position;
	public VectorPacket direction;
	public float speed;

	public ushort Protocol { get { return (ushort)PacketID.C_Move; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadDataPacketData(segment, count, out position);
		count += PacketUtility.ReadDataPacketData(segment, count, out direction);
		count += PacketUtility.ReadFloatData(segment, count, out speed);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendDataPacketData(this.position, segment, count);
		count += PacketUtility.AppendDataPacketData(this.direction, segment, count);
		count += PacketUtility.AppendFloatData(this.speed, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_Rotate : IPacket
{
	public QuaternionPacket rotation;

	public ushort Protocol { get { return (ushort)PacketID.C_Rotate; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadDataPacketData(segment, count, out rotation);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendDataPacketData(this.rotation, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_Move : IPacket
{
	public List<PlayerMovePacket> moves;

	public ushort Protocol { get { return (ushort)PacketID.S_Move; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadListData(segment, count, out moves);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendListData(this.moves, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_SyncTimer : IPacket
{
	public float time;

	public ushort Protocol { get { return (ushort)PacketID.S_SyncTimer; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadFloatData(segment, count, out time);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendFloatData(this.time, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_UpdateRoomState : IPacket
{
	public ushort state;

	public ushort Protocol { get { return (ushort)PacketID.S_UpdateRoomState; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadUshortData(segment, count, out state);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendUshortData(this.state, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_RoundStart : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.S_RoundStart; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_ResetGame : IPacket
{
	public List<PlayerInitPacket> playerinits;

	public ushort Protocol { get { return (ushort)PacketID.S_ResetGame; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadListData(segment, count, out playerinits);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendListData(this.playerinits, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_Shoot : IPacket
{
	public VectorPacket direction;
	public VectorPacket startPos;

	public ushort Protocol { get { return (ushort)PacketID.C_Shoot; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadDataPacketData(segment, count, out direction);
		count += PacketUtility.ReadDataPacketData(segment, count, out startPos);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendDataPacketData(this.direction, segment, count);
		count += PacketUtility.AppendDataPacketData(this.startPos, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_PlayAnimation : IPacket
{
	public int index;
	public ushort animType;

	public ushort Protocol { get { return (ushort)PacketID.S_PlayAnimation; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out index);
		count += PacketUtility.ReadUshortData(segment, count, out animType);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.index, segment, count);
		count += PacketUtility.AppendUshortData(this.animType, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_Attack : IPacket
{
	public int hitIndex;

	public ushort Protocol { get { return (ushort)PacketID.C_Attack; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out hitIndex);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.hitIndex, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_Shoot : IPacket
{
	public int index;
	public VectorPacket direction;
	public VectorPacket startPos;

	public ushort Protocol { get { return (ushort)PacketID.S_Shoot; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out index);
		count += PacketUtility.ReadDataPacketData(segment, count, out direction);
		count += PacketUtility.ReadDataPacketData(segment, count, out startPos);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.index, segment, count);
		count += PacketUtility.AppendDataPacketData(this.direction, segment, count);
		count += PacketUtility.AppendDataPacketData(this.startPos, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_ResourceLoadComplete : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.C_ResourceLoadComplete; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_Dead : IPacket
{
	public int index;

	public ushort Protocol { get { return (ushort)PacketID.S_Dead; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out index);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.index, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_ChangeModel : IPacket
{
	public List<PlayerModelPacket> modelInfos;

	public ushort Protocol { get { return (ushort)PacketID.S_ChangeModel; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadListData(segment, count, out modelInfos);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendListData(this.modelInfos, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_Teleport : IPacket
{
	public VectorPacket position;

	public ushort Protocol { get { return (ushort)PacketID.S_Teleport; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadDataPacketData(segment, count, out position);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendDataPacketData(this.position, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_UseSkill : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.C_UseSkill; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_UseSkill : IPacket
{
	public ushort skillType;

	public ushort Protocol { get { return (ushort)PacketID.S_UseSkill; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadUshortData(segment, count, out skillType);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendUshortData(this.skillType, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_Scanned : IPacket
{
	public List<ScannedInfo> scanned;

	public ushort Protocol { get { return (ushort)PacketID.C_Scanned; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadListData(segment, count, out scanned);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendListData(this.scanned, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_Scanned : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.S_Scanned; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_MoveLock : IPacket
{
	public int index;
	public bool value;

	public ushort Protocol { get { return (ushort)PacketID.S_MoveLock; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out index);
		count += PacketUtility.ReadBoolData(segment, count, out value);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.index, segment, count);
		count += PacketUtility.AppendBoolData(this.value, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_MoveLock : IPacket
{
	public bool value;

	public ushort Protocol { get { return (ushort)PacketID.C_MoveLock; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadBoolData(segment, count, out value);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendBoolData(this.value, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct C_Fall : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.C_Fall; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_RoomStateChange : IPacket
{
	public ushort state;

	public ushort Protocol { get { return (ushort)PacketID.S_RoomStateChange; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadUshortData(segment, count, out state);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendUshortData(this.state, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_HostChange : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.S_HostChange; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_SetSkill : IPacket
{
	public ushort skill;

	public ushort Protocol { get { return (ushort)PacketID.S_SetSkill; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadUshortData(segment, count, out skill);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendUshortData(this.skill, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public struct S_GameEnd : IPacket
{
	public ushort winner;
	public ushort loser;

	public ushort Protocol { get { return (ushort)PacketID.S_GameEnd; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadUshortData(segment, count, out winner);
		count += PacketUtility.ReadUshortData(segment, count, out loser);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendUshortData(this.winner, segment, count);
		count += PacketUtility.AppendUshortData(this.loser, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

