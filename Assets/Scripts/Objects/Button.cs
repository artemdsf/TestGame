using System.Collections.Generic;
using UnityEngine;

public class Button : InteractiveObject
{
	[SerializeField] private List<GameObject> _gameObjects = new();
	[SerializeField] private List<Button> _buttons = new();
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Color _normalColor;
	[SerializeField] private Color _pressedColor;
	[SerializeField] private bool _mayBeCanceled = true;

	public bool IsPressed => _isPressed;

	private bool _isPressed = false;

	public void Click()
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

			if (_buttons.Count > 0)
			{
				foreach (Button button in _buttons)
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
			_spriteRenderer.color = _normalColor;
		}
		else
		{
			_spriteRenderer.color = _pressedColor;
		}

		_isPressed = !_isPressed;
	}
}
