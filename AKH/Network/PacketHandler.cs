using AKH.Scripts.Packet;
using AKH.Scripts.Players;
using AKH.Scripts.Players.Active;
using Assets._00.Work.AKH.Scripts.Packet;
using Assets._00.Work.CSH._01_Scripts.Executor;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Events;
using Assets._00.Work.YHB.Scripts.Players;
using DewmoLib.Utiles;
using KHG.Events;
using KHG.Managers;
using ServerCore;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class PacketHandler
{
    private EventChannelSO _packetChannel;
    private int _myIndex = -1;
    private bool _entered = false;
    public PacketHandler(EventChannelSO packetChannel)
    {
        _packetChannel = packetChannel;
    }

    internal void S_PacketResponseHandler(PacketSession session, IPacket packet)
    {
        var response = (S_PacketResponse)packet;
        var evt = PacketEvents.PacketResponse;
        evt.success = response.success;
        evt.packetId = (PacketID)response.responsePacket;
        _packetChannel.InvokeEvent(evt);
    }
    public void S_MoveHandler(PacketSession session, IPacket packet)
    {
        if (!_entered)
            return;
        S_Move move = (S_Move)packet;
        foreach (var item in move.moves)
        {
            if (item.index == _myIndex)
                continue;
            Vector3 velocity = item.direction.ToVector3() /** move.speed*/;
            Debug.Log(item.index);
            var other = EntityManager.Instance.GetObject<DummyClient>(item.index);
            other.Registry.ResolveComponent<EntityMovement>().SetPosition(item.position.ToVector3());
            other.HandleDummyClientMove(velocity);
            other.HandleDummyClientRotation(item.rotation.ToQuaternion());
        }
    }

    public void S_RoomEnterFirstHandler(PacketSession session, IPacket packet) //나만 확인용
    {
        S_RoomEnterFirst first = (S_RoomEnterFirst)packet;
        _myIndex = first.myIndex;
        foreach (var item in first.inits)
        {
            Player newPlayer = null;
            if (item.index == first.myIndex)
            {
                EntityManager.Instance.CreateObject<Player>(
                    item.index,
                    ObjectType.Player,
                    Vector3.zero,
                    out var player);
                player.Registry.ResolveComponent<EntityMovement>().SetPosition(item.position.ToVector3());
                newPlayer = player;
            }
            else
            {
                EntityManager.Instance.CreateObject<DummyClient>(
                    item.index,
                    ObjectType.OtherPlayer,
                    item.position.ToVector3(),
                    out var player);
                player.HandleDummyClientRotation(item.rotation.ToQuaternion());
                newPlayer = player;
            }
            newPlayer.InitPlayer(item.index, item.name, Role.None);
            newPlayer.Registry.ResolveComponent<RoleController>().ChangeRole(Role.None);
        }
        _entered = true;
        _currentState = RoomState.Lobby;
    }
    public void S_RoomEnterHandler(PacketSession session, IPacket packet)// 전체 확인용
    {
        S_RoomEnter enter = (S_RoomEnter)packet;
        PlayerInitPacket init = enter.newPlayer;
        ShowText(0, 0.3f, 3, TitleMode.MainTitle, $"{init.name}님이 들어왔습니다.");
        if (enter.newPlayer.index == _myIndex)
            return;
        EntityManager.Instance.CreateObject<DummyClient>(
            init.index,
            ObjectType.OtherPlayer,
            init.position.ToVector3(),
            out var player);
        player.InitPlayer(init.index, init.name, Role.None);
        player.HandleDummyClientRotation(init.rotation.ToQuaternion());
        player.Registry.ResolveComponent<RoleController>().ChangeRole(Role.None);
    }

    public void S_ResetGameHandler(PacketSession session, IPacket packet)
    {
        S_ResetGame resetGame = (S_ResetGame)packet;
        foreach (var item in resetGame.playerinits)
        {
            Player player = EntityManager.Instance.GetObject<Player>(item.index);
            var movement = player.Registry.ResolveComponent<EntityMovement>();
            var roleController = player.Registry.ResolveComponent<RoleController>();
            movement.SetPosition(item.position.ToVector3());
            player.transform.rotation = item.rotation.ToQuaternion();
            player.Role = (Role)item.role;
            roleController.ChangeRole(0);
            roleController.ChangeRole((Role)item.role);
            if ((Role)item.role == Role.Runner)
                player.Registry.ResolveComponent<RunnerVisualController>()
                    .SetVisualFromIndex(item.modelIndex);
        }
        if (_entered)
        {
            Player myPlayer = EntityManager.Instance.GetObject<Player>(_myIndex);

            switch (myPlayer.Role)
            {
                case Role.Hunter:
                    ShowText(0, 0.5f, 3, TitleMode.SubTitle, "시작하기 전 맵의 오브젝트를 외우세요!");
                    break;
                case Role.Runner:
                    ShowText(0, 0.5f, 3, TitleMode.SubTitle, "시작하기 전 안들키도록 숨으세요!");
                    break;
            }
        }
    }

    public void S_PlayAnimationHandler(PacketSession session, IPacket packet)
    {
        S_PlayAnimation anim = (S_PlayAnimation)packet;
        Debug.Log($"AnimPacket: {_myIndex}, {anim.index}, Type: {(AnimType)anim.animType}");
        if (anim.index == _myIndex)
            return;
        var player = EntityManager.Instance.GetObject<Player>(anim.index);
        player.Registry.ResolveComponent<DummyAnimationEvent>().PlayAnimation((AnimType)anim.animType);
    }

    public void S_RoomExitHandler(PacketSession session, IPacket packet)
    {
        S_RoomExit exit = (S_RoomExit)packet;
        if (exit.Index == _myIndex)
        {
            _myIndex = -1;
            _entered = false;
            Debug.Log("ExitMe");
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            var player = EntityManager.Instance.GetObject<Player>(exit.Index);
            ShowText(0, 0.3f, 3, TitleMode.MainTitle, $"{player.Name} 님이 나갔습니다.");
            EntityManager.Instance.RemoveObject(exit.Index);
        }
    }

    public void S_SyncTimerHandler(PacketSession session, IPacket packet)
    {
        S_SyncTimer timer = (S_SyncTimer)packet;
        var evt = PacketEvents.SyncTimer;
        evt.remaintime = timer.time;
        _packetChannel.InvokeEvent(evt);
    }

    public void S_ShootHandler(PacketSession session, IPacket packet)
    {
        S_Shoot shoot = (S_Shoot)packet;
        if (shoot.index == _myIndex)
            return;
        var dummy = EntityManager.Instance.GetObject<DummyClient>(shoot.index);
        dummy.HandleDummyClientShoot(shoot.startPos.ToVector3(), shoot.direction.ToVector3());
    }

    public void S_DeadHandler(PacketSession session, IPacket packet)
    {
        S_Dead dead = (S_Dead)packet;
        var player = EntityManager.Instance.GetObject<Player>(dead.index);
        if (player == null)
            return;
        player.Registry.ResolveComponent<RoleController>().ChangeRole(Role.Dead);
        player.Role = Role.Dead;
        if (player.Index == _myIndex)
        {
            var evt = GameObjectChangeEvents.PlayerChangeRoleEvent;
            evt.role = Role.Dead;
            _packetChannel.InvokeEvent(evt);

            ShowText(0, 2f, 3f, TitleMode.MainTitle, "사망하였습니다..");
        }
    }

    public void S_ChangeModelHandler(PacketSession session, IPacket packet)
    {
        S_ChangeModel modelPacket = (S_ChangeModel)packet;
        foreach (var item in modelPacket.modelInfos)
        {
            if (item.index == _myIndex)
            {
                //바뀌었다고 알림
                ShowText(0, 0.5f, 3f, TitleMode.MainTitle, "모습이 변경되었습니다!");
            }
            Player runner = EntityManager.Instance.GetObject<Player>(item.index);
            runner.Registry.ResolveComponent<RunnerVisualController>()
    .SetVisualFromIndex(item.modelIndex);
        }
    }

    public void S_UseSkillHandler(PacketSession session, IPacket packet)
    {
        S_UseSkill skill = (S_UseSkill)packet;
        var player = EntityManager.Instance.GetObject<Player>(_myIndex);
        player.Registry.ResolveComponent<SkillInputExecutor>().UseSkill((SkillType)skill.skillType);
    }

    public void S_ScannedHandler(PacketSession session, IPacket packet)
    {
        ShowText(0, 0.3f, 5f, TitleMode.SubTitle, "술래에게 발각되었습니다!");
    }

    private void ShowText(float fadeInTime, float fadeOutTime, float lifeTime, TitleMode mode, string text)
    {
        var evt = UserInterfaceEvents.MessageEvent;
        evt.Data = new() { fadeInTime = fadeInTime, fadeOutTime = fadeOutTime, lifeTime = lifeTime, mode = mode };
        evt.Message = text;
        _packetChannel.InvokeEvent(evt);
    }

    public void S_TeleportHandler(PacketSession session, IPacket packet)
    {
        S_Teleport teleport = (S_Teleport)packet;
        var player = EntityManager.Instance.GetObject<Player>(_myIndex);
        var movement = player.Registry.ResolveComponent<EntityMovement>();
        movement.SetPosition(teleport.position.ToVector3());
        movement.FallTime.Value = 0;
        player.Registry.ResolveComponent<FallTimeDeadInvoker>().FallDead = false;
        Debug.Log($"Teleport: {teleport.position.ToVector3()}");
    }

    public void S_MoveLockHandler(PacketSession session, IPacket packet)
    {
        S_MoveLock lockPacket = (S_MoveLock)packet;
        if (lockPacket.index == _myIndex)
            return;
        var player = EntityManager.Instance.GetObject<DummyClient>(lockPacket.index);
        player.Registry.ResolveComponent<EntityMovement>().SetKinematic(lockPacket.value);
    }

    public void S_SetSkillHandler(PacketSession session, IPacket packet)
    {
        S_SetSkill setSkill = (S_SetSkill)packet;
        switch ((SkillType)setSkill.skill)
        {
            case SkillType.Scan:
                ShowText(0, 0.5f, 5, TitleMode.SubTitle, "당신은 범위내 도망자들을 발광시킵니다(게임당 3번)");
                break;
            case SkillType.Force:
                ShowText(0, 0.5f, 5, TitleMode.SubTitle, "당신은모든 도망자들의 모습을 바꿉니다(게임당 2번)");
                break;
        }
    }
}