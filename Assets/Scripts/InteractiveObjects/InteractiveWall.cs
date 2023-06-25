using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class InteractiveWall : MonoBehaviour, IInteractive
{
	[System.Serializable]
	public class InteractMapping
	{
		public InteractElement Element;
		public InteractType Type;
		public string AnimationTriggerName;
	}

	[SerializeField] private List<InteractMapping> _interactElements = new();

	private Animator _animator;

	public InteractType? Interact(InteractElement element)
	{
		InteractMapping interactMapping = GetInteractMapping(element);

		if (interactMapping != null && interactMapping.AnimationTriggerName != "")
		{
			_animator.SetTrigger(interactMapping.AnimationTriggerName);
		}

		return interactMapping.Type;
	}

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private InteractMapping GetInteractMapping(InteractElement element)
	{
		foreach (InteractMapping interactMapping in _interactElements)
		{
			if (interactMapping.Element == element)
			{
				return interactMapping;
			}
		}

		return null;
	}
}
