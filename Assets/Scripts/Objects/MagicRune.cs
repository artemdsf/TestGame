using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRune : InteractiveObject
{
	[SerializeField] private Color _activeColor = Color.black;
	[SerializeField] private Color _disactiveColor = Color.white;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private List<GameObject> _gameObjects = new();
	[SerializeField] private bool _mayBeCanceled = false;

	private bool _isActivated = false;

	public void ChangeActivation()
	{
		bool firstActivationCondition = _mayBeCanceled == true || (_mayBeCanceled == false && _isActivated == false);

		if (firstActivationCondition)
		{
			_isActivated = !_isActivated;

			if (_isActivated)
			{
				_spriteRenderer.color = _activeColor;
			}
			else
			{
				_spriteRenderer.color = _disactiveColor;
			}

			if (_gameObjects.Count > 0)
			{
				foreach (GameObject item in _gameObjects)
				{
					item.SetActive(!item.activeInHierarchy);
				}
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}
}
