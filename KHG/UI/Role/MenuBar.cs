using AKH.Network;
using Assets._00.Work.YHB.Scripts.Core;
using KHG.UIs;
using UnityEngine;

public class MenuBar : MonoBehaviour, ICoreUI
{
    [SerializeField] private GameObject _menuBar;

    private bool _menuState;
    private InputSO _input;

    public void Initialize(ICoreUIContext coreUIContext)
    {
        _input = coreUIContext.Input;
        _input.OnEscapeEvent += EscapeCalled;
    }

    private void OnDestroy()
    {
        if (_input != null)
            _input.OnEscapeEvent -= EscapeCalled;
    }

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

    private void SetMenuBar(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        _menuBar.SetActive(value);
    }
}
