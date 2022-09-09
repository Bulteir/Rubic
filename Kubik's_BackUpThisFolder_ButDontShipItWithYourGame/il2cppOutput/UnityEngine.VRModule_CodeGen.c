#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.String UnityEngine.XR.XRSettings::get_loadedDeviceName()
extern void XRSettings_get_loadedDeviceName_m0EC0BC3BFBF1483DBC766D882A54677F2DBFC4BA (void);
// 0x00000002 System.Void UnityEngine.XR.XRDevice::InvokeDeviceLoaded(System.String)
extern void XRDevice_InvokeDeviceLoaded_m07DEE6645B38728C7B8615DAAD6BE592C1DC59F9 (void);
// 0x00000003 System.Boolean UnityEngine.XR.XRStats::TryGetGPUTimeLastFrame(System.Single&)
extern void XRStats_TryGetGPUTimeLastFrame_mD340EDCF60E2E4CCFD048087551477671729C3FB (void);
// 0x00000004 System.Boolean UnityEngine.XR.XRStats::TryGetDroppedFrameCount(System.Int32&)
extern void XRStats_TryGetDroppedFrameCount_mA262AEC96A356A1A39BBE213DEE5D980A08114BA (void);
// 0x00000005 System.Boolean UnityEngine.XR.XRStats::TryGetFramePresentCount(System.Int32&)
extern void XRStats_TryGetFramePresentCount_mB3EACD7ED7427BCCEAA3056253D67A8B8174E1B6 (void);
static Il2CppMethodPointer s_methodPointers[5] = 
{
	XRSettings_get_loadedDeviceName_m0EC0BC3BFBF1483DBC766D882A54677F2DBFC4BA,
	XRDevice_InvokeDeviceLoaded_m07DEE6645B38728C7B8615DAAD6BE592C1DC59F9,
	XRStats_TryGetGPUTimeLastFrame_mD340EDCF60E2E4CCFD048087551477671729C3FB,
	XRStats_TryGetDroppedFrameCount_mA262AEC96A356A1A39BBE213DEE5D980A08114BA,
	XRStats_TryGetFramePresentCount_mB3EACD7ED7427BCCEAA3056253D67A8B8174E1B6,
};
static const int32_t s_InvokerIndices[5] = 
{
	8398,
	8288,
	7832,
	7832,
	7832,
};
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_UnityEngine_VRModule_CodeGenModule;
const Il2CppCodeGenModule g_UnityEngine_VRModule_CodeGenModule = 
{
	"UnityEngine.VRModule.dll",
	5,
	s_methodPointers,
	0,
	NULL,
	s_InvokerIndices,
	0,
	NULL,
	0,
	NULL,
	0,
	NULL,
	NULL,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};
