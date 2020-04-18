﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SpellforceDataEditor.SFMap
{
    public class SFMapOcean
    {
        public SFMap map;
        static SF3D.SFModel3D ocean_mesh = null;
        SF3D.SceneSynchro.SceneNodeSimple ocean_obj = null;
        public SFMapOcean()
        {
            // generate selection 3d model
            ocean_mesh = new SF3D.SFModel3D();

            Vector3[] vertices = new Vector3[4];
            Vector2[] uvs = new Vector2[4];
            Vector4[] colors = new Vector4[4];
            Vector3[] normals = new Vector3[4];

            vertices[0] = new Vector3(-256, 0f, -256);
            vertices[1] = new Vector3(256, 0f, -256);
            vertices[2] = new Vector3(-256, 0f, 256);
            vertices[3] = new Vector3(256, 0f, 256);
            uvs[0] = new Vector2(-SFMapHeightMapGeometryPool.CHUNK_SIZE, -SFMapHeightMapGeometryPool.CHUNK_SIZE);
            uvs[1] = new Vector2(SFMapHeightMapGeometryPool.CHUNK_SIZE, -SFMapHeightMapGeometryPool.CHUNK_SIZE);
            uvs[2] = new Vector2(-SFMapHeightMapGeometryPool.CHUNK_SIZE, SFMapHeightMapGeometryPool.CHUNK_SIZE);
            uvs[3] = new Vector2(SFMapHeightMapGeometryPool.CHUNK_SIZE, SFMapHeightMapGeometryPool.CHUNK_SIZE);
            for (int i = 0; i < 4; i++)
            {
                colors[i] = new Vector4(0.2f, 0.7f, 0.9f, 0.7f);
                normals[i] = new Vector3(0.0f, 1.0f, 0.0f);
            }

            uint[] indices = { 0, 1, 2, 1, 3, 2 };

            SF3D.SFMaterial material = new SF3D.SFMaterial();
            material.indexStart = (uint)0;
            material.indexCount = (uint)6;
            material.casts_shadow = false;

            string tex_name = "test_ocean_relief_4_l8";
            SF3D.SFTexture tex = null;
            int tex_code = SFResources.SFResourceManager.Textures.Load(tex_name);
            if ((tex_code != 0) && (tex_code != -1))
                LogUtils.Log.Warning(LogUtils.LogSource.SF3D, "SFMapOcean(): Could not load texture (texture name = " + tex_name + ")");
            else
            {
                tex = SFResources.SFResourceManager.Textures.Get(tex_name);
                tex.FreeMemory();
            }
            material.texture = tex;

            SF3D.SFSubModel3D sbm1 = new SF3D.SFSubModel3D();
            sbm1.CreateRaw(vertices, uvs, colors, normals, indices, material);

            ocean_mesh.CreateRaw(new SF3D.SFSubModel3D[] { sbm1 });
            SFResources.SFResourceManager.Models.AddManually(ocean_mesh, "_OCEAN_");
        }

        public void CreateOceanObject()
        {
            ocean_obj = SF3D.SFRender.SFRenderEngine.scene.AddSceneNodeSimple(SF3D.SFRender.SFRenderEngine.scene.root, "_OCEAN_", "_OCEAN_");
            ocean_obj.Rotation = Quaternion.FromEulerAngles(0, (float)Math.PI / 2, 0);
        }

        public void SetPosition(Vector3 center_pos)
        {
            float _x = ((int)(center_pos.X / 16)) * 16;
            float _z = ((int)(center_pos.Z / 16)) * 16;
            ocean_obj.Position = new Vector3(_x, 3, _z);
        }

        public void Dispose()
        {
            LogUtils.Log.Info(LogUtils.LogSource.SFMap, "SFMapOcean.Dispose() called");
            SF3D.SFRender.SFRenderEngine.scene.RemoveSceneNode(SF3D.SFRender.SFRenderEngine.scene.root.FindNode<SF3D.SceneSynchro.SceneNodeSimple>("_OCEAN_"));
            ocean_obj = null;
        }
    }
}