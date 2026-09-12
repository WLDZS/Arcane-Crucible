const string folder = "Assets/__SceneModuleValidation";
const string backup = "E:/Unity project/Arcane-Crucible/output/scene-module/collector-before.json";
if (UnityEditor.EditorApplication.isPlaying)
    return "Refused: editor is playing.";
if (UnityEditor.AssetDatabase.IsValidFolder(folder))
    return "Refused: validation folder already exists.";

var setting = YooAsset.Editor.BundleCollectorSettingData.Setting;
if (setting == null || UnityEditor.EditorUtility.IsDirty(setting))
    return "Refused: collector settings are missing or have unsaved edits.";
var package = setting.Packages.Find(p => p.PackageName == "DefaultPackage");
if (package == null)
    return "Refused: DefaultPackage is missing.";

System.IO.File.WriteAllText(backup, UnityEditor.EditorJsonUtility.ToJson(setting));
UnityEditor.AssetDatabase.CreateFolder("Assets", "__SceneModuleValidation");
var active = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
foreach (var name in new[] { "SceneModuleA", "SceneModuleB" })
{
    var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(
        UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,
        UnityEditor.SceneManagement.NewSceneMode.Additive);
    UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene, folder + "/" + name + ".unity");
    UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);
}
UnityEngine.SceneManagement.SceneManager.SetActiveScene(active);

var group = new YooAsset.Editor.BundleCollectorGroup { GroupName = "SceneModuleValidation" };
group.Collectors.Add(new YooAsset.Editor.BundleCollector
{
    CollectPath = folder,
    CollectorGUID = UnityEditor.AssetDatabase.AssetPathToGUID(folder),
    CollectorType = YooAsset.Editor.ECollectorType.MainAssetCollector
});
package.Groups.Add(group);
UnityEditor.EditorUtility.SetDirty(setting);
UnityEditor.AssetDatabase.SaveAssetIfDirty(setting);
return new { setup = true, originalScene = active.path, temporaryScenes = 2 };
