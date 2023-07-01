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
	[SerializeField] private List<AttackElement> _attackElements = new();

	public int ElementsUnlocked = 0;

	private readonly KeyCode _attackKey = KeyCode.Mouse0;
	private InteractElement _currentElement = InteractElement.Fire;

	private void Update()
	{
		if (ElementsUnlocked > 0 && Input.GetKeyDown(_attackKey))
		{
			ShootProjectile();
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

		projectileScript.InitialDirection = shootingDirection;
		projectileScript.Speed = _projectileSpeed;
		projectileScript.InteractElement = _currentElement;
	}
}
