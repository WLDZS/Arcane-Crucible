using UnityEngine;

namespace BorFramework
{
    /// <summary>
    /// YooAsset 自定义日志处理器：过滤掉退出 Play 时「下载调度器被正常中止」这类无意义的警告。
    /// </summary>
    public class YooAssetLogger : YooAsset.ILogger
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogWarning(string message)
        {
            if (message.Contains("has been aborted"))
                return;

            Debug.LogWarning(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }

        public void LogException(System.Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
