//
//  PowerStatus.h
//  
//
//  Created by Brian Turner on 8/14/17.
//
//

#import <Foundation/Foundation.h>

@interface CFPPowerStatus : NSObject

@property (atomic, retain) NSString * targetObject;

+ (BOOL) checkLowPowerMode;

- (instancetype) initWithTarget: (NSString *)target NS_DESIGNATED_INITIALIZER;
- (void) startMonitor;
- (void) stopMonitor;

@end
