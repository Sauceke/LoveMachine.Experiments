using IllusionUtility.GetUtility;
using KKAPI.MainGame;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace LoveMachine.Experiments
{
    internal class KoikatsuDepthController<T> : GameCustomFunctionController
        where T : DepthPOC
    {
        private static readonly string[] supportedAnimations = new[]
        {
            "WLoop", "SLoop",
            // anal
            "A_WLoop", "A_SLoop", "A_OLoop"
        };

        private static readonly string[] penetrableAnimations = new[]
        {
            "Idle", "OUT_A"
        };

        private static readonly string[] femaleBoneNames = new[]
        {
            "cf_n_pee", "cf_J_MouthCavity", "cf_j_index04_L",
            "cf_j_index04_R", "k_f_toeL_00", "k_f_toeR_00",
            "k_f_munenipL_00", "k_f_munenipR_00"
        };

        private T depthSensor;
        private HFlag flags;

        private void Start() => depthSensor = gameObject.AddComponent<T>();

        protected override void OnStartH(MonoBehaviour proc, HFlag hFlag, bool vr)
        {
            base.OnStartH(proc, hFlag, vr);
            flags = hFlag;
            StartCoroutine(Run());
        }

        protected override void OnEndH(MonoBehaviour proc, HFlag hFlag, bool vr)
        {
            base.OnEndH(proc, hFlag, vr);
            StopAllCoroutines();
        }

        private bool IsControllable => supportedAnimations.Contains(flags.nowAnimStateName);

        private bool IsPenetrable => penetrableAnimations.Contains(flags.nowAnimStateName);

        private IEnumerator Run()
        {
            if (depthSensor == null)
            {
                Config.Logger.LogInfo($"{GetType()} is disabled.");
                yield break;
            }
            int animHash = 0;
            float phase = 0f;
            IEnumerator calibrate()
            {
                float min_distance = float.PositiveInfinity;
                for (float t = 0f; t < 1f; t += 0.1f)
                {
                    SkipToTime(t);
                    yield return new WaitForEndOfFrame();
                    float distance = GetDistance();
                    phase = distance < min_distance ? t : phase;
                    min_distance = Mathf.Min(distance, min_distance);
                }
            }
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (!depthSensor.IsDeviceConnected)
                {
                    yield return new WaitForSecondsRealtime(1f);
                    continue;
                }
                if (IsPenetrable)
                {
                    Config.Logger.LogInfo("Penetrable position found. Inserting.");
                    yield return StartCoroutine(Penetrate());
                    continue;
                }
                if (!IsControllable)
                {
                    SetSpeed(1f);
                    continue;
                }
                yield return new WaitForEndOfFrame();
                int currentHash = flags.nowAnimationInfo.GetHashCode();
                if (animHash != currentHash)
                {
                    animHash = currentHash;
                    yield return calibrate();
                }
                StartCoroutine(HandleDepth(phase));
            }
        }

        private float GetDistance()
        {
            var female = flags.lstHeroine[0].chaCtrl.objBodyBone.transform;
            var male = flags.player.chaCtrl.objBodyBone.transform;
            var balls = male.FindLoop("k_f_tamaL_00").transform;
            var femaleBones = femaleBoneNames.Select(name => female.FindLoop(name).transform);
            return femaleBones.Min(bone => (balls.position - bone.position).sqrMagnitude);
        }

        private IEnumerator Penetrate()
        {
            flags.isCondom = true;
            do
            {
                flags.click = HFlag.ClickKind.insert;
                yield return new WaitForSeconds(.1f);
            }
            while (IsPenetrable);
            do
            {
                flags.click = HFlag.ClickKind.modeChange;
                flags.speedCalc = 0.5f;
                yield return new WaitForSeconds(.1f);
            }
            while (!flags.nowAnimStateName.Contains("WLoop"));
            flags.click = HFlag.ClickKind.motionchange;
            SetSpeed(0f);
        }

        private IEnumerator HandleDepth(float phase)
        {
            SetSpeed(0f);
            float startNormTime = flags.lstHeroine[0].chaCtrl.animBody
                .GetCurrentAnimatorStateInfo(0)
                .normalizedTime;
            float depth = depthSensor.Depth;
            float targetNormTime = phase + 0.5f - depth / 2f;
            float delta = targetNormTime - startNormTime;
            float step = Mathf.Sign(delta) / 30f;
            int steps = (int)(delta / step);
            for (int i = 1; i <= steps; i++)
            {
                SkipToTime(startNormTime + step * i);
                yield return new WaitForEndOfFrame();
                if (depthSensor.Depth != depth)
                {
                    yield break;
                }
            }
        }

        private void SetSpeed(float speed)
        {
            flags.lstHeroine[0].chaCtrl.animBody.speed = speed;
            flags.player.chaCtrl.animBody.speed = speed;
        }

        private void SkipToTime(float normalizedTime)
        {
            int animStateHash = flags.player.chaCtrl.animBody.GetCurrentAnimatorStateInfo(0)
                .fullPathHash;
            flags.lstHeroine[0].chaCtrl.animBody.Play(animStateHash, 0, normalizedTime);
            flags.player.chaCtrl.animBody.Play(animStateHash, 0, normalizedTime);
        }
    }

    internal class KoikatsuCalorDepthController : KoikatsuDepthController<CalorDepthPOC>
    { }

    internal class KoikatsuHotdogDepthController : KoikatsuDepthController<HotdogDepthPOC>
    { }
}