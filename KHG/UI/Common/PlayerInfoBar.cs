using Assets._00.Work.YHB.Scripts.Players;
using AYellowpaper.SerializedCollections;
using DewmoLib.ObjectPool.RunTime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBar : MonoBehaviour,IPoolable
{
    [SerializeField] private TMP_Text nameTmp;
    [SerializeField] private TMP_Text roleTmp;
    [SerializeField] private Image roleImage;
    [SerializeField] private Image statusImage;
    [SerializeField] private PoolItemSO poolItemSO;

    [Space]
    [SerializeField] private SerializedDictionary<Role, Color> roleColor;


    private Pool _myPool;
    public PoolItemSO PoolItem => poolItemSO;

    public GameObject GameObject => gameObject;

    public void ResetItem()
    {
        nameTmp.text = string.Empty;
        roleTmp.text = string.Empty;
    }

    public void SetInfo(Player info)
    {
        nameTmp.SetText(info.Name);
        roleTmp.SetText(GetRoleText(info.Role));
        roleImage.color = roleColor[info.Role];
    }

    public void SetUpPool(Pool pool)
    {
        _myPool = pool;
    }

    public void Push() => _myPool.Push(this);

    private string GetRoleText(Role role)
    {
        switch(role)
        {
            case Role.None:
                return "없음";
            case Role.Hunter:
                return "술래";
            case Role.Runner:
                return "사물";
            case Role.Dead:
                return "사망";
            default:
                return "없음";
        }
    }
}
