using DewmoLib.Utiles;
using KHG.Events;
using TMPro;
using UnityEngine;

public class RoomUIManager : MonoBehaviour
{
    [SerializeField] private EventChannelSO playerInfoChannel;
    [SerializeField] private TMP_Text playerNameText;

    private void Awake()
    {
        playerInfoChannel.AddListener<PlayerNameEvent>(HandleNameApply);
    }

    private void OnDestroy()
    {
        playerInfoChannel.RemoveListener<PlayerNameEvent>(HandleNameApply);
    }

    private void HandleNameApply(PlayerNameEvent @event)
    {
        if (playerNameText != null)
            playerNameText.SetText(@event.Name);
    }
}
