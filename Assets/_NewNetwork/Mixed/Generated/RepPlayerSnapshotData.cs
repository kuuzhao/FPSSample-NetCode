using Unity.Mathematics;
using Unity.Networking.Transport;

public struct RepPlayerSnapshotData : ISnapshotData<RepPlayerSnapshotData>
{
    public uint tick;
    int RepPlayerComponentDatanetworkId;
    int RepPlayerComponentDatapositionX;
    int RepPlayerComponentDatapositionY;
    int RepPlayerComponentDatapositionZ;
    int RepPlayerComponentDatarotation;
    int RepPlayerComponentDataaimYaw;
    int RepPlayerComponentDataaimPitch;
    int RepPlayerComponentDatamoveYaw;
    int RepPlayerComponentDatacharLocoState;
    int RepPlayerComponentDatacharLocoTick;
    int RepPlayerComponentDatacharAction;
    int RepPlayerComponentDatacharActionTick;
    int RepPlayerComponentDatadamageTick;
    int RepPlayerComponentDatadamageDirection;
    int RepPlayerComponentDatasprinting;
    int RepPlayerComponentDatasprintWeight;
    int RepPlayerComponentDatapreviousCharLocoState;
    int RepPlayerComponentDatalastGroundMoveTick;
    int RepPlayerComponentDatamoveAngleLocal;
    int RepPlayerComponentDatashootPoseWeight;
    int RepPlayerComponentDatalocomotionVectorX;
    int RepPlayerComponentDatalocomotionVectorY;
    int RepPlayerComponentDatalocomotionPhase;
    int RepPlayerComponentDatabanking;
    int RepPlayerComponentDatalandAnticWeight;
    int RepPlayerComponentDataturnStartAngle;
    int RepPlayerComponentDataturnDirection;
    int RepPlayerComponentDatasquashTime;
    int RepPlayerComponentDatasquashWeight;
    int RepPlayerComponentDatainAirTime;
    int RepPlayerComponentDatajumpTime;
    int RepPlayerComponentDatasimpleTime;
    int RepPlayerComponentDatafootIkOffsetX;
    int RepPlayerComponentDatafootIkOffsetY;
    int RepPlayerComponentDatafootIkNormalLeftX;
    int RepPlayerComponentDatafootIkNormalLeftY;
    int RepPlayerComponentDatafootIkNormalLeftZ;
    int RepPlayerComponentDatafootIkNormaRightX;
    int RepPlayerComponentDatafootIkNormaRightY;
    int RepPlayerComponentDatafootIkNormaRightZ;
    int RepPlayerComponentDatavelocityX;
    int RepPlayerComponentDatavelocityY;
    int RepPlayerComponentDatavelocityZ;


