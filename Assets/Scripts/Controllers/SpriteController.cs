using System.Collections;
using System.Drawing;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private SpriteSwitcher switcher;
    private Animator animator;
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private Vector3 mainSize;

    private void Awake()
    {
        switcher = GetComponent<SpriteSwitcher>();
        animator = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(Sprite sprite)
    {
        switcher.SetImage(sprite);
        mainSize = rect.localScale;
    }
    public void ChangeSize(float size)
    {
        rect.localScale = new Vector3(/*
             (mainSize.x/ size2) * size,
             (mainSize.y/ size2) * size,
             (mainSize.z / size2 ) * size

             (mainSize.x) * size,
             (mainSize.y) * size,
             (mainSize.z) * size*/

             (mainSize.x) * size,
             (mainSize.y) * size,
             (mainSize.z) * size
            );
    }
    public void ChangeLocation(Vector2 coords)
    {
        rect.localPosition = coords;
    }

    public void Show(Vector2 coords, bool isAnimated = true)
    {
        if (isAnimated)
        {
            animator.enabled = true;
            animator.SetTrigger("Show");
        }
        else
        {
            animator.enabled = false;
            canvasGroup.alpha = 1;
        }
        rect.localPosition = coords;
    }

    public void Hide(bool isAnimated = true)
    {
        if (isAnimated)
        {
            animator.enabled = true;
            switcher.SyncImages();
            animator.SetTrigger("Hide");
        }
        else
        {
            animator.enabled = false;
            canvasGroup.alpha = 0;
        }
    }

    public void Move(Vector2 coords, float speed, bool isAnimated = true)
    {
        if (isAnimated)
        {
            StartCoroutine(MoveCoroutine(coords, speed));
        }
        else
        {
            rect.localPosition = coords;
        }
    }

    private IEnumerator MoveCoroutine(Vector2 coords, float speed)
    {
        while(rect.localPosition.x != coords.x || rect.localPosition.y != coords.y)
        {
            rect.localPosition = Vector2.MoveTowards(rect.localPosition, coords,
                Time.deltaTime * 1000f * speed);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SwitchSprite(Sprite sprite, bool isAnimated = true)
    {
        if(switcher.GetImage() != sprite)
        {
            if (isAnimated)
            {
                switcher.SwitchImage(sprite);
            }
            else
            {
                switcher.SetImage(sprite);
            }
        }
    }
}
