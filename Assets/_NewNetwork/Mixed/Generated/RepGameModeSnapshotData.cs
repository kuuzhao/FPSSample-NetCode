using Unity.Mathematics;
using Unity.Networking.Transport;

public struct RepGameModeSnapshotData : ISnapshotData<RepGameModeSnapshotData>
{
    public uint tick;
    int RepGameModegameTimerSeconds;
    uint RepGameModegameTimerMessage0;
    uint RepGameModegameTimerMessage1;
    uint RepGameModegameTimerMessage2;
    uint RepGameModegameTimerMessage3;
    uint RepGameModegameTimerMessage4;
    uint RepGameModegameTimerMessage5;
    uint RepGameModegameTimerMessage6;
    uint RepGameModegameTimerMessage7;
    uint RepGameModegameTimerMessage8;
    uint RepGameModegameTimerMessage9;
    uint RepGameModegameTimerMessage10;
    uint RepGameModegameTimerMessage11;
    uint RepGameModegameTimerMessage12;
    uint RepGameModegameTimerMessage13;
    uint RepGameModegameTimerMessage14;
    uint RepGameModegameTimerMessage15;
    uint RepGameModeteamName00;
    uint RepGameModeteamName01;
    uint RepGameModeteamName02;
    uint RepGameModeteamName03;
    uint RepGameModeteamName04;
    uint RepGameModeteamName05;
    uint RepGameModeteamName06;
    uint RepGameModeteamName07;
    uint RepGameModeteamName08;
    uint RepGameModeteamName09;
    uint RepGameModeteamName010;
    uint RepGameModeteamName011;
    uint RepGameModeteamName012;
    uint RepGameModeteamName013;
    uint RepGameModeteamName014;
    uint RepGameModeteamName015;
    uint RepGameModeteamName10;
    uint RepGameModeteamName11;
    uint RepGameModeteamName12;
    uint RepGameModeteamName13;
    uint RepGameModeteamName14;
    uint RepGameModeteamName15;
    uint RepGameModeteamName16;
    uint RepGameModeteamName17;
    uint RepGameModeteamName18;
    uint RepGameModeteamName19;
    uint RepGameModeteamName110;
    uint RepGameModeteamName111;
    uint RepGameModeteamName112;
    uint RepGameModeteamName113;
    uint RepGameModeteamName114;
    uint RepGameModeteamName115;
    int RepGameModeteamScore0;
    int RepGameModeteamScore1;


