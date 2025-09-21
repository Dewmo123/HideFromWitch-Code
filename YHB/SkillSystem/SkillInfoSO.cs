using DewmoLib.Utiles;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.SkillSystem
{
	// [CreateAssetMenu(fileName = "SkillInfo", menuName = "SO/SkillInfo", order = 0)]
	public abstract class SkillInfoSO : ScriptableObject
	{
		private const string ASSEMBLY_NAME = "SkillInfoDataClassAssembly";
		private const string MODULE_NAME = "SkillInfoModule";

		[Header("Skill Data Setting")]
		[field: SerializeField] public string SkillName { get; private set; }
		[field: SerializeField] public EventChannelSO SkillEventChannel { get; private set; }

		// 이것보다 상속받는 사람이 더 편하게 못 만드나..?
		//private BaseEventType _data;
		//private void OnEnable()
		//{
		//	_data = CreateInstanceOfNewEventClass<BaseEventType>();
		//}

		// 상속받은 InfoSO에서 사용할 데이터 타입을 반환해야합니다.
		protected abstract Type GetEventType(); // 아래에서 생성된 클래스의 타입을 반환해야합니다.

		// SkillInfoSO마다 이벤트를 따로 받게 처리를 하기 위해 baseType을 상속받는 타입을 동적 생성
		// SO로 스킬의 상태를 공유 할 수 있고, 이벤트도 동일한 채널에서 다른 타입으로 구분하여 생성가능합
		protected T CreateInstanceOfNewClass<T>() where T : class
		{
			AssemblyName assemblyName = new AssemblyName(ASSEMBLY_NAME); // 어셈블리 이름 설정
			AssemblyBuilder assembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run); // 런타임용 어셈블리 설정
			ModuleBuilder module = assembly.DefineDynamicModule(MODULE_NAME); // 모듈 생성

			// 타입 설정
			TypeBuilder typeBuilder = module.DefineType(
				$"{SkillName}Info",
				TypeAttributes.Public | TypeAttributes.Class,
				typeof(T)
			);

			Type createdType = typeBuilder.CreateType(); // 타임 생성

			T eventClass = Activator.CreateInstance(createdType) as T;
			return eventClass;
		}
#if UNITY_EDITOR
		protected virtual void OnValidate()
		{
			if (!string.IsNullOrEmpty(SkillName) && !EqualityComparer<string>.Default.Equals(name, SkillName))
			{
				// SO의 인스펙터 상 이름 변경
				name = SkillName;

				// 실제 에셋 파일 이름도 변경
				string assetPath = AssetDatabase.GetAssetPath(this);
				AssetDatabase.RenameAsset(assetPath, SkillName);
				AssetDatabase.SaveAssets();
			}
		}
#endif
	}
}
