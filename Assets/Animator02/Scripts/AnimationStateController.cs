// ***********************************************************************************
// FileName: AnimationStateController.cs
// Description:
// 
// Version: v1.0.0
// Creator: Jacky(jackylvm@foxmail.com)
// CreationTime: 2026-07-13 15:33:19
// ==============================================
// History update record:
// 
// ==============================================
// *************************************************************************************

using System;
using UnityEngine;

// ReSharper disable CheckNamespace
namespace Animator02
{
    public class AnimationStateController : MonoBehaviour
    {
        private static readonly int VelocityHash = Animator.StringToHash("Velocity");

        [SerializeField]
        private Animator animator;

        private const float Acceleration = 0.4f;
        private const float Deceleration = 0.6f;

        private const float MaxWalkVelocity = 0.1f;
        private const float MaxRunVelocity = 0.7f;

        private float _velocity = 0;

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
            var runningPressed = Input.GetKey(KeyCode.LeftShift);

            var currentMaxVelocity = runningPressed ? MaxRunVelocity : MaxWalkVelocity;

            if (forwardPressed)
            {
                if (_velocity < currentMaxVelocity)
                {
                    _velocity += Time.deltaTime * Acceleration;
                    _velocity = Mathf.Min(_velocity, currentMaxVelocity);
                }
                else
                {
                    _velocity -= Time.deltaTime * Deceleration;
                    _velocity = Mathf.Max(_velocity, currentMaxVelocity);
                }
            }
            else
            {
                if (_velocity > 0.0f)
                {
                    _velocity -= Time.deltaTime * Deceleration;
                    _velocity = Mathf.Max(_velocity, 0.0f);
                }
            }

            animator.SetFloat(VelocityHash, _velocity);
        }
    }
}