#include "KochavaTracker.h"

char* AutonomousStringCopy (const char* string)
{
	if (string == NULL)
	return NULL;
	
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}



@interface NativeWrapper: NSObject <KochavaTrackerDelegate>

@end

@implementation NativeWrapper

- (void)tracker:(nonnull KochavaTracker *)tracker didRetrieveAttributionDictionary:(nonnull NSDictionary *)attributionDictionary
{
	NSError *error = nil;
       
	NSData *attributionDictionaryJSONData = [NSJSONSerialization dataWithJSONObject:attributionDictionary options:0 error:&error];
	
	if (attributionDictionaryJSONData != nil)
	{
		NSString *attributionDictionaryJSONString = [[NSString alloc] initWithData:attributionDictionaryJSONData encoding:NSUTF8StringEncoding];
		// send this message back to the host app, which must always have a game object and listener method with these names
		const char* a = "_Kochava Analytics";
		const char* b = "KochavaAttributionListener";
		UnitySendMessage(a, b, AutonomousStringCopy([attributionDictionaryJSONString UTF8String]));
	}
	
}

@end

// create the object for the class
NativeWrapper *nativeWrapper;




// convert a c# stringified dictionary to an NSDictionary
NSMutableDictionary * ConvertToNSDictionary(const char *stringifiedDictionary) {
	
	NSString *str = [NSString stringWithUTF8String:stringifiedDictionary];
    NSData* data = [str dataUsingEncoding:NSUTF8StringEncoding];
    
    NSError *jsonSerializationError;
    id responseObject = [NSJSONSerialization JSONObjectWithData: data options: NSJSONReadingAllowFragments error: &jsonSerializationError];

    // reponse dictionary
    if ([responseObject isKindOfClass:[NSDictionary class]])
   	{
       	return [responseObject mutableCopy];
   	}
    
    return nil;
}



