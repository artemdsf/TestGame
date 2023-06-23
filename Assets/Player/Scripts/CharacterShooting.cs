using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
	[Header("Shooting Parameters")]
	[SerializeField] private GameObject _projectilePrefab;
	[SerializeField] private float _projectileSpeed = 10f;
	[SerializeField] private float _projectileLifetime = 3f;

	private readonly KeyCode _attackKey = KeyCode.Mouse0;

	private void Update()
	{
		if (Input.GetKeyDown(_attackKey))
		{
			ShootProjectile();
		}
	}

	private void ShootProjectile()
	{
		Vector2 cursorPositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector2 shootingDirection = (cursorPositionInWorld - (Vector2)transform.position).normalized;

		GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
		Projectile projectileScript = projectile.GetComponent<Projectile>();

		projectileScript.InitialDirection = shootingDirection;
		projectileScript.Speed = _projectileSpeed;

		Destroy(projectile, _projectileLifetime);
	}
}
