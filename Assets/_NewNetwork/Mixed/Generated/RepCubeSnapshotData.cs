using Unity.Mathematics;
using Unity.Networking.Transport;

public struct RepCubeSnapshotData : ISnapshotData<RepCubeSnapshotData>
{
    public uint tick;
    int TranslationValueX;
    int TranslationValueY;
    int TranslationValueZ;


    public uint Tick => tick;
    public float3 GetTranslationValue()
    {
        return new float3(TranslationValueX, TranslationValueY, TranslationValueZ) * 0.1f;
    }
    public void SetTranslationValue(float3 val)
    {
        TranslationValueX = (int)(val.x * 10);
        TranslationValueY = (int)(val.y * 10);
        TranslationValueZ = (int)(val.z * 10);
    }


    public void PredictDelta(uint tick, ref RepCubeSnapshotData baseline1, ref RepCubeSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        TranslationValueX = predictor.PredictInt(TranslationValueX, baseline1.TranslationValueX, baseline2.TranslationValueX);
        TranslationValueY = predictor.PredictInt(TranslationValueY, baseline1.TranslationValueY, baseline2.TranslationValueY);
        TranslationValueZ = predictor.PredictInt(TranslationValueZ, baseline1.TranslationValueZ, baseline2.TranslationValueZ);

    }

    public void Serialize(ref RepCubeSnapshotData baseline, DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        writer.WritePackedIntDelta(TranslationValueX, baseline.TranslationValueX, compressionModel);
        writer.WritePackedIntDelta(TranslationValueY, baseline.TranslationValueY, compressionModel);
        writer.WritePackedIntDelta(TranslationValueZ, baseline.TranslationValueZ, compressionModel);

    }

    public void Deserialize(uint tick, ref RepCubeSnapshotData baseline, DataStreamReader reader, ref DataStreamReader.Context ctx,
        NetworkCompressionModel compressionModel)
    {
        this.tick = tick;
        TranslationValueX = reader.ReadPackedIntDelta(ref ctx, baseline.TranslationValueX, compressionModel);
        TranslationValueY = reader.ReadPackedIntDelta(ref ctx, baseline.TranslationValueY, compressionModel);
        TranslationValueZ = reader.ReadPackedIntDelta(ref ctx, baseline.TranslationValueZ, compressionModel);

    }
    public void Interpolate(ref RepCubeSnapshotData target, float factor)
    {

    }
}