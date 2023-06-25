using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Parameters")]
	[SerializeField] private float _speed = 5f;
	[SerializeField] private LayerMask _gapLayer;
	[SerializeField] private LayerMask _wallLayer;
	[SerializeField] private LayerMask _interactiveLayer;

	private const string HorizontalAxis = "Horizontal";
	private const string VerticalAxis = "Vertical";

	private InteractElement _interactElement = InteractElement.Touch;
	private float _radius;

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
		Vector2 movementInput = new Vector2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis)).normalized;

		MoveInDirection(Vector2.right * movementInput.x);
		MoveInDirection(Vector2.up * movementInput.y);
	}

	private void MoveInDirection(Vector2 direction)
	{
		Vector2 movement = _speed * Time.deltaTime * direction;

		if (IsObstacleInDirection(direction, movement.magnitude) == false &&
			CheckAndHandleInteractiveCollision(direction, movement.magnitude) == false)
		{
			transform.position += (Vector3)movement;
		}
	}

	private bool CheckAndHandleInteractiveCollision(Vector2 direction, float distance)
	{
		IInteractive interactiveObject = GetCollidingInteractiveObject(direction, distance);

		if (interactiveObject != null)
		{
			InteractType? interactType = interactiveObject.Interact(_interactElement);

			switch (interactType)
			{
				case InteractType.Reflection:
					return true;

				case InteractType.Destroy:
					Debug.LogWarning("Death");
					break;
			}
		}

		return false;
	}

	private bool IsObstacleInDirection(Vector2 direction, float distance)
	{
		return Physics2D.CircleCast(transform.position, _radius, direction, distance, _gapLayer) ||
			   Physics2D.CircleCast(transform.position, _radius, direction, distance, _wallLayer);
	}

	private IInteractive GetCollidingInteractiveObject(Vector2 direction, float distance)
	{
		RaycastHit2D hit2D = Physics2D.CircleCast(transform.position, _radius, direction, distance, _interactiveLayer);

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