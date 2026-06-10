using System;

[Serializable]
public struct ShelterState
{
    public int provision;
    public int fiber;
    public int ore;
    public int oil;
    public int crystalShard;
    public int elderTreeFlower;

    public ShelterState(int provision, int fiber, int ore, int oil, int crystalShard, int elderTreeFlower)
    {
        this.provision = provision;
        this.fiber = fiber;
        this.ore = ore;
        this.oil = oil;
        this.crystalShard = crystalShard;
        this.elderTreeFlower = elderTreeFlower;
    }
}
