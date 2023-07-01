using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColorSetter : MonoBehaviour
{
	[SerializeField] private Color color = Color.white;
	[SerializeField] private List<SpriteRenderer> targetRenderers = new();

	private Color lastColor;

	private void Update()
	{
		if (color != lastColor && targetRenderers.Count > 0)
		{
			foreach (SpriteRenderer renderer in targetRenderers)
			{
				if (renderer != null)
				{
					renderer.color = color;
				}
			}

			lastColor = color;
		}
	}
}
