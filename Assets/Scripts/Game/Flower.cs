using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    // consider different colors from Leaf's ones
    public static readonly Color defaultColor = new Color(1.0f, 100.0f / 255.0f, 40.0f / 255.0f, 1.0f);
    public static readonly Color springColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public static readonly Color summerColor = new Color(130.0f / 255.0f, 1.0f, 140.0f / 255.0f, 1.0f);
    public static readonly Color autumnColor = new Color(1.0f, 240.0f / 255.0f, 120.0f / 255.0f, 1.0f);

    private static Shader grayscaleShaderCache = null;
    private static Shader defaultSpriteShader = null;

    private GameController.Seasons season = GameController.Seasons.Spring;

    public void DefaultShader()
    {
        if (Flower.defaultSpriteShader == null)
        {
            Flower.defaultSpriteShader = Shader.Find("Sprites/Default");
        }

        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        renderer.material.shader = Flower.defaultSpriteShader;
    }
    public void SeasonColor()
    {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        switch (this.season)
        {
            case GameController.Seasons.Spring: renderer.color = Flower.springColor; break;
            case GameController.Seasons.Summer: renderer.color = Flower.summerColor; break;
            case GameController.Seasons.Autumn: renderer.color = Flower.autumnColor; break;
            default: renderer.color = Flower.springColor; break;
        }
    }
    public void GrayShader()
    {
        if (Flower.grayscaleShaderCache == null)
        {
            Flower.grayscaleShaderCache = Resources.Load<Shader>("Shaders/GrayScale");
        }

        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        renderer.material.shader = Flower.grayscaleShaderCache;
    }
    public void SetSeason(GameController.Seasons season)
    {
        this.season = season;
    }

}