    public uint Tick => tick;
    public int GetRepPlayerComponentDatanetworkId()
    {
        return RepPlayerComponentDatanetworkId;
    }
    public void SetRepPlayerComponentDatanetworkId(int val)
    {
        RepPlayerComponentDatanetworkId = val;
    }
    public float3 GetRepPlayerComponentDataposition()
    {
        return new float3(RepPlayerComponentDatapositionX, RepPlayerComponentDatapositionY, RepPlayerComponentDatapositionZ) * 0.01f;
    }
    public void SetRepPlayerComponentDataposition(float3 val)
    {
        RepPlayerComponentDatapositionX = (int)(val.x * 100);
        RepPlayerComponentDatapositionY = (int)(val.y * 100);
        RepPlayerComponentDatapositionZ = (int)(val.z * 100);
    }
    public float GetRepPlayerComponentDatarotation()
    {
        return (float)RepPlayerComponentDatarotation * 0.01f;
    }
    public void SetRepPlayerComponentDatarotation(float val)
    {
        RepPlayerComponentDatarotation = (int)(val * 100);
    }
    public float GetRepPlayerComponentDataaimYaw()
    {
        return (float)RepPlayerComponentDataaimYaw * 0.01f;
    }
    public void SetRepPlayerComponentDataaimYaw(float val)
    {
        RepPlayerComponentDataaimYaw = (int)(val * 100);
    }
    public float GetRepPlayerComponentDataaimPitch()
    {
        return (float)RepPlayerComponentDataaimPitch * 0.01f;
    }
    public void SetRepPlayerComponentDataaimPitch(float val)
    {
        RepPlayerComponentDataaimPitch = (int)(val * 100);
    }
    public float GetRepPlayerComponentDatamoveYaw()
    {
        return (float)RepPlayerComponentDatamoveYaw * 0.01f;
    }
    public void SetRepPlayerComponentDatamoveYaw(float val)
    {
        RepPlayerComponentDatamoveYaw = (int)(val * 100);
    }
    public int GetRepPlayerComponentDatacharLocoState()
    {
        return RepPlayerComponentDatacharLocoState;
    }
    public void SetRepPlayerComponentDatacharLocoState(int val)
    {
        RepPlayerComponentDatacharLocoState = val;
    }
    public int GetRepPlayerComponentDatacharLocoTick()
    {
        return RepPlayerComponentDatacharLocoTick;
    }
    public void SetRepPlayerComponentDatacharLocoTick(int val)
    {
        RepPlayerComponentDatacharLocoTick = val;
    }
    public int GetRepPlayerComponentDatacharAction()
    {
        return RepPlayerComponentDatacharAction;
    }
    public void SetRepPlayerComponentDatacharAction(int val)
    {
        RepPlayerComponentDatacharAction = val;
    }
    public int GetRepPlayerComponentDatacharActionTick()
    {
        return RepPlayerComponentDatacharActionTick;
    }
    public void SetRepPlayerComponentDatacharActionTick(int val)
    {
        RepPlayerComponentDatacharActionTick = val;
    }
    public int GetRepPlayerComponentDatadamageTick()
    {
        return RepPlayerComponentDatadamageTick;
    }
    public void SetRepPlayerComponentDatadamageTick(int val)
    {
        RepPlayerComponentDatadamageTick = val;
    }
    public float GetRepPlayerComponentDatadamageDirection()
    {
        return (float)RepPlayerComponentDatadamageDirection * 0.01f;
    }
    public void SetRepPlayerComponentDatadamageDirection(float val)
    {
        RepPlayerComponentDatadamageDirection = (int)(val * 100);
    }
    public int GetRepPlayerComponentDatasprinting()
    {
        return RepPlayerComponentDatasprinting;
    }
    public void SetRepPlayerComponentDatasprinting(int val)
    {
        RepPlayerComponentDatasprinting = val;
    }
    public float GetRepPlayerComponentDatasprintWeight()
    {
        return (float)RepPlayerComponentDatasprintWeight * 0.01f;
    }
    public void SetRepPlayerComponentDatasprintWeight(float val)
    {
        RepPlayerComponentDatasprintWeight = (int)(val * 100);
    }
    public int GetRepPlayerComponentDatapreviousCharLocoState()
    {
        return RepPlayerComponentDatapreviousCharLocoState;
    }
    public void SetRepPlayerComponentDatapreviousCharLocoState(int val)
    {
        RepPlayerComponentDatapreviousCharLocoState = val;
    }
    public int GetRepPlayerComponentDatalastGroundMoveTick()
    {
        return RepPlayerComponentDatalastGroundMoveTick;
    }
    public void SetRepPlayerComponentDatalastGroundMoveTick(int val)
    {
        RepPlayerComponentDatalastGroundMoveTick = val;
    }
    public float GetRepPlayerComponentDatamoveAngleLocal()
    {
        return (float)RepPlayerComponentDatamoveAngleLocal * 0.01f;
    }
    public void SetRepPlayerComponentDatamoveAngleLocal(float val)
    {
        RepPlayerComponentDatamoveAngleLocal = (int)(val * 100);
    }
    public float GetRepPlayerComponentDatashootPoseWeight()
    {
        return (float)RepPlayerComponentDatashootPoseWeight * 0.01f;
    }
    public void SetRepPlayerComponentDatashootPoseWeight(float val)
    {
        RepPlayerComponentDatashootPoseWeight = (int)(val * 100);
    }
    public float2 GetRepPlayerComponentDatalocomotionVector()
    {
        return new float2(RepPlayerComponentDatalocomotionVectorX, RepPlayerComponentDatalocomotionVectorY) * 0.01f;
    }
    public void SetRepPlayerComponentDatalocomotionVector(float2 val)
    {
        RepPlayerComponentDatalocomotionVectorX = (int)(val.x * 100);
        RepPlayerComponentDatalocomotionVectorY = (int)(val.y * 100);
    }
    public float GetRepPlayerComponentDatalocomotionPhase()
    {
        return (float)RepPlayerComponentDatalocomotionPhase * 0.01f;
    }
    public void SetRepPlayerComponentDatalocomotionPhase(float val)
    {
        RepPlayerComponentDatalocomotionPhase = (int)(val * 100);
    }
    public float GetRepPlayerComponentDatabanking()
    {
        return (float)RepPlayerComponentDatabanking * 0.01f;
    }
    public void SetRepPlayerComponentDatabanking(float val)
    {
        RepPlayerComponentDatabanking = (int)(val * 100);
    }
    public float GetRepPlayerComponentDatalandAnticWeight()
    {
        return (float)RepPlayerComponentDatalandAnticWeight * 0.01f;
    }
    public void SetRepPlayerComponentDatalandAnticWeight(float val)
    {
        RepPlayerComponentDatalandAnticWeight = (int)(val * 100);
    }
    public float GetRepPlayerComponentDataturnStartAngle()
    {
        return (float)RepPlayerComponentDataturnStartAngle * 0.01f;
    }
    public void SetRepPlayerComponentDataturnStartAngle(float val)
    {
        RepPlayerComponentDataturnStartAngle = (int)(val * 100);
    }
    public int GetRepPlayerComponentDataturnDirection()
    {
        return RepPlayerComponentDataturnDirection;
    }
    public void SetRepPlayerComponentDataturnDirection(int val)
    {
        RepPlayerComponentDataturnDirection = val;
    }
    public float GetRepPlayerComponentDatasquashTime()
    {
        return (float)RepPlayerComponentDatasquashTime * 0.01f;
    }
    public void SetRepPlayerComponentDatasquashTime(float val)
    {
        RepPlayerComponentDatasquashTime = (int)(val * 100);
    }
    public float GetRepPlayerComponentDatasquashWeight()
    {
        return (float)RepPlayerComponentDatasquashWeight * 0.01f;
    }
    public void SetRepPlayerComponentDatasquashWeight(float val)
    {
        RepPlayerComponentDatasquashWeight = (int)(val * 100);
    }
    public float GetRepPlayerComponentDatainAirTime()
    {
        return (float)RepPlayerComponentDatainAirTime * 0.01f;
    }
    public void SetRepPlayerComponentDatainAirTime(float val)
    {
        RepPlayerComponentDatainAirTime = (int)(val * 100);
    }
    public float GetRepPlayerComponentDatajumpTime()
    {
        return (float)RepPlayerComponentDatajumpTime * 0.01f;
    }
    public void SetRepPlayerComponentDatajumpTime(float val)
    {
        RepPlayerComponentDatajumpTime = (int)(val * 100);
    }
    public float GetRepPlayerComponentDatasimpleTime()
    {
        return (float)RepPlayerComponentDatasimpleTime * 0.01f;
    }
    public void SetRepPlayerComponentDatasimpleTime(float val)
    {
        RepPlayerComponentDatasimpleTime = (int)(val * 100);
    }
    public float2 GetRepPlayerComponentDatafootIkOffset()
    {
        return new float2(RepPlayerComponentDatafootIkOffsetX, RepPlayerComponentDatafootIkOffsetY) * 0.01f;
    }
    public void SetRepPlayerComponentDatafootIkOffset(float2 val)
    {
        RepPlayerComponentDatafootIkOffsetX = (int)(val.x * 100);
        RepPlayerComponentDatafootIkOffsetY = (int)(val.y * 100);
    }
    public float3 GetRepPlayerComponentDatafootIkNormalLeft()
    {
        return new float3(RepPlayerComponentDatafootIkNormalLeftX, RepPlayerComponentDatafootIkNormalLeftY, RepPlayerComponentDatafootIkNormalLeftZ) * 0.01f;
    }
    public void SetRepPlayerComponentDatafootIkNormalLeft(float3 val)
    {
        RepPlayerComponentDatafootIkNormalLeftX = (int)(val.x * 100);
        RepPlayerComponentDatafootIkNormalLeftY = (int)(val.y * 100);
        RepPlayerComponentDatafootIkNormalLeftZ = (int)(val.z * 100);
    }
    public float3 GetRepPlayerComponentDatafootIkNormaRight()
    {
        return new float3(RepPlayerComponentDatafootIkNormaRightX, RepPlayerComponentDatafootIkNormaRightY, RepPlayerComponentDatafootIkNormaRightZ) * 0.01f;
    }
    public void SetRepPlayerComponentDatafootIkNormaRight(float3 val)
    {
        RepPlayerComponentDatafootIkNormaRightX = (int)(val.x * 100);
        RepPlayerComponentDatafootIkNormaRightY = (int)(val.y * 100);
        RepPlayerComponentDatafootIkNormaRightZ = (int)(val.z * 100);
    }
    public float3 GetRepPlayerComponentDatavelocity()
    {
        return new float3(RepPlayerComponentDatavelocityX, RepPlayerComponentDatavelocityY, RepPlayerComponentDatavelocityZ) * 0.01f;
    }
    public void SetRepPlayerComponentDatavelocity(float3 val)
    {
        RepPlayerComponentDatavelocityX = (int)(val.x * 100);
        RepPlayerComponentDatavelocityY = (int)(val.y * 100);
        RepPlayerComponentDatavelocityZ = (int)(val.z * 100);
    }


