using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static InteractiveObject;

public class Projectile : MonoBehaviour
{
	[Header("Projectile Parameters")]
	[SerializeField] private float _speed;
	[SerializeField] private LayerMask _interactiveLayer;

	public List<IInteractive> LastInteractiveObjects = new();
	public List<IInteractive> CurrentInteractiveObjects = new();

	private GameObject _projectilePrefab;
	private InteractElement _interactElement = InteractElement.Fire;
	private Vector2 _currentDirection;
	private float _radius;

	public Vector2 InitialDirection
	{
		set { _currentDirection = value; }
	}

	public float Speed
	{
		set { _speed = value; }
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
				List<InteractType> interactTypes = item.Object.Interact(_interactElement, item.IsEntrance);

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
								Debug.Log("Change to fire");
								_interactElement = InteractElement.Fire;
								break;

							case InteractType.ChangeToWater:
								Debug.Log("Change to water");
								_interactElement = InteractElement.Water;
								break;

							case InteractType.ChangeToEarth:
								Debug.Log("Change to earth");
								_interactElement = InteractElement.Earth;
								break;

							case InteractType.ChangeToAir:
								Debug.Log("Change to air");
								_interactElement = InteractElement.Air;
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
		projectileScript.InitialDirection = _currentDirection;
		projectileScript.Speed = _speed;
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