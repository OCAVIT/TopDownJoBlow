using UnityEngine;
using UnityEngine.UI;

public class SpriteSheetAnimator : MonoBehaviour
{
    public Sprite[] sprites;
    public float framesPerSecond = 10f;

    private Image imageComponent;
    private int currentFrame;
    private float timer;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        if (sprites.Length > 0)
        {
            imageComponent.sprite = sprites[0];
        }
    }

    void Update()
    {
        if (sprites.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= 1f / framesPerSecond)
        {
            timer -= 1f / framesPerSecond;
            currentFrame = (currentFrame + 1) % sprites.Length;
            imageComponent.sprite = sprites[currentFrame];
        }
    }
}