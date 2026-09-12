using UnityEngine;
using UnityEngine.InputSystem;

namespace BorFramework
{
    public sealed class GameBoot : SingletonMono<GameBoot>
    {
        private const string DefaultInputAssetName = "Input/DefultInputSystem";

        [SerializeField]
        private InputActionAsset _inputActions;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateAutomatically()
        {
            if (Ins != null)
                return;

            var go = new GameObject("GameBoot");
            DontDestroyOnLoad(go);
            go.AddComponent<GameBoot>();
        }

        protected override void Awake()
        {
            base.Awake();

            if (Ins != this)
                return;

            DontDestroyOnLoad(gameObject);
            Init();
        }

        private void Start()
        {
            if (Ins != this || !GameHub.Ins.IsRunning)
                return;

            var eventModule = GameHub.Ins.GetModule<IEventModule>();
            eventModule?.Publish(new FrameworkReadyEvent());
        }

        private void Init()
        {
            if (GameHub.Ins.IsInitialized)
                return;

            GameHub.Ins.Init();

            GameHub.Ins.RegisterModule<ILogModule>(new LogModule());

            var monoModule = new MonoModule();
            GameHub.Ins.RegisterModule<IMonoModule>(monoModule);
            GameHub.Ins.RegisterModule<IEventModule>(new EventModule());
            GameHub.Ins.RegisterModule<IInputModule>(new InputModule(ResolveInputActions()));
            GameHub.Ins.RegisterModule<IEntityModule>(new EntityModule(monoModule));
            GameHub.Ins.RegisterModule<IResourceModule>(new ResourceModule());
            GameHub.Ins.RegisterModule<ISaveModule>(new SaveModule());
            GameHub.Ins.RegisterModule<IConfigModule>(new ConfigModule());
            GameHub.Ins.RegisterModule<ISceneModule>(new SceneModule());
            GameHub.Ins.RegisterModule<IUIModule>(new UIModule());

            GameHub.Ins.InitModules();
            GameHub.Ins.StartModules();

            var logger = GameHub.Ins.GetModule<ILogModule>();
            logger?.Log("GameHub初始化完成");
        }

        private InputActionAsset ResolveInputActions()
        {
            if (_inputActions != null)
                return _inputActions;

            _inputActions = Resources.Load<InputActionAsset>(DefaultInputAssetName);
            return _inputActions;
        }

        private void FixedUpdate()
        {
            if (!GameHub.Ins.IsRunning)
                return;

            GameHub.Ins.GetModule<IMonoModule>()?.DoFixedUpdate(Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (!GameHub.Ins.IsRunning)
                return;

            GameHub.Ins.GetModule<IMonoModule>()?.DoUpdate(Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (!GameHub.Ins.IsRunning)
                return;

            GameHub.Ins.GetModule<IMonoModule>()?.DoLateUpdate(Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (Ins != this)
                return;

            GameHub.Ins.StopModules();
            GameHub.Ins.DisposeModules();
        }
    }
}
