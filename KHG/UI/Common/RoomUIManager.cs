using DewmoLib.Utiles;
using KHG.Events;
using System;
using UnityEngine;

public class RoomUIManager : MonoBehaviour
{
    [SerializeField] private EventChannelSO playerInfoChannel;

    private void Awake()
    {
        playerInfoChannel.AddListener<PlayerNameEvent>(HandleNameApply);
    }

    private void HandleNameApply(PlayerNameEvent @event)
    {
        throw new NotImplementedException();
    }
}
