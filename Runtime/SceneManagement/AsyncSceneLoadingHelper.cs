using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.SceneManagement
{
    public static class AsyncSceneLoadingHelper
    {
        public static IEnumerator WaitForScenesLoadAsync(IEnumerable<AsyncOperation> sceneLoadOperations, Action callback)
        {
            if (callback == null) throw new NullReferenceException();

            while (true)
            {
                if (ScenesFinishedLoading(sceneLoadOperations))
                {
                    break;
                }
                yield return new WaitForSecondsRealtime(0.02f);
            }

            callback();
        }

        public static bool ScenesFinishedLoading(IEnumerable<AsyncOperation> sceneLoadOperations)
        {
            foreach(var operation in sceneLoadOperations)
            {
                if (!operation.isDone)
                {
                    return false;
                }
            }
            return true;
        }

        public static IEnumerator WaitForSceneLoadAsync(AsyncOperation sceneLoadOperation, Action callback)
        {
            float taskProgress = 0;
            while(taskProgress < 1)
            {
                taskProgress = sceneLoadOperation.isDone ? 1 : sceneLoadOperation.progress;
                yield return new WaitForSecondsRealtime(0.02f);
            }

            callback();
        }
    }
}