    public uint Tick => tick;
    public int GetRepGameModegameTimerSeconds()
    {
        return RepGameModegameTimerSeconds;
    }
    public void SetRepGameModegameTimerSeconds(int val)
    {
        RepGameModegameTimerSeconds = val;
    }
    public unsafe Unity.Entities.NativeString64 GetRepGameModegameTimerMessage()
    {
        Unity.Entities.NativeString64 ns = new Unity.Entities.NativeString64("");
        ns.buffer[0] = RepGameModegameTimerMessage0;ns.buffer[1] = RepGameModegameTimerMessage1;ns.buffer[2] = RepGameModegameTimerMessage2;
        ns.buffer[3] = RepGameModegameTimerMessage3;ns.buffer[4] = RepGameModegameTimerMessage4;ns.buffer[5] = RepGameModegameTimerMessage5;
        ns.buffer[6] = RepGameModegameTimerMessage6;ns.buffer[7] = RepGameModegameTimerMessage7;ns.buffer[8] = RepGameModegameTimerMessage8;
        ns.buffer[9] = RepGameModegameTimerMessage9;ns.buffer[10] = RepGameModegameTimerMessage10;ns.buffer[11] = RepGameModegameTimerMessage11;
        ns.buffer[12] = RepGameModegameTimerMessage12;ns.buffer[13] = RepGameModegameTimerMessage13;ns.buffer[14] = RepGameModegameTimerMessage14;
        ns.Length = (int)RepGameModegameTimerMessage15;

        return ns;
    }
    public unsafe void SetRepGameModegameTimerMessage(Unity.Entities.NativeString64 ns)
    {
        RepGameModegameTimerMessage0 = ns.buffer[0];RepGameModegameTimerMessage1 = ns.buffer[1];RepGameModegameTimerMessage2 = ns.buffer[2];
        RepGameModegameTimerMessage3 = ns.buffer[3];RepGameModegameTimerMessage4 = ns.buffer[4];RepGameModegameTimerMessage5 = ns.buffer[5];
        RepGameModegameTimerMessage6 = ns.buffer[6];RepGameModegameTimerMessage7 = ns.buffer[7];RepGameModegameTimerMessage8 = ns.buffer[8];
        RepGameModegameTimerMessage9 = ns.buffer[9];RepGameModegameTimerMessage10 = ns.buffer[10];RepGameModegameTimerMessage11 = ns.buffer[11];
        RepGameModegameTimerMessage12 = ns.buffer[12];RepGameModegameTimerMessage13 = ns.buffer[13];RepGameModegameTimerMessage14 = ns.buffer[14];
        RepGameModegameTimerMessage15 = (uint)ns.Length;
    }
    public unsafe Unity.Entities.NativeString64 GetRepGameModeteamName0()
    {
        Unity.Entities.NativeString64 ns = new Unity.Entities.NativeString64("");
        ns.buffer[0] = RepGameModeteamName00;ns.buffer[1] = RepGameModeteamName01;ns.buffer[2] = RepGameModeteamName02;
        ns.buffer[3] = RepGameModeteamName03;ns.buffer[4] = RepGameModeteamName04;ns.buffer[5] = RepGameModeteamName05;
        ns.buffer[6] = RepGameModeteamName06;ns.buffer[7] = RepGameModeteamName07;ns.buffer[8] = RepGameModeteamName08;
        ns.buffer[9] = RepGameModeteamName09;ns.buffer[10] = RepGameModeteamName010;ns.buffer[11] = RepGameModeteamName011;
        ns.buffer[12] = RepGameModeteamName012;ns.buffer[13] = RepGameModeteamName013;ns.buffer[14] = RepGameModeteamName014;
        ns.Length = (int)RepGameModeteamName015;

        return ns;
    }
    public unsafe void SetRepGameModeteamName0(Unity.Entities.NativeString64 ns)
    {
        RepGameModeteamName00 = ns.buffer[0];RepGameModeteamName01 = ns.buffer[1];RepGameModeteamName02 = ns.buffer[2];
        RepGameModeteamName03 = ns.buffer[3];RepGameModeteamName04 = ns.buffer[4];RepGameModeteamName05 = ns.buffer[5];
        RepGameModeteamName06 = ns.buffer[6];RepGameModeteamName07 = ns.buffer[7];RepGameModeteamName08 = ns.buffer[8];
        RepGameModeteamName09 = ns.buffer[9];RepGameModeteamName010 = ns.buffer[10];RepGameModeteamName011 = ns.buffer[11];
        RepGameModeteamName012 = ns.buffer[12];RepGameModeteamName013 = ns.buffer[13];RepGameModeteamName014 = ns.buffer[14];
        RepGameModeteamName015 = (uint)ns.Length;
    }
    public unsafe Unity.Entities.NativeString64 GetRepGameModeteamName1()
    {
        Unity.Entities.NativeString64 ns = new Unity.Entities.NativeString64("");
        ns.buffer[0] = RepGameModeteamName10;ns.buffer[1] = RepGameModeteamName11;ns.buffer[2] = RepGameModeteamName12;
        ns.buffer[3] = RepGameModeteamName13;ns.buffer[4] = RepGameModeteamName14;ns.buffer[5] = RepGameModeteamName15;
        ns.buffer[6] = RepGameModeteamName16;ns.buffer[7] = RepGameModeteamName17;ns.buffer[8] = RepGameModeteamName18;
        ns.buffer[9] = RepGameModeteamName19;ns.buffer[10] = RepGameModeteamName110;ns.buffer[11] = RepGameModeteamName111;
        ns.buffer[12] = RepGameModeteamName112;ns.buffer[13] = RepGameModeteamName113;ns.buffer[14] = RepGameModeteamName114;
        ns.Length = (int)RepGameModeteamName115;

        return ns;
    }
    public unsafe void SetRepGameModeteamName1(Unity.Entities.NativeString64 ns)
    {
        RepGameModeteamName10 = ns.buffer[0];RepGameModeteamName11 = ns.buffer[1];RepGameModeteamName12 = ns.buffer[2];
        RepGameModeteamName13 = ns.buffer[3];RepGameModeteamName14 = ns.buffer[4];RepGameModeteamName15 = ns.buffer[5];
        RepGameModeteamName16 = ns.buffer[6];RepGameModeteamName17 = ns.buffer[7];RepGameModeteamName18 = ns.buffer[8];
        RepGameModeteamName19 = ns.buffer[9];RepGameModeteamName110 = ns.buffer[10];RepGameModeteamName111 = ns.buffer[11];
        RepGameModeteamName112 = ns.buffer[12];RepGameModeteamName113 = ns.buffer[13];RepGameModeteamName114 = ns.buffer[14];
        RepGameModeteamName115 = (uint)ns.Length;
    }
    public int GetRepGameModeteamScore0()
    {
        return RepGameModeteamScore0;
    }
    public void SetRepGameModeteamScore0(int val)
    {
        RepGameModeteamScore0 = val;
    }
    public int GetRepGameModeteamScore1()
    {
        return RepGameModeteamScore1;
    }
    public void SetRepGameModeteamScore1(int val)
    {
        RepGameModeteamScore1 = val;
    }


