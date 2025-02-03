using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Base.Component {

    public class Body : BaseComponent {
        public Geometry geometry;

        public override void OnInitialize() {
            // temp code
            string path = $"{Application.streamingAssetsPath}/resource_packs/default/geometry/passive_animal.geo.json";
            Utils.JsonDeserializer.TryParseJson<Geometry>(path, out geometry);

            /* debug
            Debug.Log("ID: " + geometry.Identifier);
            Debug.Log("Tex Width: " + geometry.Description.TextureWidth);
            Debug.Log("Tex Height: " + geometry.Description.TextureHeight);
            foreach (var bone in geometry.Bones) {
                Debug.Log(bone.Name + ":");
                Debug.Log("\tBone Parent: " + bone.Parent);
                Debug.Log("\tBone Pivot: " + bone.Pivot);
                foreach (var cube in bone.Cubes) {
                    Debug.Log("\t\tCube origin:" + cube.Origin);
                    Debug.Log("\t\tCube size:" + cube.Size);
                    Debug.Log("\t\tCube uv:" + cube.UV);
                }
            }*/
            ConstructMesh();
        }

        private void ConstructMesh() {
            Dictionary<string, GameObject> bones = new();
            foreach (var bone in geometry.Bones) {
                GameObject boneObject = new(bone.Name);
                bones.Add(bone.Name, boneObject);
                boneObject.transform.SetParent(bone.Parent != null ? bones[bone.Parent].transform : gameObject.transform);
                boneObject.transform.localPosition = bone.Pivot;

                if (bone.Cubes != null)
                    foreach (var cube in bone.Cubes) {
                        // skinned mesh later
                        GameObject meshObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        MeshFilter meshFilter = meshObject.GetComponent<MeshFilter>();
                        MeshRenderer meshRenderer = meshObject.GetComponent<MeshRenderer>();

                        Destroy(meshObject.GetComponent<Collider>());

                        meshObject.transform.SetParent(boneObject.transform);
                        meshObject.transform.localPosition = cube.Origin - bone.Pivot;
                        meshObject.transform.localScale = cube.Size;

                        meshRenderer.material.mainTexture = LoadTexture($"{Application.streamingAssetsPath}/resource_packs/default/textures/passive_animal.png");
                    }
            }
        }

        private Texture2D LoadTexture(string path) {
            if (!File.Exists(path)) {
                Debug.LogError("File not found: " + path);
                return null;
            }

            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(fileData)) // LoadImage auto-resizes the texture
            {
                return texture;
            }

            return null;
        }
    }
}

namespace Base {

    public class Geometry {

        [JsonProperty("identifier"), JsonRequired]
        public string Identifier { get; }

        [JsonProperty("description"), JsonRequired]
        public GeoDescription Description { get; }

        [JsonProperty("bones"), JsonRequired]
        public IReadOnlyList<Bone> Bones { get; }

        [JsonConstructor]
        public Geometry(string identifier, GeoDescription description, IReadOnlyList<Bone> bones) {
            Identifier = identifier;
            Description = description;
            Bones = bones;
        }

        public class GeoDescription {

            [JsonProperty("texture_width"), JsonRequired]
            public int TextureWidth { get; }

            [JsonProperty("texture_height"), JsonRequired]
            public int TextureHeight { get; }

            public GeoDescription(int textureWidth, int textureHeight) {
                TextureWidth = textureWidth;
                TextureHeight = textureHeight;
            }
        }

        public class Bone {

            [JsonProperty("name"), JsonRequired]
            public string Name { get; }

#nullable enable

            [JsonProperty("parent")]
            public string? Parent { get; }

#nullable disable

            [JsonProperty("pivot"), JsonRequired]
            public Vector3 Pivot { get; }

            [JsonProperty("cubes")]
            public IReadOnlyList<Cube> Cubes { get; }

            public Bone(string name, string parent, Vector3 pivot, IReadOnlyList<Cube> cubes) {
                Name = name;
                Parent = parent;
                Pivot = pivot;
                Cubes = cubes;
            }
        }

        public class Cube {

            [JsonProperty("origin"), JsonRequired]
            public Vector3 Origin { get; }

            [JsonProperty("size"), JsonRequired]
            public Vector3 Size { get; }

            //[JsonProperty("uv"), JsonRequired]
            //public Vector2 UV { get; }

            public Cube(Vector3 origin, Vector3 size /*,Vector2 uV*/) {
                Origin = origin;
                Size = size;
                //UV = uV;
            }
        }
    }
}