using Coffee.UIExtensions;
using Coffee.UIParticleExtensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Coffee.UIExtensions
{
    [ExecuteAlways]
    public class ParticleAttractor : MonoBehaviour
    {
        public enum Movement
        {
            Linear,
            Smooth,
            Sphere
        }

        public enum UpdateMode
        {
            Normal,
            UnscaledTime
        }

        [SerializeField]
        private List<ParticleSystem> m_ParticleSystems = new List<ParticleSystem>();

        [Range(0.1f, 10f)]
        [SerializeField]
        private float m_DestinationRadius = 1;

        [Range(0f, 0.95f)]
        [SerializeField]
        private float m_DelayRate;

        [Range(0.001f, 100f)]
        [SerializeField]
        private float m_MaxSpeed = 1;

        [SerializeField]
        private Movement m_Movement;

        [SerializeField]
        private UpdateMode m_UpdateMode;

        [SerializeField]
        private UnityEvent m_OnAttracted;

        private List<UIParticle> _uiParticles = new List<UIParticle>();

        public float destinationRadius
        {
            get => m_DestinationRadius;
            set => m_DestinationRadius = Mathf.Clamp(value, 0.1f, 10f);
        }

        public float delay
        {
            get => m_DelayRate;
            set => m_DelayRate = value;
        }

        public float maxSpeed
        {
            get => m_MaxSpeed;
            set => m_MaxSpeed = value;
        }

        public Movement movement
        {
            get => m_Movement;
            set => m_Movement = value;
        }

        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        public UnityEvent onAttracted
        {
            get => m_OnAttracted;
            set => m_OnAttracted = value;
        }

        public List<ParticleSystem> particleSystems
        {
            get => m_ParticleSystems;
            set
            {
                m_ParticleSystems = value;
                ApplyParticleSystems();
            }
        }

        private void OnEnable()
        {
            ApplyParticleSystems();
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        private void OnDestroy()
        {
            _uiParticles.Clear();
            m_ParticleSystems.Clear();
        }

        internal void Attract()
        {
            foreach (var particleSystem in m_ParticleSystems)
            {
                if (particleSystem == null) continue;

                var count = particleSystem.particleCount;
                if (count == 0) continue;

                var particles = ParticleSystemExtensions.GetParticleArray(count);
                particleSystem.GetParticles(particles, count);

                var dstPos = GetDestinationPosition(particleSystem);
                for (var i = 0; i < count; i++)
                {
                    // Attracted
                    var p = particles[i];
                    if (0f < p.remainingLifetime && Vector3.Distance(p.position, dstPos) < m_DestinationRadius)
                    {
                        p.remainingLifetime = 0f;
                        p.velocity = Vector3.zero;  // 파티클의 속도를 0으로 설정
                        particles[i] = p;

                        if (m_OnAttracted != null)
                        {
                            try
                            {
                                m_OnAttracted.Invoke();
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }

                        continue;
                    }

                    // Calc attracting time
                    var delayTime = p.startLifetime * m_DelayRate;
                    var duration = p.startLifetime - delayTime;
                    var time = Mathf.Max(0, p.startLifetime - p.remainingLifetime - delayTime);

                    // Delay
                    if (time <= 0) continue;

                    // Attract
                    p.position = GetAttractedPosition(p.position, dstPos, duration, time);
                    p.velocity *= 0.5f;
                    particles[i] = p;
                }

                particleSystem.SetParticles(particles, count);
            }
        }

        private Vector3 GetDestinationPosition(ParticleSystem particleSystem)
        {
            var isUI = _uiParticles.Contains(particleSystem.GetComponentInParent<UIParticle>());
            var psPos = particleSystem.transform.position;
            var attractorPos = transform.position;
            var dstPos = attractorPos;
            var isLocalSpace = particleSystem.IsLocalSpace();

            if (isLocalSpace)
            {
                dstPos = particleSystem.transform.InverseTransformPoint(dstPos);
            }

            if (isUI)
            {
                var uiParticle = particleSystem.GetComponentInParent<UIParticle>();
                var inverseScale = uiParticle.parentScale.Inverse();
                var scale3d = uiParticle.scale3DForCalc;
                dstPos = dstPos.GetScaled(inverseScale, scale3d.Inverse());

                // Relative mode
                if (uiParticle.positionMode == UIParticle.PositionMode.Relative)
                {
                    var diff = uiParticle.transform.position - psPos;
                    diff.Scale(scale3d - inverseScale);
                    diff.Scale(scale3d.Inverse());
                    dstPos += diff;
                }

#if UNITY_EDITOR
                if (!Application.isPlaying && !isLocalSpace)
                {
                    dstPos += psPos - psPos.GetScaled(inverseScale, scale3d.Inverse());
                }
#endif
            }

            return dstPos;
        }

        private Vector3 GetAttractedPosition(Vector3 current, Vector3 target, float duration, float time)
        {
            var speed = m_MaxSpeed;
            switch (m_UpdateMode)
            {
                case UpdateMode.Normal:
                    speed *= 60 * Time.deltaTime;
                    break;
                case UpdateMode.UnscaledTime:
                    speed *= 60 * Time.unscaledDeltaTime;
                    break;
            }

            switch (m_Movement)
            {
                case Movement.Linear:
                    speed /= duration;
                    break;
                case Movement.Smooth:
                    target = Vector3.Lerp(current, target, time / duration);
                    break;
                case Movement.Sphere:
                    target = Vector3.Slerp(current, target, time / duration);
                    break;
            }

            return Vector3.MoveTowards(current, target, speed);
        }

        private void ApplyParticleSystems()
        {
            _uiParticles.Clear();
            foreach (var particleSystem in m_ParticleSystems)
            {
                if (particleSystem == null) continue;

                var uiParticle = particleSystem.GetComponentInParent<UIParticle>(true);
                if (uiParticle && uiParticle.particles.Contains(particleSystem))
                {
                    _uiParticles.Add(uiParticle);
                }
            }

            if (_uiParticles.Count == 0)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                {
                    Debug.LogError("No particle system attached to particle attractor script", this);
                }
            }
        }

        private void Register()
        {
            if (Application.isPlaying)
            {
                ParticleAttractorUpdater.Instance.Register(this);
            }
        }

        private void Unregister()
        {
            if (Application.isPlaying && ParticleAttractorUpdater.HasInstance)
            {
                ParticleAttractorUpdater.Instance.Unregister(this);
            }
        }
    }
}

public class ParticleAttractorUpdater : MonoBehaviour
{
    private static ParticleAttractorUpdater _instance;
    public static ParticleAttractorUpdater Instance
    {
        get
        {
            if (_instance == null && Application.isPlaying)
            {
                var go = new GameObject("ParticleAttractorUpdater");
                _instance = go.AddComponent<ParticleAttractorUpdater>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public static bool HasInstance => _instance != null;

    private readonly List<ParticleAttractor> _attractors = new List<ParticleAttractor>();

    public void Register(ParticleAttractor attractor)
    {
        if (!_attractors.Contains(attractor))
        {
            _attractors.Add(attractor);
        }
    }

    public void Unregister(ParticleAttractor attractor)
    {
        if (_attractors.Contains(attractor))
        {
            _attractors.Remove(attractor);
        }
    }

    private void Update()
    {
        foreach (var attractor in _attractors)
        {
            attractor.Attract();
        }
    }
}