    public void PredictDelta(uint tick, ref RepGameModeSnapshotData baseline1, ref RepGameModeSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        RepGameModegameTimerSeconds = predictor.PredictInt(RepGameModegameTimerSeconds, baseline1.RepGameModegameTimerSeconds, baseline2.RepGameModegameTimerSeconds);
        RepGameModeteamScore0 = predictor.PredictInt(RepGameModeteamScore0, baseline1.RepGameModeteamScore0, baseline2.RepGameModeteamScore0);
        RepGameModeteamScore1 = predictor.PredictInt(RepGameModeteamScore1, baseline1.RepGameModeteamScore1, baseline2.RepGameModeteamScore1);

    }

    public void Serialize(ref RepGameModeSnapshotData baseline, DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        writer.WritePackedIntDelta(RepGameModegameTimerSeconds, baseline.RepGameModegameTimerSeconds, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage0, baseline.RepGameModegameTimerMessage0, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage1, baseline.RepGameModegameTimerMessage1, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage2, baseline.RepGameModegameTimerMessage2, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage3, baseline.RepGameModegameTimerMessage3, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage4, baseline.RepGameModegameTimerMessage4, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage5, baseline.RepGameModegameTimerMessage5, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage6, baseline.RepGameModegameTimerMessage6, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage7, baseline.RepGameModegameTimerMessage7, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage8, baseline.RepGameModegameTimerMessage8, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage9, baseline.RepGameModegameTimerMessage9, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage10, baseline.RepGameModegameTimerMessage10, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage11, baseline.RepGameModegameTimerMessage11, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage12, baseline.RepGameModegameTimerMessage12, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage13, baseline.RepGameModegameTimerMessage13, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage14, baseline.RepGameModegameTimerMessage14, compressionModel);
        writer.WritePackedUIntDelta(RepGameModegameTimerMessage15, baseline.RepGameModegameTimerMessage15, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName00, baseline.RepGameModeteamName00, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName01, baseline.RepGameModeteamName01, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName02, baseline.RepGameModeteamName02, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName03, baseline.RepGameModeteamName03, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName04, baseline.RepGameModeteamName04, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName05, baseline.RepGameModeteamName05, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName06, baseline.RepGameModeteamName06, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName07, baseline.RepGameModeteamName07, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName08, baseline.RepGameModeteamName08, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName09, baseline.RepGameModeteamName09, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName010, baseline.RepGameModeteamName010, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName011, baseline.RepGameModeteamName011, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName012, baseline.RepGameModeteamName012, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName013, baseline.RepGameModeteamName013, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName014, baseline.RepGameModeteamName014, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName015, baseline.RepGameModeteamName015, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName10, baseline.RepGameModeteamName10, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName11, baseline.RepGameModeteamName11, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName12, baseline.RepGameModeteamName12, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName13, baseline.RepGameModeteamName13, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName14, baseline.RepGameModeteamName14, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName15, baseline.RepGameModeteamName15, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName16, baseline.RepGameModeteamName16, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName17, baseline.RepGameModeteamName17, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName18, baseline.RepGameModeteamName18, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName19, baseline.RepGameModeteamName19, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName110, baseline.RepGameModeteamName110, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName111, baseline.RepGameModeteamName111, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName112, baseline.RepGameModeteamName112, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName113, baseline.RepGameModeteamName113, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName114, baseline.RepGameModeteamName114, compressionModel);
        writer.WritePackedUIntDelta(RepGameModeteamName115, baseline.RepGameModeteamName115, compressionModel);
        writer.WritePackedIntDelta(RepGameModeteamScore0, baseline.RepGameModeteamScore0, compressionModel);
        writer.WritePackedIntDelta(RepGameModeteamScore1, baseline.RepGameModeteamScore1, compressionModel);

    }

