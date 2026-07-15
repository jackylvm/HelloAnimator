// ***********************************************************************************
// FileName: AnimationStateController.cs
// Description:
// 
// Version: v1.0.0
// Creator: Jacky(jackylvm@foxmail.com)
// CreationTime: 2026-07-14 21:58:23
// ==============================================
// History update record:
// 
// ==============================================
// *************************************************************************************

using System;
using UnityEngine;

// ReSharper disable CheckNamespace
namespace Animator04
{
    public class AnimationStateController : MonoBehaviour
    {
        private static readonly int IsJump = Animator.StringToHash("IsJump04");

        [SerializeField]
        private Animator animator;

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
            var isJumpPressed = Input.GetKeyDown(KeyCode.Space);
            if (isJumpPressed)
            {
                animator.SetTrigger(IsJump);
            }
        }
    }
}