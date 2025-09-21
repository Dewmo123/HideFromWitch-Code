using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using UnityEngine;

namespace AKH.Scripts.Players
{
    [CreateAssetMenu(fileName = "DummyMoveBehaviour", menuName = "SO/ScriptableBehaviour/Behaviour/Move/DummyMove", order = 0)]

    public class DummyMoveBehaviourSO : ScriptableBehaviourSO
    {
        protected override bool LogicExecute<T>(T data)
        {
            if (data is not MovementData movementData)
                return false;
            movementData.movement.SetMovement(movementData.moveDirection);
            return true;
        }
    }
}
