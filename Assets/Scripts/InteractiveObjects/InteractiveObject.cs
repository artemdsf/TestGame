using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class InteractiveObject : MonoBehaviour, IInteractive
{
	public class InteractiveObjectInfo
	{
		public IInteractive Object;
		public bool IsEntrance;
	}

	[System.Serializable]
	private class InteractMapping
	{
		public InteractElement Element;
		public InteractType Type;
		public string AnimationTriggerName;
		public bool IsInteractionOnlyAtEntrance;
	}

	[SerializeField] private List<InteractMapping> _interactElements = new();

	private Animator _animator;

	public List<InteractType> Interact(InteractElement element, bool isEntrance)
	{
		List<InteractMapping> interactMappings = GetInteractMapping(element);
		List<InteractType> interactTypes = new(); 

		if (interactMappings.Count > 0)
		{
			foreach (InteractMapping interactMapping in interactMappings)
			{
				bool checkEntrance = interactMapping.IsInteractionOnlyAtEntrance == false ||
					(isEntrance == true && interactMapping.IsInteractionOnlyAtEntrance == true);

				if (checkEntrance)
				{
					if (interactMapping.AnimationTriggerName != "")
					{
						_animator.SetTrigger(interactMapping.AnimationTriggerName);
					}

					interactTypes.Add(interactMapping.Type);
				}
			}
		}

		return interactTypes;
	}

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private List<InteractMapping> GetInteractMapping(InteractElement element)
	{
		List<InteractMapping> interactMappings = new();

		foreach (InteractMapping interactMapping in _interactElements)
		{
			bool isAnyElement = interactMapping.Element == InteractElement.AnyElement && element != InteractElement.Touch;
			if (interactMapping.Element == element || isAnyElement)
			{
				interactMappings.Add(interactMapping);
			}
		}

		return interactMappings;
	}
}
