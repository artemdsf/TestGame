using System.Collections.Generic;
using UnityEngine;

public class Button : InteractiveObject
{
	[SerializeField] private List<GameObject> _gameObjects = new();
	[SerializeField] private List<Button> _buttons = new();
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Color _normalColor;
	[SerializeField] private Color _pressedColor;

	public bool IsPressed = false;

	public void Click()
	{
		if (_gameObjects.Count > 0)
		{
			foreach (GameObject item in _gameObjects)
			{
				item.SetActive(IsPressed);
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

	public void ChangeClick()
	{
		if (IsPressed)
		{
			_spriteRenderer.color = _normalColor;
		}
		else
		{
			_spriteRenderer.color = _pressedColor;
		}

		IsPressed = !IsPressed;
	}
}
