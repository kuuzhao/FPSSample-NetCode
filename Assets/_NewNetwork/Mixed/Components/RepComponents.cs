using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct RepBarrelTagComponentData : IComponentData
{

}

public struct RepBarrelGoCreatedTag : IComponentData
{

}

public struct RepGameMode : IComponentData
{
    public int gameTimerSeconds;
    public NativeString64 gameTimerMessage;
    public NativeString64 teamName0;
    public NativeString64 teamName1;
    public int teamScore0;
    public int teamScore1;
}

public struct RepPlayerTagComponentData : IComponentData
{

}

public struct RepPlayerGoCreatedTag : IComponentData
{

}

public struct RepPlayerComponentData : IComponentData
{
    public int networkId;
    /// <summary>
    /// From CharacterInterpolatedData
    /// </summary>
    public float3 position;
    public float rotation;
    public float aimYaw;
    public float aimPitch;
    public float moveYaw;                                       // Global rotation 0->360 deg

    public int charLocoState;                                   // CharacterPredictedData.LocoState
    public int charLocoTick;                                    // predictedState.locoStartTick
    public int charAction;                                      // CharacterPredictedData.Action
    public int charActionTick;
    public int damageTick;
    public float damageDirection;
    public int sprinting;
    public float sprintWeight;

    // Custom properties for Animation states
    public int previousCharLocoState;                           // CharacterPredictedData.LocoState
    public int lastGroundMoveTick;
    public float moveAngleLocal;                                // Movement rotation relative to character forward -180->180 deg clockwise
    public float shootPoseWeight;
    public float2 locomotionVector;
    public float locomotionPhase;
    public float banking;
    public float landAnticWeight;
    public float turnStartAngle;
    public int turnDirection;                                   // -1 TurnLeft, 0 Idle, 1 TurnRight
    public float squashTime;
    public float squashWeight;
    public float inAirTime;
    public float jumpTime;
    public float simpleTime;
    public float2 footIkOffset;
    public float3 footIkNormalLeft;
    public float3 footIkNormaRight;

    /// <summary>
    /// LZ add
    /// </summary>
    public float3 velocity;

    /// <summary>
    /// not supposed to be ghosted !!!
    /// </summary>
    public uint tick;
}

public struct RepGrenadeTagComponentData : IComponentData
{

}

public struct RepGrenadeGoCreatedTag : IComponentData
{

}
