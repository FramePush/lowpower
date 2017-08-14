#import <Foundation/NSProcessInfo.h>

#if __cplusplus
extern "C" {
#endif
	
	bool _isLowPowerModeOn()
	{
		return [[NSProcessInfo processInfo] isLowPowerModeEnabled];
	}
	
#if __cplusplus
}
#endif
