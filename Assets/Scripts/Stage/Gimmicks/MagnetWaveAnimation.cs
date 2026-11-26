using UnityEngine;

public class MagnetWaveAnimation : MonoBehaviour
{
    private SpriteRenderer[] waves; // 吸い込み波
    [SerializeField] private GameObject wave;
    private int waveCount = 13;

    [Header("WaveMovement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float leftX = 0f;
    [SerializeField] private float rightX = 10f;

    [SerializeField] private float fadeWidth = 1f; // フェードのかかる長さ（両端）

    private void Start()
    {
        waves = new SpriteRenderer[waveCount];
        float space = (rightX - leftX) / waveCount;

        for (int i = 0; i < waveCount; i++)
        {
            GameObject instance = Instantiate(wave, transform, false);

            Vector3 pos = instance.transform.localPosition;
            pos.y = leftX + space * (i + 1);
            instance.transform.localPosition = pos;

            waves[i] = instance.GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        foreach (var wave in waves)
        {
            Vector3 pos = wave.transform.localPosition;

            // 左へ移動
            pos.y -= speed * Time.deltaTime;

            float width = rightX - leftX;
            if (pos.y < leftX)
            {
                pos.y = rightX - Mathf.Repeat(leftX - pos.y, width);
            }

            wave.transform.localPosition = pos;

            // 両端透明にする処理
            float leftAlpha = Mathf.InverseLerp(leftX, leftX + fadeWidth, pos.y);
            float rightAlpha = Mathf.InverseLerp(rightX, rightX - fadeWidth, pos.y);

            float alpha = Mathf.Min(leftAlpha, rightAlpha);
            Color color = wave.color;
            color.a = alpha;
            wave.color = color;
        }
    }
}