    public void PredictDelta(uint tick, ref RepPlayerSnapshotData baseline1, ref RepPlayerSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        RepPlayerComponentDatanetworkId = predictor.PredictInt(RepPlayerComponentDatanetworkId, baseline1.RepPlayerComponentDatanetworkId, baseline2.RepPlayerComponentDatanetworkId);
        RepPlayerComponentDatapositionX = predictor.PredictInt(RepPlayerComponentDatapositionX, baseline1.RepPlayerComponentDatapositionX, baseline2.RepPlayerComponentDatapositionX);
        RepPlayerComponentDatapositionY = predictor.PredictInt(RepPlayerComponentDatapositionY, baseline1.RepPlayerComponentDatapositionY, baseline2.RepPlayerComponentDatapositionY);
        RepPlayerComponentDatapositionZ = predictor.PredictInt(RepPlayerComponentDatapositionZ, baseline1.RepPlayerComponentDatapositionZ, baseline2.RepPlayerComponentDatapositionZ);
        RepPlayerComponentDatarotation = predictor.PredictInt(RepPlayerComponentDatarotation, baseline1.RepPlayerComponentDatarotation, baseline2.RepPlayerComponentDatarotation);
        RepPlayerComponentDataaimYaw = predictor.PredictInt(RepPlayerComponentDataaimYaw, baseline1.RepPlayerComponentDataaimYaw, baseline2.RepPlayerComponentDataaimYaw);
        RepPlayerComponentDataaimPitch = predictor.PredictInt(RepPlayerComponentDataaimPitch, baseline1.RepPlayerComponentDataaimPitch, baseline2.RepPlayerComponentDataaimPitch);
        RepPlayerComponentDatamoveYaw = predictor.PredictInt(RepPlayerComponentDatamoveYaw, baseline1.RepPlayerComponentDatamoveYaw, baseline2.RepPlayerComponentDatamoveYaw);
        RepPlayerComponentDatacharLocoState = predictor.PredictInt(RepPlayerComponentDatacharLocoState, baseline1.RepPlayerComponentDatacharLocoState, baseline2.RepPlayerComponentDatacharLocoState);
        RepPlayerComponentDatacharLocoTick = predictor.PredictInt(RepPlayerComponentDatacharLocoTick, baseline1.RepPlayerComponentDatacharLocoTick, baseline2.RepPlayerComponentDatacharLocoTick);
        RepPlayerComponentDatacharAction = predictor.PredictInt(RepPlayerComponentDatacharAction, baseline1.RepPlayerComponentDatacharAction, baseline2.RepPlayerComponentDatacharAction);
        RepPlayerComponentDatacharActionTick = predictor.PredictInt(RepPlayerComponentDatacharActionTick, baseline1.RepPlayerComponentDatacharActionTick, baseline2.RepPlayerComponentDatacharActionTick);
        RepPlayerComponentDatadamageTick = predictor.PredictInt(RepPlayerComponentDatadamageTick, baseline1.RepPlayerComponentDatadamageTick, baseline2.RepPlayerComponentDatadamageTick);
        RepPlayerComponentDatadamageDirection = predictor.PredictInt(RepPlayerComponentDatadamageDirection, baseline1.RepPlayerComponentDatadamageDirection, baseline2.RepPlayerComponentDatadamageDirection);
        RepPlayerComponentDatasprinting = predictor.PredictInt(RepPlayerComponentDatasprinting, baseline1.RepPlayerComponentDatasprinting, baseline2.RepPlayerComponentDatasprinting);
        RepPlayerComponentDatasprintWeight = predictor.PredictInt(RepPlayerComponentDatasprintWeight, baseline1.RepPlayerComponentDatasprintWeight, baseline2.RepPlayerComponentDatasprintWeight);
        RepPlayerComponentDatapreviousCharLocoState = predictor.PredictInt(RepPlayerComponentDatapreviousCharLocoState, baseline1.RepPlayerComponentDatapreviousCharLocoState, baseline2.RepPlayerComponentDatapreviousCharLocoState);
        RepPlayerComponentDatalastGroundMoveTick = predictor.PredictInt(RepPlayerComponentDatalastGroundMoveTick, baseline1.RepPlayerComponentDatalastGroundMoveTick, baseline2.RepPlayerComponentDatalastGroundMoveTick);
        RepPlayerComponentDatamoveAngleLocal = predictor.PredictInt(RepPlayerComponentDatamoveAngleLocal, baseline1.RepPlayerComponentDatamoveAngleLocal, baseline2.RepPlayerComponentDatamoveAngleLocal);
        RepPlayerComponentDatashootPoseWeight = predictor.PredictInt(RepPlayerComponentDatashootPoseWeight, baseline1.RepPlayerComponentDatashootPoseWeight, baseline2.RepPlayerComponentDatashootPoseWeight);
        RepPlayerComponentDatalocomotionVectorX = predictor.PredictInt(RepPlayerComponentDatalocomotionVectorX, baseline1.RepPlayerComponentDatalocomotionVectorX, baseline2.RepPlayerComponentDatalocomotionVectorX);
        RepPlayerComponentDatalocomotionVectorY = predictor.PredictInt(RepPlayerComponentDatalocomotionVectorY, baseline1.RepPlayerComponentDatalocomotionVectorY, baseline2.RepPlayerComponentDatalocomotionVectorY);
        RepPlayerComponentDatalocomotionPhase = predictor.PredictInt(RepPlayerComponentDatalocomotionPhase, baseline1.RepPlayerComponentDatalocomotionPhase, baseline2.RepPlayerComponentDatalocomotionPhase);
        RepPlayerComponentDatabanking = predictor.PredictInt(RepPlayerComponentDatabanking, baseline1.RepPlayerComponentDatabanking, baseline2.RepPlayerComponentDatabanking);
        RepPlayerComponentDatalandAnticWeight = predictor.PredictInt(RepPlayerComponentDatalandAnticWeight, baseline1.RepPlayerComponentDatalandAnticWeight, baseline2.RepPlayerComponentDatalandAnticWeight);
        RepPlayerComponentDataturnStartAngle = predictor.PredictInt(RepPlayerComponentDataturnStartAngle, baseline1.RepPlayerComponentDataturnStartAngle, baseline2.RepPlayerComponentDataturnStartAngle);
        RepPlayerComponentDataturnDirection = predictor.PredictInt(RepPlayerComponentDataturnDirection, baseline1.RepPlayerComponentDataturnDirection, baseline2.RepPlayerComponentDataturnDirection);
        RepPlayerComponentDatasquashTime = predictor.PredictInt(RepPlayerComponentDatasquashTime, baseline1.RepPlayerComponentDatasquashTime, baseline2.RepPlayerComponentDatasquashTime);
        RepPlayerComponentDatasquashWeight = predictor.PredictInt(RepPlayerComponentDatasquashWeight, baseline1.RepPlayerComponentDatasquashWeight, baseline2.RepPlayerComponentDatasquashWeight);
        RepPlayerComponentDatainAirTime = predictor.PredictInt(RepPlayerComponentDatainAirTime, baseline1.RepPlayerComponentDatainAirTime, baseline2.RepPlayerComponentDatainAirTime);
        RepPlayerComponentDatajumpTime = predictor.PredictInt(RepPlayerComponentDatajumpTime, baseline1.RepPlayerComponentDatajumpTime, baseline2.RepPlayerComponentDatajumpTime);
        RepPlayerComponentDatasimpleTime = predictor.PredictInt(RepPlayerComponentDatasimpleTime, baseline1.RepPlayerComponentDatasimpleTime, baseline2.RepPlayerComponentDatasimpleTime);
        RepPlayerComponentDatafootIkOffsetX = predictor.PredictInt(RepPlayerComponentDatafootIkOffsetX, baseline1.RepPlayerComponentDatafootIkOffsetX, baseline2.RepPlayerComponentDatafootIkOffsetX);
        RepPlayerComponentDatafootIkOffsetY = predictor.PredictInt(RepPlayerComponentDatafootIkOffsetY, baseline1.RepPlayerComponentDatafootIkOffsetY, baseline2.RepPlayerComponentDatafootIkOffsetY);
        RepPlayerComponentDatafootIkNormalLeftX = predictor.PredictInt(RepPlayerComponentDatafootIkNormalLeftX, baseline1.RepPlayerComponentDatafootIkNormalLeftX, baseline2.RepPlayerComponentDatafootIkNormalLeftX);
        RepPlayerComponentDatafootIkNormalLeftY = predictor.PredictInt(RepPlayerComponentDatafootIkNormalLeftY, baseline1.RepPlayerComponentDatafootIkNormalLeftY, baseline2.RepPlayerComponentDatafootIkNormalLeftY);
        RepPlayerComponentDatafootIkNormalLeftZ = predictor.PredictInt(RepPlayerComponentDatafootIkNormalLeftZ, baseline1.RepPlayerComponentDatafootIkNormalLeftZ, baseline2.RepPlayerComponentDatafootIkNormalLeftZ);
        RepPlayerComponentDatafootIkNormaRightX = predictor.PredictInt(RepPlayerComponentDatafootIkNormaRightX, baseline1.RepPlayerComponentDatafootIkNormaRightX, baseline2.RepPlayerComponentDatafootIkNormaRightX);
        RepPlayerComponentDatafootIkNormaRightY = predictor.PredictInt(RepPlayerComponentDatafootIkNormaRightY, baseline1.RepPlayerComponentDatafootIkNormaRightY, baseline2.RepPlayerComponentDatafootIkNormaRightY);
        RepPlayerComponentDatafootIkNormaRightZ = predictor.PredictInt(RepPlayerComponentDatafootIkNormaRightZ, baseline1.RepPlayerComponentDatafootIkNormaRightZ, baseline2.RepPlayerComponentDatafootIkNormaRightZ);
        RepPlayerComponentDatavelocityX = predictor.PredictInt(RepPlayerComponentDatavelocityX, baseline1.RepPlayerComponentDatavelocityX, baseline2.RepPlayerComponentDatavelocityX);
        RepPlayerComponentDatavelocityY = predictor.PredictInt(RepPlayerComponentDatavelocityY, baseline1.RepPlayerComponentDatavelocityY, baseline2.RepPlayerComponentDatavelocityY);
        RepPlayerComponentDatavelocityZ = predictor.PredictInt(RepPlayerComponentDatavelocityZ, baseline1.RepPlayerComponentDatavelocityZ, baseline2.RepPlayerComponentDatavelocityZ);

    }

