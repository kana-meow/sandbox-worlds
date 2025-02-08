using UnityEngine;

public static class WorldData {
    public const int OCTREE_CHUNK_SIZE = 32;
    public const int MIN_VOXELS_PER_NODE = 4; // has to be divisible by 2, can't be below 4
}

public struct VoxelData {
    public Vector3Int position;
    public int blockID;
}