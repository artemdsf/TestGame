using System.Collections.Generic;
using UnityEngine;

public class MagicRune : InteractiveObject
{
	[SerializeField] private List<GameObject> _gameObjects = new();
	[SerializeField] private List<MagicRune> _runes = new();
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Color _activeColor = Color.black;
	[SerializeField] private Color _disactiveColor = Color.white;
	[SerializeField] private bool _mayBeCanceled = false;

	public bool IsPressed => _isPressed;

	private bool _isPressed = false;

	public void ChangeActivation()
	{
		bool firstActivationCondition = _mayBeCanceled == true || (_mayBeCanceled == false && _isPressed == false);

		if (firstActivationCondition)
		{
			if (_gameObjects.Count > 0)
			{
				foreach (GameObject item in _gameObjects)
				{
					item.SetActive(!item.activeInHierarchy);
				}
			}

			if (_runes.Count > 0)
			{
				foreach (MagicRune button in _runes)
				{
					button.ChangeClick();
				}
			}

			ChangeClick();
		}
	}

	public void ChangeClick()
	{
		if (_isPressed)
		{
			_spriteRenderer.color = _disactiveColor;
		}
		else
		{
			_spriteRenderer.color = _activeColor;
		}

		_isPressed = !_isPressed;
	}

	protected override void Awake()
	{
		base.Awake();
	}
}