    public void Serialize(ref RepPlayerSnapshotData baseline, DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        writer.WritePackedIntDelta(RepPlayerComponentDatanetworkId, baseline.RepPlayerComponentDatanetworkId, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatapositionX, baseline.RepPlayerComponentDatapositionX, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatapositionY, baseline.RepPlayerComponentDatapositionY, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatapositionZ, baseline.RepPlayerComponentDatapositionZ, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatarotation, baseline.RepPlayerComponentDatarotation, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDataaimYaw, baseline.RepPlayerComponentDataaimYaw, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDataaimPitch, baseline.RepPlayerComponentDataaimPitch, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatamoveYaw, baseline.RepPlayerComponentDatamoveYaw, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatacharLocoState, baseline.RepPlayerComponentDatacharLocoState, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatacharLocoTick, baseline.RepPlayerComponentDatacharLocoTick, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatacharAction, baseline.RepPlayerComponentDatacharAction, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatacharActionTick, baseline.RepPlayerComponentDatacharActionTick, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatadamageTick, baseline.RepPlayerComponentDatadamageTick, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatadamageDirection, baseline.RepPlayerComponentDatadamageDirection, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatasprinting, baseline.RepPlayerComponentDatasprinting, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatasprintWeight, baseline.RepPlayerComponentDatasprintWeight, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatapreviousCharLocoState, baseline.RepPlayerComponentDatapreviousCharLocoState, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatalastGroundMoveTick, baseline.RepPlayerComponentDatalastGroundMoveTick, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatamoveAngleLocal, baseline.RepPlayerComponentDatamoveAngleLocal, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatashootPoseWeight, baseline.RepPlayerComponentDatashootPoseWeight, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatalocomotionVectorX, baseline.RepPlayerComponentDatalocomotionVectorX, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatalocomotionVectorY, baseline.RepPlayerComponentDatalocomotionVectorY, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatalocomotionPhase, baseline.RepPlayerComponentDatalocomotionPhase, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatabanking, baseline.RepPlayerComponentDatabanking, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatalandAnticWeight, baseline.RepPlayerComponentDatalandAnticWeight, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDataturnStartAngle, baseline.RepPlayerComponentDataturnStartAngle, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDataturnDirection, baseline.RepPlayerComponentDataturnDirection, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatasquashTime, baseline.RepPlayerComponentDatasquashTime, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatasquashWeight, baseline.RepPlayerComponentDatasquashWeight, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatainAirTime, baseline.RepPlayerComponentDatainAirTime, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatajumpTime, baseline.RepPlayerComponentDatajumpTime, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatasimpleTime, baseline.RepPlayerComponentDatasimpleTime, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatafootIkOffsetX, baseline.RepPlayerComponentDatafootIkOffsetX, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatafootIkOffsetY, baseline.RepPlayerComponentDatafootIkOffsetY, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatafootIkNormalLeftX, baseline.RepPlayerComponentDatafootIkNormalLeftX, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatafootIkNormalLeftY, baseline.RepPlayerComponentDatafootIkNormalLeftY, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatafootIkNormalLeftZ, baseline.RepPlayerComponentDatafootIkNormalLeftZ, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatafootIkNormaRightX, baseline.RepPlayerComponentDatafootIkNormaRightX, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatafootIkNormaRightY, baseline.RepPlayerComponentDatafootIkNormaRightY, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatafootIkNormaRightZ, baseline.RepPlayerComponentDatafootIkNormaRightZ, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatavelocityX, baseline.RepPlayerComponentDatavelocityX, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatavelocityY, baseline.RepPlayerComponentDatavelocityY, compressionModel);
        writer.WritePackedIntDelta(RepPlayerComponentDatavelocityZ, baseline.RepPlayerComponentDatavelocityZ, compressionModel);

    }

