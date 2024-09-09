using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public enum BgmType
    {
        BGM,
    }
    
    public enum ClipType
    {
        Fire1,
        Fire2,
        Fire3,
        Jump,
        GetItem,
        RealJump,
        Zap,
        Damaged,
    }
    
    public class SoundData : ScriptableSingleton<SoundData>
    {
        
        [Serializable]
        public class BgmPair
        {
            public BgmType type;
            public AudioClip clip;
        }
        
        [Serializable]
        public class ClipPair
        {
            public ClipType type;
            public AudioClip clip;
        }
        
        [SerializeField] private List<BgmPair> _bgmPairs;
        [SerializeField] private List<ClipPair> _clipPairs;

        public AudioClip GetBgmClip(BgmType type) => _bgmPairs.Find(pair => pair.type == type).clip;
        public AudioClip GetSfxClip(ClipType type) => _clipPairs.Find(pair => pair.type == type).clip;
    }
}
