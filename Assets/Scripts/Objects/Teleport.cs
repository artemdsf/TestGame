using UnityEngine;

public class Teleport : InteractiveObject
{
	[SerializeField] private GameObject _anotherTeleport;
	[SerializeField] private PlayerMovement _player;

	private IInteractive _interactiveAnotherTeleport;

	protected override void Awake()
	{
		_interactiveAnotherTeleport = _anotherTeleport.GetComponent<IInteractive>();

		base.Awake();
	}

	public void TeleportPlayer()
	{
		_player.transform.position = _anotherTeleport.transform.position;
		_player.CurrentInteractiveObjects.Add(_interactiveAnotherTeleport);
		_player.LastInteractiveObjects.Add(_interactiveAnotherTeleport);
	}
}
