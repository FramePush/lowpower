//
//  PowerStatus.m
//  
//
//  Created by Brian Turner on 8/14/17.
//
//

#import "PowerStatus.h"
#import <Foundation/NSProcessInfo.h>

@implementation PowerStatus

+ (BOOL) checkLowPowerMode
{
    return [[NSProcessInfo processInfo] isLowPowerModeEnabled];
}

- (instancetype) init
{
	return [self initWithTarget:[NSString string]];
}

- (instancetype) initWithTarget: (NSString *) target
{
	self = [super init];
	if (!self) return self;
	self.targetObject = target;
	[self startMonitor];
	return self;
}

- (void) receiveChange
{
	UnitySendMessage([self.targetObject UTF8String], "HandleSaverChange", [PowerStatus checkLowPowerMode] ? "true" : "false");
}

- (void) startMonitor
{
	[self stopMonitor];
	[[NSNotificationCenter defaultCenter] addObserver: self selector:@selector(receiveChange) name: NSProcessInfoPowerStateDidChangeNotification object: nil];
}

- (void) stopMonitor
{
	[[NSNotificationCenter defaultCenter] removeObserver: self name:NSProcessInfoPowerStateDidChangeNotification object: nil];
}

- (void) dealloc
{
	[self stopMonitor];
}

@end

#if __cplusplus
extern "C" {
#endif
	
	bool _isLowPowerModeOn()
	{
		return [PowerStatus checkLowPowerMode];
	}
	
	CFTypeRef _createPowerStatus(const char * targetName)
	{
		return CFBridgingRetain([[PowerStatus alloc] initWithTarget:[NSString stringWithUTF8String:targetName]]);
	}
	
	void _destroyPowerStatus(CFTypeRef powerStatus)
	{
		if (powerStatus == NULL) {
			return;
		}
		
		CFRelease(powerStatus);
	}
	
#if __cplusplus
}
#endif
