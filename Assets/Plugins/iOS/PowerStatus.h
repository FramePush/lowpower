//
//  PowerStatus.h
//  
//
//  Created by Brian Turner on 8/14/17.
//
//

#import <Foundation/Foundation.h>

@interface PowerStatus : NSObject

@property (atomic, retain) NSString * targetObject;

- (instancetype) initWithTarget: (NSString *)target NS_DESIGNATED_INITIALIZER;
- (void) startMonitor;
- (void) stopMonitor;

@end
