using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Players;
using KHG.Managers;
using KHG.UIs;
using System.Collections.Generic;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour, ICoreUI,INeedInject
{
    [Inject] private PoolManagerMono poolManager;
    [SerializeField] private PoolItemSO poolItem;
    [SerializeField] private GameObject mainUi;
    [SerializeField] private Transform holder;
    
    private InputSO _input;
    private List<Player> _players = new();
    private List<PlayerInfoBar> _playerBars = new();

    private bool _enabled;

    public bool Injected { get; set; }

    private AwaitableCompletionSource _waitInject = new();
    public AwaitableCompletionSource WaitInject => _waitInject;

    public void Initialize(CoreUI coreUI)
    {
        _input = coreUI.inputSO;
        _input.OnTabPressedEvent += HandleTabPressed;
    }

    private void OnDestroy()
    {
        _input.OnTabPressedEvent -= HandleTabPressed;
    }

    private void HandleTabPressed(bool value)
    {
        if (value == false) return;

        if (_enabled)
            Close();
        else 
            Show();
        _enabled = !_enabled;
    }

    private void Show()
    {
        print("�������� Ű��");
        _players.Clear();
        _players = EntityManager.Instance.GetAllPlayer();
        PopPlayerList();
        mainUi.SetActive(true);
    }

    private void PopPlayerList()
    {
        print($"������ �ο���:{_players.Count}");
        foreach (Player player in _players)
        {
            PlayerInfoBar infoBar = poolManager.Pop<PlayerInfoBar>(poolItem);
            infoBar.transform.parent = holder;
            infoBar.transform.rotation = transform.rotation;
            infoBar.transform.position = transform.position;
            infoBar.transform.localScale = Vector3.one;
            infoBar.SetInfo(player);
            
            _playerBars.Add(infoBar);
            print($"������ ����Ʈ�� ����:{player.Name}, ����:{player.Role}");
        }
        print($"������ �ο���:{_playerBars.Count}");
    }

    private void PushPlayerList()
    {
        if (_playerBars.Count == 0)
        {
            print("����");
            return;
        }
        foreach(PlayerInfoBar player in _playerBars)
        {
            player.Push();
        }
        _playerBars.Clear();
    }

    private void Close()
    {
        PushPlayerList();
        mainUi.SetActive(false);
    }
}
