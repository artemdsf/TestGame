using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButonsSet : MonoBehaviour
{
	[SerializeField] private List<Button> _buttons = new();
	[SerializeField] private GameObject _gameObject;

	private bool _isAllButtonsDown;

	private void Update()
	{
		_isAllButtonsDown = true;

		foreach (Button button in _buttons)
		{
			if (button.IsPressed == false)
			{
				_isAllButtonsDown = false;
			}
		}

		if (_isAllButtonsDown)
		{
			_gameObject.SetActive(false);
		}
	}
}
