using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Parameters")]
	[SerializeField] private float _speed = 5f;
	[SerializeField] private LayerMask _gapLayer;
	[SerializeField] private LayerMask _wallLayer;

	private const string HorizontalAxis = "Horizontal";
	private const string VerticalAxis = "Vertical";

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

		if (!IsObstacleInDirection(direction, movement.magnitude))
		{
			transform.position += (Vector3)movement;
		}
	}

	private bool IsObstacleInDirection(Vector2 direction, float distance)
	{
		return Physics2D.CircleCast(transform.position, _radius, direction, distance, _gapLayer) ||
			   Physics2D.CircleCast(transform.position, _radius, direction, distance, _wallLayer);
	}
}