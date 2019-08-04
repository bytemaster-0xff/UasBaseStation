// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

#pragma once

#include <dxgiformat.h>
#include <d3d11.h>
#include <d3d11_1.h>
#include <d3dcompiler.h>
#include <directxmath.h>
#include <vector>
#include <map>
#include <string>

namespace VSD3DStarter
{

/*

The header contains starter classes that can be used to load and render 3D assets. Assets include
textures (png, bmp, etc.), effects (dgsl) and meshes (fbx). 

Textures should be converted into .dds files
Effects should be converted into .cso files
Meshes should be converted into .cmo files


======================================================================
Structures for Constant Buffer Data
======================================================================

struct MaterialConstants
{
    DirectX::XMFLOAT4   Ambient;
    DirectX::XMFLOAT4   Diffuse;
    DirectX::XMFLOAT4   Specular;
    DirectX::XMFLOAT4   Emissive;
    float               SpecularPower;
    float               Padding0;
    float               Padding1;
    float               Padding2;
};

struct LightConstants
{
    DirectX::XMFLOAT4   Ambient;
    DirectX::XMFLOAT4   LightColor[4];
    DirectX::XMFLOAT4   LightAttenuation[4];
    DirectX::XMFLOAT4   LightDirection[4];
    DirectX::XMFLOAT4   LightSpecularIntensity[4];
    UINT                IsPointLight[4*4];
    UINT                ActiveLights;
    float               Padding0;
    float               Padding1;
    float               Padding2;
};

struct ObjectConstants
{
    DirectX::XMMATRIX   LocalToWorld4x4;
    DirectX::XMMATRIX   LocalToProjected4x4;
    DirectX::XMMATRIX   WorldToLocal4x4;
    DirectX::XMMATRIX   WorldToView4x4;
    DirectX::XMMATRIX   UvTransform4x4;
    DirectX::XMFLOAT3   EyePosition;
    float               Padding0;
};

struct MiscConstants
{
    float ViewportWidth;
    float ViewportHeight;
    float Time;
    float Padding1;
};

struct Vertex
{
    float x, y, z;
    float nx, ny, nz;
    float tx, ty, tz, tw;
    UINT color;
    float u, v;
};


======================================================================
Graphics Class 
======================================================================

class Graphics
{
    //
    // initialization/shutdown
    //
    void Initialize(ID3D11Device* device, ID3D11DeviceContext* deviceContext);
    void Shutdown();

    //
    // accessors
    //
    Camera& GetCamera() const;

    ID3D11Device* GetDevice() const;
    ID3D11DeviceContext* GetDeviceContext() const;

    ID3D11Buffer* GetMaterialConstants() const;
    ID3D11Buffer* GetLightConstants() const;
    ID3D11Buffer* GetObjectConstants() const;
    ID3D11Buffer* GetMiscConstants() const;

    ID3D11SamplerState* GetSamplerState() const;
    ID3D11InputLayout* GetVertexInputLayout() const;
    ID3D11VertexShader* GetVertexShader() const;

    //
    // resource management for pixel shaders and textures
    //
    ID3D11PixelShader* GetOrCreatePixelShader(const std::wstring& shaderName);
    ID3D11ShaderResourceView* GetOrCreateTexture(const std::wstring& textureName, bool generateMipsWhenNeeded);

    //
    // methods to update constant buffers
    //
    void UpdateMaterialConstants(const MaterialConstants& data) const;
    void UpdateLightConstants(const LightConstants& data) const;
    void UpdateObjectConstants(const ObjectConstants& data) const;
    void UpdateMiscConstants(const MiscConstants& data) const;
}


======================================================================
Camera Class 
======================================================================

class Camera
{
public:
    Camera();

    const XMMATRIX& GetView() const;
    const XMMATRIX& GetProjection() const;

    const XMFLOAT3& GetPosition() const;
    const XMFLOAT3& GetLookAt() const;

    void SetProjection(float fovY, float aspect, float zn, float zf);
    void SetProjectionOrthographic(float viewWidth, float viewHeight, float zn, float zf);
    void SetProjectionOrthographicOffCenter(float viewLeft, float viewRight, float viewBottom, float viewTop, float zn, float zf);
    void SetPosition(const XMFLOAT3& position);
    void SetLookAt(const XMFLOAT3& lookAt);
}


======================================================================
Mesh Class 
======================================================================

class Mesh
{
    static const UINT MaxTextures = 8;

    struct SubMesh
    {
        UINT MaterialIndex;
        UINT IndexBufferIndex;
        UINT VertexBufferIndex;
        UINT StartIndex;
        UINT PrimCount;
    };

    struct Material
    {
        std::wstring Name;
        float Ambient[4];
        float Diffuse[4];
        float Specular[4];
        float Emissive[4];
        float SpecularPower;

        ID3D11ShaderResourceView* Textures[MaxTextures];
        ID3D11VertexShader* VertexShader;
        ID3D11PixelShader* PixelShader;
        ID3D11SamplerState* SamplerState;
    };

    struct MeshExtents
    {
        float CenterX, CenterY, CenterZ;
        float Radius;

        float MinX, MinY, MinZ;
        float MaxX, MaxY, MaxZ;
    };

    //
    // access to mesh data
    //
    const std::vector<SubMesh>& SubMeshes() const;
    const std::vector<Material>& Materials() const;
    const std::vector<ID3D11Buffer*>& VertexBuffers();
    const std::vector<ID3D11Buffer*>& IndexBuffers() const;
    const MeshExtents& MeshExtents() const;

    //
    // render the mesh to the current render target
    //
    void Render(const Graphics& graphics, const DirectX::XMMATRIX& world);

    //
    // loads a scene from the specified file, returning a vector of mesh objects
    //
    static void LoadFromFile(
        Graphics& graphics, 
        const std::wstring& meshFilename, 
        const std::wstring& shaderPathLocation,
        const std::wstring& texturePathLocation,
        std::vector<Mesh*>& loadedMeshes
        );
}

*/

///////////////////////////////////////////////////////////////////////////////////////////
//
// simple COM helper template functions for safe AddRef() and Release() of IUnknown objects
//
template <class T> inline LONG SafeAddRef(T* pUnk) { ULONG lr = 0; if (pUnk != nullptr) { lr = pUnk->AddRef(); } return lr; }
template <class T> inline LONG SafeRelease(T*& pUnk) { ULONG lr = 0; if (pUnk != nullptr) { lr = pUnk->Release(); pUnk = nullptr; } return lr; }
//
//
///////////////////////////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////////////////////
//
// simple COM helper template functions for safe AddRef() and Release() of IUnknown objects
//

const BYTE VSD3DStarter_VS[] =
{
     68,  88,  66,  67,  60, 148, 
      1, 119,  96, 132,  12,  40, 
      4, 175, 115, 234, 221,  12, 
    216, 113,   1,   0,   0,   0, 
     48,   8,   0,   0,   5,   0, 
      0,   0,  52,   0,   0,   0, 
     20,   3,   0,   0, 196,   3, 
      0,   0, 176,   4,   0,   0, 
    180,   7,   0,   0,  82,  68, 
     69,  70, 216,   2,   0,   0, 
      2,   0,   0,   0, 116,   0, 
      0,   0,   2,   0,   0,   0, 
     28,   0,   0,   0,   0,   4, 
    254, 255,   0,   1,   0,   0, 
    164,   2,   0,   0,  92,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   1,   0, 
      0,   0,   1,   0,   0,   0, 
    105,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   2,   0,   0,   0, 
      1,   0,   0,   0,   1,   0, 
      0,   0,  77,  97, 116, 101, 
    114, 105,  97, 108,  86,  97, 
    114, 115,   0,  79,  98, 106, 
    101,  99, 116,  86,  97, 114, 
    115,   0,  92,   0,   0,   0, 
      5,   0,   0,   0, 164,   0, 
      0,   0,  80,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0, 105,   0,   0,   0, 
      6,   0,   0,   0, 148,   1, 
      0,   0,  80,   1,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,  28,   1,   0,   0, 
      0,   0,   0,   0,  16,   0, 
      0,   0,   0,   0,   0,   0, 
     44,   1,   0,   0,   0,   0, 
      0,   0,  60,   1,   0,   0, 
     16,   0,   0,   0,  16,   0, 
      0,   0,   2,   0,   0,   0, 
     44,   1,   0,   0,   0,   0, 
      0,   0,  76,   1,   0,   0, 
     32,   0,   0,   0,  16,   0, 
      0,   0,   0,   0,   0,   0, 
     44,   1,   0,   0,   0,   0, 
      0,   0,  93,   1,   0,   0, 
     48,   0,   0,   0,  16,   0, 
      0,   0,   0,   0,   0,   0, 
     44,   1,   0,   0,   0,   0, 
      0,   0, 110,   1,   0,   0, 
     64,   0,   0,   0,   4,   0, 
      0,   0,   0,   0,   0,   0, 
    132,   1,   0,   0,   0,   0, 
      0,   0,  77,  97, 116, 101, 
    114, 105,  97, 108,  65, 109, 
     98, 105, 101, 110, 116,   0, 
      1,   0,   3,   0,   1,   0, 
      4,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,  77,  97, 
    116, 101, 114, 105,  97, 108, 
     68, 105, 102, 102, 117, 115, 
    101,   0,  77,  97, 116, 101, 
    114, 105,  97, 108,  83, 112, 
    101,  99, 117, 108,  97, 114, 
      0,  77,  97, 116, 101, 114, 
    105,  97, 108,  69, 109, 105, 
    115, 115, 105, 118, 101,   0, 
     77,  97, 116, 101, 114, 105, 
     97, 108,  83, 112, 101,  99, 
    117, 108,  97, 114,  80, 111, 
    119, 101, 114,   0,   0,   0, 
      3,   0,   1,   0,   1,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,  36,   2,   0,   0, 
      0,   0,   0,   0,  64,   0, 
      0,   0,   2,   0,   0,   0, 
     52,   2,   0,   0,   0,   0, 
      0,   0,  68,   2,   0,   0, 
     64,   0,   0,   0,  64,   0, 
      0,   0,   2,   0,   0,   0, 
     52,   2,   0,   0,   0,   0, 
      0,   0,  88,   2,   0,   0, 
    128,   0,   0,   0,  64,   0, 
      0,   0,   0,   0,   0,   0, 
     52,   2,   0,   0,   0,   0, 
      0,   0, 104,   2,   0,   0, 
    192,   0,   0,   0,  64,   0, 
      0,   0,   0,   0,   0,   0, 
     52,   2,   0,   0,   0,   0, 
      0,   0, 119,   2,   0,   0, 
      0,   1,   0,   0,  64,   0, 
      0,   0,   2,   0,   0,   0, 
     52,   2,   0,   0,   0,   0, 
      0,   0, 134,   2,   0,   0, 
     64,   1,   0,   0,  12,   0, 
      0,   0,   2,   0,   0,   0, 
    148,   2,   0,   0,   0,   0, 
      0,   0,  76, 111,  99,  97, 
    108,  84, 111,  87, 111, 114, 
    108, 100,  52, 120,  52,   0, 
      3,   0,   3,   0,   4,   0, 
      4,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,  76, 111, 
     99,  97, 108,  84, 111,  80, 
    114, 111, 106, 101,  99, 116, 
    101, 100,  52, 120,  52,   0, 
     87, 111, 114, 108, 100,  84, 
    111,  76, 111,  99,  97, 108, 
     52, 120,  52,   0,  87, 111, 
    114, 108, 100,  84, 111,  86, 
    105, 101, 119,  52, 120,  52, 
      0,  85,  86,  84, 114,  97, 
    110, 115, 102, 111, 114, 109, 
     52, 120,  52,   0,  69, 121, 
    101,  80, 111, 115, 105, 116, 
    105, 111, 110,   0, 171, 171, 
      1,   0,   3,   0,   1,   0, 
      3,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,  77, 105, 
     99, 114, 111, 115, 111, 102, 
    116,  32,  40,  82,  41,  32, 
     72,  76,  83,  76,  32,  83, 
    104,  97, 100, 101, 114,  32, 
     67, 111, 109, 112, 105, 108, 
    101, 114,  32,  57,  46,  51, 
     48,  46,  57,  54,  48,  46, 
     56,  51,  48,  56,   0, 171, 
    171, 171,  73,  83,  71,  78, 
    168,   0,   0,   0,   5,   0, 
      0,   0,   8,   0,   0,   0, 
    128,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      3,   0,   0,   0,   0,   0, 
      0,   0,  15,  15,   0,   0, 
    137,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      3,   0,   0,   0,   1,   0, 
      0,   0,   7,   7,   0,   0, 
    144,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      3,   0,   0,   0,   2,   0, 
      0,   0,  15,  15,   0,   0, 
    152,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      3,   0,   0,   0,   3,   0, 
      0,   0,  15,  15,   0,   0, 
    158,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      3,   0,   0,   0,   4,   0, 
      0,   0,   3,   3,   0,   0, 
     80,  79,  83,  73,  84,  73, 
     79,  78,   0,  78,  79,  82, 
     77,  65,  76,   0,  84,  65, 
     78,  71,  69,  78,  84,   0, 
     67,  79,  76,  79,  82,   0, 
     84,  69,  88,  67,  79,  79, 
     82,  68,   0, 171,  79,  83, 
     71,  78, 228,   0,   0,   0, 
      8,   0,   0,   0,   8,   0, 
      0,   0, 200,   0,   0,   0, 
      0,   0,   0,   0,   1,   0, 
      0,   0,   3,   0,   0,   0, 
      0,   0,   0,   0,  15,   0, 
      0,   0, 212,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   3,   0,   0,   0, 
      1,   0,   0,   0,  15,   0, 
      0,   0, 218,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   3,   0,   0,   0, 
      2,   0,   0,   0,   3,  12, 
      0,   0, 218,   0,   0,   0, 
      1,   0,   0,   0,   0,   0, 
      0,   0,   3,   0,   0,   0, 
      3,   0,   0,   0,   7,   8, 
      0,   0, 218,   0,   0,   0, 
      2,   0,   0,   0,   0,   0, 
      0,   0,   3,   0,   0,   0, 
      4,   0,   0,   0,   7,   8, 
      0,   0, 218,   0,   0,   0, 
      3,   0,   0,   0,   0,   0, 
      0,   0,   3,   0,   0,   0, 
      5,   0,   0,   0,   7,   8, 
      0,   0, 218,   0,   0,   0, 
      4,   0,   0,   0,   0,   0, 
      0,   0,   3,   0,   0,   0, 
      6,   0,   0,   0,  15,   0, 
      0,   0, 218,   0,   0,   0, 
      5,   0,   0,   0,   0,   0, 
      0,   0,   3,   0,   0,   0, 
      7,   0,   0,   0,   7,   8, 
      0,   0,  83,  86,  95,  80, 
     79,  83,  73,  84,  73,  79, 
     78,   0,  67,  79,  76,  79, 
     82,   0,  84,  69,  88,  67, 
     79,  79,  82,  68,   0, 171, 
     83,  72,  68,  82, 252,   2, 
      0,   0,  64,   0,   1,   0, 
    191,   0,   0,   0,  89,   0, 
      0,   4,  70, 142,  32,   0, 
      0,   0,   0,   0,   2,   0, 
      0,   0,  89,   0,   0,   4, 
     70, 142,  32,   0,   2,   0, 
      0,   0,  21,   0,   0,   0, 
     95,   0,   0,   3, 242,  16, 
     16,   0,   0,   0,   0,   0, 
     95,   0,   0,   3, 114,  16, 
     16,   0,   1,   0,   0,   0, 
     95,   0,   0,   3, 242,  16, 
     16,   0,   2,   0,   0,   0, 
     95,   0,   0,   3, 242,  16, 
     16,   0,   3,   0,   0,   0, 
     95,   0,   0,   3,  50,  16, 
     16,   0,   4,   0,   0,   0, 
    103,   0,   0,   4, 242,  32, 
     16,   0,   0,   0,   0,   0, 
      1,   0,   0,   0, 101,   0, 
      0,   3, 242,  32,  16,   0, 
      1,   0,   0,   0, 101,   0, 
      0,   3,  50,  32,  16,   0, 
      2,   0,   0,   0, 101,   0, 
      0,   3, 114,  32,  16,   0, 
      3,   0,   0,   0, 101,   0, 
      0,   3, 114,  32,  16,   0, 
      4,   0,   0,   0, 101,   0, 
      0,   3, 114,  32,  16,   0, 
      5,   0,   0,   0, 101,   0, 
      0,   3, 242,  32,  16,   0, 
      6,   0,   0,   0, 101,   0, 
      0,   3, 114,  32,  16,   0, 
      7,   0,   0,   0, 104,   0, 
      0,   2,   1,   0,   0,   0, 
     17,   0,   0,   8,  18,  32, 
     16,   0,   0,   0,   0,   0, 
     70,  30,  16,   0,   0,   0, 
      0,   0,  70, 142,  32,   0, 
      2,   0,   0,   0,   4,   0, 
      0,   0,  17,   0,   0,   8, 
     34,  32,  16,   0,   0,   0, 
      0,   0,  70,  30,  16,   0, 
      0,   0,   0,   0,  70, 142, 
     32,   0,   2,   0,   0,   0, 
      5,   0,   0,   0,  17,   0, 
      0,   8,  66,  32,  16,   0, 
      0,   0,   0,   0,  70,  30, 
     16,   0,   0,   0,   0,   0, 
     70, 142,  32,   0,   2,   0, 
      0,   0,   6,   0,   0,   0, 
     17,   0,   0,   8, 130,  32, 
     16,   0,   0,   0,   0,   0, 
     70,  30,  16,   0,   0,   0, 
      0,   0,  70, 142,  32,   0, 
      2,   0,   0,   0,   7,   0, 
      0,   0,  56,   0,   0,   8, 
    242,  32,  16,   0,   1,   0, 
      0,   0,  70,  30,  16,   0, 
      3,   0,   0,   0,  70, 142, 
     32,   0,   0,   0,   0,   0, 
      1,   0,   0,   0,  54,   0, 
      0,   5,  50,   0,  16,   0, 
      0,   0,   0,   0,  70,  16, 
     16,   0,   4,   0,   0,   0, 
     54,   0,   0,   5,  66,   0, 
     16,   0,   0,   0,   0,   0, 
      1,  64,   0,   0,   0,   0, 
    128,  63,  16,   0,   0,   8, 
     18,  32,  16,   0,   2,   0, 
      0,   0,  70,   2,  16,   0, 
      0,   0,   0,   0,  70, 131, 
     32,   0,   2,   0,   0,   0, 
     16,   0,   0,   0,  16,   0, 
      0,   8,  34,  32,  16,   0, 
      2,   0,   0,   0,  70,   2, 
     16,   0,   0,   0,   0,   0, 
     70, 131,  32,   0,   2,   0, 
      0,   0,  17,   0,   0,   0, 
     16,   0,   0,   8,  18,  32, 
     16,   0,   3,   0,   0,   0, 
     70,  18,  16,   0,   1,   0, 
      0,   0,  70, 130,  32,   0, 
      2,   0,   0,   0,   0,   0, 
      0,   0,  16,   0,   0,   8, 
     34,  32,  16,   0,   3,   0, 
      0,   0,  70,  18,  16,   0, 
      1,   0,   0,   0,  70, 130, 
     32,   0,   2,   0,   0,   0, 
      1,   0,   0,   0,  16,   0, 
      0,   8,  66,  32,  16,   0, 
      3,   0,   0,   0,  70,  18, 
     16,   0,   1,   0,   0,   0, 
     70, 130,  32,   0,   2,   0, 
      0,   0,   2,   0,   0,   0, 
     17,   0,   0,   8,  18,   0, 
     16,   0,   0,   0,   0,   0, 
     70,  30,  16,   0,   0,   0, 
      0,   0,  70, 142,  32,   0, 
      2,   0,   0,   0,   0,   0, 
      0,   0,  17,   0,   0,   8, 
     34,   0,  16,   0,   0,   0, 
      0,   0,  70,  30,  16,   0, 
      0,   0,   0,   0,  70, 142, 
     32,   0,   2,   0,   0,   0, 
      1,   0,   0,   0,  17,   0, 
      0,   8,  66,   0,  16,   0, 
      0,   0,   0,   0,  70,  30, 
     16,   0,   0,   0,   0,   0, 
     70, 142,  32,   0,   2,   0, 
      0,   0,   2,   0,   0,   0, 
     54,   0,   0,   5, 114,  32, 
     16,   0,   4,   0,   0,   0, 
     70,   2,  16,   0,   0,   0, 
      0,   0,   0,   0,   0,   9, 
    114,  32,  16,   0,   5,   0, 
      0,   0,  70,   2,  16, 128, 
     65,   0,   0,   0,   0,   0, 
      0,   0,  70, 130,  32,   0, 
      2,   0,   0,   0,  20,   0, 
      0,   0,  54,   0,   0,   5, 
    242,  32,  16,   0,   6,   0, 
      0,   0,  70,  30,  16,   0, 
      2,   0,   0,   0,  54,   0, 
      0,   5, 114,  32,  16,   0, 
      7,   0,   0,   0,  70,  18, 
     16,   0,   1,   0,   0,   0, 
     62,   0,   0,   1,  83,  84, 
     65,  84, 116,   0,   0,   0, 
     20,   0,   0,   0,   1,   0, 
      0,   0,   0,   0,   0,   0, 
     13,   0,   0,   0,  14,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   1,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   6,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0,   0,   0,   0,   0, 
      0,   0
};
//
//
///////////////////////////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////////////////////
//
// DDS structures and definitions
//
struct DDS_PIXELFORMAT
{
    DWORD dwSize;
    DWORD dwFlags;
    DWORD dwFourCC;
    DWORD dwRGBBitCount;
    DWORD dwRBitMask;
    DWORD dwGBitMask;
    DWORD dwBBitMask;
    DWORD dwABitMask;
};
struct DDS_HEADER
{
    DWORD dwSize;
    DWORD dwHeaderFlags;
    DWORD dwHeight;
    DWORD dwWidth;
    DWORD dwPitchOrLinearSize;
    DWORD dwDepth; 
    DWORD dwMipMapCount;
    DWORD dwReserved1[11];
    DDS_PIXELFORMAT ddspf;
    DWORD dwSurfaceFlags;
    DWORD dwCubemapFlags;
    DWORD dwReserved2[3];
};

struct DDS_HEADER_DXT10
{
    DXGI_FORMAT dxgiFormat;
    D3D11_RESOURCE_DIMENSION resourceDimension;
    UINT miscFlag;
    UINT arraySize;
    UINT reserved;
};

#ifndef MAKEFOURCC
    #define MAKEFOURCC(ch0, ch1, ch2, ch3)                              \
                ((DWORD)(BYTE)(ch0) | ((DWORD)(BYTE)(ch1) << 8) |       \
                ((DWORD)(BYTE)(ch2) << 16) | ((DWORD)(BYTE)(ch3) << 24 ))
#endif /* defined(MAKEFOURCC) */

#define DDS_FOURCC      0x00000004  // DDPF_FOURCC
#define DDS_RGB         0x00000040  // DDPF_RGB
#define DDS_LUMINANCE   0x00020000  // DDPF_LUMINANCE
#define DDS_ALPHA       0x00000002  // DDPF_ALPHA

#define DDS_MAGIC 0x20534444 // "DDS "

#define DDS_HEADER_FLAGS_TEXTURE        0x00001007  // DDSD_CAPS | DDSD_HEIGHT | DDSD_WIDTH | DDSD_PIXELFORMAT 
#define DDS_HEADER_FLAGS_MIPMAP         0x00020000  // DDSD_MIPMAPCOUNT
#define DDS_HEADER_FLAGS_VOLUME         0x00800000  // DDSD_DEPTH
#define DDS_HEADER_FLAGS_PITCH          0x00000008  // DDSD_PITCH
#define DDS_HEADER_FLAGS_LINEARSIZE     0x00080000  // DDSD_LINEARSIZE

#define DDS_HEIGHT 0x00000002 // DDSD_HEIGHT
#define DDS_WIDTH  0x00000004 // DDSD_WIDTH

#define DDS_CUBEMAP_POSITIVEX 0x00000600 // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_POSITIVEX
#define DDS_CUBEMAP_NEGATIVEX 0x00000a00 // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_NEGATIVEX
#define DDS_CUBEMAP_POSITIVEY 0x00001200 // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_POSITIVEY
#define DDS_CUBEMAP_NEGATIVEY 0x00002200 // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_NEGATIVEY
#define DDS_CUBEMAP_POSITIVEZ 0x00004200 // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_POSITIVEZ
#define DDS_CUBEMAP_NEGATIVEZ 0x00008200 // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_NEGATIVEZ

#define DDS_CUBEMAP_ALLFACES ( DDS_CUBEMAP_POSITIVEX | DDS_CUBEMAP_NEGATIVEX |\
                               DDS_CUBEMAP_POSITIVEY | DDS_CUBEMAP_NEGATIVEY |\
                               DDS_CUBEMAP_POSITIVEZ | DDS_CUBEMAP_NEGATIVEZ )

