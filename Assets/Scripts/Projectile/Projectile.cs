using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static InteractiveObject;

public class Projectile : MonoBehaviour
{
	[Serializable]
	public class ProjectileElement
	{
		public InteractElement Element;
		public GameObject Prefab;
	}

	[Header("Projectile Parameters")]
	[SerializeField] private float _speed;
	[SerializeField] private LayerMask _interactiveLayer;
	[SerializeField] private List<ProjectileElement> _projectileElements;

	public List<IInteractive> LastInteractiveObjects = new();
	public List<IInteractive> CurrentInteractiveObjects = new();

	public InteractElement CurrentElement = InteractElement.Fire;

	private GameObject _child;
	private GameObject _projectilePrefab;
	private Vector2 _currentDirection;
	private float _radius;

	public void Init(Vector2 direction, float speed, InteractElement element)
	{
		_currentDirection = direction;
		_speed = speed;
		ChangeElement(element);
	}

	public void ChangeElement(InteractElement element)
	{
		foreach (ProjectileElement item in _projectileElements)
		{
			if (item.Element == element)
			{
				if (_child != null)
				{
					Destroy(_child);
				}

				_child = Instantiate(item.Prefab, transform);
			}
		}
		
		CurrentElement = element;
	}

	private void Awake()
	{
		_projectilePrefab = gameObject;
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
		MoveInDirection(Vector2.right, ref _currentDirection.x);
		MoveInDirection(Vector2.up, ref _currentDirection.y);
	}

	private void MoveInDirection(Vector2 direction, ref float projection)
	{
		float distance = _speed * projection * Time.deltaTime;
		HandleInteractiveCollision(direction, distance, out bool isReflection);

		if (isReflection)
		{
			projection *= -1;
			distance *= -1;
		}

		transform.position += (Vector3)(distance * direction);
	}

	private void HandleInteractiveCollision(Vector2 direction, float distance, out bool isReflection)
	{
		List<InteractiveObjectInfo> interactiveObjects = GetInfoAboutCollidingObjects(direction, distance);
		
		isReflection = false;

		if (interactiveObjects != null)
		{
			foreach (InteractiveObjectInfo item in interactiveObjects)
			{
				List<InteractType> interactTypes = item.Object.Interact(CurrentElement, item.IsEntrance);

				if (interactTypes.Count > 0)
				{
					foreach (InteractType interactType in interactTypes)
					{
						switch (interactType)
						{
							case InteractType.Destroy:
								Destroy(gameObject);
								break;

							case InteractType.Reflection:
								isReflection = true;
								break;

							case InteractType.Prism:
								ActivePrismEffect();
								isReflection = true;
								break;

							case InteractType.ChangeToFire:
								ChangeElement(InteractElement.Fire);
								break;

							case InteractType.ChangeToWater:
								ChangeElement(InteractElement.Water);
								break;

							case InteractType.ChangeToEarth:
								ChangeElement(InteractElement.Earth);
								break;

							case InteractType.ChangeToAir:
								ChangeElement(InteractElement.Air);
								break;
						}
					}
				}
			}
		}
	}

	private void ActivePrismEffect()
	{
		GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
		Projectile projectileScript = projectile.GetComponent<Projectile>();

		projectileScript.CurrentInteractiveObjects = CurrentInteractiveObjects.ToList();
		projectileScript.LastInteractiveObjects = CurrentInteractiveObjects.ToList();
		projectileScript.Init(_currentDirection, _speed, CurrentElement);
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
