using DiskCardGame;
using System.Collections;
using UnityEngine;

namespace JLPlugin.Data
{
    public class CoroutineWithData
    {
        public Coroutine coroutine { get; private set; }
        public object result;
        private IEnumerator target;
        public CoroutineWithData(IEnumerator target)
        {
            this.target = target;
            this.coroutine = Singleton<BoardManager>.Instance.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (target.MoveNext())
            {
                result = target.Current;
                yield return result;
            }
        }
    }
}