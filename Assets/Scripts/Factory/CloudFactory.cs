using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CloudFactory
{
    // Start is called before the first frame update

    public static (GameObject,Cloud) MakeCloud(bool isActive = false)
    {
        GameObject cloud = new GameObject();
        SpriteRenderer spriteRenderer = cloud.AddComponent<SpriteRenderer>();
        
        
        var texArray = Resources.LoadAll<Texture2D>("Sprites/Clouds");
        var tex = texArray[Random.Range(0, texArray.Length)];
        
        
        Sprite sprite = Sprite.Create(tex, new Rect(0,0,tex.width, tex.height), new Vector2(0.5f, 0.5f));
        spriteRenderer.sprite = sprite;

        cloud.SetActive(isActive);
        return (cloud,cloud.AddComponent<Cloud>());
    }
    

}
