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
    int currentSpawnerRoomIndex = 0;
    int checkpointSpawnerIndex = 0;
    RoomBounds checkpointRoom;

    [System.Serializable]
    public class RoomSpawnerPair
    {
        [HideInInspector]public string name;
        public RoomBounds room;
        public EnemySpawner[] enemySpawners;
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
        roomListeners[1] = EnterGenerator;
        roomListeners[2] = EnterBarRoom;
        roomListeners[3] = EnterDressingRoom;
        roomListeners[4] = EnterStageArea;
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

    void SetSpawner(RoomSpawnerPair[] pairArray, int index, bool value)
    {
        foreach(var enemySpawner in pairArray[index].enemySpawners)
        {
            enemySpawner.enabled = value;
        }
    }

    void ResetRoom(RoomSpawnerPair[] pairArray, int index)
    {
        pairArray[index].room.enterRoom.RemoveAllListeners();
        pairArray[index].room.enterRoom.AddListener(roomListeners[index]);
        pairArray[index].room.SetEnterStatus(false);
        pairArray[index].room.SetExitStatus(false);
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
        checkpointSpawnerIndex = currentSpawnerRoomIndex;
        checkpointRoom = playerRoomTracker.currentRoom;
    }

    public void PlayerRespawn()
    {
        currentSpawnerRoomIndex = checkpointSpawnerIndex;
        for (int i = currentSpawnerRoomIndex; i < enemySpawnerRooms.Length; i++)
        {
            SetSpawner(enemySpawnerRooms, i, false);
            ResetRoom(enemySpawnerRooms, i);
        }
        playerRoomTracker.currentRoom = checkpointRoom;
        DisableKillTracker(mixedKillTracker);
        DisableKillTracker(pastFlexibleKillTracker);
        DisableKillTracker(futureFlexibleKillTracker);

        ClearTimers();
    }

    void CompleteRoom()
    {
        currentSpawnerRoomIndex++;
        mixedKillTracker.onTargetReached.RemoveAllListeners();
        pastFlexibleKillTracker.onTargetReached.RemoveAllListeners();
        futureFlexibleKillTracker.onTargetReached.RemoveAllListeners();
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
        SetSpawner(enemySpawnerRooms, currentSpawnerRoomIndex, true);
        EnableKillTracker(pastFlexibleKillTracker, 5, CompleteGallery);
    }

    public void CompleteGallery()
    {
        SetSpawner(enemySpawnerRooms, currentSpawnerRoomIndex, false);
        pastPassiveSpawner.PassiveSpawn(1);
        CompleteRoom();
    }

    // Walk Thourgh Generator - level pathing
    public void EnterGenerator()
    {
        SetSpawner(enemySpawnerRooms, currentSpawnerRoomIndex, true);
        EnableKillTracker(pastFlexibleKillTracker, 8, MiddleGenerator);
        EnableKillTracker(futureFlexibleKillTracker, 2, CompleteGenerator);
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
        SetSpawner(enemySpawnerRooms, currentSpawnerRoomIndex, false);
        pastPassiveSpawner.PassiveSpawn(1);
        futurePassiveSpawner.PassiveSpawn(2);
        CompleteRoom();
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

    // Walk Though Bar - level pathing
    public void EnterBarRoom()
    {
        SetSpawner(enemySpawnerRooms, currentSpawnerRoomIndex, true);
        EnableKillTracker(mixedKillTracker, 12, CompleteBar);
    }

    public void CompleteBar()
    {
        SetSpawner(enemySpawnerRooms, currentSpawnerRoomIndex, false);
        CompleteRoom();
    }


    // Walk Through dressing Room - level pathing
    public void EnterDressingRoom()
    {
        var pastTimer = new AggroSpawnTimer(pastAIManager, pastPassiveSpawner, 2);
        aggroSpawnTimers.Add(pastTimer);
        var futureTimer = new AggroSpawnTimer(futureAIManager, futurePassiveSpawner, 7);
        aggroSpawnTimers.Add(futureTimer);

        foreach(var timer in aggroSpawnTimers)
        {
            timer.AddToEvent(updateEvent);
        }
    }

    public void CompleteDressingRoom()
    {
        ClearTimers();
    }

    public void EnterStageArea()
    {
        if(currentSpawnerRoomIndex != 3)
        {
            enemySpawnerRooms[currentSpawnerRoomIndex].room.SetEnterStatus(false);
            return;
        }

        CompleteDressingRoom();
    }
}
