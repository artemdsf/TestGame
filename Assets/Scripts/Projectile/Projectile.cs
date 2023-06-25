using UnityEngine;

public class Projectile : MonoBehaviour
{
	[Header("Projectile Parameters")]
	[SerializeField] private float _speed = 10f;
	[SerializeField] private LayerMask _wallLayer;
	[SerializeField] private LayerMask _interactiveLayer;

	private InteractElement _interactElement = InteractElement.Fire;
	private Vector2 _currentDirection;
	private float _radius;

	public Vector2 InitialDirection
	{
		set { _currentDirection = value.normalized; }
	}

	public float Speed
	{
		set { _speed = value; }
	}

	private void Awake()
	{
		_radius = transform.localScale.x / 2;
	}

	private void Update()
	{
		ProcessMovement();
	}

	private void ProcessMovement()
	{
		Vector2 movement = _speed * Time.deltaTime * _currentDirection;

		if (IsObstacleInDirection(Vector2.right * _currentDirection.x, Mathf.Abs(movement.x)) ||
			CheckAndHandleInteractiveCollision(Vector2.right * _currentDirection.x))
		{
			_currentDirection.x = -_currentDirection.x;
		}

		if (IsObstacleInDirection(Vector2.up * _currentDirection.y, Mathf.Abs(movement.y)) ||
			CheckAndHandleInteractiveCollision(Vector2.up * _currentDirection.y))
		{
			_currentDirection.y = -_currentDirection.y;
		}

		transform.position = (Vector2)transform.position + _currentDirection * _speed * Time.deltaTime;
	}

	private bool CheckAndHandleInteractiveCollision(Vector2 direction)
	{
		IInteractive interactiveObject = GetCollidingInteractiveObject(direction);

		if (interactiveObject != null)
		{
			InteractType? interactType = interactiveObject.Interact(_interactElement);

			switch (interactType)
			{
				case InteractType.Destroy:
					Destroy(gameObject);
					return true;

				case InteractType.Reflection:
					return true;
			}
		}

		return false;
	}

	private bool IsObstacleInDirection(Vector2 direction, float distance)
	{
		return Physics2D.CircleCast(transform.position, _radius, direction, distance, _wallLayer);
	}

	private IInteractive GetCollidingInteractiveObject(Vector2 direction)
	{
		RaycastHit2D hit2D = Physics2D.Raycast(transform.position, direction, _radius, _interactiveLayer);

		if (hit2D)
		{
			if (hit2D.transform.gameObject.TryGetComponent(out IInteractive interactive))
			{
				return interactive;
			}
		}

		return null;
	}
}
