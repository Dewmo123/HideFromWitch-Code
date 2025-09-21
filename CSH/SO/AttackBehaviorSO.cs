using Assets._00.Work.CSH._01_Scripts.Component;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using UnityEngine;

namespace Assets._00.Work.CSH._01_Scripts.SO
{
    [CreateAssetMenu(fileName = "AttackBehaviour", menuName = "SO/ScriptableBehaviour/Behaviour/Attack", order = 0)]
    public class AttackBehaviorSO : ScriptableBehaviourSO
    {
        protected override bool LogicExecute<T>(T data)
        {
            if(data is not AttackCompo attackCompo)
            {
                return false;
            }

            return attackCompo.Fire();
        }
    }
}