using AgamaLibrary.Unity.Methods;
using Alchemy.Inspector;
using Assets._00.Work.CDH.Code.Sound;
using Assets._00.Work.YHB.Scripts.Events;
using Assets._00.Work.YHB.Scripts.Executors;
using DewmoLib.Utiles;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Players
{
	public class RunnerVisualController : ActiverMono
	{
		[Header("Physics")]
		[SerializeField] private Collider hunterCollider;
		[SerializeField] private MeshCollider runnerCollider;
		[SerializeField] private PhysicsMaterial playerMaterial;

		[Header("Setting")]
		[SerializeField] private bool isActiveWhenSetVisual;
		[SerializeField] private EventChannelSO changeGroundCheckerValue;

		[Header("Visuals")]
		[SerializeField] private MeshFilter runnerMeshFilter;
		[SerializeField] private MeshRenderer runnerMeshRenderer;
		[SerializeField] private RunnerVisualRegisterSO visualRegister;

		[Header("Sound Setting")]
		[SerializeField] private SoundPlayCompo visualChangeSoundPlayer;

		private int _currentVisualObjectIndex;
		public int CurrentVisualObjectIndex
		{
			get => _currentVisualObjectIndex;
			set => _currentVisualObjectIndex = value;
		}

		private Collider _addCollider;

		public override void EarlyInitialize()
		{
			base.EarlyInitialize();

			runnerCollider.convex = true;
		}

		protected override void ActivateCore()
		{
			hunterCollider.enabled = false;
			runnerCollider.enabled = true;

			runnerMeshFilter.sharedMesh = null;
			runnerMeshRenderer.enabled = true;

			if (isActiveWhenSetVisual)
				SetRandomVisual();
		}

		protected override void DeActivateCore()
		{
			hunterCollider.enabled = true;
			runnerCollider.sharedMesh = null;
			runnerCollider.enabled = false;

			runnerMeshFilter.sharedMesh = null;
			runnerMeshRenderer.enabled = false;

			TryRemoveAddCollider();
			// 여기서 얘가 꺼져서 기본상태로 돌아가라고 알려주는게 썩 좋은 구조는 아닌데
			changeGroundCheckerValue.InvokeEvent(GameObjectChangeEvents.GroundCheckerSizeChangedEvent.Initialize(Vector3.zero));
		}

		[Button]
		public void SetRandomVisual()
		{
			SetVisualFromIndex(Random.Range(0, visualRegister.Count));
		}

		public void SetVisualFromIndex(int index)
		{
			Debug.Log(index);
			TryRemoveAddCollider();

			CurrentVisualObjectIndex = index;
			RunnerVisualObject visualObject = visualRegister.GetAt(CurrentVisualObjectIndex);

			runnerCollider.sharedMesh = null;
			runnerCollider.sharedMesh = visualObject.Filter.sharedMesh;

			runnerMeshFilter.sharedMesh = null;
			runnerMeshFilter.sharedMesh = visualObject.Filter.sharedMesh;

			runnerMeshRenderer.sharedMaterials = visualObject.SharedMaterials;

			runnerMeshRenderer.enabled = true;

			if (visualObject.HaveCollider)
			{
				runnerCollider.enabled = false;
				visualObject.UseCollider.enabled = false;
				Collider copiedCollider = runnerCollider.gameObject.CopyComponent(visualObject.UseCollider);
				copiedCollider.enabled = true;
				copiedCollider.material = playerMaterial;
				_addCollider = copiedCollider;
			}
			else
				runnerCollider.enabled = true;

			visualChangeSoundPlayer.PlaySound();

            changeGroundCheckerValue.InvokeEvent(GameObjectChangeEvents.GroundCheckerSizeChangedEvent.Initialize(visualObject.groundCheckerSize));
		}

		private bool TryRemoveAddCollider()
		{
			bool remove = _addCollider != null;

			if (remove)
				Destroy(_addCollider);

			_addCollider = null;

			return remove;
		}
	}
}
