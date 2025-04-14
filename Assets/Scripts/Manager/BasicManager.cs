using DesignPattern;
using UnityEngine;

namespace Manager {
    public class BasicManager<T> : SingletonMonoBehaivour<T>, ISingletonable where T : MonoBehaviour, ISingletonable {
        public virtual string Name => "Manager";
        public virtual bool IsIndestructible => true;

        public virtual bool IsDebugBeforeName => false;
    }
}