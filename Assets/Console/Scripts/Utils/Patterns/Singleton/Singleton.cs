namespace Patterns {
    public class SingletonComponent<T> : UnityEngine.MonoBehaviour where T : UnityEngine.MonoBehaviour {
        public static T CreateReference() {
            UnityEngine.GameObject gameObject = new UnityEngine.GameObject(typeof(T).Name);
            return gameObject.AddComponent<T>();
        }
        public static T Instance {
            get {
                if (!instance) {
                    UnityEngine.GameObject instanceObject = new UnityEngine.GameObject(typeof(T).Name);
                    instance = instanceObject.AddComponent<T>();
                }

                return instance;
            }
        }

        private static T instance;

        protected virtual void OnEnable() {
            if (instance && !Equals(instance)) {
                Destroy(gameObject);
                return;
            }

            else {
                instance = this as T;
            }
        }
        protected virtual void OnDestroy() {
            if (instance && Equals(instance))
                instance = null;
        }
    }
}