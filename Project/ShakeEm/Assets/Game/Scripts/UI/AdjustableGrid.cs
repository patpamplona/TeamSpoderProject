using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AdjustableGrid : MonoBehaviour 
{
	[SerializeField] RectTransform   rect;
	[SerializeField] ScrollRect      scroll;
	[SerializeField] GridLayoutGroup grid;

	[SerializeField] int instanceMargin = 2;
	[SerializeField] GameObject imagePrefab;

	[SerializeField] List<Image> images;

	public void FocusOnChild(int child)
	{
		child = Mathf.Clamp(child, 0, scroll.content.childCount);
		images.ForEach(img => ScaleOnFocus(img, child));
		float normalized = ((float)child + ((float)child - 1)) / ((float)scroll.content.childCount + ((scroll.content.childCount - 1) * (grid.spacing.x / grid.cellSize.x)));
		scroll.horizontalNormalizedPosition = normalized;
	}

	private void ScaleOnFocus(Image img, int child)
	{
		bool focus = images[child] == img;
		img.transform.localScale = focus ? new Vector3(1.5f, 1.5f, 1.0f) : Vector3.one;
		Color c = img.color;
		c.a = focus ? 1.0f : 0.5f;
		img.color = c;
	}

	public void SetGrid(List<string> spriteIds)
	{
		if(images == null)
		{
			this.images = new List<Image>();
		}

		for(int i = 0; i < spriteIds.Count || i < this.images.Count; i++)
		{
			if(i >= spriteIds.Count)
			{
				if(i < this.images.Count)
				{
					this.images[i].gameObject.SetActive(false);
				}
			}
			else
			{
				Image img = null;

				if(i >= images.Count)
				{
					GameObject go = new GameObject();
					go.transform.SetParent(transform);
					go.transform.localScale = Vector3.one;
					DisplayUtils.SetLayer(go, gameObject.layer);
					img = go.AddComponent<Image>();
					images.Add(img);
				}

				img = images[i];
				img.preserveAspect = true;
				img.gameObject.name = spriteIds[i];
				img.gameObject.SetActive(true);
				img.sprite = SpritePool.Instance.GetSprite(spriteIds[i]);
			}
		}

		int objCount  = spriteIds.Count + instanceMargin;
		float _width  = objCount * grid.cellSize.x;
		float _margin = (objCount - 1) * grid.spacing.x;

		Vector2 _size = rect.sizeDelta;
		_size.x = _width + _margin;

		rect.sizeDelta = _size;
	}

	public void Clear()
	{
		foreach(Image img in images)
		{
			Destroy (img);
		}

		images.Clear();
	}
}