using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(SoundsHandler))]
public class EnemyAI : MonoBehaviour
{
    #region Variables

    public string currentAction;

    [Header("Unity Settings")] [Tooltip("Only works in standard game objects.")] [SerializeField]
    private bool debug;

    [SerializeField] public Animator rigAnimator;
    [SerializeField] private Transform eyes;

    [Header("Health Settings")] [SerializeField]
    private float lowHealthThreshold;

    [Header("Combat Settings")] [SerializeField]
    private Transform playerTransform;

    [SerializeField] public GameObject bulletObject;
    [SerializeField] public Transform shootPosition;
    [SerializeField] public ParticleSystem[] muzzleFlash;
    [SerializeField] public ParticleSystem[] hitEffects;
    [SerializeField] public TrailRenderer bulletTracer;
    [SerializeField] public Rig[] rigLayers;
    [SerializeField] public float bulletDamage = 5f;
    [SerializeField] public int startingAmmo = 20;
    [SerializeField] public float fireRate = 1f;
    [SerializeField] public float bulletSpeed = 25f;
    [SerializeField] public float stoppingDistance = 20f;
    [SerializeField] public float retreatDistance = 10f;
    [SerializeField] private Location[] availableCovers;
    [HideInInspector] public int currentAmmo;
    [HideInInspector] public Vector3 lastPlayerPos;

    [Header("Passive Settings")] [SerializeField]
    public float runSpeed = 3.5f;

    [SerializeField] public float walkSpeed = 1f;
    [SerializeField] private Location[] patrolLocations;
    [HideInInspector] public Health health;
    [HideInInspector] public SoundsHandler soundsHandler;

    public bool randomPatrol;
    private Material _material;
    private NavMeshAgent _agent;
    private Node _topNode;
    private static readonly int VelZ = Animator.StringToHash("VelZ");

    public bool debugMode { get; private set; }
    public Animator animator { get; private set; }
    public Transform bestCoverSpot { get; set; }

    #endregion

    private void Awake()
    {
        debugMode = debug;
        _agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        soundsHandler = GetComponent<SoundsHandler>();
        if (debugMode) _material = GetComponent<MeshRenderer>().material;
        else animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        currentAmmo = startingAmmo;
        lastPlayerPos = Vector3.down * 100;
        ConstructBehaviorTree();
    }

    private void Update()
    {
        if (health.isDead)
        {
            _agent.enabled = false;
            rigAnimator.Play("Disabled");
            return;
        }

        _topNode.Evaluate();

        if (_topNode.nodeState == NodeState.FAILURE)
        {
            if (debugMode) SetColor(Color.red);
            _agent.isStopped = true;
        }

        animator.SetFloat(VelZ, (float) Math.Round(_agent.velocity.magnitude, 3));
    }

    private void ConstructBehaviorTree()
    {
        FieldOfView fov = GetComponent<FieldOfView>();
        
        // Cover
        //Node goToCoverAndCheckStatus = new Selector(new List<Node> {goToCoverNode, reloadAndRegen});

        Node reloadAndRegen = new ReloadAndRegen(this, _agent);
        Node isCoveredNode = new IsCoveredNode(this, playerTransform, eyes);
        
        Node goToCoverNode = new GoToCoverNode(_agent, this);
        Node isCoverAvailableNode = new IsCoverAvailableNode(availableCovers, playerTransform, this);
        Node goToCoverSequence = new Inverter(new Sequence(new List<Node> {isCoverAvailableNode, goToCoverNode}));
        
        Node tryToTakeCoverSequence = new Selector(new List<Node> {goToCoverSequence, reloadAndRegen});
        
        Node needsCoverNode = new NeedsCoverNode(this);
        Node coverSequence = new Sequence(new List<Node> {needsCoverNode, tryToTakeCoverSequence});

        // Shoot
        Node canSeePlayerNode = new CanSeePlayerNode(this, _agent, fov, playerTransform);
        Node shootNode = new ShootNode(_agent, this, playerTransform);
        Node shootSequence = new Sequence(new List<Node> {canSeePlayerNode, shootNode});

        // Chase
        Node hasSpotToCheckNode = new HasSpotToCheckNode(this, _agent);
        Node chaseNode = new LookAroundNode(_agent, this);
        Node chaseSequence = new Sequence(new List<Node> {hasSpotToCheckNode, chaseNode});

        // Patrol
        Node havesPathNode = new HavesPathNode(this, patrolLocations);
        Node patrolNode = new PatrolNode(patrolLocations, _agent, this);
        Node patrolSequence = new Sequence(new List<Node> {havesPathNode, patrolNode});

        _topNode = new Selector(new List<Node>
            {coverSequence, shootSequence, chaseSequence, patrolSequence});
    }

    public void SetColor(Color color)
    {
        _material.color = color;
    }
}