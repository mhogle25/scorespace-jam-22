using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite up;
    [SerializeField] private Sprite down;

    private bool flag = false;

    public void Pull()
    {
        flag = !flag;
        spriteRenderer.sprite = flag ? up : down;
    }
}