extern "C" {
	
	// migrate the previously persisted data from unity v1
	void NativeMigrate() {		
		
		// MIGRATE LEGACY UNITY SDK'S NSUSERDEFAULTS
		// oldDeviceIdStringKey, oldWatchlistPropertiesKey, oldKochavaQueueStorageKey, and oldAttributionDictionaryStringKey
		NSString * const oldDeviceIdStringKey = @"kochava_device_id";
		
		NSString * const oldWatchlistPropertiesKey = @"watchlistProperties";
		
		NSString * const oldKochavaQueueStorageKey = @"kochava_queue_storage";
		
		NSString * const oldAttributionDictionaryStringKey = @"attribution";
		
		// oldDeviceIdString
		NSString *oldDeviceIdString = [NSUserDefaults.standardUserDefaults objectForKey:oldDeviceIdStringKey];
		
		// Discussion:  We only proceed if we find an oldDeviceIdString.  If we don't, we assume that this is either a new install or else already migrated.
		if (oldDeviceIdString != nil)
		{
			// watchlistPropertiesObject
			NSObject *watchlistPropertiesObject = [NSUserDefaults.standardUserDefaults objectForKey:oldWatchlistPropertiesKey];
			
			// oldKochavaQueueStorageObject
			NSObject *oldKochavaQueueStorageObject = [NSUserDefaults.standardUserDefaults objectForKey:oldKochavaQueueStorageKey];
			
			// oldKochavaQueueStorageString
			NSString *oldKochavaQueueStorageString = nil;
			
			if ([oldKochavaQueueStorageObject isKindOfClass:[NSString class]])
			{
				oldKochavaQueueStorageString = (NSString *)oldKochavaQueueStorageObject;
			}
			
			// watchlistPropertiesExistsBool
			BOOL watchlistPropertiesExistsBool = (watchlistPropertiesObject != nil);
			
			// oldKochavaQueueStorageContainsInitialBool
			BOOL oldKochavaQueueStorageContainsInitialBool = NO;
			
			if ((oldKochavaQueueStorageString != nil) && (oldKochavaQueueStorageString.length > 0))
			{
				NSRange range = [oldKochavaQueueStorageString rangeOfString:@"initial" options:NSCaseInsensitiveSearch];
				
				if (range.location != NSNotFound)
				{
					oldKochavaQueueStorageContainsInitialBool = YES;
				}
			}
			
			// oldAttributionObject
			NSObject *oldAttributionObject = [NSUserDefaults.standardUserDefaults objectForKey:oldAttributionDictionaryStringKey];
			
			// oldAttributionString
			NSString *oldAttributionString = nil;
			
			if ([oldAttributionObject isKindOfClass:[NSString class]])
			{
				oldAttributionString = (NSString *)oldAttributionObject;
			}
			// oldAttributionDictionary
			NSDictionary *oldAttributionDictionary = nil;
			
			if ([oldAttributionObject isKindOfClass:[NSDictionary class]])
			{
				oldAttributionDictionary = (NSDictionary *)oldAttributionObject;
			}
			
			// oldAttributionStringData
			NSData *oldAttributionStringData = nil;
			
			if ((oldAttributionDictionary == nil) && (oldAttributionString != nil))
			{
				oldAttributionStringData = [oldAttributionString dataUsingEncoding:NSUTF8StringEncoding];
			}
			
			// attributionJSONObject and oldAttributionStringDataError
			id oldAttributionJSONObject = nil;
			
			NSError *oldAttributionStringDataError = nil;
			
			if (oldAttributionStringData != nil)
			{
				oldAttributionJSONObject = [NSJSONSerialization JSONObjectWithData:oldAttributionStringData options:NSJSONReadingMutableContainers error:&oldAttributionStringDataError];
			}
			
			// oldAttributionJSONDictionary
			NSDictionary *oldAttributionJSONDictionary = nil;
			
			if ([oldAttributionJSONObject isKindOfClass:[NSDictionary class]])
			{
				oldAttributionJSONDictionary = (NSDictionary *)oldAttributionJSONObject;
			}
			
			// newAttributionDictionary
			NSDictionary *newAttributionDictionary = nil;
			
			if (oldAttributionDictionary != nil)
			{
				newAttributionDictionary = oldAttributionDictionary;
			}
			else if (oldAttributionJSONDictionary != nil)
			{
				newAttributionDictionary = oldAttributionJSONDictionary;
			}
			else if (oldAttributionString != nil)
			{
				newAttributionDictionary = @{ @"attribution": oldAttributionString };
			}
			
			// initialNetTransactionFirstCompletedBool
			BOOL initialNetTransactionFirstCompletedBool = (watchlistPropertiesExistsBool && !oldKochavaQueueStorageContainsInitialBool);
			
			// initialNetTransactionPendingBool
			BOOL initialNetTransactionPendingBool = !initialNetTransactionFirstCompletedBool;
			
			// deviceIdStringValidDictionary
			NSMutableDictionary *deviceIdStringValidDictionary = NSMutableDictionary.dictionary;
			deviceIdStringValidDictionary[@"rawValueObject"] = oldDeviceIdString;
			deviceIdStringValidDictionary[@"validNameString"] = @"Tracker.deviceIdStringValid";
			deviceIdStringValidDictionary[@"validServerFormattedValueObject"] = oldDeviceIdString;
			deviceIdStringValidDictionary[@"valueDate"] = NSDate.date;
			deviceIdStringValidDictionary[@"valueObject"] = oldDeviceIdString;
			
			// initialNetTransactionFirstCompletedBoolValidDictionary
			NSMutableDictionary *initialNetTransactionFirstCompletedBoolValidDictionary = NSMutableDictionary.dictionary;
			initialNetTransactionFirstCompletedBoolValidDictionary[@"rawValueObject"] = @(initialNetTransactionFirstCompletedBool);
			initialNetTransactionFirstCompletedBoolValidDictionary[@"validNameString"] = @"Tracker.initialNetTransactionFirstCompletedBoolValid";
			initialNetTransactionFirstCompletedBoolValidDictionary[@"validServerFormattedValueObject"] = @(initialNetTransactionFirstCompletedBool);
			initialNetTransactionFirstCompletedBoolValidDictionary[@"valueDate"] = NSDate.date;
			initialNetTransactionFirstCompletedBoolValidDictionary[@"valueObject"] = @(initialNetTransactionFirstCompletedBool);
			
			// initialNetTransactionPendingBoolValidDictionary
			NSMutableDictionary *initialNetTransactionPendingBoolValidDictionary = NSMutableDictionary.dictionary;
			initialNetTransactionPendingBoolValidDictionary[@"rawValueObject"] = @(initialNetTransactionPendingBool);
			initialNetTransactionPendingBoolValidDictionary[@"validNameString"] = @"Tracker.initialNetTransactionPendingBoolValid";
			initialNetTransactionPendingBoolValidDictionary[@"validServerFormattedValueObject"] = @(initialNetTransactionPendingBool);
			initialNetTransactionPendingBoolValidDictionary[@"valueDate"] = NSDate.date;
			initialNetTransactionPendingBoolValidDictionary[@"valueObject"] = @(initialNetTransactionPendingBool);
			
			// attributionDictionaryValidDictionary
			NSMutableDictionary *attributionDictionaryValidDictionary = NSMutableDictionary.dictionary;
			attributionDictionaryValidDictionary[@"rawValueObject"] = newAttributionDictionary;
			attributionDictionaryValidDictionary[@"validNameString"] = @"Tracker.attributionDictionaryValid";
			attributionDictionaryValidDictionary[@"validServerFormattedValueObject"] = newAttributionDictionary;
			attributionDictionaryValidDictionary[@"valueDate"] = NSDate.date;
			attributionDictionaryValidDictionary[@"valueObject"] = newAttributionDictionary;
			
			// NSUserDefaults.standardUserDefaults
			// ... set the new keys
			[NSUserDefaults.standardUserDefaults setObject:attributionDictionaryValidDictionary forKey:@"com.kochava.KochavaTracker.Tracker.attributionDictionaryValid"];
			
			[NSUserDefaults.standardUserDefaults setObject:initialNetTransactionPendingBoolValidDictionary forKey:@"com.kochava.KochavaTracker.Tracker.initialNetTransactionPendingBoolValid"];
			
			[NSUserDefaults.standardUserDefaults setObject:initialNetTransactionFirstCompletedBoolValidDictionary forKey:@"com.kochava.KochavaTracker.Tracker.initialNetTransactionFirstCompletedBoolValid"];
			
			[NSUserDefaults.standardUserDefaults setObject:deviceIdStringValidDictionary forKey:@"com.kochava.KochavaTracker.Tracker.deviceIdStringValid"];
			
			// ... remove the old keys
			[NSUserDefaults.standardUserDefaults removeObjectForKey:oldAttributionDictionaryStringKey];
			
			[NSUserDefaults.standardUserDefaults removeObjectForKey:oldKochavaQueueStorageKey];
			
			[NSUserDefaults.standardUserDefaults removeObjectForKey:oldWatchlistPropertiesKey];
			
			[NSUserDefaults.standardUserDefaults removeObjectForKey:oldDeviceIdStringKey];
		}		

	}
	
	// initializer
	void StartiOSNative(const char *inputParameters)
	{
		// migrate settings from the previous v1 unity sdk if applicable
		NativeMigrate();
		
		nativeWrapper = [[NativeWrapper alloc] init];

		NSMutableDictionary *dictionary = NSMutableDictionary.dictionary;
		dictionary = ConvertToNSDictionary(inputParameters);

		// convert wrapper keys to native keys then get rid of the wrapper entry
		// (dictionary.count should remain the same before and after)
		NSUInteger num = dictionary.count;
		dictionary[kKVAParamAppGUIDStringKey] = dictionary[@"appGUID"];
		if (dictionary.count > num) dictionary[@"appGUID"] = nil;		
		dictionary[@"partnerNameString"] = dictionary[@"partnerName"];
		if (dictionary.count > num) dictionary[@"partnerName"] = nil;
		dictionary[kKVAParamAppLimitAdTrackingBoolKey] = dictionary[@"appLimitAdTracking"];
		if (dictionary.count > num) dictionary[@"appLimitAdTracking"] = nil;
		dictionary[kKVAParamLogLevelEnumKey] = dictionary[@"logLevel"];
		if (dictionary.count > num) dictionary[@"logLevel"] = nil;
		dictionary[kKVAParamRetrieveAttributionBoolKey] = dictionary[@"retrieveAttribution"];
		if (dictionary.count > num) dictionary[@"retrieveAttribution"] = nil;
		dictionary[kKVAParamIdentityLinkDictionaryKey] = dictionary[@"identityLink"];
		if (dictionary.count > num) dictionary[@"identityLink"] = nil;

		[KochavaTracker.shared configureWithParametersDictionary:dictionary delegate:nativeWrapper];

	}

}







