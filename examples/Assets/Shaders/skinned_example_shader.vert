#version 450

layout(set = 0, binding = 0) uniform uniformBufferObject {
    mat4 model;
    mat4 view;
    mat4 proj;
} ubo;


layout(set = 2, binding = 0) readonly buffer JointMatrices {
    mat4 BonesTransformations[];
};

layout(location = 0) in vec3 inPosition;
layout(location = 1) in vec3 inColor;
layout(location = 2) in vec2 inTexCoord;
layout(location = 3) in uvec4 inBoneIndices;
layout(location = 4) in vec4 inBoneWeights;

layout(location = 0) out vec3 fragColor;
layout(location = 1) out vec2 fragTexCoord;

void main() {
    mat4 boneTransformation = BonesTransformations[inBoneIndices.x]  * inBoneWeights.x;
    boneTransformation += BonesTransformations[inBoneIndices.y]  * inBoneWeights.y;
    boneTransformation += BonesTransformations[inBoneIndices.z]  * inBoneWeights.z;
    boneTransformation += BonesTransformations[inBoneIndices.w]  * inBoneWeights.w;

    vec4 transformed = boneTransformation * vec4(inPosition, 1);
//    vec4 transformed =  vec4(inPosition, 1);
    vec4 worldPosition = ubo.model * transformed;
    vec4 viewPosition = ubo.view * worldPosition;
    gl_Position = ubo.proj * viewPosition;

    fragColor = inColor;
    fragTexCoord = inTexCoord;
}
