using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomSpawnManager : MonoBehaviour
{
    // Other gameobjects
    public Player player;
    RoomTracker playerRoomTracker;

    public AIManager pastAIManager;
    public AIManager futureAIManager;

    // Attached components
    public ObjectiveKillTracker mixedKillTracker;
    public ObjectiveKillTracker pastFlexibleKillTracker;
    public ObjectiveKillTracker futureFlexibleKillTracker;

    public ManualPassiveSpawner pastPassiveSpawner;
    public ManualPassiveSpawner futurePassiveSpawner;

    public RoomSpawnerPair[] enemySpawnerRooms;

    UnityAction[] roomListeners;

    // timers for aggresive enemies
    UnityEvent updateEvent;
    List<AggroSpawnTimer> aggroSpawnTimers;

    // Used in checkpointing
    RoomBounds checkpointRoom;

    bool inDressingRoom = false;

    [System.Serializable]
    public class RoomSpawnerPair
    {
        [HideInInspector]public string name;
        public RoomBounds room;
        public EnemySpawner[] enemySpawners;

        public UnityEvent onEnter;
        public UnityEvent onComplete;

        [HideInInspector] public bool completed = false;
        [HideInInspector] public bool shouldResetOnRespawn = true;

        public void SetSpawner(bool value)
        {
            foreach (var enemySpawner in enemySpawners)
            {
                enemySpawner.enabled = value;
            }
        }

        public void ResetRoom(UnityAction listener)
        {
            room.enterRoom.RemoveAllListeners();
            room.enterRoom.AddListener(listener);
            room.SetEnterStatus(false);
            room.SetExitStatus(false);
            completed = !shouldResetOnRespawn;
        }
    }

    public class AggroSpawnTimer
    {
        float timer = 0.0f;
        float maxTime = 5.0f;

        AIManager aiManager;
        ManualPassiveSpawner manualSpawner;
        int manualIndex;

        AggroSpawnTimer() { }

        public AggroSpawnTimer(AIManager aiManager, ManualPassiveSpawner manualSpawner, int manualIndex)
        {
            this.aiManager = aiManager;
            this.manualSpawner = manualSpawner;
            this.manualIndex = manualIndex;
        }

        public void ExecuteTimer()
        {
            if (aiManager.playerInTimePeriod)
            {
                timer += Time.deltaTime;
                if (timer > maxTime)
                {
                    timer -= maxTime;
                    manualSpawner.AggroSpawn(manualIndex);
                }
            }
        }

        public void AddToEvent(UnityEvent updateEvent)
        {
            updateEvent.AddListener(ExecuteTimer);
        }

        public void RemoveFromEvent(UnityEvent updateEvent)
        {
            updateEvent.RemoveListener(ExecuteTimer);
        }
    }

    enum RoomIndex
    {
        gallery,
        generator,
        bar,
        dressing,
        audotoriumLower,
        greenRoom
    }

    // Start is called before the first frame update
    void Start()
    {
        pastAIManager.agentdeathEvent.AddListener(PastDeath);
        futureAIManager.agentdeathEvent.AddListener(FutureDeath);

        playerRoomTracker = player.GetComponent<RoomTracker>();

        CreateRoomListeners();
        AddRoomListeners();

        updateEvent = new UnityEvent();
        aggroSpawnTimers = new List<AggroSpawnTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        updateEvent.Invoke();
    }

    private void OnValidate()
    {
        foreach (RoomSpawnerPair roomPair in enemySpawnerRooms)
        {
            if(roomPair.room != null)
            {
                roomPair.name = roomPair.room.name;
            }
        }
    }

    void CreateRoomListeners()
    {
        roomListeners = new UnityAction[enemySpawnerRooms.Length];
        roomListeners[0] = EnterGallery;
        roomListeners[1] = () => { };
        roomListeners[2] = () => { };
        roomListeners[3] = EnterDressingRoom;
        roomListeners[4] = EnterStageArea;
        roomListeners[5] = EnterGreenRoom;
    }

    void AddRoomListeners()
    {
        for (int i = 0; i < enemySpawnerRooms.Length; i++)
        {
            enemySpawnerRooms[i].room.enterRoom.AddListener(roomListeners[i]);
        }
    }

    void PastDeath()
    {
        pastFlexibleKillTracker.LogKill();
        mixedKillTracker.LogKill();
    }

    void FutureDeath()
    {
        futureFlexibleKillTracker.LogKill();
        mixedKillTracker.LogKill();
    }

    void SetSpawner(int index, bool value)
    {
        enemySpawnerRooms[index].SetSpawner(value);
    }

    void EnableKillTracker(ObjectiveKillTracker killTracker, int targetKillCount, UnityAction targetGoalAction)
    {
        killTracker.isTrackingEnabled = true;
        killTracker.killTarget = targetKillCount;
        killTracker.onTargetReached.AddListener(targetGoalAction);
    }

    void DisableKillTracker(ObjectiveKillTracker killTracker)
    {
        killTracker.isTrackingEnabled = false;
        killTracker.onTargetReached.RemoveAllListeners();
    }

    public void PlayerCheckpoint()
    {
        checkpointRoom = playerRoomTracker.currentRoom;
        for (int i = 0; i < enemySpawnerRooms.Length; i++)
        {
            enemySpawnerRooms[i].shouldResetOnRespawn = !enemySpawnerRooms[i].completed;
        }
    }

    public void PlayerRespawn()
    {
        for (int i = 0; i < enemySpawnerRooms.Length; i++)
        {
            SetSpawner(i, false);
            if(enemySpawnerRooms[i].shouldResetOnRespawn)
            {
                enemySpawnerRooms[i].ResetRoom(roomListeners[i]);
            }
        }
        playerRoomTracker.currentRoom = checkpointRoom;
        DisableKillTracker(mixedKillTracker);
        DisableKillTracker(pastFlexibleKillTracker);
        DisableKillTracker(futureFlexibleKillTracker);

        ClearTimers();
    }

    void CompleteRoom(int roomIndex)
    {
        mixedKillTracker.onTargetReached.RemoveAllListeners();
        pastFlexibleKillTracker.onTargetReached.RemoveAllListeners();
        futureFlexibleKillTracker.onTargetReached.RemoveAllListeners();
        enemySpawnerRooms[roomIndex].completed = true;
        enemySpawnerRooms[roomIndex].onComplete.Invoke();
    }

    void ClearTimers()
    {
        foreach (var timer in aggroSpawnTimers)
        {
            timer.RemoveFromEvent(updateEvent);
        }
        aggroSpawnTimers.Clear();
    }

    // Walk though Gallery - level pathing
    public void EnterGallery()
    {
        SetSpawner((int)RoomIndex.gallery, true);
        EnableKillTracker(pastFlexibleKillTracker, 5, CompleteGallery);
        enemySpawnerRooms[(int)RoomIndex.gallery].onEnter.Invoke();
    }

    public void CompleteGallery()
    {
        SetSpawner((int)RoomIndex.gallery, false);
        pastPassiveSpawner.PassiveSpawn(1);
        CompleteRoom((int)RoomIndex.gallery);
    }

    // Start Generator - objective
    public void StartGenerator()
    {
        SetSpawner((int)RoomIndex.generator, true);
        EnableKillTracker(pastFlexibleKillTracker, 8, MiddleGenerator);
        EnableKillTracker(futureFlexibleKillTracker, 2, CompleteGenerator);
        enemySpawnerRooms[(int)RoomIndex.generator].onEnter.Invoke();
    }

    public void MiddleGenerator()
    {
        futurePassiveSpawner.PassiveSpawn(0);
        futurePassiveSpawner.PassiveSpawn(1);
        pastFlexibleKillTracker.onTargetReached.RemoveAllListeners();
        EnableKillTracker(pastFlexibleKillTracker, 4, CompleteGenerator);
    }

    public void CompleteGenerator()
    {
        SetSpawner((int)RoomIndex.generator, false);
        pastPassiveSpawner.PassiveSpawn(1);
        futurePassiveSpawner.PassiveSpawn(2);
        CompleteRoom((int)RoomIndex.generator);
    }

    // Turn On Projector - objective
    public void SpawnProjectorPassiveBelchers()
    {
        futurePassiveSpawner.PassiveSpawn(2);
        futurePassiveSpawner.PassiveSpawn(3);
        futurePassiveSpawner.PassiveSpawn(4);
        futurePassiveSpawner.PassiveSpawn(5);
        futurePassiveSpawner.PassiveSpawn(6);
    }

    // Start Bar - objective
    public void StartBarRoom()
    {
        SetSpawner((int)RoomIndex.bar, true);
        EnableKillTracker(mixedKillTracker, 8, CompleteBar);
        enemySpawnerRooms[(int)RoomIndex.bar].onEnter.Invoke();
        ClearAllDudes();
    }

    public void CompleteBar()
    {
        SetSpawner((int)RoomIndex.bar, false);
        CompleteRoom((int)RoomIndex.bar);

        //StartPastAggroTimer(2);
        StartFutureAggroTimer(7);
    }


    // Walk Through dressing Room - level pathing
    public void EnterDressingRoom()
    {
        ClearTimers();
        StartPastAggroTimer(2);
        inDressingRoom = true;
        enemySpawnerRooms[(int)RoomIndex.dressing].onEnter.Invoke();
        SetSpawner((int)RoomIndex.dressing, true);
    }

    public void CompleteDressingRoom()
    {
        ClearTimers();
        CompleteRoom((int)RoomIndex.dressing);
    }

    public void EnterGreenRoom()
    {
        CompleteDressingRoom();
        StartFutureAggroTimer(8);
        enemySpawnerRooms[(int)RoomIndex.greenRoom].onEnter.Invoke();
    }

    public void CompleteGreenRoom()
    {
        ClearTimers();
        CompleteRoom((int)RoomIndex.greenRoom);
    }

    public void EnterStageArea()
    {
        if(!inDressingRoom)
        {
            enemySpawnerRooms[(int)RoomIndex.audotoriumLower].room.SetEnterStatus(false);
            return;
        }

        CompleteGreenRoom();
        enemySpawnerRooms[(int)RoomIndex.audotoriumLower].onEnter.Invoke();
    }

    public void StartFinalFightWaves()
    {
        SetSpawner((int)RoomIndex.bar, false); // Resuse Bar spawners. They have good numbers
        SetSpawner((int)RoomIndex.audotoriumLower, true);
    }

    void StartPastAggroTimer(int passiveIndex)
    {
        var pastTimer = new AggroSpawnTimer(pastAIManager, pastPassiveSpawner, passiveIndex);
        aggroSpawnTimers.Add(pastTimer);
        pastTimer.AddToEvent(updateEvent);
    }

    void StartFutureAggroTimer(int passiveIndex)
    {
        var futureTimer = new AggroSpawnTimer(futureAIManager, futurePassiveSpawner, passiveIndex);
        aggroSpawnTimers.Add(futureTimer);
        futureTimer.AddToEvent(updateEvent);
    }

    public void ClearAllDudes()
    {
        foreach(var agent in pastAIManager.allAgents)
        {
            if(agent.isAlive)
            {
                agent.DisablePoolObject();
            }
        }

        foreach (var agent in futureAIManager.allAgents)
        {
            if (agent.isAlive)
            {
                agent.DisablePoolObject();
            }
        }
    }
}
