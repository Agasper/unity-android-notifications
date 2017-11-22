#import "LocalNotifications.h"
#import <UIKit/UIKit.h>

NSMutableDictionary *registeredThisLaunch = nil;

void registerCategory(struct NotificationStruct *notifStruct) {
    NSString *categoryIdentifier = [NSString stringWithUTF8String:notifStruct->category];
    if (registeredThisLaunch == nil)
        registeredThisLaunch = [NSMutableDictionary dictionary];
    else if ([registeredThisLaunch objectForKey:categoryIdentifier] != nil)
        return;
    [registeredThisLaunch setValue:@YES forKey:categoryIdentifier];

    UIUserNotificationType types = UIUserNotificationTypeBadge | UIUserNotificationTypeAlert;
    if (notifStruct->sound)
        types |= UIUserNotificationTypeSound;

    UIMutableUserNotificationCategory *category = [[UIMutableUserNotificationCategory alloc] init];
    category.identifier = categoryIdentifier;

    if (notifStruct->actionCount > 0) {
        NSMutableArray<UIUserNotificationAction *> *actions = [NSMutableArray array];
        struct NotificationActionStruct *actionStructs = &notifStruct->action1;
        for (int i = 0; i < MIN(4, notifStruct->actionCount); i++) {
            struct NotificationActionStruct actionStruct = actionStructs[i];
            UIMutableUserNotificationAction *action = [[UIMutableUserNotificationAction alloc] init];
            action.title = [NSString stringWithUTF8String:actionStruct.title];
            NSString *gameObject = [NSString stringWithUTF8String:actionStruct.gameObject];
            if (gameObject == nil)
                gameObject = @"";
            NSString *handlerMethod = [NSString stringWithUTF8String:actionStruct.handlerMethod];
            if (handlerMethod == nil)
                handlerMethod = @"";
            NSString *identifier = [NSString stringWithUTF8String:actionStruct.identifier];
            if (identifier == nil)
                identifier = action.title;
            action.identifier = [NSString stringWithFormat:@"%@:%@:%@", gameObject, handlerMethod, identifier];
            action.activationMode = actionStruct.foreground ? UIUserNotificationActivationModeForeground : UIUserNotificationActivationModeBackground;
            [actions addObject:action];
        }

        [category setActions:actions forContext:UIUserNotificationActionContextDefault];

        NSArray<UIUserNotificationAction *> *minimalActions = [actions subarrayWithRange:NSMakeRange(0, MIN(2, actions.count))];
        [category setActions:minimalActions forContext:UIUserNotificationActionContextMinimal];
    }

    NSMutableSet<UIUserNotificationCategory *> *categories = [NSMutableSet set];
    [categories addObject:category];

    UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:types categories: categories];
    [[UIApplication sharedApplication] registerUserNotificationSettings:settings];
}

NSString* findSoundResourceForName(NSString *soundName) {
    soundName = [@"Data/Raw/" stringByAppendingPathComponent:soundName];
    for (NSString *extension in @[@"wav", @"caf", @"aif", @"aiff"]) {
        NSString *soundPath = [[NSBundle mainBundle] pathForResource:soundName ofType:extension];
        if (soundPath != nil) {
            return [soundName stringByAppendingPathExtension:extension];
        }
    }
    return nil;
}

void scheduleNotification(struct NotificationStruct *notifStruct) {
    registerCategory(notifStruct);
    cancelNotification(notifStruct->identifier);

    UILocalNotification *notification = [[UILocalNotification alloc] init];
    notification.alertBody = [NSString stringWithUTF8String:notifStruct->message];
//    notification.alertAction = "open" // text that is displayed after "slide to..." on the lock screen - defaults to "slide to view"
    notification.fireDate = [NSDate dateWithTimeIntervalSinceNow:((NSTimeInterval)notifStruct->delay)];
    notification.repeatInterval = notifStruct->repeat;
    if (notifStruct->soundName) {
        notification.soundName = findSoundResourceForName([NSString stringWithUTF8String:notifStruct->soundName]);
    }
    notification.userInfo = @{@"identifier": [NSNumber numberWithInteger:notifStruct->identifier]};
    notification.category = [NSString stringWithUTF8String:notifStruct->category];

    [UIApplication.sharedApplication scheduleLocalNotification:notification];
}

void cancelNotification(int identifier) {
    for (UILocalNotification *notification in UIApplication.sharedApplication.scheduledLocalNotifications) {
        NSNumber *notificationIdentifier = [notification.userInfo objectForKey:@"identifier"];
        if (notificationIdentifier.intValue == identifier) {
            [UIApplication.sharedApplication cancelLocalNotification:notification];
        }
    }
}

void cancelAllNotifications() {
    [UIApplication.sharedApplication cancelAllLocalNotifications];
}
