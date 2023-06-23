using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Parameters")]
	[SerializeField] private float _speed = 5f;
	[SerializeField] private LayerMask _gapLayer;
	[SerializeField] private LayerMask _wallLayer;

	private Vector2 _movementInput;
	private float _raycastDistance;

	private void Awake()
	{
		_raycastDistance = transform.localScale.x / 2;
	}

	private void Update()
	{
		_movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

		Vector2 newPosition = transform.position;

		Vector2 horizontalCheck = Vector2.right * _movementInput.x;

		if (IsGapInDirection(horizontalCheck) == false && IsWallInDirection(horizontalCheck) == false)
		{
			newPosition.x += _movementInput.x * _speed * Time.deltaTime;
		}

		Vector2 verticalCheck = Vector2.up * _movementInput.y;

		if (IsGapInDirection(verticalCheck) == false && IsWallInDirection(verticalCheck) == false)
		{
			newPosition.y += _movementInput.y * _speed * Time.deltaTime;
		}

		transform.position = newPosition;
	}

	private bool IsGapInDirection(Vector2 direction)
	{
		return Physics2D.Raycast(transform.position, direction, _raycastDistance, _gapLayer);
	}

	private bool IsWallInDirection(Vector2 direction)
	{
		return Physics2D.Raycast(transform.position, direction, _raycastDistance, _wallLayer);
	}
}
