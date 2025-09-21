using AKH.Network;
using KHG.UIs;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBar : MonoBehaviour, ICoreUI
{
    [SerializeField] private GameObject _menuBar;
    private bool _menuState;

    public void EscapeCalled()
    {
        _menuState = !_menuState;
        SetMenuBar(_menuState);
    }

    public void Canceled()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitRoom()
    {
        Cursor.lockState = CursorLockMode.None;
        NetworkManager.Instance.SendPacket(new C_RoomExit());
    }

    public void Initialize(CoreUI coreUI)
    {
    }

    private void SetMenuBar(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        _menuBar.SetActive(value);
    }
}
