using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerWeaponController))]
public class PlayerMovement : MonoBehaviour
{
    #region Public/Serialized Variables

    [SerializeField] private Transform mainCamera;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float dashDistance = 2f;
    [SerializeField] private ParticleSystem dashEffect;
    [HideInInspector] public bool isDead;

    #endregion

    #region Private Variables

    private CharacterController _controller;
    private Animator _animator;
    private PlayerWeaponController _weaponController;
    private CountdownTimer _passiveTimer;
    private CountdownTimer _comboTimer;
    private Health _health;
    private SoundsHandler _soundsHandler;
    private bool _isRunning;
    private bool _weaponEquipped;
    private bool _canClick = true;
    private int _numOfLeftClicks;
    private int _numOfRightClicks;
    private float _turnSmoothVelocity;
    private float _lerpTime;

    private static readonly int Turn = Animator.StringToHash("Turn");
    private static readonly int Forward = Animator.StringToHash("Forward");
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int WeaponEquipped = Animator.StringToHash("WeaponEquipped");
    private static readonly int PassiveMode = Animator.StringToHash("PassiveMode");
    private static readonly int ComboAnim = Animator.StringToHash("ComboAnim");

    #endregion

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _weaponController = GetComponent<PlayerWeaponController>();
        _soundsHandler = GetComponent<SoundsHandler>();
        _passiveTimer = new CountdownTimer(5);
        _comboTimer = new CountdownTimer(1f);
    }

    private void Update()
    {
        if (_health.isDead || PauseMenu.gameIsPaused) return;
        CheckKeysPressed();
        TickTimers();
    }

    private void FixedUpdate()
    {
        if (_health.isDead) return;
        Movement();
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Vertical");
        float v = Input.GetAxisRaw("Horizontal");

        if (_isRunning)
        {
            h *= 2;
            v *= 2;
        }

        _animator.SetFloat(Forward, h, 0.02f, Time.deltaTime * 0.25f);
        _animator.SetFloat(Turn, v, 0.02f, Time.deltaTime * 0.25f);

        Vector3 direction = new Vector3(v, 0f, h).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void CheckKeysPressed()
    {
        _isRunning = Input.GetAxisRaw("Run") < 0;
        _animator.SetBool(Running, _isRunning);

        if (Input.GetButtonDown("DodgeRoll"))
        {
            _animator.SetTriggerOneFrame(this, "DodgeRoll", 0.2f);
            _passiveTimer.Start();
            return;
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (!Physics.Raycast(transform.position + Vector3.up / 2, transform.forward, dashDistance + 1))
            {
                _animator.SetTriggerOneFrame(this, "Dash", 0.2f);
                _passiveTimer.Start();
                return;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetBool(PassiveMode, false);
            if (_canClick) _numOfLeftClicks++;
            if (_numOfLeftClicks == 1) _animator.SetInteger(ComboAnim, 1);
            _passiveTimer.Start();
            _comboTimer.Start();
            return;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            _animator.SetBool(PassiveMode, false);
            if (_canClick) _numOfRightClicks++;
            if (_numOfRightClicks == 1) _animator.SetInteger(ComboAnim, 5);
            _passiveTimer.Start();
            _comboTimer.Start();
        }
    }

    private void TickTimers()
    {
        _passiveTimer.Tick();
        _comboTimer.Tick();

        _animator.SetBool(PassiveMode, !_passiveTimer.isRunning);
        if (!_comboTimer.isRunning) _numOfLeftClicks = _numOfRightClicks = 0;
    }

    public void ComboCheck()
    {
        _canClick = false;
        AnimatorStateInfo currAnimation = _animator.GetCurrentAnimatorStateInfo(0);

        int numCombo = ComboAttacks.Combo(currAnimation, _numOfLeftClicks, _numOfRightClicks);
        if (numCombo <= 0) _numOfLeftClicks = _numOfRightClicks = 0;

        _animator.SetInteger(ComboAnim, numCombo);
        _canClick = true;
        _passiveTimer.Start();
    }

    public void ComboFreezeAvoidance()
    {
        _animator.SetInteger(ComboAnim, 0);
        _numOfLeftClicks = _numOfRightClicks = 0;
    }

    public void WithdrawSword()
    {
        if (_weaponEquipped) _weaponController.UnequipWeapon();
        else _weaponController.EquipWeapon(_weaponController.weapons[0]);

        _weaponEquipped = !_weaponEquipped;
        _animator.SetBool(WeaponEquipped, _weaponEquipped);
    }

    public void PerformWeaponAttackEvent(AnimationEvent animationEvent)
    {
        _weaponController.PerformWeaponAttack(animationEvent.floatParameter);
    }

    public void DodgeRoll(AnimationEvent animationEvent)
    {
        float _initialHeight = _controller.height;
        Vector3 position = transform.position;
        if (animationEvent.stringParameter.Equals("Down"))
        {
            _controller.height = _initialHeight / 2;
            _controller.center = new Vector3(0, 0.5f, 0);
            position.y += 50;
        }
        else
        {
            _controller.height = _initialHeight * 2;
            _controller.center = new Vector3(0, 1f, 0);
            position.y -= (_controller.height - _initialHeight) * 0.5F;
        }
    }

    private void Dash(AnimationEvent animationEvent)
    {
        if (animationEvent.stringParameter.Equals("Start"))
        {
            _soundsHandler.DashAudio();
            dashEffect.Emit(40);
            _animator.applyRootMotion = false;
            transform.position += transform.forward * dashDistance;
        }
        else
        {
            _animator.applyRootMotion = true;
        }
    }
}