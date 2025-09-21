using Server.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Server.Rooms.States
{
    internal class RoomStateMachine
    {
        private Dictionary<RoomState, GameRoomState> _states;
        public GameRoomState CurrentState { get; private set; }
        public RoomState CurrentStateEnum { get; private set; }
        private static List<Type> _types;
        private static Dictionary<RoomState,Func<GameRoom, GameRoomState>> _stateFactory;
        public RoomStateMachine(GameRoom room)
        {
            _states = new Dictionary<RoomState, GameRoomState>();
            _states = _stateFactory.ToDictionary(item => item.Key, item => item.Value.Invoke(room));
        }
        public static void SetUpStates()
        {
            _types = new();
            _stateFactory = new();
            Assembly fsmAssembly = Assembly.GetAssembly(typeof(GameRoomState));
            _types = fsmAssembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(GameRoomState)))
                .ToList();
            foreach(Type type in _types)
            {
                ConstructorInfo ctor = type.GetConstructor(new[] { typeof(GameRoom) });
                ParameterExpression roomParam = Expression.Parameter(typeof(GameRoom), "room");
                Expression newExpr = Expression.New(ctor, roomParam);
                LambdaExpression lambda = Expression.Lambda<Func<GameRoom, GameRoomState>>(newExpr, roomParam);
                _stateFactory.Add(
                    EnumHelper.StringToEnum<RoomState>(type.Name.Replace("State", "")),
                    lambda.Compile() as Func<GameRoom,GameRoomState>);
            }
        }
        public void ChangeState(RoomState type)
        {
            Console.WriteLine($"ChangeState: {type}");
            if (_states.TryGetValue(type, out GameRoomState state))
            {
                CurrentState?.Exit();
                CurrentStateEnum = type;
                CurrentState = state;
                state.Enter();
            }
            else
            {
                Console.WriteLine($"{type}State does not exist");
                throw new System.Exception();
            }
        }
        public void UpdateRoom()
            => CurrentState.Update();
    }
}
