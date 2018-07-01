using UnityEngine;
using System.Collections;
using MTUnity.Utils;

public class TrackData  {

    public string StartStateName ="null";
    public string LvId = "null";

    public string trackMode = "null";

    public string Win = "null";

    const string TrackGameStateName = "GameStateName";
    const string TrackLvId = "LvId";

    const string TrackWin = "Win";


    public MTJSONObject ToJson()
    {
        var js = MTJSONObject.CreateDict();
        js.Add(TrackGameStateName, StartStateName);
        js.Add(TrackLvId, LvId);
        js.Add("trackMode", trackMode);
        js.Add(TrackWin, Win);

        return js;

    }

    public void InitBy(MTJSONObject js)
    {
        StartStateName = js.GetString(TrackGameStateName);
        LvId = js.GetString(TrackLvId);
        trackMode = js.GetString("trackMode");
        Win = js.GetString(TrackWin);

    }


}
