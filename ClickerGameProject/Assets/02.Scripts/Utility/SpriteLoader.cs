using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteLoader : MonoBehaviour
{
    public Dictionary<string,SpriteAtlas> spriteAtlases = new Dictionary<string, SpriteAtlas>();

    private void Start()
    {
        spriteAtlases = GetSprites();
    }

    private Dictionary<string, SpriteAtlas> GetSprites()
    {
        Dictionary<string, SpriteAtlas> table = new Dictionary<string, SpriteAtlas>();
        SpriteAtlas[] allSpriteAtlases = Resources.LoadAll<SpriteAtlas>("Sprites"); 

        foreach(var element in allSpriteAtlases)
        {
            table.Add(element.name, element);
            Debug.Log("atlas add " + element.name);
        }
        return table;
    }


    private void RequestAtlas(string _key)
    {
        bool exist = ExistSpriteAtlas(_key);
        if(exist == false)
        {
            var result = Resources.Load<SpriteAtlas>("Sprites/" + _key);
            if(result == null)
                return;
            spriteAtlases.Add(_key, result);
        }
    }

    private bool ExistSpriteAtlas(string _key)
    {
        if (spriteAtlases == null) 
            return false;
        return spriteAtlases.ContainsKey(_key);
    }

    private SpriteAtlas GetSpriteAtlas(string _key)
    {
        if(ExistSpriteAtlas(_key) == false) 
            return null;
        spriteAtlases.TryGetValue(_key, out SpriteAtlas returnValue);
        return returnValue;
    }
}
