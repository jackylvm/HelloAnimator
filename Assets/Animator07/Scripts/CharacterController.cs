// ***********************************************************************************
// FileName: CharacterController.cs
// Description:
// 
// Version: v1.0.0
// Creator: Jacky(jackylvm@foxmail.com)
// CreationTime: 2026-07-19 10:55:32
// ==============================================
// History update record:
// 
// ==============================================
// *************************************************************************************

using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace Animator07
{
    public class CharacterController : MonoBehaviour
    {
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        [SerializeField]
        private Animator animator;

        private PlayerInput _input;

        private Vector2 _currentMovement;
        private bool _movementPressed;
        private bool _runPressed;

        /// <summary>
        /// Awake 函数会在游戏对象被实例化之后立即调用，而且在所有对象的 Start 函数调用之前执行。一般在 Awake 函数里进行以下操作：
        /// <para>1. 初始化引用：查找并存储对其他组件或者游戏对象的引用，确保后续代码能够方便地访问这些对象。</para>
        /// <para>2. 初始化设置：对脚本中的变量进行初始赋值。</para>
        /// <para>3. 建立连接：比如和数据库或者网络服务建立连接。</para>
        /// </summary>
        private void Awake()
        {
            _input = new PlayerInput();
        }

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

            _input.CharacterControls.Movement.performed += OnMovementPerformed;
            _input.CharacterControls.Run.performed += OnRunPerformed;
        }

        /// <summary>
        /// OnEnable 函数会在对象被启用时调用。函数在对象的生命周期中可能会被多次调用，只要对象被启用一次，它就会执行一次；而 Awake 只在对象实例化后调用一次，Start 只在第一帧更新前调用一次。
        /// <para>1. 注册事件：当你需要在对象启用时注册一些事件监听器，就可以在 OnEnable 函数中完成。这样在对象被启用后，就能及时响应相应的事件。</para>
        /// <para>2. 资源重置：如果对象在禁用时某些状态或者资源被改变，在启用时需要将其重置为初始状态，就可以在 OnEnable 里实现。</para>
        /// <para>3. 初始化临时数据：对于一些只在对象启用期间有效的临时数据，可以在 OnEnable 中进行初始化。</para>
        /// </summary>
        private void OnEnable()
        {
            _input.CharacterControls.Enable();
        }

        private void Update()
        {
            HandleMovement();
            HandleRatation();
        }

        /// <summary>
        /// 当游戏对象的 SetActive(false) 方法被调用，或者脚本组件的 enabled 属性被设置为 false 时，OnDisable 函数就会被触发执行。另外，当游戏对象被销毁时，在销毁之前也会先调用 OnDisable 函数。和 OnEnable 一样，OnDisable 在对象的生命周期中也可能会被多次调用，只要对象被禁用一次，该函数就会执行一次。
        /// <para>1. 注销事件：如果在 OnEnable 里注册了事件监听器，为避免出现内存泄漏问题，就需要在 OnDisable 中注销这些事件监听器。</para>
        /// <para>2. 保存数据：在对象被禁用时，可以将对象的状态或数据保存到文件、数据库或者其他持久化存储中。</para>
        /// <para>3. 释放资源：若对象在启用期间占用了一些资源，像网络连接、文件句柄等，在禁用时需要释放这些资源。</para>
        /// </summary>
        private void OnDisable()
        {
            _input.CharacterControls.Disable();
        }

        /// <summary>
        /// OnDestroy 函数会在以下几种情况下被调用：
        /// <para>1. 当你使用 GameObject.Destroy 方法明确销毁一个游戏对象时，OnDestroy 会在销毁操作执行前被调用。</para>
        /// <para>2. 当场景切换时，如果某个游戏对象没有设置为 DontDestroyOnLoad，那么在场景切换过程中该对象被销毁时，OnDestroy 也会被触发。</para>
        /// <para>3. 当游戏结束或应用程序关闭时，所有存在的游戏对象都会被销毁，此时也会调用它们各自的 OnDestroy 函数。</para>
        /// OnDestroy 函数适用的场景：
        /// <para>1. 资源释放：当游戏对象持有一些外部资源，如网络连接、文件句柄、数据库连接等，在对象销毁时需要确保这些资源被正确释放，避免资源泄漏。</para>
        /// <para>2. 事件注销：如果在 OnEnable 或者其他地方注册了事件监听器，为了防止出现内存泄漏，需要在 OnDestroy 中注销这些事件监听器。</para>
        /// <para>3. 数据保存：在对象销毁前，可能需要将对象的一些状态数据保存到文件或者服务器，以便后续恢复或分析。</para>
        /// </summary>
        private void OnDestroy()
        {
            _input.CharacterControls.Movement.performed -= OnMovementPerformed;
            _input.CharacterControls.Run.performed -= OnRunPerformed;
        }

        private void OnRunPerformed(InputAction.CallbackContext obj)
        {
            _runPressed = obj.ReadValueAsButton();
        }

        private void OnMovementPerformed(InputAction.CallbackContext obj)
        {
            _currentMovement = obj.ReadValue<Vector2>();
            _movementPressed = _currentMovement.x != 0 || _currentMovement.y != 0;
        }

        private void HandleMovement()
        {
            var isWalking = animator.GetBool(IsWalking);
            var isRunning = animator.GetBool(IsRunning);

            switch (_movementPressed)
            {
                case true when !isWalking:
                    animator.SetBool(IsWalking, true);
                    break;
                case false when isWalking:
                    animator.SetBool(IsWalking, false);
                    break;
            }

            if ((_movementPressed && _runPressed) && !isRunning)
            {
                animator.SetBool(IsRunning, true);
            }

            if ((!_movementPressed || !_runPressed) && isRunning)
            {
                animator.SetBool(IsRunning, false);
            }
        }

        private void HandleRatation()
        {
            var currentPosition = transform.position;
            var newPosition = new Vector3(_currentMovement.x, 0, _currentMovement.y);
            var positionToLookAt = currentPosition + newPosition;
            transform.LookAt(positionToLookAt);
        }
    }
}