#define DDS_CUBEMAP 0x00000200 // DDSCAPS2_CUBEMAP
//
//
///////////////////////////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////////////////////
//
// Constant buffer structures
//
// These structs use padding and different data types in places to adhere
// to the shader constant's alignment.
//
struct MaterialConstants
{
    MaterialConstants()
    {
        Ambient = DirectX::XMFLOAT4(0.0f,0.0f,0.0f,1.0f);
        Diffuse = DirectX::XMFLOAT4(1.0f,1.0f,1.0f,1.0f);
        Specular = DirectX::XMFLOAT4(0.0f, 0.0f, 0.0f, 0.0f);
        Emissive = DirectX::XMFLOAT4(0.0f, 0.0f, 0.0f, 0.0f);
        SpecularPower = 1.0f;
        Padding0 = 0.0f;
        Padding1 = 0.0f;
        Padding2 = 0.0f;
    }

    DirectX::XMFLOAT4   Ambient;
    DirectX::XMFLOAT4   Diffuse;
    DirectX::XMFLOAT4   Specular;
    DirectX::XMFLOAT4   Emissive;
    float               SpecularPower;
    float               Padding0;
    float               Padding1;
    float               Padding2;
};

struct LightConstants
{
    LightConstants()
    {
        ZeroMemory(this, sizeof(LightConstants));
        Ambient = DirectX::XMFLOAT4(1.0f,1.0f,1.0f,1.0f);
    }

