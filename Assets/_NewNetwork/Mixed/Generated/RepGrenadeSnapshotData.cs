using Unity.Mathematics;
using Unity.Networking.Transport;

public struct RepGrenadeSnapshotData : ISnapshotData<RepGrenadeSnapshotData>
{
    public uint tick;
    int TranslationValueX;
    int TranslationValueY;
    int TranslationValueZ;


    public uint Tick => tick;
    public float3 GetTranslationValue()
    {
        return new float3(TranslationValueX, TranslationValueY, TranslationValueZ) * 0.01f;
    }
    public void SetTranslationValue(float3 val)
    {
        TranslationValueX = (int)(val.x * 100);
        TranslationValueY = (int)(val.y * 100);
        TranslationValueZ = (int)(val.z * 100);
    }


    public void PredictDelta(uint tick, ref RepGrenadeSnapshotData baseline1, ref RepGrenadeSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        TranslationValueX = predictor.PredictInt(TranslationValueX, baseline1.TranslationValueX, baseline2.TranslationValueX);
        TranslationValueY = predictor.PredictInt(TranslationValueY, baseline1.TranslationValueY, baseline2.TranslationValueY);
        TranslationValueZ = predictor.PredictInt(TranslationValueZ, baseline1.TranslationValueZ, baseline2.TranslationValueZ);

    }

    public void Serialize(ref RepGrenadeSnapshotData baseline, DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        writer.WritePackedIntDelta(TranslationValueX, baseline.TranslationValueX, compressionModel);
        writer.WritePackedIntDelta(TranslationValueY, baseline.TranslationValueY, compressionModel);
        writer.WritePackedIntDelta(TranslationValueZ, baseline.TranslationValueZ, compressionModel);

    }

    public void Deserialize(uint tick, ref RepGrenadeSnapshotData baseline, DataStreamReader reader, ref DataStreamReader.Context ctx,
        NetworkCompressionModel compressionModel)
    {
        this.tick = tick;
        TranslationValueX = reader.ReadPackedIntDelta(ref ctx, baseline.TranslationValueX, compressionModel);
        TranslationValueY = reader.ReadPackedIntDelta(ref ctx, baseline.TranslationValueY, compressionModel);
        TranslationValueZ = reader.ReadPackedIntDelta(ref ctx, baseline.TranslationValueZ, compressionModel);

    }
    public void Interpolate(ref RepGrenadeSnapshotData target, float factor)
    {
        SetTranslationValue(math.lerp(GetTranslationValue(), target.GetTranslationValue(), factor));

    }
}