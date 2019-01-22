package com.framepush.lowpower;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.PowerManager;
import com.unity3d.player.UnityPlayer;

/**
 * @author Brian Turner
 */
public class PowerStatus implements AutoCloseable {

    public static boolean isPowerSaveModeOn() {
        PowerManager pm = (PowerManager) UnityPlayer.currentActivity.getSystemService(Context.POWER_SERVICE);
        return pm.isPowerSaveMode();
    }

    private String _name;
    private BroadcastReceiver _receiver;

    public PowerStatus(String name) {
        _name = name;
        
        startReceiver();
    }

    public void finalize() {
        close();
    }

    public void stopReceiver() {
        if (_receiver == null) return;
        UnityPlayer.currentActivity.unregisterReceiver(_receiver);
        _receiver = null;
    }

    public void startReceiver() {
        stopReceiver();

        _receiver = new BroadcastReceiver() {
            @Override
            public void onReceive(Context context, Intent intent) {
                UnityPlayer.UnitySendMessage(_name, "HandleSaverChange", isPowerSaveModeOn() ? "true" : "false");
            }
        };
        UnityPlayer.currentActivity.registerReceiver(_receiver, new IntentFilter(PowerManager.ACTION_POWER_SAVE_MODE_CHANGED));
    }

    //region AutoClosable implementations
    @Override
    public void close() {
        stopReceiver();
    }
    //endregion
}