    DirectX::XMFLOAT4   Ambient;
    DirectX::XMFLOAT4   LightColor[4];
    DirectX::XMFLOAT4   LightAttenuation[4];
    DirectX::XMFLOAT4   LightDirection[4];
    DirectX::XMFLOAT4   LightSpecularIntensity[4];
    UINT                IsPointLight[4*4];
    UINT                ActiveLights;
    float               Padding0;
    float               Padding1;
    float               Padding2;
};

struct ObjectConstants
{
    ObjectConstants()
    {
        ZeroMemory(this, sizeof(ObjectConstants));
    }

    DirectX::XMMATRIX   LocalToWorld4x4;
    DirectX::XMMATRIX   LocalToProjected4x4;
    DirectX::XMMATRIX   WorldToLocal4x4;
    DirectX::XMMATRIX   WorldToView4x4;
    DirectX::XMMATRIX   UvTransform4x4;
    DirectX::XMFLOAT3   EyePosition;
    float               Padding0;
};

struct MiscConstants
{
    MiscConstants()
    {
        ViewportWidth = 1.0f;
        ViewportHeight = 1.0f;
        Time = 0.0f;
        Padding1 = 0.0f;
    }

    float ViewportWidth;
    float ViewportHeight;
    float Time;
    float Padding1;
};

struct Vertex
{
    float x, y, z;
    float nx, ny, nz;
    float tx, ty, tz, tw;
    UINT color;
    float u, v;
};

#define NUM_BONE_INFLUENCES 4
struct SkinningVertex
{
    UINT boneIndex[NUM_BONE_INFLUENCES];
    float boneWeight[NUM_BONE_INFLUENCES];
};

struct SkinningVertexInput
{
    byte boneIndex[NUM_BONE_INFLUENCES];
    float boneWeight[NUM_BONE_INFLUENCES];
};

//
//
///////////////////////////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////////////////////
//
// Camera class provides simple camera functionality.
//
class Camera
{
public:
    Camera()
    {
        DirectX::XMMATRIX identity = DirectX::XMMatrixIdentity();
        DirectX::XMStoreFloat4x4(&m_view, identity);
        DirectX::XMStoreFloat4x4(&m_proj, identity);
        
        m_viewWidth = 1;
        m_viewHeight = 1;

        m_position = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);
        m_lookAt   = DirectX::XMFLOAT3(0.0f, 0.0f, 1.0f);
        m_up       = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
    }

    const DirectX::XMMATRIX GetView() const  { return XMLoadFloat4x4(&m_view); }
    const DirectX::XMMATRIX GetProjection() const { return XMLoadFloat4x4(&m_proj); }
    const DirectX::XMMATRIX GetOrientationMatrix() const { return XMLoadFloat4x4(&m_orientationMatrix); }
    const DirectX::XMFLOAT3& GetPosition() const { return m_position; }
    const DirectX::XMFLOAT3& GetLookAt() const { return m_lookAt; }
    const DirectX::XMFLOAT3& GetUpVector() const { return m_up; }

    void SetViewport(UINT w, UINT h)
    {
        m_viewWidth = w;
        m_viewHeight = h;
    }

    void SetProjection(float fovY, float aspect, float zn, float zf)
    {
        DirectX::XMMATRIX p = DirectX::XMMatrixPerspectiveFovRH(fovY, aspect, zn, zf);
        XMStoreFloat4x4(&m_proj, p);
    }

    void SetProjectionOrthographic(float viewWidth, float viewHeight, float zn, float zf)
    {
        DirectX::XMMATRIX p = DirectX::XMMatrixOrthographicRH(viewWidth, viewHeight, zn, zf);
        XMStoreFloat4x4(&m_proj, p);
    }

    void SetProjectionOrthographicOffCenter(float viewLeft, float viewRight, float viewBottom, float viewTop, float zn, float zf)
    {
        DirectX::XMMATRIX p = DirectX::XMMatrixOrthographicOffCenterRH(viewLeft, viewRight, viewBottom, viewTop, zn, zf);
        XMStoreFloat4x4(&m_proj, p);
    }

    void SetPosition(const DirectX::XMFLOAT3& position)
    {
        m_position = position;
        this->UpdateView();
    }

    void SetLookAt(const DirectX::XMFLOAT3& lookAt)
    {
        m_lookAt = lookAt;
        this->UpdateView();
    }

    void SetUpVector(const DirectX::XMFLOAT3& up)
    {
        m_up = up;
        this->UpdateView();
    }

	
    void SetOrientationMatrix(const DirectX::XMFLOAT4X4& orientationMatrix)
    {
        m_orientationMatrix = orientationMatrix;
    }

    void GetWorldLine(UINT pixelX, UINT pixelY, DirectX::XMFLOAT3* outPoint, DirectX::XMFLOAT3* outDir)
    {
        DirectX::XMFLOAT4 p0 = DirectX::XMFLOAT4((float)pixelX, (float)pixelY, 0, 1);
        DirectX::XMFLOAT4 p1 = DirectX::XMFLOAT4((float)pixelX, (float)pixelY, 1, 1);

        DirectX::XMVECTOR screen0 =  DirectX::XMLoadFloat4(&p0);
        DirectX::XMVECTOR screen1 = DirectX::XMLoadFloat4(&p1);

        DirectX::XMMATRIX projMat = XMLoadFloat4x4(&m_proj);
        DirectX::XMMATRIX viewMat = XMLoadFloat4x4(&m_view);

        DirectX::XMVECTOR pp0 = DirectX::XMVector3Unproject(screen0, 0, 0, (float)m_viewWidth, (float)m_viewHeight, 0, 1, projMat, viewMat, DirectX::XMMatrixIdentity());
        DirectX::XMVECTOR pp1 = DirectX::XMVector3Unproject(screen1, 0, 0, (float)m_viewWidth, (float)m_viewHeight, 0, 1, projMat, viewMat, DirectX::XMMatrixIdentity());

        DirectX::XMStoreFloat3(outPoint, pp0);
        DirectX::XMStoreFloat3(outDir, pp1);

        outDir->x -= outPoint->x;
        outDir->y -= outPoint->y;
        outDir->z -= outPoint->z;
    }

private:
    void UpdateView()
    {
        DirectX::XMVECTOR vPosition = DirectX::XMLoadFloat3(&m_position);
        DirectX::XMVECTOR vLook = DirectX::XMLoadFloat3(&m_lookAt);
        DirectX::XMVECTOR vUp = DirectX::XMLoadFloat3(&m_up);

        DirectX::XMMATRIX v = DirectX::XMMatrixLookAtRH(vPosition, vLook, vUp);
        DirectX::XMStoreFloat4x4(&m_view, v);
    }

    DirectX::XMFLOAT4X4 m_view;
    DirectX::XMFLOAT4X4 m_proj;
    DirectX::XMFLOAT3 m_position;
	DirectX::XMFLOAT4X4 m_orientationMatrix;
    DirectX::XMFLOAT3 m_lookAt;
    DirectX::XMFLOAT3 m_up;
    UINT m_viewWidth;
    UINT m_viewHeight;
};
//
//
///////////////////////////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////////////////////
//
// Graphics wraps D3D engine and related constant buffers
//
class Graphics
{
public:
    //
    // construction/destruction
    //
    Graphics()
    {
    }

    ~Graphics()
    {
        this->Shutdown(); 
    }

