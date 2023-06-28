using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static InteractiveObject;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Parameters")]
	[SerializeField] private float _speed = 5f;
	[SerializeField] private LayerMask _interactiveLayer;

	public List<IInteractive> LastInteractiveObjects = new();
	public List<IInteractive> CurrentInteractiveObjects = new();

	private const string HorizontalAxis = "Horizontal";
	private const string VerticalAxis = "Vertical";

	private readonly InteractElement _interactElement = InteractElement.Touch;
	private float _radius;

	private void Awake()
	{
		_radius = transform.localScale.x / 2;
	}

	private void Update()
	{
		ProcessMovement();
		LastInteractiveObjects = CurrentInteractiveObjects.ToList();
		CurrentInteractiveObjects.Clear();
	}

	private void ProcessMovement()
	{
		Vector2 movementInput = new Vector2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis)).normalized;

		MoveInDirection(Vector2.right * movementInput.x);
		MoveInDirection(Vector2.up * movementInput.y);
	}

	private void MoveInDirection(Vector2 direction)
	{
		float distance = _speed * Time.deltaTime;
		HandleInteractiveCollision(direction, distance, out bool isReflection);

		if (isReflection == false)
		{
			transform.position += (Vector3)(distance * direction);
		}
	}

	private void HandleInteractiveCollision(Vector2 direction, float distance, out bool isReflection)
	{
		List<InteractiveObjectInfo> interactiveObjects = GetInfoAboutCollidingObjects(direction, distance);

		isReflection = false;

		if (interactiveObjects != null)
		{
			foreach (InteractiveObjectInfo interactiveObject in interactiveObjects)
			{
				List<InteractType> interactTypes = interactiveObject.Object.Interact(_interactElement, interactiveObject.IsEntrance);

				if (interactTypes.Count > 0)
				{
					foreach (InteractType interactType in interactTypes)
					{
						switch (interactType)
						{
							case InteractType.Destroy:
								Debug.LogWarning("Death");
								break;

							case InteractType.Reflection:
								isReflection = true;
								break;
						}
					}
				}
			}
		}
	}

	private List<InteractiveObjectInfo> GetInfoAboutCollidingObjects(Vector2 direction, float distance)
	{
		List<InteractiveObjectInfo> objects = new();
		RaycastHit2D[] hits2D = Physics2D.CircleCastAll(transform.position, _radius, direction, distance, _interactiveLayer);

		if (hits2D.Length > 0)
		{
			foreach (RaycastHit2D hit2D in hits2D)
			{
				if (hit2D.transform.gameObject.TryGetComponent(out IInteractive interactive))
				{
					InteractiveObjectInfo interactInfo = new()
					{
						Object = interactive
					};

					bool isEntrance = true;

					for (int i = 0; i < LastInteractiveObjects.Count; i++)
					{
						if (LastInteractiveObjects[i] == interactive)
						{
							isEntrance = false;
							break;
						}
					}

					interactInfo.IsEntrance = isEntrance;

					objects.Add(interactInfo);

					CurrentInteractiveObjects.Add(interactive);
				}
			}

			return objects;
		}

		return null;
	}
}
