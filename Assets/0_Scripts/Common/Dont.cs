using System;
using UnityEngine;

namespace Common
{
    public class Dont : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
