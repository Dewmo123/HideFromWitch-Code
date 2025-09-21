using UnityEngine;

public class HostManager : MonoBehaviour
{
    private bool _isHost;
    private static HostManager _instance;
    public static HostManager Instance => _instance;

    public bool IsHost => _isHost;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public void SetHost()
    {
        print("호스트 설정");
        _isHost = true;
    }
}
