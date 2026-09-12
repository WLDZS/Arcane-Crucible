const string report = "E:/Unity project/Arcane-Crucible/output/scene-module/smoke-results.txt";
System.IO.File.WriteAllText(report, "SceneModule simplified implementation\n");
UnityEngine.Application.runInBackground = true;
int failures = 0;
void Check(string name, bool passed)
{
    if (!passed) failures++;
    System.IO.File.AppendAllText(report, (passed ? "PASS " : "FAIL ") + name + "\n");
}
async Cysharp.Threading.Tasks.UniTask Run()
{
    var scenes = BorFramework.GameHub.Ins.GetModule<BorFramework.ISceneModule>();
    Check("empty address", !await scenes.LoadSceneAsync(""));
    var first = scenes.LoadSceneAsync("SceneModuleA", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    Check("busy during load", scenes.IsBusy);
    Check("concurrent load rejected", !await scenes.LoadSceneAsync("SceneModuleB"));
    Check("additive A", await first);
    Check("lookup A", scenes.TryGetScene("SceneModuleA", out _));
    Check("duplicate rejected", !await scenes.LoadSceneAsync("SceneModuleA"));
    Check("additive B", await scenes.LoadSceneAsync("SceneModuleB", UnityEngine.SceneManagement.LoadSceneMode.Additive));
    Check("set active B", scenes.SetActiveScene("SceneModuleB") && scenes.ActiveScene.name == "SceneModuleB");
    Check("unload A", await scenes.UnloadSceneAsync("SceneModuleA"));
    Check("A no longer loaded", !scenes.TryGetScene("SceneModuleA", out _));
    Check("single A", await scenes.LoadSceneAsync("SceneModuleA"));
    Check("Single removes B", !scenes.TryGetScene("SceneModuleB", out _) && scenes.ActiveScene.name == "SceneModuleA");
    Check("last scene protected", !await scenes.UnloadSceneAsync("SceneModuleA"));
    Check("unknown address rejected", !await scenes.LoadSceneAsync("__MissingSceneModuleScene"));
    Check("non-scene address rejected", !await scenes.LoadSceneAsync("YooAssetSettings"));
    Check("busy cleared", !scenes.IsBusy);
    System.IO.File.AppendAllText(report, "DONE failures=" + failures + "\n");
}
Cysharp.Threading.Tasks.UniTaskExtensions.Forget(Run());
return "started";
