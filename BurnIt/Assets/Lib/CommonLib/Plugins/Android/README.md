To add Google Play Services to project, do the following:

1. Add the following line to <application> section of Assets/Plugins/Android/AndroidManifest.xml:
<meta-data android:name="com.google.android.gms.version" android:value="@integer/google_play_services_version" />

If there is no AndroidManifest.xml, just copy the one from your project's Temp/StagingArea (you need to build apk once).

2. Copy "res" folder to Assets/Plugins/Android/
