using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
	[Header("Shooting Parameters")]
	[SerializeField] private GameObject _projectilePrefab;
	[SerializeField] private float _projectileSpeed = 10f;
	[SerializeField] private float _attackCooldown = 1f;

	private readonly KeyCode _attackKey = KeyCode.Mouse0;
	private InteractElement _currentElement = InteractElement.Fire;
	private float _currentCooldown;

	public void ChangeElement(InteractElement element)
	{
		_currentElement = element;
	}

	private void Awake()
	{
		_currentCooldown = _attackCooldown;
		ChangeElement(_currentElement);
	}

	private void Update()
	{
		_currentCooldown += Time.deltaTime;

		if (_currentCooldown > _attackCooldown && Input.GetKeyDown(_attackKey))
		{
			ShootProjectile();

			_currentCooldown = 0;
		}
	}

	private void ShootProjectile()
	{
		Vector2 cursorPositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector2 shootingDirection = (cursorPositionInWorld - (Vector2)transform.position).normalized;

		GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
		Projectile projectileScript = projectile.GetComponent<Projectile>();

		projectileScript.Init(shootingDirection, _projectileSpeed, _currentElement);
	}
}
