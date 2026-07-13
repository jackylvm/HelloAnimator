// ***********************************************************************************
// FileName: AnimationStateController.cs
// Description:
// 
// Version: v1.0.0
// Creator: Jacky(jackylvm@foxmail.com)
// CreationTime: 2026-07-13 21:57:29
// ==============================================
// History update record:
// 
// ==============================================
// *************************************************************************************

using UnityEngine;

// ReSharper disable CheckNamespace
namespace Animator03
{
    public class AnimationStateController : MonoBehaviour
    {
        private static readonly int ZVelocityHash = Animator.StringToHash("ZVelocity");
        private static readonly int XVelocityHash = Animator.StringToHash("XVelocity");

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float acceleration = 2.0f;

        [SerializeField]
        private float deceleration = 2.0f;

        private float _maxWalkVelocity = 0.5f;
        private float _maxRunVelocity = 2.0f;

        private float _zVelocity = 0;
        private float _xVelocity = 0;

        /// <summary>
        /// Start 函数会在脚本实例被启用时调用，并且只在第一帧更新之前执行一次。它一般用于：
        /// <para>1. 依赖初始化：当某些初始化操作依赖于其他对象的 Awake 函数完成时，可以在 Start 函数里进行。</para>
        /// <para>2. 数据加载：从文件或者服务器加载数据。</para>
        /// </summary>
        private void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        private void Update()
        {
            var forwardPressed = Input.GetKey(KeyCode.W);
            var leftPressed = Input.GetKey(KeyCode.A);
            var rightPressed = Input.GetKey(KeyCode.D);
            var runningPressed = Input.GetKey(KeyCode.LeftShift);
            var currentMaxVelocity = runningPressed ? _maxRunVelocity : _maxWalkVelocity;

            // 前进方向加速
            if (forwardPressed)
            {
                if (_zVelocity < currentMaxVelocity)
                {
                    _zVelocity += Time.deltaTime * acceleration;
                    _zVelocity = Mathf.Min(_zVelocity, currentMaxVelocity);
                }
                else
                {
                    _zVelocity -= Time.deltaTime * deceleration;
                    _zVelocity = Mathf.Max(_zVelocity, currentMaxVelocity);
                }
            }
            else
            {
                // 减速
                if (_zVelocity > 0.0f)
                {
                    _zVelocity -= Time.deltaTime * deceleration;
                    _zVelocity = Mathf.Max(_zVelocity, 0.0f);
                }
            }

            // 左方向加速
            if (leftPressed)
            {
                if (_xVelocity > -currentMaxVelocity)
                {
                    _xVelocity += -Time.deltaTime * acceleration;
                    _xVelocity = Mathf.Max(_xVelocity, -currentMaxVelocity);
                }
                else
                {
                    _xVelocity += Time.deltaTime * deceleration;
                    _xVelocity = Mathf.Min(_xVelocity, -currentMaxVelocity);
                }
            }
            else
            {
                // 减速
                if (_xVelocity < 0.0f)
                {
                    _xVelocity += Time.deltaTime * deceleration;
                    _xVelocity = Mathf.Min(_xVelocity, 0.0f);
                }
            }

            // 右方向加速
            if (rightPressed)
            {
                if (_xVelocity < currentMaxVelocity)
                {
                    _xVelocity += Time.deltaTime * acceleration;
                    _xVelocity = Mathf.Min(_xVelocity, currentMaxVelocity);
                }
                else
                {
                    _xVelocity += -Time.deltaTime * deceleration;
                    _xVelocity = Mathf.Max(_xVelocity, currentMaxVelocity);
                }
            }
            else
            {
                // 减速
                if (_xVelocity > 0.0f)
                {
                    _xVelocity += -Time.deltaTime * deceleration;
                    _xVelocity = Mathf.Max(_xVelocity, 0.0f);
                }
            }

            animator.SetFloat(ZVelocityHash, _zVelocity);
            animator.SetFloat(XVelocityHash, _xVelocity);
        }
    }
}