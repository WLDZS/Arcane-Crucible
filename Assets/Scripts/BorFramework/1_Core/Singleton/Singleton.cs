namespace BorFramework
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;

        public static T Ins
        {
            get
            {
                _instance ??= new T();
                return _instance;
            }
        }
    }
}