using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public class NoiseLayer : MonoBehaviour {
    public int seed = 1234567890;

    private const int previewTextureSize = 100;

    [HideInInspector]
    public List<NoiseSettings> noises = new();

    public float GetNoiseValue(int index, float x, float y) {
        float noiseValue = 0;
        NoiseSettings settings = noises[index];

        switch (noises[index].type) {
            case NoiseType.Sine1DX:
                noiseValue = Mathf.Sin((x + settings.offset.x) * settings.scale) * settings.amplitude;
                break;

            case NoiseType.Sine1DY:
                noiseValue = Mathf.Sin((y + settings.offset.y) * settings.scale) * settings.amplitude;
                break;

            case NoiseType.Sine2D:
                noiseValue = Mathf.Sin((x + y + settings.offset.x + settings.offset.y) * settings.scale) * settings.amplitude;
                break;

            case NoiseType.Perlin:
                noiseValue = Mathf.Sin(Mathf.PerlinNoise((x + seed + settings.offset.x) * settings.scale, (y + seed + settings.offset.y) * settings.scale)) * settings.amplitude;
                break;
        }

        return noiseValue;
    }

    public float GetFinalNoise(float x, float y) {
        if (noises.Count == 0)
            return -1;

        float noiseValue = 0;
        for (int i = 0; i < noises.Count; i++) {
            noiseValue += GetNoiseValue(i, x, y);
        }
        return noiseValue;
    }

    public Texture2D GenerateNoiseTexture(int index) {
        Texture2D noiseTexture = new(previewTextureSize, previewTextureSize);

        for (int x = 0; x < previewTextureSize; x++) {
            for (int y = 0; y < previewTextureSize; y++) {
                float noiseValue = GetNoiseValue(index, x, y);

                // normalize the noise value from [-1,1] to [0,1]
                noiseValue = (noiseValue + 1f) / 2f;

                // create a grayscale color based on the noise value
                Color color = new(noiseValue, noiseValue, noiseValue);

                noiseTexture.SetPixel(x, y, color);
            }
        }
        noiseTexture.Apply();
        return noiseTexture;
    }

    public Texture2D GenerateFinalNoiseTexture() {
        Texture2D noiseTexture = new(previewTextureSize, previewTextureSize);

        for (int x = 0; x < previewTextureSize; x++) {
            for (int y = 0; y < previewTextureSize; y++) {
                float noiseValue = GetFinalNoise(x, y);

                // normalize the noise value from [-1,1] to [0,1]
                noiseValue = (noiseValue + 1f) / 2f;

                // create a grayscale color based on the noise value
                Color color = new(noiseValue, noiseValue, noiseValue);

                noiseTexture.SetPixel(x, y, color);
            }
        }
        noiseTexture.Apply();
        return noiseTexture;
    }
}

[System.Serializable]
public class NoiseSettings {
    public NoiseType type;
    public float scale = 0.1f;
    public float amplitude = 10f;
    public Vector2 offset = Vector2.zero;
    public Texture2D texture;
    // octaves
}

public enum NoiseType {
    Sine1DX,
    Sine1DY,
    Sine2D,
    Perlin,
    Simplex
}