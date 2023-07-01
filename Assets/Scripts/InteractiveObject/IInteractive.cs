using System.Collections.Generic;
using static InteractiveObject;

public interface IInteractive
{
	public List<InteractType> Interact(InteractElement element, bool isEntrance);
}
