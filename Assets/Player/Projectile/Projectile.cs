using UnityEngine;

public class Projectile : MonoBehaviour
{
	[Header("Projectile Parameters")]
	[SerializeField] private float _speed = 10f;
	[SerializeField] private LayerMask _wallLayer;

	private Vector2 _currentDirection;
	private float _raycastDistance;

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
		_raycastDistance = transform.localScale.x / 2;
	}

	private void Update()
	{
		Vector2 newPosition = transform.position;

		RaycastHit2D hitX = Physics2D.Raycast(transform.position, Vector2.right * _currentDirection.x, _raycastDistance, _wallLayer);
		
		if (hitX)
		{
			_currentDirection.x = -_currentDirection.x;
		}
		else
		{
			newPosition.x += _currentDirection.x * _speed * Time.deltaTime;
		}

		RaycastHit2D hitY = Physics2D.Raycast(transform.position, Vector2.up * _currentDirection.y, _raycastDistance, _wallLayer);
		
		if (hitY)
		{
			_currentDirection.y = -_currentDirection.y;
		}
		else
		{
			newPosition.y += _currentDirection.y * _speed * Time.deltaTime;
		}

		transform.position = newPosition;
	}
}
