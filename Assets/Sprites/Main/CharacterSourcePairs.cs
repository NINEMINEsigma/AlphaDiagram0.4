using System;
using AD.Experimental.EditorAsset.Cache;
using UnityEngine;

namespace AD.ProjectTwilight.MainScene
{
    [Serializable]
    [CreateAssetMenu(menuName = "AD/AllCharAssets")]
    public class CharacterSourcePairs : CacheAssets<CacheAssetsKey, CharacterSourcePair>
    {

    }
}