    public void Deserialize(uint tick, ref RepGameModeSnapshotData baseline, DataStreamReader reader, ref DataStreamReader.Context ctx,
        NetworkCompressionModel compressionModel)
    {
        this.tick = tick;
        RepGameModegameTimerSeconds = reader.ReadPackedIntDelta(ref ctx, baseline.RepGameModegameTimerSeconds, compressionModel);
        RepGameModegameTimerMessage0 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage0, compressionModel);
        RepGameModegameTimerMessage1 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage1, compressionModel);
        RepGameModegameTimerMessage2 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage2, compressionModel);
        RepGameModegameTimerMessage3 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage3, compressionModel);
        RepGameModegameTimerMessage4 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage4, compressionModel);
        RepGameModegameTimerMessage5 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage5, compressionModel);
        RepGameModegameTimerMessage6 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage6, compressionModel);
        RepGameModegameTimerMessage7 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage7, compressionModel);
        RepGameModegameTimerMessage8 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage8, compressionModel);
        RepGameModegameTimerMessage9 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage9, compressionModel);
        RepGameModegameTimerMessage10 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage10, compressionModel);
        RepGameModegameTimerMessage11 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage11, compressionModel);
        RepGameModegameTimerMessage12 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage12, compressionModel);
        RepGameModegameTimerMessage13 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage13, compressionModel);
        RepGameModegameTimerMessage14 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage14, compressionModel);
        RepGameModegameTimerMessage15 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModegameTimerMessage15, compressionModel);
        RepGameModeteamName00 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName00, compressionModel);
        RepGameModeteamName01 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName01, compressionModel);
        RepGameModeteamName02 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName02, compressionModel);
        RepGameModeteamName03 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName03, compressionModel);
        RepGameModeteamName04 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName04, compressionModel);
        RepGameModeteamName05 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName05, compressionModel);
        RepGameModeteamName06 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName06, compressionModel);
        RepGameModeteamName07 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName07, compressionModel);
        RepGameModeteamName08 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName08, compressionModel);
        RepGameModeteamName09 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName09, compressionModel);
        RepGameModeteamName010 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName010, compressionModel);
        RepGameModeteamName011 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName011, compressionModel);
        RepGameModeteamName012 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName012, compressionModel);
        RepGameModeteamName013 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName013, compressionModel);
        RepGameModeteamName014 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName014, compressionModel);
        RepGameModeteamName015 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName015, compressionModel);
        RepGameModeteamName10 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName10, compressionModel);
        RepGameModeteamName11 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName11, compressionModel);
        RepGameModeteamName12 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName12, compressionModel);
        RepGameModeteamName13 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName13, compressionModel);
        RepGameModeteamName14 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName14, compressionModel);
        RepGameModeteamName15 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName15, compressionModel);
        RepGameModeteamName16 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName16, compressionModel);
        RepGameModeteamName17 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName17, compressionModel);
        RepGameModeteamName18 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName18, compressionModel);
        RepGameModeteamName19 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName19, compressionModel);
        RepGameModeteamName110 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName110, compressionModel);
        RepGameModeteamName111 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName111, compressionModel);
        RepGameModeteamName112 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName112, compressionModel);
        RepGameModeteamName113 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName113, compressionModel);
        RepGameModeteamName114 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName114, compressionModel);
        RepGameModeteamName115 = reader.ReadPackedUIntDelta(ref ctx, baseline.RepGameModeteamName115, compressionModel);
        RepGameModeteamScore0 = reader.ReadPackedIntDelta(ref ctx, baseline.RepGameModeteamScore0, compressionModel);
        RepGameModeteamScore1 = reader.ReadPackedIntDelta(ref ctx, baseline.RepGameModeteamScore1, compressionModel);

    }
    public void Interpolate(ref RepGameModeSnapshotData target, float factor)
    {

    }
}