    //
    // initialization/shutdown
    //
    void Initialize(ID3D11Device* device, ID3D11DeviceContext* deviceContext, D3D_FEATURE_LEVEL deviceFeatureLevel)
    {
        //
        // make sure shutdown first
        //
        this->Shutdown();

        //
        // remember the device interfaces
        //
        m_device = device;
        m_deviceContext = deviceContext;
        
        //
        // create constant buffers
        //
        D3D11_BUFFER_DESC bufferDesc;
        bufferDesc.Usage = D3D11_USAGE_DEFAULT;
        bufferDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
        bufferDesc.CPUAccessFlags = 0;
        bufferDesc.MiscFlags = 0;
        bufferDesc.StructureByteStride = 0;

        bufferDesc.ByteWidth = sizeof(MaterialConstants);
        m_device->CreateBuffer(&bufferDesc, nullptr, &m_materialConstants);

        bufferDesc.ByteWidth = sizeof(LightConstants);
        m_device->CreateBuffer(&bufferDesc, nullptr, &m_lightConstants);

        bufferDesc.ByteWidth = sizeof(ObjectConstants);
        m_device->CreateBuffer(&bufferDesc, nullptr, &m_objectConstants);

        bufferDesc.ByteWidth = sizeof(MiscConstants);
        m_device->CreateBuffer(&bufferDesc, nullptr, &m_miscConstants);

        //
        // create sampler state
        //
        D3D11_SAMPLER_DESC samplerDesc;
        samplerDesc.Filter = D3D11_FILTER_ANISOTROPIC;
        samplerDesc.AddressU = D3D11_TEXTURE_ADDRESS_WRAP;
        samplerDesc.AddressV = D3D11_TEXTURE_ADDRESS_WRAP;
        samplerDesc.AddressW = D3D11_TEXTURE_ADDRESS_WRAP;
        samplerDesc.MipLODBias = 0.0f;
        samplerDesc.MaxAnisotropy = 4;
        samplerDesc.ComparisonFunc = D3D11_COMPARISON_NEVER;
        samplerDesc.BorderColor[0] = 0.0f;
        samplerDesc.BorderColor[1] = 0.0f;
        samplerDesc.BorderColor[2] = 0.0f;
        samplerDesc.BorderColor[3] = 0.0f;
        samplerDesc.MinLOD = 0;
        samplerDesc.MaxLOD = D3D11_FLOAT32_MAX;
        m_device->CreateSamplerState(&samplerDesc, &m_sampler);

        //
        // create the vertex layout
        //
        D3D11_INPUT_ELEMENT_DESC layout[] =
        {
            { "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_PER_VERTEX_DATA, 0 },
            { "NORMAL", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 12, D3D11_INPUT_PER_VERTEX_DATA, 0 },
            { "TANGENT", 0, DXGI_FORMAT_R32G32B32A32_FLOAT, 0, 24, D3D11_INPUT_PER_VERTEX_DATA, 0 },
            { "COLOR", 0, DXGI_FORMAT_B8G8R8A8_UNORM, 0, 40, D3D11_INPUT_PER_VERTEX_DATA, 0 },
            { "TEXCOORD", 0, DXGI_FORMAT_R32G32_FLOAT, 0, 44, D3D11_INPUT_PER_VERTEX_DATA, 0 },
        };
        m_device->CreateInputLayout(layout, ARRAYSIZE(layout), VSD3DStarter_VS, ARRAYSIZE(VSD3DStarter_VS), &m_vertexLayout);

        //
        // create the vertex shader
        //
        m_device->CreateVertexShader(VSD3DStarter_VS, ARRAYSIZE(VSD3DStarter_VS), nullptr, &m_vertexShader);

        //
        // create null texture (a 1x1 white texture so shaders work when textures are not set on meshes correctly)
        //
        D3D11_USAGE d3d11Usage = D3D11_USAGE_DEFAULT;
        UINT32 cpuAccess = 0;
        UINT32 d3d11Binding = D3D11_BIND_SHADER_RESOURCE;

        D3D11_TEXTURE2D_DESC desc;
        desc.Width = 1;
        desc.Height = 1;
        desc.MipLevels = 1;
        desc.ArraySize = 1;
        desc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
        desc.SampleDesc.Count = 1;
        desc.SampleDesc.Quality = 0;
        desc.Usage = d3d11Usage;
        desc.BindFlags = d3d11Binding;
        desc.CPUAccessFlags = cpuAccess;
        desc.MiscFlags = 0;
        m_device->CreateTexture2D(&desc, nullptr, &m_nullTexture);

        INT32 white = 0xffffffff;
        m_deviceContext->UpdateSubresource(m_nullTexture.Get(), 0, nullptr, &white, sizeof(white), sizeof(white));

        Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> nullTextureView;
        m_device->CreateShaderResourceView(m_nullTexture.Get(), nullptr, &nullTextureView);
        m_textureResources[L""] = nullTextureView;
    }

    void Shutdown()
    {
        m_pixelShaderResources.clear();
        m_textureResources.clear();
    }

    //
    // accessors
    //
    Camera& GetCamera() const { return m_camera; }

    ID3D11Device* GetDevice() const { return m_device.Get(); }
    ID3D11DeviceContext* GetDeviceContext() const { return m_deviceContext.Get(); }

    ID3D11Buffer* GetMaterialConstants() const { return m_materialConstants.Get(); }
    ID3D11Buffer* GetLightConstants() const { return m_lightConstants.Get(); }
    ID3D11Buffer* GetObjectConstants() const { return m_objectConstants.Get(); }
    ID3D11Buffer* GetMiscConstants() const { return m_miscConstants.Get(); }
   D3D_FEATURE_LEVEL GetDeviceFeatureLevel() const { return m_deviceFeatureLevel; }


    ID3D11SamplerState* GetSamplerState() const { return m_sampler.Get(); }
    ID3D11InputLayout* GetVertexInputLayout() const { return m_vertexLayout.Get(); }
    ID3D11VertexShader* GetVertexShader() const { return m_vertexShader.Get(); }

    ID3D11PixelShader* GetOrCreatePixelShader(const std::wstring& shaderName)
    {
        Microsoft::WRL::ComPtr<ID3D11PixelShader> result = nullptr;

        auto iter = m_pixelShaderResources.find(shaderName);
        if (iter != m_pixelShaderResources.end())
        {
            result = iter->second;
        }
        else
        {
            std::vector<BYTE> psBuffer;
            Graphics::ReadFile(shaderName, psBuffer);
            if (psBuffer.size() > 0)
            {
                this->GetDevice()->CreatePixelShader(&psBuffer[0], psBuffer.size(), nullptr, &result);
                if (result == nullptr) 
                {
                    throw std::exception("Pixel Shader could not be created");
                }

                m_pixelShaderResources[shaderName] = result;
            }
        }

        return result.Get();
    }

    ID3D11ShaderResourceView* GetOrCreateTexture(const std::wstring& textureName, bool generateMipsWhenNeeded = true)
    {
        Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> result;
		
        auto iter = m_textureResources.find(textureName);
        if (iter != m_textureResources.end())
        {
            result = iter->second;
        }
        else
        {
            std::vector<BYTE> ddsBuffer;
            Graphics::ReadFile(textureName, ddsBuffer);
            if (ddsBuffer.size() > 0)
            {
                result = this->CreateTextureFromDDSInMemory(&ddsBuffer[0], ddsBuffer.size(), generateMipsWhenNeeded);
                if (result == nullptr) 
                {
                    throw std::exception("Texture could not be created");
                }
                m_textureResources[textureName] = result;
            }
        }

        return result.Get();
    }

    //
    // methods to update constant buffers
    //
    void UpdateMaterialConstants(const MaterialConstants& data) const
    {
        m_deviceContext->UpdateSubresource(m_materialConstants.Get(), 0, nullptr, &data, 0, 0);
    }
    void UpdateLightConstants(const LightConstants& data) const
    {
        m_deviceContext->UpdateSubresource(m_lightConstants.Get(), 0, nullptr, &data, 0, 0);
    }
    void UpdateObjectConstants(const ObjectConstants& data) const
    {
        m_deviceContext->UpdateSubresource(m_objectConstants.Get(), 0, nullptr, &data, 0, 0);
    }
    void UpdateMiscConstants(const MiscConstants& data) const
    {
        m_deviceContext->UpdateSubresource(m_miscConstants.Get(), 0, nullptr, &data, 0, 0);
    }

private:
    static void ReadFile(const std::wstring& filename, std::vector<BYTE>& data)
    {
        //
        // clear file data
        //
        data.clear();

        //
        // open the file
        //
        FILE* fp = nullptr;
        _wfopen_s(&fp, filename.c_str(), L"rb");
        if (fp == nullptr)
        {
            std::wstring error = L"*** File could not be opened \"" + filename + L"\" \n";
            OutputDebugString(error.c_str());
        }
        else
        {
            // 
            // determine file size and prepare buffer to read all the data
            //
            fseek(fp, 0, SEEK_END);
            long pos = ftell(fp);
            data.resize(pos);
            fseek(fp, 0, SEEK_SET);

            //
            // read data into the prepared buffer
            //
            if (pos > 0)
            {
                fread(&data[0], 1, pos, fp);
            }

            //
            // close the file
            //
            fclose(fp);
        }
    }

    ID3D11ShaderResourceView* CreateTextureFromDDSInMemory(const BYTE* ddsData, size_t ddsDataSize, bool generateMipsWhenNeeded)
    {
        ID3D11ShaderResourceView* textureView = nullptr;

        if (ddsData != nullptr && ddsDataSize >= sizeof(UINT) + sizeof(DDS_HEADER))
        {
            UINT dwMagicNumber = *(const UINT*)(ddsData);
            if (dwMagicNumber == DDS_MAGIC)
            {
                const DDS_HEADER* header = reinterpret_cast<const DDS_HEADER*>(ddsData + sizeof(UINT));
                if (header->dwSize == sizeof(DDS_HEADER) && header->ddspf.dwSize == sizeof(DDS_PIXELFORMAT))
                {
                    // Check for DX10 extension
                    bool dxt10Header = ((header->ddspf.dwFlags & DDS_FOURCC) && (MAKEFOURCC( 'D', 'X', '1', '0' ) == header->ddspf.dwFourCC));

                    size_t offset = sizeof(UINT) + sizeof(DDS_HEADER) + (dxt10Header ? sizeof(DDS_HEADER_DXT10) : 0);
                    if (ddsDataSize >= offset)
                    {
                        textureView = this->CreateTextureFromDDS(header, dxt10Header, ddsData+offset, ddsDataSize-offset, generateMipsWhenNeeded);
                    }
                }
            }
        }

        return textureView;
    }

    UINT BitsPerPixel(DXGI_FORMAT fmt) const
    {
        switch( fmt )
        {
        case DXGI_FORMAT_R32G32B32A32_TYPELESS:
        case DXGI_FORMAT_R32G32B32A32_FLOAT:
        case DXGI_FORMAT_R32G32B32A32_UINT:
        case DXGI_FORMAT_R32G32B32A32_SINT:
            return 128;

        case DXGI_FORMAT_R32G32B32_TYPELESS:
        case DXGI_FORMAT_R32G32B32_FLOAT:
        case DXGI_FORMAT_R32G32B32_UINT:
        case DXGI_FORMAT_R32G32B32_SINT:
            return 96;

        case DXGI_FORMAT_R16G16B16A16_TYPELESS:
        case DXGI_FORMAT_R16G16B16A16_FLOAT:
        case DXGI_FORMAT_R16G16B16A16_UNORM:
        case DXGI_FORMAT_R16G16B16A16_UINT:
        case DXGI_FORMAT_R16G16B16A16_SNORM:
        case DXGI_FORMAT_R16G16B16A16_SINT:
        case DXGI_FORMAT_R32G32_TYPELESS:
        case DXGI_FORMAT_R32G32_FLOAT:
        case DXGI_FORMAT_R32G32_UINT:
        case DXGI_FORMAT_R32G32_SINT:
        case DXGI_FORMAT_R32G8X24_TYPELESS:
        case DXGI_FORMAT_D32_FLOAT_S8X24_UINT:
        case DXGI_FORMAT_R32_FLOAT_X8X24_TYPELESS:
        case DXGI_FORMAT_X32_TYPELESS_G8X24_UINT:
            return 64;

        case DXGI_FORMAT_R10G10B10A2_TYPELESS:
        case DXGI_FORMAT_R10G10B10A2_UNORM:
        case DXGI_FORMAT_R10G10B10A2_UINT:
        case DXGI_FORMAT_R11G11B10_FLOAT:
        case DXGI_FORMAT_R8G8B8A8_TYPELESS:
        case DXGI_FORMAT_R8G8B8A8_UNORM:
        case DXGI_FORMAT_R8G8B8A8_UNORM_SRGB:
        case DXGI_FORMAT_R8G8B8A8_UINT:
        case DXGI_FORMAT_R8G8B8A8_SNORM:
        case DXGI_FORMAT_R8G8B8A8_SINT:
        case DXGI_FORMAT_R16G16_TYPELESS:
        case DXGI_FORMAT_R16G16_FLOAT:
        case DXGI_FORMAT_R16G16_UNORM:
        case DXGI_FORMAT_R16G16_UINT:
        case DXGI_FORMAT_R16G16_SNORM:
        case DXGI_FORMAT_R16G16_SINT:
        case DXGI_FORMAT_R32_TYPELESS:
        case DXGI_FORMAT_D32_FLOAT:
        case DXGI_FORMAT_R32_FLOAT:
        case DXGI_FORMAT_R32_UINT:
        case DXGI_FORMAT_R32_SINT:
        case DXGI_FORMAT_R24G8_TYPELESS:
        case DXGI_FORMAT_D24_UNORM_S8_UINT:
        case DXGI_FORMAT_R24_UNORM_X8_TYPELESS:
        case DXGI_FORMAT_X24_TYPELESS_G8_UINT:
        case DXGI_FORMAT_R9G9B9E5_SHAREDEXP:
        case DXGI_FORMAT_R8G8_B8G8_UNORM:
        case DXGI_FORMAT_G8R8_G8B8_UNORM:
        case DXGI_FORMAT_B8G8R8A8_UNORM:
        case DXGI_FORMAT_B8G8R8X8_UNORM:
        case DXGI_FORMAT_R10G10B10_XR_BIAS_A2_UNORM:
        case DXGI_FORMAT_B8G8R8A8_TYPELESS:
        case DXGI_FORMAT_B8G8R8A8_UNORM_SRGB:
        case DXGI_FORMAT_B8G8R8X8_TYPELESS:
        case DXGI_FORMAT_B8G8R8X8_UNORM_SRGB:
            return 32;

        case DXGI_FORMAT_R8G8_TYPELESS:
        case DXGI_FORMAT_R8G8_UNORM:
        case DXGI_FORMAT_R8G8_UINT:
        case DXGI_FORMAT_R8G8_SNORM:
        case DXGI_FORMAT_R8G8_SINT:
        case DXGI_FORMAT_R16_TYPELESS:
        case DXGI_FORMAT_R16_FLOAT:
        case DXGI_FORMAT_D16_UNORM:
        case DXGI_FORMAT_R16_UNORM:
        case DXGI_FORMAT_R16_UINT:
        case DXGI_FORMAT_R16_SNORM:
        case DXGI_FORMAT_R16_SINT:
        case DXGI_FORMAT_B5G6R5_UNORM:
        case DXGI_FORMAT_B5G5R5A1_UNORM:
        case DXGI_FORMAT_B4G4R4A4_UNORM:
            return 16;

        case DXGI_FORMAT_R8_TYPELESS:
        case DXGI_FORMAT_R8_UNORM:
        case DXGI_FORMAT_R8_UINT:
        case DXGI_FORMAT_R8_SNORM:
        case DXGI_FORMAT_R8_SINT:
        case DXGI_FORMAT_A8_UNORM:
            return 8;

        case DXGI_FORMAT_R1_UNORM:
            return 1;

        case DXGI_FORMAT_BC1_TYPELESS:
        case DXGI_FORMAT_BC1_UNORM:
        case DXGI_FORMAT_BC1_UNORM_SRGB:
        case DXGI_FORMAT_BC4_TYPELESS:
        case DXGI_FORMAT_BC4_UNORM:
        case DXGI_FORMAT_BC4_SNORM:
            return 4;

        case DXGI_FORMAT_BC2_TYPELESS:
        case DXGI_FORMAT_BC2_UNORM:
        case DXGI_FORMAT_BC2_UNORM_SRGB:
        case DXGI_FORMAT_BC3_TYPELESS:
        case DXGI_FORMAT_BC3_UNORM:
        case DXGI_FORMAT_BC3_UNORM_SRGB:
        case DXGI_FORMAT_BC5_TYPELESS:
        case DXGI_FORMAT_BC5_UNORM:
        case DXGI_FORMAT_BC5_SNORM:
        case DXGI_FORMAT_BC6H_TYPELESS:
        case DXGI_FORMAT_BC6H_UF16:
        case DXGI_FORMAT_BC6H_SF16:
        case DXGI_FORMAT_BC7_TYPELESS:
        case DXGI_FORMAT_BC7_UNORM:
        case DXGI_FORMAT_BC7_UNORM_SRGB:
            return 8;

        default:
            return 0;
        }
    }

    #define ISBITMASK( r,g,b,a ) ( ddpf.dwRBitMask == r && ddpf.dwGBitMask == g && ddpf.dwBBitMask == b && ddpf.dwABitMask == a )
    DXGI_FORMAT GetDXGIFormat(const DDS_PIXELFORMAT& ddpf) const
    {
        if (ddpf.dwFlags & DDS_RGB)
        {
            // Note that sRGB formats are written using the "DX10" extended header

            switch (ddpf.dwRGBBitCount)
            {
            case 32:
                if (ISBITMASK(0x000000ff,0x0000ff00,0x00ff0000,0xff000000))
                {
                    return DXGI_FORMAT_R8G8B8A8_UNORM;
                }

                if (ISBITMASK(0x00ff0000,0x0000ff00,0x000000ff,0xff000000))
                {
                    return DXGI_FORMAT_B8G8R8A8_UNORM;
                }

                if (ISBITMASK(0x00ff0000,0x0000ff00,0x000000ff,0x00000000))
                {
                    return DXGI_FORMAT_B8G8R8X8_UNORM;
                }

                // No DXGI format maps to ISBITMASK(0x000000ff,0x0000ff00,0x00ff0000,0x00000000) aka D3DFMT_X8B8G8R8

                // Note that many common DDS reader/writers (including D3DX) swap the
                // the RED/BLUE masks for 10:10:10:2 formats. We assumme
                // below that the 'backwards' header mask is being used since it is most
                // likely written by D3DX. The more robust solution is to use the 'DX10'
                // header extension and specify the DXGI_FORMAT_R10G10B10A2_UNORM format directly

                // For 'correct' writers, this should be 0x000003ff,0x000ffc00,0x3ff00000 for RGB data
                if (ISBITMASK(0x3ff00000,0x000ffc00,0x000003ff,0xc0000000))
                {
                    return DXGI_FORMAT_R10G10B10A2_UNORM;
                }

                // No DXGI format maps to ISBITMASK(0x000003ff,0x000ffc00,0x3ff00000,0xc0000000) aka D3DFMT_A2R10G10B10

                if (ISBITMASK(0x0000ffff,0xffff0000,0x00000000,0x00000000))
                {
                    return DXGI_FORMAT_R16G16_UNORM;
                }

                if (ISBITMASK(0xffffffff,0x00000000,0x00000000,0x00000000))
                {
                    // Only 32-bit color channel format in D3D9 was R32F
                    return DXGI_FORMAT_R32_FLOAT; // D3DX writes this out as a FourCC of 114
                }
                break;

            case 24:
                // No 24bpp DXGI formats aka D3DFMT_R8G8B8
                break;

            case 16:
                if (ISBITMASK(0x7c00,0x03e0,0x001f,0x8000))
                {
                    return DXGI_FORMAT_B5G5R5A1_UNORM;
                }
                if (ISBITMASK(0xf800,0x07e0,0x001f,0x0000))
                {
                    return DXGI_FORMAT_B5G6R5_UNORM;
                }

                // No DXGI format maps to ISBITMASK(0x7c00,0x03e0,0x001f,0x0000) aka D3DFMT_X1R5G5B5
                if (ISBITMASK(0x0f00,0x00f0,0x000f,0xf000))
                {
                    return DXGI_FORMAT_B4G4R4A4_UNORM;
                }

                // No DXGI format maps to ISBITMASK(0x0f00,0x00f0,0x000f,0x0000) aka D3DFMT_X4R4G4B4

                // No 3:3:2, 3:3:2:8, or paletted DXGI formats aka D3DFMT_A8R3G3B2, D3DFMT_R3G3B2, D3DFMT_P8, D3DFMT_A8P8, etc.
                break;
            }
        }
        else if (ddpf.dwFlags & DDS_LUMINANCE)
        {
            if (8 == ddpf.dwRGBBitCount)
            {
                if (ISBITMASK(0x000000ff,0x00000000,0x00000000,0x00000000))
                {
                    return DXGI_FORMAT_R8_UNORM; // D3DX10/11 writes this out as DX10 extension
                }

                // No DXGI format maps to ISBITMASK(0x0f,0x00,0x00,0xf0) aka D3DFMT_A4L4
            }

            if (16 == ddpf.dwRGBBitCount)
            {
                if (ISBITMASK(0x0000ffff,0x00000000,0x00000000,0x00000000))
                {
                    return DXGI_FORMAT_R16_UNORM; // D3DX10/11 writes this out as DX10 extension
                }
                if (ISBITMASK(0x000000ff,0x00000000,0x00000000,0x0000ff00))
                {
                    return DXGI_FORMAT_R8G8_UNORM; // D3DX10/11 writes this out as DX10 extension
                }
            }
        }
        else if (ddpf.dwFlags & DDS_ALPHA)
        {
            if (8 == ddpf.dwRGBBitCount)
            {
                return DXGI_FORMAT_A8_UNORM;
            }
        }
        else if (ddpf.dwFlags & DDS_FOURCC)
        {
            if (MAKEFOURCC( 'D', 'X', 'T', '1' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_BC1_UNORM;
            }
            if (MAKEFOURCC( 'D', 'X', 'T', '3' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_BC2_UNORM;
            }
            if (MAKEFOURCC( 'D', 'X', 'T', '5' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_BC3_UNORM;
            }

            // While pre-mulitplied alpha isn't directly supported by the DXGI formats,
            // they are basically the same as these BC formats so they can be mapped
            if (MAKEFOURCC( 'D', 'X', 'T', '2' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_BC2_UNORM;
            }
            if (MAKEFOURCC( 'D', 'X', 'T', '4' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_BC3_UNORM;
            }

            if (MAKEFOURCC( 'A', 'T', 'I', '1' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_BC4_UNORM;
            }
            if (MAKEFOURCC( 'B', 'C', '4', 'U' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_BC4_UNORM;
            }
            if (MAKEFOURCC( 'B', 'C', '4', 'S' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_BC4_SNORM;
            }

            if (MAKEFOURCC( 'A', 'T', 'I', '2' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_BC5_UNORM;
            }
            if (MAKEFOURCC( 'B', 'C', '5', 'U' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_BC5_UNORM;
            }
            if (MAKEFOURCC( 'B', 'C', '5', 'S' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_BC5_SNORM;
            }

            // BC6H and BC7 are written using the "DX10" extended header

            if (MAKEFOURCC( 'R', 'G', 'B', 'G' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_R8G8_B8G8_UNORM;
            }
            if (MAKEFOURCC( 'G', 'R', 'G', 'B' ) == ddpf.dwFourCC)
            {
                return DXGI_FORMAT_G8R8_G8B8_UNORM;
            }

            // Check for D3DFORMAT enums being set here
            switch( ddpf.dwFourCC )
            {
            case 36: // D3DFMT_A16B16G16R16
                return DXGI_FORMAT_R16G16B16A16_UNORM;

            case 110: // D3DFMT_Q16W16V16U16
                return DXGI_FORMAT_R16G16B16A16_SNORM;

            case 111: // D3DFMT_R16F
                return DXGI_FORMAT_R16_FLOAT;

            case 112: // D3DFMT_G16R16F
                return DXGI_FORMAT_R16G16_FLOAT;

            case 113: // D3DFMT_A16B16G16R16F
                return DXGI_FORMAT_R16G16B16A16_FLOAT;

            case 114: // D3DFMT_R32F
                return DXGI_FORMAT_R32_FLOAT;

            case 115: // D3DFMT_G32R32F
                return DXGI_FORMAT_R32G32_FLOAT;

            case 116: // D3DFMT_A32B32G32R32F
                return DXGI_FORMAT_R32G32B32A32_FLOAT;
            }
        }

        return DXGI_FORMAT_UNKNOWN;
    }

    void GetSurfaceInfo(UINT width, UINT height, DXGI_FORMAT fmt, UINT* outNumBytes, UINT* outRowBytes, UINT* outNumRows) const
    {
        UINT numBytes = 0;
        UINT rowBytes = 0;
        UINT numRows = 0;

        bool bc = false;
        bool packed  = false;
        UINT bcnumBytesPerBlock = 0;
        switch (fmt)
        {
        case DXGI_FORMAT_BC1_TYPELESS:
        case DXGI_FORMAT_BC1_UNORM:
        case DXGI_FORMAT_BC1_UNORM_SRGB:
        case DXGI_FORMAT_BC4_TYPELESS:
        case DXGI_FORMAT_BC4_UNORM:
        case DXGI_FORMAT_BC4_SNORM:
            bc=true;
            bcnumBytesPerBlock = 8;
            break;

        case DXGI_FORMAT_BC2_TYPELESS:
        case DXGI_FORMAT_BC2_UNORM:
        case DXGI_FORMAT_BC2_UNORM_SRGB:
        case DXGI_FORMAT_BC3_TYPELESS:
        case DXGI_FORMAT_BC3_UNORM:
        case DXGI_FORMAT_BC3_UNORM_SRGB:
        case DXGI_FORMAT_BC5_TYPELESS:
        case DXGI_FORMAT_BC5_UNORM:
        case DXGI_FORMAT_BC5_SNORM:
        case DXGI_FORMAT_BC6H_TYPELESS:
        case DXGI_FORMAT_BC6H_UF16:
        case DXGI_FORMAT_BC6H_SF16:
        case DXGI_FORMAT_BC7_TYPELESS:
        case DXGI_FORMAT_BC7_UNORM:
        case DXGI_FORMAT_BC7_UNORM_SRGB:
            bc = true;
            bcnumBytesPerBlock = 16;
            break;

        case DXGI_FORMAT_R8G8_B8G8_UNORM:
        case DXGI_FORMAT_G8R8_G8B8_UNORM:
            packed = true;
            break;
        }

        if (bc)
        {
            UINT numBlocksWide = 0;
            if (width > 0)
            {
                numBlocksWide = max( 1, (width + 3) / 4 );
            }
            UINT numBlocksHigh = 0;
            if (height > 0)
            {
                numBlocksHigh = max( 1, (height + 3) / 4 );
            }
            rowBytes = numBlocksWide * bcnumBytesPerBlock;
            numRows = numBlocksHigh;
        }
        else if (packed)
        {
            rowBytes = ( ( width + 1 ) >> 1 ) * 4;
            numRows = height;
        }
        else
        {
            UINT bpp = BitsPerPixel( fmt );
            rowBytes = ( width * bpp + 7 ) / 8; // round up to nearest byte
            numRows = height;
        }

        numBytes = rowBytes * numRows;
        if (outNumBytes)
        {
            *outNumBytes = numBytes;
        }
        if (outRowBytes)
        {
            *outRowBytes = rowBytes;
        }
        if (outNumRows)
        {
            *outNumRows = numRows;
        }
    }

    ID3D11ShaderResourceView* CreateTextureFromDDS(const DDS_HEADER* header, bool dxt10Header, const BYTE* ddsData, size_t ddsDataSize, bool generateMipsWhenNeeded)
    {
        ID3D11ShaderResourceView* textureView = nullptr;

        DWORD width = header->dwWidth;
        DWORD height = header->dwHeight;
        DWORD depth = header->dwDepth;

        UINT resDim = D3D11_RESOURCE_DIMENSION_UNKNOWN;
        DWORD arraySize = 1;
        DXGI_FORMAT format = DXGI_FORMAT_UNKNOWN;
        bool isCubeMap = false;

        DWORD srcMipCount = header->dwMipMapCount;
        if (0 == srcMipCount)
        {
            srcMipCount = 1;
        }

        DWORD actualMipCount = ((generateMipsWhenNeeded && srcMipCount <= 1) ? 0 : srcMipCount);

        if (dxt10Header)
        {
            const DDS_HEADER_DXT10* d3d10ext = reinterpret_cast<const DDS_HEADER_DXT10*>((const char*)header + sizeof(DDS_HEADER));

            arraySize = d3d10ext->arraySize;
            if (arraySize == 0)
            {
                return nullptr;
            }

            if (BitsPerPixel(d3d10ext->dxgiFormat) == 0)
            {
                return nullptr;
            }
           
            format = d3d10ext->dxgiFormat;

            switch ( d3d10ext->resourceDimension )
            {
                case D3D11_RESOURCE_DIMENSION_TEXTURE1D:
                    // D3DX writes 1D textures with a fixed Height of 1
                    if ((header->dwHeaderFlags & DDS_HEIGHT) && height != 1)
                    {
                        return nullptr;
                    }
                    height = depth = 1;
                    break;

                case D3D11_RESOURCE_DIMENSION_TEXTURE2D:
                    if (d3d10ext->miscFlag & D3D11_RESOURCE_MISC_TEXTURECUBE)
                    {
                        arraySize *= 6;
                        isCubeMap = true;
                    }
                    depth = 1;
                    break;

                case D3D11_RESOURCE_DIMENSION_TEXTURE3D:
                    if (!(header->dwHeaderFlags & DDS_HEADER_FLAGS_VOLUME))
                    {
                        return nullptr;
                    }

                    if (arraySize > 1)
                    {
                        return nullptr;
                    }
                    break;

                default:
                    return nullptr;
            }

            resDim = d3d10ext->resourceDimension;
        }
        else
        {
            format = GetDXGIFormat( header->ddspf );

            if (format == DXGI_FORMAT_UNKNOWN)
            {
               return nullptr;
            }

            if (BitsPerPixel(format) == 0)
            {
                return nullptr;
            }

            if (header->dwHeaderFlags & DDS_HEADER_FLAGS_VOLUME)
            {
                resDim = D3D11_RESOURCE_DIMENSION_TEXTURE3D;
            }
            else 
            {
                if (header->dwCubemapFlags & DDS_CUBEMAP)
                {
                    // We require all six faces to be defined
                    if ((header->dwCubemapFlags & DDS_CUBEMAP_ALLFACES ) != DDS_CUBEMAP_ALLFACES)
                    {
                       return nullptr;
                    }

                    arraySize = 6;
                    isCubeMap = true;
                }

                depth = 1;
                resDim = D3D11_RESOURCE_DIMENSION_TEXTURE2D;

                // Note there's no way for a legacy Direct3D 9 DDS to express a '1D' texture
            }
        }

        // Bound sizes
        if (actualMipCount > D3D11_REQ_MIP_LEVELS)
        {
            return nullptr;
        }

        switch ( resDim )
        {
            case D3D11_RESOURCE_DIMENSION_TEXTURE1D:
                if ((arraySize > D3D11_REQ_TEXTURE1D_ARRAY_AXIS_DIMENSION) ||
                    (width > D3D11_REQ_TEXTURE1D_U_DIMENSION) )
                {
                    return nullptr;
                }
                break;

            case D3D11_RESOURCE_DIMENSION_TEXTURE2D:
                if (isCubeMap)
                {
                    // This is the right bound because we set arraySize to (NumCubes*6) above
                    if ((arraySize > D3D11_REQ_TEXTURECUBE_DIMENSION) ||
                        (width > D3D11_REQ_TEXTURECUBE_DIMENSION) ||
                        (height > D3D11_REQ_TEXTURECUBE_DIMENSION))
                    {
                        return nullptr;
                    }
                }
                else if ((arraySize > D3D11_REQ_TEXTURE2D_ARRAY_AXIS_DIMENSION) ||
                         (width > D3D11_REQ_TEXTURE2D_U_OR_V_DIMENSION) ||
                         (height > D3D11_REQ_TEXTURE2D_U_OR_V_DIMENSION))
                {
                    return nullptr;
                }
                break;

            case D3D11_RESOURCE_DIMENSION_TEXTURE3D:
                if ((arraySize > 1) ||
                    (width > D3D11_REQ_TEXTURE3D_U_V_OR_W_DIMENSION) ||
                    (height > D3D11_REQ_TEXTURE3D_U_V_OR_W_DIMENSION) ||
                    (depth > D3D11_REQ_TEXTURE3D_U_V_OR_W_DIMENSION) )
                {
                    return nullptr;
                }
                break;
        }

        // Create the texture
        std::vector<D3D11_SUBRESOURCE_DATA> initData(srcMipCount * arraySize);

        UINT NumBytes = 0;
        UINT RowBytes = 0;
        UINT NumRows = 0;
        const BYTE* pSrcBits = ddsData;
        const BYTE *pEndBits = ddsData + ddsDataSize;

        UINT index = 0;
        for( UINT j = 0; j < arraySize; j++ )
        {
            UINT w = width;
            UINT h = height;
            UINT d = depth;
            for( UINT i = 0; i < srcMipCount; i++ )
            {
                GetSurfaceInfo( w,
                                h,
                                format,
                                &NumBytes,
                                &RowBytes,
                                &NumRows
                              );

                initData[index].pSysMem = ( const void* )pSrcBits;
                initData[index].SysMemPitch = static_cast<UINT>( RowBytes );
                initData[index].SysMemSlicePitch = static_cast<UINT>( NumBytes );
                ++index;

                if (pSrcBits + (NumBytes*d) > pEndBits)
                {
                    return nullptr;
                }
  
                pSrcBits += NumBytes * d;

                w = w >> 1;
                h = h >> 1;
                d = d >> 1;
                if (w == 0)
                {
                    w = 1;
                }
                if (h == 0)
                {
                    h = 1;
                }
                if (d == 0)
                {
                    d = 1;
                }
            }
        }

        HRESULT hr = S_OK;
        switch ( resDim ) 
        {
            case D3D11_RESOURCE_DIMENSION_TEXTURE1D:
                {
                    D3D11_TEXTURE1D_DESC desc;
                    desc.Width = static_cast<UINT>( width ); 
                    desc.MipLevels = static_cast<UINT>( actualMipCount );
                    desc.ArraySize = static_cast<UINT>( arraySize );
                    desc.Format = format;
                    desc.Usage = D3D11_USAGE_DEFAULT;
                    desc.BindFlags = D3D11_BIND_SHADER_RESOURCE;
                    if (actualMipCount == 0)
                    {
                        desc.BindFlags |= D3D11_BIND_RENDER_TARGET;
                    }
                    desc.CPUAccessFlags = 0;
                    desc.MiscFlags = (actualMipCount == 0 ? D3D11_RESOURCE_MISC_GENERATE_MIPS : 0);

                    ID3D11Texture1D* tex = nullptr;
                    hr = m_device->CreateTexture1D(&desc, (actualMipCount == 0 ? nullptr : &initData[0]), &tex);
                    if (SUCCEEDED(hr) && tex != nullptr)
                    {
                        tex->GetDesc(&desc);

                        if (actualMipCount == 0)
                        {
                            for (UINT arrayIndex = 0; arrayIndex < desc.ArraySize; arrayIndex++)
                            {
                                UINT dataIndex = arrayIndex * srcMipCount;
                                m_deviceContext->UpdateSubresource(
                                    tex, 
                                    D3D11CalcSubresource(0, arrayIndex, desc.MipLevels), 
                                    nullptr, 
                                    initData[dataIndex].pSysMem, 
                                    initData[dataIndex].SysMemPitch, 
                                    initData[dataIndex].SysMemSlicePitch
                                    );
                            }
                        }

                        D3D11_SHADER_RESOURCE_VIEW_DESC SRVDesc;
                        memset( &SRVDesc, 0, sizeof( SRVDesc ) );
                        SRVDesc.Format = format;

                        if (arraySize > 1)
                        {
                            SRVDesc.ViewDimension = D3D_SRV_DIMENSION_TEXTURE1DARRAY;
                            SRVDesc.Texture1DArray.MipLevels = desc.MipLevels;
                            SRVDesc.Texture1DArray.ArraySize = static_cast<UINT>( arraySize );
                        }
                        else
                        {
                            SRVDesc.ViewDimension = D3D_SRV_DIMENSION_TEXTURE1D;
                            SRVDesc.Texture1D.MipLevels = desc.MipLevels;
                        }

                        hr = m_device->CreateShaderResourceView(tex, &SRVDesc, &textureView);
                    }
                    SafeRelease(tex);
                }
               break;

            case D3D11_RESOURCE_DIMENSION_TEXTURE2D:
                {
                    D3D11_TEXTURE2D_DESC desc;
                    desc.Width = static_cast<UINT>( width );
                    desc.Height = static_cast<UINT>( height );
                    desc.MipLevels = static_cast<UINT>( actualMipCount );
                    desc.ArraySize = static_cast<UINT>( arraySize );
                    desc.Format = format;
                    desc.SampleDesc.Count = 1;
                    desc.SampleDesc.Quality = 0;
                    desc.Usage = D3D11_USAGE_DEFAULT;
                    desc.BindFlags = D3D11_BIND_SHADER_RESOURCE;
                    if (actualMipCount == 0)
                    {
                        desc.BindFlags |= D3D11_BIND_RENDER_TARGET;
                    }
                    desc.CPUAccessFlags = 0;
                    desc.MiscFlags = (isCubeMap) ? D3D11_RESOURCE_MISC_TEXTURECUBE : 0;
                    desc.MiscFlags |= (actualMipCount == 0 ? D3D11_RESOURCE_MISC_GENERATE_MIPS : 0);

                    ID3D11Texture2D* tex = nullptr;
                    hr = m_device->CreateTexture2D(&desc, (actualMipCount == 0 ? nullptr : &initData[0]), &tex);
                    if (SUCCEEDED(hr) && tex != nullptr)
                    {
                        tex->GetDesc(&desc);

                        if (actualMipCount == 0)
                        {
                            for (UINT arrayIndex = 0; arrayIndex < desc.ArraySize; arrayIndex++)
                            {
                                UINT dataIndex = arrayIndex * srcMipCount;
                                m_deviceContext->UpdateSubresource(
                                    tex, 
                                    D3D11CalcSubresource(0, arrayIndex, desc.MipLevels), 
                                    nullptr, 
                                    initData[dataIndex].pSysMem, 
                                    initData[dataIndex].SysMemPitch, 
                                    initData[dataIndex].SysMemSlicePitch
                                    );
                            }
                        }

                        D3D11_SHADER_RESOURCE_VIEW_DESC SRVDesc;
                        memset( &SRVDesc, 0, sizeof( SRVDesc ) );
                        SRVDesc.Format = format;

                        if (isCubeMap)
                        {
                            if (arraySize > 6)
                            {
                                SRVDesc.ViewDimension = D3D_SRV_DIMENSION_TEXTURECUBEARRAY;
                                SRVDesc.TextureCubeArray.MipLevels = desc.MipLevels;

                                // Earlier we set arraySize to (NumCubes * 6)
                                SRVDesc.TextureCubeArray.NumCubes = static_cast<UINT>( arraySize / 6 );
                            }
                            else
                            {
                                SRVDesc.ViewDimension = D3D_SRV_DIMENSION_TEXTURECUBE;
                                SRVDesc.TextureCube.MipLevels = desc.MipLevels;
                            }
                        }
                        else if (arraySize > 1)
                        {
                            SRVDesc.ViewDimension = D3D_SRV_DIMENSION_TEXTURE2DARRAY;
                            SRVDesc.Texture2DArray.MipLevels = desc.MipLevels;
                            SRVDesc.Texture2DArray.ArraySize = static_cast<UINT>( arraySize );
                        }
                        else
                        {
                            SRVDesc.ViewDimension = D3D_SRV_DIMENSION_TEXTURE2D;
                            SRVDesc.Texture2D.MipLevels = desc.MipLevels;
                        }

                        hr = m_device->CreateShaderResourceView(tex, &SRVDesc, &textureView);
                    }
                    SafeRelease(tex);
                }
                break;

            case D3D11_RESOURCE_DIMENSION_TEXTURE3D:
                {
                    D3D11_TEXTURE3D_DESC desc;
                    desc.Width = static_cast<UINT>( width );
                    desc.Height = static_cast<UINT>( height );
                    desc.Depth = static_cast<UINT>( depth );
                    desc.MipLevels = static_cast<UINT>( actualMipCount );
                    desc.Format = format;
                    desc.Usage = D3D11_USAGE_DEFAULT;
                    desc.BindFlags = D3D11_BIND_SHADER_RESOURCE;
                    if (actualMipCount == 0)
                    {
                        desc.BindFlags |= D3D11_BIND_RENDER_TARGET;
                    }
                    desc.CPUAccessFlags = 0;
                    desc.MiscFlags = (actualMipCount == 0 ? D3D11_RESOURCE_MISC_GENERATE_MIPS : 0);

                    ID3D11Texture3D* tex = nullptr;
                    hr = m_device->CreateTexture3D(&desc, (actualMipCount == 0 ? nullptr : &initData[0]), &tex);
                    if (SUCCEEDED(hr) && tex != nullptr)
                    {
                        tex->GetDesc(&desc);

                        if (actualMipCount == 0)
                        {
                            m_deviceContext->UpdateSubresource(
                                tex, 
                                0, 
                                nullptr, 
                                initData[0].pSysMem, 
                                initData[0].SysMemPitch, 
                                initData[0].SysMemSlicePitch
                                );
                        }

                        D3D11_SHADER_RESOURCE_VIEW_DESC SRVDesc;
                        memset( &SRVDesc, 0, sizeof( SRVDesc ) );
                        SRVDesc.Format = format;
                        SRVDesc.ViewDimension = D3D_SRV_DIMENSION_TEXTURE3D;
                        SRVDesc.Texture3D.MipLevels = desc.MipLevels;

                        hr = m_device->CreateShaderResourceView(tex, &SRVDesc, &textureView);
                    }
                    SafeRelease(tex);
                }
                break; 
        }

        if (SUCCEEDED(hr) && actualMipCount == 0)
        {
            m_deviceContext->GenerateMips(textureView);
        }

        return textureView;
    }

    std::map<std::wstring, Microsoft::WRL::ComPtr<ID3D11PixelShader>> m_pixelShaderResources;
    std::map<std::wstring, Microsoft::WRL::ComPtr<ID3D11ShaderResourceView>> m_textureResources;

    mutable Camera m_camera; 

    Microsoft::WRL::ComPtr<ID3D11Device> m_device;
    Microsoft::WRL::ComPtr<ID3D11DeviceContext> m_deviceContext;
	D3D_FEATURE_LEVEL m_deviceFeatureLevel;

    Microsoft::WRL::ComPtr<ID3D11Buffer> m_materialConstants;
    Microsoft::WRL::ComPtr<ID3D11Buffer> m_lightConstants;
    Microsoft::WRL::ComPtr<ID3D11Buffer> m_objectConstants;
    Microsoft::WRL::ComPtr<ID3D11Buffer> m_miscConstants;

    Microsoft::WRL::ComPtr<ID3D11SamplerState> m_sampler;
    Microsoft::WRL::ComPtr<ID3D11InputLayout> m_vertexLayout;
    Microsoft::WRL::ComPtr<ID3D11VertexShader> m_vertexShader;
    Microsoft::WRL::ComPtr<ID3D11Texture2D> m_nullTexture;
};
//
//
///////////////////////////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////////////////////
//
// Mesh is a class used to display meshes in 3d which are converted
// during build-time from fbx and dgsl files.
//
class Mesh
{
public:
    static const UINT MaxTextures = 8;  // 8 unique textures are supported.

    struct SubMesh
    {
        SubMesh() : MaterialIndex(0), IndexBufferIndex(0), VertexBufferIndex(0), StartIndex(0), PrimCount(0) { }

        UINT MaterialIndex;
        UINT IndexBufferIndex;
        UINT VertexBufferIndex;
        UINT StartIndex;
        UINT PrimCount;
    };

    struct Material
    {
        Material() { ZeroMemory(this, sizeof(Material)); }
        ~Material() { }

        std::wstring Name;
        
        DirectX::XMFLOAT4X4 UVTransform;

        float Ambient[4];
        float Diffuse[4];
        float Specular[4];
        float Emissive[4];
        float SpecularPower;

        Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> Textures[MaxTextures];
        Microsoft::WRL::ComPtr<ID3D11VertexShader> VertexShader;
        Microsoft::WRL::ComPtr<ID3D11PixelShader> PixelShader;
        Microsoft::WRL::ComPtr<ID3D11SamplerState> SamplerState;
    };

    struct MeshExtents
    {
        float CenterX, CenterY, CenterZ;
        float Radius;

        float MinX, MinY, MinZ;
        float MaxX, MaxY, MaxZ;
    };

    struct Triangle
    {
        DirectX::XMFLOAT3 points[3];
    };

    typedef std::vector<Triangle> TriangleCollection;

    struct BoneInfo
    {
        std::wstring Name;
        INT ParentIndex;
        DirectX::XMFLOAT4X4 InvBindPos;
        DirectX::XMFLOAT4X4 BindPose;
        DirectX::XMFLOAT4X4 BoneLocalTransform;
    };

    struct Keyframe
    {
        Keyframe() : BoneIndex(0), Time(0.0f)
        {}

        UINT BoneIndex;
        float Time;
        DirectX::XMFLOAT4X4 Transform;
    };

    typedef std::vector<Keyframe> KeyframeArray;

    struct AnimClip
    {
        float StartTime;
        float EndTime;
        KeyframeArray Keyframes;
    };

    typedef std::map<const std::wstring, AnimClip> AnimationClipMap;

    //
    // access to mesh data
    //
    std::vector<SubMesh>& SubMeshes()  { return m_submeshes; }
    std::vector<Material>& Materials() { return m_materials; }
    std::vector<ID3D11Buffer*>& VertexBuffers() { return m_vertexBuffers; }
    std::vector<ID3D11Buffer*>& SkinningVertexBuffers() { return m_skinningVertexBuffers; }
    std::vector<ID3D11Buffer*>& IndexBuffers()  { return m_indexBuffers; }
    MeshExtents& Extents() { return m_meshExtents; }
    AnimationClipMap& AnimationClips() { return m_animationClips; }
    std::vector<BoneInfo>& BoneInfoCollection() { return m_boneInfo; }
    TriangleCollection& Triangles() { return m_triangles; }
    const wchar_t* Name() const { return m_name.c_str(); }

    void* Tag;

    //
    // destructor
    //
    ~Mesh()
    {
        for (UINT i = 0; i < m_indexBuffers.size(); i++)
        {
            SafeRelease(m_indexBuffers[i]);
        }

        for (UINT i = 0; i < m_vertexBuffers.size(); i++)
        {
            SafeRelease(m_vertexBuffers[i]);
        }

        for (UINT i = 0; i < m_skinningVertexBuffers.size(); i++)
        {
            SafeRelease(m_skinningVertexBuffers[i]);
        }

        m_submeshes.clear();
        m_materials.clear();
        m_indexBuffers.clear();
        m_vertexBuffers.clear();
        m_skinningVertexBuffers.clear();
    }

    //
    // render the mesh to the current render target
    //
    void Render(const Graphics& graphics, const DirectX::XMMATRIX& world)
    {
        ID3D11DeviceContext* deviceContext = graphics.GetDeviceContext();

        const DirectX::XMMATRIX& view = graphics.GetCamera().GetView();
        const DirectX::XMMATRIX& projection = graphics.GetCamera().GetProjection();

        //
        // compute the object matrices
        //
        DirectX::XMMATRIX localToView = world * view;
        DirectX::XMMATRIX localToProj = world * view * projection;

        //
        // initialize object constants and update the constant buffer
        //
        ObjectConstants objConstants;
        objConstants.LocalToWorld4x4 = DirectX::XMMatrixTranspose(world);
        objConstants.LocalToProjected4x4 = DirectX::XMMatrixTranspose(localToProj);
        objConstants.WorldToLocal4x4 = DirectX::XMMatrixTranspose(DirectX::XMMatrixInverse(nullptr, world));
        objConstants.WorldToView4x4 = DirectX::XMMatrixTranspose(view);
        objConstants.UvTransform4x4 = DirectX::XMMatrixIdentity();
        objConstants.EyePosition = graphics.GetCamera().GetPosition();
        graphics.UpdateObjectConstants(objConstants);

        //
        // assign constant buffers to correct slots
        //
        ID3D11Buffer* constantBuffer = graphics.GetLightConstants();
        deviceContext->VSSetConstantBuffers(1, 1, &constantBuffer);
        deviceContext->PSSetConstantBuffers(1, 1, &constantBuffer);

        constantBuffer = graphics.GetMiscConstants();
        deviceContext->VSSetConstantBuffers(3, 1, &constantBuffer);
        deviceContext->PSSetConstantBuffers(3, 1, &constantBuffer);

        //
        // prepare to draw
        //
        deviceContext->IASetInputLayout(graphics.GetVertexInputLayout());
        deviceContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

        //
        // loop over each submesh
        //
        for (UINT idx = m_submeshes.size(); idx != 0 ; idx--)
        {
            //
            // only draw submeshes that have valid materials
            //
            MaterialConstants materialConstants;
            
            SubMesh& submesh = m_submeshes[idx-1];
            if (submesh.IndexBufferIndex < m_indexBuffers.size() &&
                submesh.VertexBufferIndex < m_vertexBuffers.size())
            {
                UINT stride = sizeof(Vertex);
                UINT offset = 0;
                deviceContext->IASetVertexBuffers(0, 1, &m_vertexBuffers[submesh.VertexBufferIndex], &stride, &offset);
                deviceContext->IASetIndexBuffer(m_indexBuffers[submesh.IndexBufferIndex], DXGI_FORMAT_R16_UINT, 0);
            }

            if (submesh.MaterialIndex < m_materials.size())
            {
                Material& material = m_materials[submesh.MaterialIndex];

                //
                // update material constant buffer
                //
                memcpy(&materialConstants.Ambient, material.Ambient, sizeof(material.Ambient));
                memcpy(&materialConstants.Diffuse, material.Diffuse, sizeof(material.Diffuse));
                memcpy(&materialConstants.Specular, material.Specular, sizeof(material.Specular));
                memcpy(&materialConstants.Emissive, material.Emissive, sizeof(material.Emissive));
                materialConstants.SpecularPower = material.SpecularPower;
                
                graphics.UpdateMaterialConstants(materialConstants);

                //
                // assign material buffer to correct slots
                //
                constantBuffer = graphics.GetMaterialConstants();
                deviceContext->VSSetConstantBuffers(0, 1, &constantBuffer);
                deviceContext->PSSetConstantBuffers(0, 1, &constantBuffer);

                // update UV transform
                memcpy(&objConstants.UvTransform4x4, &material.UVTransform, sizeof(objConstants.UvTransform4x4));
                graphics.UpdateObjectConstants(objConstants);

                constantBuffer = graphics.GetObjectConstants();
                deviceContext->VSSetConstantBuffers(2, 1, &constantBuffer);
                deviceContext->PSSetConstantBuffers(2, 1, &constantBuffer);

                //
                // assign shaders, samplers and texture resources
                //
                ID3D11SamplerState* sampler = material.SamplerState.Get();

                deviceContext->VSSetShader(material.VertexShader.Get(), nullptr, 0);
                deviceContext->VSSetSamplers(0, 1, &sampler);

                deviceContext->PSSetShader(material.PixelShader.Get(), nullptr, 0);
                deviceContext->PSSetSamplers(0, 1, &sampler);

                for (UINT tex = 0; tex < MaxTextures; tex++)
                {
                    ID3D11ShaderResourceView* texture = material.Textures[tex].Get();

                    deviceContext->VSSetShaderResources(0+tex, 1, &texture);
                    deviceContext->VSSetShaderResources(MaxTextures+tex, 1, &texture);

                    deviceContext->PSSetShaderResources(0+tex, 1, &texture);
                    deviceContext->PSSetShaderResources(MaxTextures+tex, 1, &texture);
                }

                //
                // draw the submesh
                //
                deviceContext->DrawIndexed(submesh.PrimCount * 3, submesh.StartIndex, 0);
            }
        }
    }

    //
    // loads a scene from the specified file, returning a vector of mesh objects
    //
    static void LoadFromFile(
        Graphics& graphics, 
        const std::wstring& meshFilename, 
        const std::wstring& shaderPathLocation,
        const std::wstring& texturePathLocation,
        std::vector<Mesh*>& loadedMeshes,
        bool clearLoadedMeshesVector = true
        )
    {
        //
        // clear output vector
        //
        if (clearLoadedMeshesVector)
        {
            loadedMeshes.clear();
        }

        //
        // open the mesh file
        //
        FILE* fp = nullptr;
        _wfopen_s(&fp, meshFilename.c_str(), L"rb"); 
        if (fp == nullptr)
        {
            std::wstring error = L"Mesh file could not be opened " + meshFilename + L"\n";
            OutputDebugString(error.c_str());
        }
        else
        {
            //
            // read how many meshes are part of the scene
            //
            UINT meshCount = 0;
            fread(&meshCount, sizeof(meshCount), 1, fp);

            //
            // for each mesh in the scene, load it from the file
            //
            for (UINT i = 0; i < meshCount; i++)
            {

				

                Mesh* mesh = nullptr;
                Mesh::Load(fp, graphics, shaderPathLocation, texturePathLocation, mesh);
                if (mesh != nullptr)
                {
					std::wstring error = L"Loading mesh " + mesh->m_name + L"\n";
					OutputDebugString(error.c_str());							
                    loadedMeshes.push_back(mesh);
                }
            }
        }
    }

private:
    Mesh()
    {
        Tag = NULL;
    }

    static void StripPath(std::wstring& path)
    {
        size_t p = path.rfind(L"\\");
        if (p != std::wstring::npos)
        {
            path = path.substr(p+1);
        }
    }

    static void Load(FILE* fp, Graphics& graphics, const std::wstring& shaderPathLocation, const std::wstring& texturePathLocation, Mesh*& outMesh)
    {
        //
        // initialize output mesh
        //
        outMesh = nullptr;
        if (fp != nullptr)
        {
            Mesh* mesh = new Mesh();

            UINT nameLen = 0;
            fread(&nameLen, sizeof(nameLen), 1, fp);
            if (nameLen > 0)
            {
                std::vector<wchar_t> objName(nameLen);
                fread(&objName[0], sizeof(wchar_t), nameLen, fp);
                mesh->m_name = &objName[0];
            }

            //
            // read material count
            //
            UINT numMaterials = 0;
            fread(&numMaterials, sizeof(UINT), 1, fp);
            mesh->m_materials.resize(numMaterials);

            //
            // load each material
            //
            for (UINT i = 0; i < numMaterials; i++)
            {
                Material& material = mesh->m_materials[i];

                //
                // read material name
                //
                UINT stringLen = 0;
                fread(&stringLen, sizeof(stringLen), 1, fp);
                if (stringLen > 0)
                {
                    std::vector<wchar_t> matName(stringLen);
                    fread(&matName[0], sizeof(wchar_t), stringLen, fp);
                    material.Name = &matName[0];
                }

                //
                // read ambient and diffuse properties of material
                //
                fread(material.Ambient, sizeof(material.Ambient), 1, fp);
                fread(material.Diffuse, sizeof(material.Diffuse), 1, fp);
                fread(material.Specular, sizeof(material.Specular), 1, fp);
                fread(&material.SpecularPower, sizeof(material.SpecularPower), 1, fp);
                fread(material.Emissive, sizeof(material.Emissive), 1, fp);
                fread(&material.UVTransform, sizeof(material.UVTransform), 1, fp);

                //
                // assign vertex shader and sampler state
                //
                material.VertexShader = graphics.GetVertexShader();
                
                material.SamplerState = graphics.GetSamplerState();
                
                //
                // read name of the pixel shader
                //
                stringLen = 0;
                fread(&stringLen, sizeof(stringLen), 1, fp);
                if (stringLen > 0)
                {
                    //
                    // read the pixel shader name
                    //
                    std::vector<wchar_t> pixelShaderName(stringLen);
                    fread(&pixelShaderName[0], sizeof(wchar_t), stringLen, fp);
                    std::wstring sourceFile = &pixelShaderName[0];
                    
                    //
                    // continue loading pixel shader if name is not empty 
                    //
                    if (!sourceFile.empty())
                    {
                        // 
                        // create well-formed file name for the pixel shader
                        //
                        Mesh::StripPath(sourceFile);
                        sourceFile = shaderPathLocation + sourceFile;

                        //
                        // get or create pixel shader
                        //
                        ID3D11PixelShader* materialPixelShader = graphics.GetOrCreatePixelShader(sourceFile);
                        material.PixelShader = materialPixelShader;
                    }
                }

                //
                // load textures
                //
                for (int t = 0; t < MaxTextures; t++)
                {
                    //
                    // read name of texture
                    //
                    stringLen = 0;
                    fread(&stringLen, sizeof(stringLen), 1, fp);
                    if (stringLen > 0)
                    {
                        //
                        // read the texture name
                        //
                        std::vector<wchar_t> textureFilename(stringLen);
                        fread(&textureFilename[0], sizeof(wchar_t), stringLen, fp);
                        std::wstring sourceFile = &textureFilename[0];

                        //
                        // get or create texture
                        //
                        ID3D11ShaderResourceView* textureResource = graphics.GetOrCreateTexture(sourceFile, true);
                        material.Textures[t] = textureResource;
                    }
                }
            }

            //
            // does this object contain skeletal animation?
            //
            BYTE isSkeletalDataPresent = FALSE;
            fread(&isSkeletalDataPresent, sizeof(BYTE), 1, fp);

            //
            // read submesh info
            //
            UINT numSubmeshes = 0;
            fread(&numSubmeshes, sizeof(UINT), 1, fp);
            mesh->m_submeshes.resize(numSubmeshes);
            for (UINT i = 0; i < numSubmeshes; i++)
            {
                fread(&(mesh->m_submeshes[i]), sizeof(SubMesh), 1, fp);
            }


            //
            // read index buffers
            //
            UINT numIndexBuffers = 0;
            fread(&numIndexBuffers, sizeof(UINT), 1, fp);
            mesh->m_indexBuffers.resize(numIndexBuffers);
            
            std::vector<std::vector<USHORT>> indexBuffers(numIndexBuffers);
            
            for (UINT i = 0; i < numIndexBuffers; i++)
            {
                UINT ibCount = 0;
                fread (&ibCount, sizeof(UINT), 1, fp);
                if (ibCount > 0)
                {
                    indexBuffers[i].resize(ibCount);

                    //
                    // read in the index data
                    //
                    fread(&indexBuffers[i][0], sizeof(USHORT), ibCount, fp);

                    //
                    // create an index buffer for this data
                    //
                    D3D11_BUFFER_DESC bd;
                    ZeroMemory(&bd, sizeof(bd));
                    bd.Usage = D3D11_USAGE_DEFAULT;
                    bd.ByteWidth = sizeof(USHORT) * ibCount;
                    bd.BindFlags = D3D11_BIND_INDEX_BUFFER;
                    bd.CPUAccessFlags = 0;

                    D3D11_SUBRESOURCE_DATA initData;
                    ZeroMemory(&initData, sizeof(initData));
                    initData.pSysMem = &indexBuffers[i][0];

                    graphics.GetDevice()->CreateBuffer(&bd, &initData, &mesh->m_indexBuffers[i]);
                }
            }

            //
            // read vertex buffers
            //
            UINT numVertexBuffers = 0;
            fread(&numVertexBuffers, sizeof(UINT), 1, fp);
            mesh->m_vertexBuffers.resize(numVertexBuffers);
            
            std::vector<std::vector<Vertex>> vertexBuffers(numVertexBuffers);
            
            for (UINT i = 0; i < numVertexBuffers; i++)
            {
                UINT vbCount = 0;
                fread (&vbCount, sizeof(UINT), 1, fp);
                if (vbCount > 0)
                {
                    vertexBuffers[i].resize(vbCount);

                    //
                    // read in the vertex data
                    //
                    fread(&vertexBuffers[i][0], sizeof(Vertex), vbCount, fp);

                    //
                    // create a vertex buffer for this data
                    //
                    D3D11_BUFFER_DESC bd;
                    ZeroMemory(&bd, sizeof(bd));
                    bd.Usage = D3D11_USAGE_DEFAULT;
                    bd.ByteWidth = sizeof(Vertex) * vbCount;
                    bd.BindFlags = D3D11_BIND_VERTEX_BUFFER;
                    bd.CPUAccessFlags = 0;

                    D3D11_SUBRESOURCE_DATA initData;
                    ZeroMemory(&initData, sizeof(initData));
                    initData.pSysMem = &vertexBuffers[i][0];

                    graphics.GetDevice()->CreateBuffer(&bd, &initData, &mesh->m_vertexBuffers[i]);
                }
            }

            for (UINT i = 0; i < mesh->m_submeshes.size(); i++)
            {
                SubMesh& subMesh = mesh->m_submeshes[i];
                std::vector<USHORT>& ib = indexBuffers[subMesh.IndexBufferIndex];
                std::vector<Vertex>& vb = vertexBuffers[subMesh.VertexBufferIndex];

                for (UINT j = 0; j < ib.size(); j += 3)
                {
                    Vertex& v0 = vb[ib[j]];
                    Vertex& v1 = vb[ib[j+1]];
                    Vertex& v2 = vb[ib[j+2]];

                    Triangle tri;
                    tri.points[0].x = v0.x;
                    tri.points[0].y = v0.y;
                    tri.points[0].z = v0.z;

                    tri.points[1].x = v1.x;
                    tri.points[1].y = v1.y;
                    tri.points[1].z = v1.z;

                    tri.points[2].x = v2.x;
                    tri.points[2].y = v2.y;
                    tri.points[2].z = v2.z;

                    mesh->m_triangles.push_back(tri);
                }
            }

            // done with temp buffers
            vertexBuffers.clear();
            indexBuffers.clear();

            //
            // read skinning vertex buffers
            //
            UINT numSkinningVertexBuffers = 0;
            fread(&numSkinningVertexBuffers, sizeof(UINT), 1, fp);
            mesh->m_skinningVertexBuffers.resize(numSkinningVertexBuffers);
            for (UINT i = 0; i < numSkinningVertexBuffers; i++)
            {
                UINT vbCount = 0;
                fread (&vbCount, sizeof(UINT), 1, fp);
                if (vbCount > 0)
                {
                    std::vector<SkinningVertex> verts(vbCount);

                    //
                    // read in the vertex data
                    //
                    fread(&verts[0], sizeof(SkinningVertex), vbCount, fp);

                    //
                    // create a vertex buffer for this data
                    //
                    D3D11_BUFFER_DESC bd;
                    ZeroMemory(&bd, sizeof(bd));
                    bd.Usage = D3D11_USAGE_DEFAULT;
                    bd.ByteWidth = sizeof(SkinningVertex) * vbCount;
                    bd.BindFlags = D3D11_BIND_VERTEX_BUFFER;
                    bd.CPUAccessFlags = 0;

                    D3D11_SUBRESOURCE_DATA initData;
                    ZeroMemory(&initData, sizeof(initData));
                    initData.pSysMem = &verts[0];

                    graphics.GetDevice()->CreateBuffer(&bd, &initData, &mesh->m_skinningVertexBuffers[i]);
                }
            }

            //
            // read extents
            //
            fread(&mesh->m_meshExtents, sizeof(MeshExtents), 1, fp);

            //
            // do we need to read bones and animation?
            //
            if (isSkeletalDataPresent)
            {
                //
                // read bones
                //
                UINT boneCount = 0;
                fread(&boneCount, sizeof(UINT), 1, fp);

                mesh->m_boneInfo.resize(boneCount);

                for (UINT b = 0; b < boneCount; b++)
                {
                    // read the bone name (length, then chars)
                    UINT nameLength = 0;
                    fread(&nameLength, sizeof(UINT), 1, fp);

                    if (nameLength > 0)
                    {
                        std::vector<wchar_t> nameVec(nameLength);
                        fread(&nameVec[0], sizeof(wchar_t), nameLength, fp);

                        mesh->m_boneInfo[b].Name = &nameVec[0];
                    }

                    // read the bone index
//                    INT parentIndex = -1;
                    
                    // read the transforms
                    fread(&mesh->m_boneInfo[b].ParentIndex, sizeof(INT), 1, fp);
                    fread(&mesh->m_boneInfo[b].InvBindPos, sizeof(DirectX::XMFLOAT4X4), 1, fp);
                    fread(&mesh->m_boneInfo[b].BindPose, sizeof(DirectX::XMFLOAT4X4), 1, fp);
                    fread(&mesh->m_boneInfo[b].BoneLocalTransform, sizeof(DirectX::XMFLOAT4X4), 1, fp);                    
                }

                //
                // read animation clips
                //
                UINT clipCount = 0;
                fread(&clipCount, sizeof(UINT), 1, fp);

                for (UINT j = 0; j < clipCount; j++)
                {
                    // read clip name
                    UINT len = 0;
                    fread(&len, sizeof(UINT), 1, fp);

                    std::wstring clipName;
                    if (len > 0)
                    {
                        std::vector<wchar_t> clipNameVec(len);
                        fread(&clipNameVec[0], sizeof(wchar_t), len, fp);

                        clipName = &clipNameVec[0];
                    }

                    fread(&mesh->m_animationClips[clipName].StartTime, sizeof(float), 1, fp);
                    fread(&mesh->m_animationClips[clipName].EndTime, sizeof(float), 1, fp);

                    KeyframeArray& keyframes = mesh->m_animationClips[clipName].Keyframes;

                    // read keyframecount
                    UINT kfCount = 0;
                    fread(&kfCount, sizeof(UINT), 1, fp);

                    // preallocate the memory
                    keyframes.reserve(kfCount);

                    // read each keyframe
                    for (UINT k = 0; k < kfCount; k++)
                    {
                        Keyframe kf;

                        // read the bone
                        fread(&kf.BoneIndex, sizeof(UINT), 1, fp);

                        // read the time
                        fread(&kf.Time, sizeof(UINT), 1, fp);

                        // read the transform
                        fread(&kf.Transform, sizeof(DirectX::XMFLOAT4X4), 1, fp);

                        // add to collection
                        keyframes.push_back(kf);
                    }
                }
            }

            //
            // set the output mesh
            //
            outMesh = mesh;
        }
    }

    std::vector<SubMesh> m_submeshes;
    std::vector<Material> m_materials;
    std::vector<ID3D11Buffer*> m_vertexBuffers;
    std::vector<ID3D11Buffer*> m_skinningVertexBuffers;
    std::vector<ID3D11Buffer*> m_indexBuffers;
    TriangleCollection m_triangles;

    MeshExtents m_meshExtents;

    AnimationClipMap m_animationClips;
    std::vector<BoneInfo> m_boneInfo;

    std::wstring m_name;
};
//
//
///////////////////////////////////////////////////////////////////////////////////////////

}
