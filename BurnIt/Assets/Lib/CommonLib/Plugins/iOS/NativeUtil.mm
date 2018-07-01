//
//  NativeUtil.m
//  Created by Zhou Yuntao on 12/12/2015.
//
//

#import <AdSupport/AdSupport.h>
#include <string>

using namespace std;

const int DEVICE_ID_TYPE_UNITY = 0;
const int DEVICE_ID_TYPE_IDFA = 1;
const int DEVICE_ID_TYPE_IDFV = 2;
const int DEVICE_ID_TYPE_AAID = 3;

string getIDFV() {
    return [[[[UIDevice currentDevice] identifierForVendor] UUIDString] UTF8String];
}

string getIDFA() {
    if ([[ASIdentifierManager sharedManager] isAdvertisingTrackingEnabled]) {
        NSUUID *idfa = [[ASIdentifierManager sharedManager] advertisingIdentifier];
        if (idfa) {
            return [[idfa UUIDString] UTF8String];
        }
    }
    // fall back to IDFV
    return "";
}

extern "C" {
    char *GetDeviceIdentifierRaw() {
        char *raw = (char*)malloc(64);
        int type = DEVICE_ID_TYPE_IDFA;
        string did = getIDFA();
        if (did == "") {
            type = DEVICE_ID_TYPE_IDFV;
            did = getIDFV();
        }
        snprintf(raw, 63, "%d%s", type, did.c_str());
        return raw;
    }
}
