
#import "GameHandler.h"
#import <string>

static std::string kTargetObject = "MessageMng"; /* 需要通知对象（U3D）*/

const char *kGameCallbackMethod = "UnityRevieverFunc";

#define _unity_send_msg(msg) UnitySendMessage(kTargetObject.c_str(), kGameCallbackMethod, msg)

@interface GameHandler ()

@end

@implementation GameHandler

+ (GameHandler *)defaultHandler
{
    static GameHandler *instance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        instance = [[[self class] alloc] init];
    });
    return instance;
}

- (instancetype)init
{
    self = [super init];
    if (self) {
        
    }
    return self;
}

#pragma mark -  DelegateProtocol

extern "C" {
void _onMessageRecieved(const char* func, const char* param)
{
    
}
}

@end
