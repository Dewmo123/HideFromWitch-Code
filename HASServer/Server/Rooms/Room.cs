using Server.Objects;
using Server.Pool;
using Server.Utiles;
using ServerCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Server.Rooms
{
    internal abstract class Room : IJobQueue
    {
        protected ObjectManager _objectManager = new();
        public ObjectManager ObjectManager => _objectManager;
        protected RoomManager _roomManager;
        public EventBus Bus { get; private set; }

        public Room(RoomManager manager, int roomId, string name)
        {
            _roomManager = manager;
            RoomId = roomId;
            RoomName = name;
            Bus = new();
        }
        protected Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
        private JobQueue _jobQueue = new JobQueue();
        private ConcurrentQueue<ArraySegment<byte>> _pendingList = new();
        public string RoomName { get; private set; }
        public int HostIndex { get; private set; } = -1;
        public int RoomId { get; private set; } = 0;
        public int MaxSessionCount { get; protected set; }
        public int SessionCount => _sessions.Count;
        public Dictionary<int, ClientSession> Sessions => _sessions;

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }
        public void Flush()
        {
            int n = 0;
            try
            {
                // N ^ 2
                //Console.WriteLine($"SessionCount : {_sessions.Values.Count}");
                var list = _pendingList.ToList();
                _pendingList.Clear();
                if (list.Count == 0)
                    return;
                foreach (ClientSession s in _sessions.Values)
                {
                    n++;
                    s.Send(list);
                }
                //Console.WriteLine("Clear");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Complete: {n}");
                Console.WriteLine(ex);
            }
        }

        public void Broadcast(IPacket packet)
        {
            _pendingList.Enqueue(packet.Serialize());
        }
        public ClientSession GetSession(int key)
        {
            return _sessions[key];
        }
        public virtual Player Enter(ClientSession session)
        {
            Player newPlayer = PoolManager.Instance.Pop<Player>();
            newPlayer.InitObj(ObjectManager);
            newPlayer.Name = session.Name;
            Console.WriteLine(newPlayer.Name);
            session.PlayerId = newPlayer.index;
            _sessions.Add(session.PlayerId, session);
            session.Room = this;
            if (HostIndex == -1)
            {
                HostIndex = session.PlayerId;
            }
            return newPlayer;
        }
        public void FirstEnter(ClientSession clientSession)
        {
            S_RoomEnterFirst first = new();
            first.myIndex = clientSession.PlayerId;
            first.inits = new();
            foreach (var item in ObjectManager.GetObjects<Player>())
                first.inits.Add((PlayerInitPacket)item.CreatePacket());
            clientSession.Send(first.Serialize());
        }
        public virtual Player Leave(ClientSession session)
        {
            Console.WriteLine("LEAVE");
            Player player = _objectManager.RemoveObject(session.PlayerId) as Player;
            _sessions.Remove(session.PlayerId);
            session.Room = null;
            if (SessionCount == 0)
            {
                Console.WriteLine("RemoveRoom");
                _roomManager.RemoveRoom(RoomId);
            }
            else if (session.PlayerId == HostIndex)
            {
                KeyValuePair<int, ClientSession> newHost = _sessions.First();
                HostIndex = newHost.Key;
                newHost.Value.Send(new S_HostChange().Serialize());
            }
            var exitPacket = new S_RoomExit() { Index = session.PlayerId };
            Broadcast(exitPacket);
            session.Send(exitPacket.Serialize());
            return player;
        }
        public abstract void PlayerDead(Player deadPlayer);
        public void InvokeEvent<T>(T evt) where T : GameEvent, new()
        {
            Push(() =>
            {
                Bus.InvokeEvent(evt);
                PoolManager.Instance.Push(evt);
            });
        }
        public virtual void UpdateRoom()
        {
            Flush();
        }
        public virtual void SetUpRoom(C_CreateRoom packet)
        {
            RoomName = packet.roomName;
            MaxSessionCount = packet.maxCount;
        }
    }
}
