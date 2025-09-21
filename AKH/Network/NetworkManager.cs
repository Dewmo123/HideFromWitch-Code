using DewmoLib.Network.Packets;
using DewmoLib.Utiles;
using ServerCore;
using System;
using System.Net;
using UnityEngine;

namespace AKH.Network
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO packetChannel;
        private static NetworkManager _instance;
        public static NetworkManager Instance => _instance;

        private Connector _connector;
        private ServerSession _session;
        private PacketQueue _packetQueue;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            _connector = new Connector();
            _packetQueue = new PacketQueue(new ClientPacketManager(packetChannel));
            try
            {
                IPAddress ip = Dns.GetHostEntry("dewmo.kro.kr").AddressList[0];
                IPEndPoint endPoint = new IPEndPoint(ip, 3303);
                _connector.Connect(endPoint, () => _session = new ServerSession(_packetQueue), 1);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            Debug.Log("DEstroy networkManager");
            if (_session != null && _packetQueue != null)
            {
                _packetQueue.Clear();
                _session.Disconnect();
            }
        }

        private void OnApplicationQuit()
        {
            Debug.Log("quit networkManager");
            if (_session != null && _packetQueue != null)
            {
                _packetQueue.Clear();
                _session.Disconnect();
            }
        }

        public void SendPacket(IPacket packet)
        {
            _session.Send(packet.Serialize());
        }

        private void Update()
        {
            _packetQueue.FlushPackets(_session);
        }
    }
}