if (UnityEditor.EditorApplication.isPlaying)
    return "Refused: exit Play Mode before cleanup.";
const string folder = "Assets/__SceneModuleValidation";
const string backup = "E:/Unity project/Arcane-Crucible/output/scene-module/collector-before.json";
if (!System.IO.File.Exists(backup))
    return "Refused: original collector snapshot is missing.";
var setting = YooAsset.Editor.BundleCollectorSettingData.Setting;
UnityEditor.EditorJsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(backup), setting);
UnityEditor.EditorUtility.SetDirty(setting);
UnityEditor.AssetDatabase.SaveAssetIfDirty(setting);
if (UnityEditor.AssetDatabase.IsValidFolder(folder))
    UnityEditor.AssetDatabase.DeleteAsset(folder);
return new { restored = true, activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path };
