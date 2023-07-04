using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
	[Serializable]
	public class AttackElement
	{
		public InteractElement Element;
		public KeyCode KeyCode;
	}

	[Header("Shooting Parameters")]
	[SerializeField] private GameObject _projectilePrefab;
	[SerializeField] private float _projectileSpeed = 10f;
	[SerializeField] private float _attackCooldown = 1f;
	[SerializeField] private List<AttackElement> _attackElements = new();

	public int ElementsUnlocked = 0;

	private readonly KeyCode _attackKey = KeyCode.Mouse0;
	private InteractElement _currentElement = InteractElement.Fire;
	private float _currentCooldown;

	private void Awake()
	{
		_currentCooldown = _attackCooldown;
	}

	private void Update()
	{
		_currentCooldown += Time.deltaTime;

		if (ElementsUnlocked > 0 && _currentCooldown > _attackCooldown && Input.GetKeyDown(_attackKey))
		{
			ShootProjectile();

			_currentCooldown = 0;
		}

		if (Input.anyKeyDown)
		{
			ChangeElement();
		}
	}

	private void ChangeElement()
	{
		if (_attackElements.Count == 0)
		{
			AttackElement attackElement;

			for (int i = 0; i < _attackElements.Count; i++)
			{
				attackElement = _attackElements[i];

				if (i <= ElementsUnlocked && Input.GetKeyDown(attackElement.KeyCode))
				{
					_currentElement = attackElement.Element;
				}
			}
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