    public void Deserialize(uint tick, ref RepPlayerSnapshotData baseline, DataStreamReader reader, ref DataStreamReader.Context ctx,
        NetworkCompressionModel compressionModel)
    {
        this.tick = tick;
        RepPlayerComponentDatanetworkId = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatanetworkId, compressionModel);
        RepPlayerComponentDatapositionX = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatapositionX, compressionModel);
        RepPlayerComponentDatapositionY = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatapositionY, compressionModel);
        RepPlayerComponentDatapositionZ = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatapositionZ, compressionModel);
        RepPlayerComponentDatarotation = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatarotation, compressionModel);
        RepPlayerComponentDataaimYaw = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDataaimYaw, compressionModel);
        RepPlayerComponentDataaimPitch = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDataaimPitch, compressionModel);
        RepPlayerComponentDatamoveYaw = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatamoveYaw, compressionModel);
        RepPlayerComponentDatacharLocoState = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatacharLocoState, compressionModel);
        RepPlayerComponentDatacharLocoTick = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatacharLocoTick, compressionModel);
        RepPlayerComponentDatacharAction = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatacharAction, compressionModel);
        RepPlayerComponentDatacharActionTick = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatacharActionTick, compressionModel);
        RepPlayerComponentDatadamageTick = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatadamageTick, compressionModel);
        RepPlayerComponentDatadamageDirection = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatadamageDirection, compressionModel);
        RepPlayerComponentDatasprinting = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatasprinting, compressionModel);
        RepPlayerComponentDatasprintWeight = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatasprintWeight, compressionModel);
        RepPlayerComponentDatapreviousCharLocoState = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatapreviousCharLocoState, compressionModel);
        RepPlayerComponentDatalastGroundMoveTick = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatalastGroundMoveTick, compressionModel);
        RepPlayerComponentDatamoveAngleLocal = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatamoveAngleLocal, compressionModel);
        RepPlayerComponentDatashootPoseWeight = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatashootPoseWeight, compressionModel);
        RepPlayerComponentDatalocomotionVectorX = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatalocomotionVectorX, compressionModel);
        RepPlayerComponentDatalocomotionVectorY = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatalocomotionVectorY, compressionModel);
        RepPlayerComponentDatalocomotionPhase = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatalocomotionPhase, compressionModel);
        RepPlayerComponentDatabanking = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatabanking, compressionModel);
        RepPlayerComponentDatalandAnticWeight = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatalandAnticWeight, compressionModel);
        RepPlayerComponentDataturnStartAngle = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDataturnStartAngle, compressionModel);
        RepPlayerComponentDataturnDirection = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDataturnDirection, compressionModel);
        RepPlayerComponentDatasquashTime = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatasquashTime, compressionModel);
        RepPlayerComponentDatasquashWeight = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatasquashWeight, compressionModel);
        RepPlayerComponentDatainAirTime = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatainAirTime, compressionModel);
        RepPlayerComponentDatajumpTime = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatajumpTime, compressionModel);
        RepPlayerComponentDatasimpleTime = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatasimpleTime, compressionModel);
        RepPlayerComponentDatafootIkOffsetX = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatafootIkOffsetX, compressionModel);
        RepPlayerComponentDatafootIkOffsetY = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatafootIkOffsetY, compressionModel);
        RepPlayerComponentDatafootIkNormalLeftX = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatafootIkNormalLeftX, compressionModel);
        RepPlayerComponentDatafootIkNormalLeftY = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatafootIkNormalLeftY, compressionModel);
        RepPlayerComponentDatafootIkNormalLeftZ = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatafootIkNormalLeftZ, compressionModel);
        RepPlayerComponentDatafootIkNormaRightX = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatafootIkNormaRightX, compressionModel);
        RepPlayerComponentDatafootIkNormaRightY = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatafootIkNormaRightY, compressionModel);
        RepPlayerComponentDatafootIkNormaRightZ = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatafootIkNormaRightZ, compressionModel);
        RepPlayerComponentDatavelocityX = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatavelocityX, compressionModel);
        RepPlayerComponentDatavelocityY = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatavelocityY, compressionModel);
        RepPlayerComponentDatavelocityZ = reader.ReadPackedIntDelta(ref ctx, baseline.RepPlayerComponentDatavelocityZ, compressionModel);

    }
    public void Interpolate(ref RepPlayerSnapshotData target, float factor)
    {
        SetRepPlayerComponentDataposition(math.lerp(GetRepPlayerComponentDataposition(), target.GetRepPlayerComponentDataposition(), factor));
        SetRepPlayerComponentDatarotation(math.lerp(GetRepPlayerComponentDatarotation(), target.GetRepPlayerComponentDatarotation(), factor));
        SetRepPlayerComponentDataaimYaw(math.lerp(GetRepPlayerComponentDataaimYaw(), target.GetRepPlayerComponentDataaimYaw(), factor));
        SetRepPlayerComponentDataaimPitch(math.lerp(GetRepPlayerComponentDataaimPitch(), target.GetRepPlayerComponentDataaimPitch(), factor));
        SetRepPlayerComponentDatamoveYaw(math.lerp(GetRepPlayerComponentDatamoveYaw(), target.GetRepPlayerComponentDatamoveYaw(), factor));
        SetRepPlayerComponentDatadamageDirection(math.lerp(GetRepPlayerComponentDatadamageDirection(), target.GetRepPlayerComponentDatadamageDirection(), factor));
        SetRepPlayerComponentDatasprintWeight(math.lerp(GetRepPlayerComponentDatasprintWeight(), target.GetRepPlayerComponentDatasprintWeight(), factor));
        SetRepPlayerComponentDatamoveAngleLocal(math.lerp(GetRepPlayerComponentDatamoveAngleLocal(), target.GetRepPlayerComponentDatamoveAngleLocal(), factor));
        SetRepPlayerComponentDatashootPoseWeight(math.lerp(GetRepPlayerComponentDatashootPoseWeight(), target.GetRepPlayerComponentDatashootPoseWeight(), factor));
        SetRepPlayerComponentDatalocomotionVector(math.lerp(GetRepPlayerComponentDatalocomotionVector(), target.GetRepPlayerComponentDatalocomotionVector(), factor));
        SetRepPlayerComponentDatalocomotionPhase(math.lerp(GetRepPlayerComponentDatalocomotionPhase(), target.GetRepPlayerComponentDatalocomotionPhase(), factor));
        SetRepPlayerComponentDatabanking(math.lerp(GetRepPlayerComponentDatabanking(), target.GetRepPlayerComponentDatabanking(), factor));
        SetRepPlayerComponentDatalandAnticWeight(math.lerp(GetRepPlayerComponentDatalandAnticWeight(), target.GetRepPlayerComponentDatalandAnticWeight(), factor));
        SetRepPlayerComponentDataturnStartAngle(math.lerp(GetRepPlayerComponentDataturnStartAngle(), target.GetRepPlayerComponentDataturnStartAngle(), factor));
        SetRepPlayerComponentDatasquashTime(math.lerp(GetRepPlayerComponentDatasquashTime(), target.GetRepPlayerComponentDatasquashTime(), factor));
        SetRepPlayerComponentDatasquashWeight(math.lerp(GetRepPlayerComponentDatasquashWeight(), target.GetRepPlayerComponentDatasquashWeight(), factor));
        SetRepPlayerComponentDatainAirTime(math.lerp(GetRepPlayerComponentDatainAirTime(), target.GetRepPlayerComponentDatainAirTime(), factor));
        SetRepPlayerComponentDatajumpTime(math.lerp(GetRepPlayerComponentDatajumpTime(), target.GetRepPlayerComponentDatajumpTime(), factor));
        SetRepPlayerComponentDatasimpleTime(math.lerp(GetRepPlayerComponentDatasimpleTime(), target.GetRepPlayerComponentDatasimpleTime(), factor));
        SetRepPlayerComponentDatafootIkOffset(math.lerp(GetRepPlayerComponentDatafootIkOffset(), target.GetRepPlayerComponentDatafootIkOffset(), factor));
        SetRepPlayerComponentDatafootIkNormalLeft(math.lerp(GetRepPlayerComponentDatafootIkNormalLeft(), target.GetRepPlayerComponentDatafootIkNormalLeft(), factor));
        SetRepPlayerComponentDatafootIkNormaRight(math.lerp(GetRepPlayerComponentDatafootIkNormaRight(), target.GetRepPlayerComponentDatafootIkNormaRight(), factor));
        SetRepPlayerComponentDatavelocity(math.lerp(GetRepPlayerComponentDatavelocity(), target.GetRepPlayerComponentDatavelocity(), factor));

    }
}