extern "C" {

	void NativeSendEvent(const char *eventName, const char *eventInfo)
	{
		NSString *evName = [NSString stringWithUTF8String:eventName];
		NSString *evInfo = [NSString stringWithUTF8String:eventInfo];

		[KochavaTracker.shared sendEventWithNameString:evName infoString:evInfo];
	}

	void NativeSendKochavaEvent(const char *eventName, const char *kochavaEventStringifiedDictionary)
	{
		// get the name
		NSString *evName = [NSString stringWithUTF8String:eventName];
		// convert the dictionary
		NSMutableDictionary *stdParamsDictionary = NSMutableDictionary.dictionary;
		stdParamsDictionary = ConvertToNSDictionary(kochavaEventStringifiedDictionary);

		// decide which enum to use based on eventName
		KochavaEventTypeEnum eventTypeEnum = KochavaEventTypeEnumUndefined;

		if([evName isEqualToString:@"Achievement"]) eventTypeEnum = KochavaEventTypeEnumAchievement;
		else if([evName isEqualToString:@"AddToCart"]) eventTypeEnum = KochavaEventTypeEnumAddToCart;
		else if([evName isEqualToString:@"AddToWishList"]) eventTypeEnum = KochavaEventTypeEnumAddToWishList;
		else if([evName isEqualToString:@"CheckoutStart"]) eventTypeEnum = KochavaEventTypeEnumCheckoutStart;
		else if([evName isEqualToString:@"LevelComplete"]) eventTypeEnum = KochavaEventTypeEnumLevelComplete;
		else if([evName isEqualToString:@"Purchase"]) eventTypeEnum = KochavaEventTypeEnumPurchase;
		else if([evName isEqualToString:@"Rating"]) eventTypeEnum = KochavaEventTypeEnumRating;
		else if([evName isEqualToString:@"RegistrationComplete"]) eventTypeEnum = KochavaEventTypeEnumRegistrationComplete;
		else if([evName isEqualToString:@"Search"]) eventTypeEnum = KochavaEventTypeEnumSearch;
		else if([evName isEqualToString:@"TutorialComplete"]) eventTypeEnum = KochavaEventTypeEnumTutorialComplete;
		else if([evName isEqualToString:@"View"]) eventTypeEnum = KochavaEventTypeEnumView;
		else if([evName isEqualToString:@"Custom"]) eventTypeEnum = KochavaEventTypeEnumCustom;

		// create a native KochavaEvent and populate it with each possible std. param which exists in the provided dictionary
		KochavaEvent* event = [KochavaEvent eventWithEventTypeEnum:eventTypeEnum];
		if(event!=nil) {

			for (NSString *key in stdParamsDictionary) {
				id value = stdParamsDictionary[key];			

				if([key isEqualToString:@"checkoutAsGuest"]) event.checkoutAsGuestString = (NSString *)value;
				else if([key isEqualToString:@"contentId"]) event.contentIdString = (NSString *)value;
				else if([key isEqualToString:@"contentTypename"]) event.contentTypeString = (NSString *)value;
				else if([key isEqualToString:@"currency"]) event.currencyString = (NSString *)value;
				else if([key isEqualToString:@"customEventName"]) event.customEventNameString = (NSString *)value;				
				else if([key isEqualToString:@"dateString"]) event.dateString = (NSString *)value;
				else if([key isEqualToString:@"description"]) event.descriptionString = (NSString *)value;
				else if([key isEqualToString:@"destination"]) event.destinationString = (NSString *)value;
				else if([key isEqualToString:@"duration"]) event.durationTimeIntervalNumber = (NSNumber *)value;
				else if([key isEqualToString:@"endDateString"]) event.endDateString = (NSString *)value;
				else if([key isEqualToString:@"infoString"]) event.infoString = (NSString *)value;
				else if([key isEqualToString:@"itemAddedFrom"]) event.itemAddedFromString = (NSString *)value;
				else if([key isEqualToString:@"level"]) event.levelString = (NSString *)value;				
				else if([key isEqualToString:@"maxRatingValue"]) event.maxRatingValueDoubleNumber = (NSNumber *)value;
				else if([key isEqualToString:@"name"]) event.nameString = (NSString *)value;
				else if([key isEqualToString:@"orderId"]) event.orderIdString = (NSString *)value;
				else if([key isEqualToString:@"origin"]) event.originString = (NSString *)value;
				else if([key isEqualToString:@"price"]) event.priceDoubleNumber = (NSNumber *)value;
				else if([key isEqualToString:@"quantity"]) event.quantityDoubleNumber = (NSNumber *)value;
				else if([key isEqualToString:@"ratingValue"]) event.ratingValueDoubleNumber = (NSNumber *)value;
				else if([key isEqualToString:@"receiptId"]) event.receiptIdString = (NSString *)value;
				else if([key isEqualToString:@"appStoreReceiptBase64EncodedString"]) event.appStoreReceiptBase64EncodedString = (NSString *)value;
				else if([key isEqualToString:@"referralFrom"]) event.referralFromString = (NSString *)value;
				else if([key isEqualToString:@"registrationMethod"]) event.registrationMethodString = (NSString *)value;
				else if([key isEqualToString:@"results"]) event.resultsString = (NSString *)value;
				else if([key isEqualToString:@"score"]) event.scoreString = (NSString *)value;
				else if([key isEqualToString:@"searchTerm"]) event.searchTermString = (NSString *)value;
				else if([key isEqualToString:@"spatialX"]) event.spatialXDoubleNumber = (NSNumber *)value;
				else if([key isEqualToString:@"spatialY"]) event.spatialYDoubleNumber = (NSNumber *)value;
				else if([key isEqualToString:@"spatialZ"]) event.spatialZDoubleNumber = (NSNumber *)value;
				else if([key isEqualToString:@"startDateString"]) event.startDateString = (NSString *)value;
				else if([key isEqualToString:@"success"]) event.successString = (NSString *)value;
				else if([key isEqualToString:@"userId"]) event.userIdString = (NSString *)value;
				else if([key isEqualToString:@"userName"]) event.userNameString = (NSString *)value;
				else if([key isEqualToString:@"validated"]) event.validatedString = (NSString *)value;

				
			}

			// now send it
			[KochavaTracker.shared sendEvent: event];
		}
	}

	void NativeSendEventWithReceipt(const char *eventName, const char *eventInfo, const char *eventReceipt)
	{
		NSString *evName = [NSString stringWithUTF8String:eventName];
		NSString *evInfo = [NSString stringWithUTF8String:eventInfo];
		NSString *evReceipt = [NSString stringWithUTF8String:eventReceipt];

		[KochavaTracker.shared sendEventWithNameString:evName infoString:evInfo appStoreReceiptBase64EncodedString:evReceipt];
	}

	void NativeSendDeepLink(const char *openURL, const char *sourceApplicationString)
	{
        NSString *strOpenUrl = [NSString stringWithUTF8String:openURL];
		NSURL *urlOpenUrl = [NSURL URLWithString:strOpenUrl];
		NSString *strSourceApplicationString = [NSString stringWithUTF8String:sourceApplicationString];

		[KochavaTracker.shared sendDeepLinkWithOpenURL:urlOpenUrl sourceApplicationString:strSourceApplicationString];
	}

	void NativeSendIdentityLink(const char *identityLinkDictionary)
	{
		NSString *strIdLink = [NSString stringWithUTF8String:identityLinkDictionary];        

		NSMutableDictionary *sendIdLinkDictionary = nil;
		sendIdLinkDictionary = ConvertToNSDictionary(identityLinkDictionary);

		[KochavaTracker.shared sendIdentityLinkWithDictionary:sendIdLinkDictionary];
	}

	char* NativeGetDeviceId()
	{
		NSString *kochavaTrackerDeviceIdString = KochavaTracker.shared.deviceIdString;

		const char* deviceId = AutonomousStringCopy([kochavaTrackerDeviceIdString UTF8String]);
		return AutonomousStringCopy(deviceId);		
	}

	char* NativeGetAttributionString()
	{
		NSDictionary *attributionResult = KochavaTracker.shared.attributionDictionary;
		if(attributionResult==nil) {
			const char* attributionString = "";
			return AutonomousStringCopy(attributionString);
		}

		NSError *error = nil;
       
       	NSData *attributionDictionaryJSONData = [NSJSONSerialization dataWithJSONObject:attributionResult options:0 error:&error];
       
		if (attributionDictionaryJSONData != nil)
		{
			NSString *attributionDictionaryJSONString = [[NSString alloc] initWithData:attributionDictionaryJSONData encoding:NSUTF8StringEncoding];
			return AutonomousStringCopy([attributionDictionaryJSONString UTF8String]);
		}

		const char* attributionString = "";
		return AutonomousStringCopy(attributionString);
	}

	void NativeSetAppLimitAdTrackingBool(bool value) {		
		
		[KochavaTracker.shared setAppLimitAdTrackingBool:value];		

	}




}