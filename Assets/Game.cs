using System;
using Sequence = System.Collections.IEnumerator;

/// <summary>
/// ゲームクラス。
/// 学生が編集すべきソースコードです。
/// </summary>
public sealed class Game : GameBase
{
    // 変数の宣言
    /// start --------------------------------------------------------
    // iphone X
    int window_h = 812;
    int window_w = 375;
    int gameState = 0;
    int startMiliSec, startSec, nowMiliSec, nowSec, 
        countSec, countMiliSec, resultSec, resultMiliSec;
    string title = "10秒ストップ";
    string msg;
    ///
    /// end ----------------------------------------------------------
    ///

    /// <summary>
    /// 初期化処理
    /// start --------------------------------------------------------
    /// </summary>
    public override void InitGame()
    {
        // 初期設定
        resetValue();
    }
    ///
    /// end ----------------------------------------------------------
    ///

    /// <summary>
    /// 動きなどの更新処理
    /// start --------------------------------------------------------
    /// </summary>
    public override void UpdateGame()
    {
        if(gameState == 0){
            //タイトル画面の処理
            updateTitle();
        } else if(gameState == 1){
            //ゲーム中の処理
            updateInGame();
        } else if(gameState == 2){
            //ゲームクリアー時の処理
            updateFinGame();
        }
    }
    ///
    /// end ----------------------------------------------------------
    ///

    // update methods
    void updateTitle()
    {
        //タイトル画面の処理
        msg = "Tap to start.";
        if (gc.GetPointerFrameCount(0) == 1 ){
            resetValue();
            startMiliSec = gc.CurrentMillisecond;
            startSec = gc.CurrentSecond;
            gameState = 1;
        }
    }

    void updateInGame()
    {
        //ゲーム中の処理
        //判定の部分
        msg = "10秒ピッタリで止めろ!!";
        nowSec = gc.CurrentSecond;
        nowMiliSec = gc.CurrentMillisecond;
        calCount(nowSec, nowMiliSec);
        if (gc.GetPointerFrameCount(0) == 1 ){
            gameState = 2;
        }
        
    }

    void updateFinGame()
    {
        //ゲーム終了時の処理
        resultSec = 10 - countSec;
        if (countSec < 10) {
            resultMiliSec = 1000 - countMiliSec; 
        } else {
            resultMiliSec = countMiliSec;
        }
        // long push
        if(gc.GetPointerFrameCount(0) > 120 ){
            gameState = 0;
        }
    }

    void calCount(int sec, int miliSec)
    {
        if (startSec < 50) {
           countSec = sec - startSec; 
        }else{
            if (sec > 50) { 
                countSec = sec - startSec; 
            } else {
                countSec = (60 - startSec) + sec;
            }
        }
        if (startMiliSec > miliSec) {
            countMiliSec = (1000 - startMiliSec) + miliSec;
        }else{
            countMiliSec = miliSec - startMiliSec;
        }
    }
    ///
    /// end ----------------------------------------------------------
    ///

    /// <summary>
    /// 描画の処理
    /// start --------------------------------------------------------
    /// </summary>
    public override void DrawGame()
    {
        gc.ClearScreen();
        if(gameState == 0){
            //タイトル画面の描画
            drawTitle();
        } else if(gameState == 1){
            //ゲーム中の描画
            drawInGame();
        } else if(gameState == 2){
            //ゲームクリアー時の描画
            drawFinGame();
        }
    }

    // draw methods
    void drawTitle()
    {
        //タイトル画面の描画
        gc.DrawCenterString(title, (window_w/2), (window_h/2)-20);
        gc.DrawCenterString(msg, (window_w/2), (window_h/2)+10);
    }

    void drawInGame()
    {
        //ゲーム中の描画
        if (countSec < 7){
            gc.DrawCenterString(""+countSec+"."+countMiliSec, (window_w/2), (window_h/3));
        } else {
            gc.DrawCenterString("■■■■■", (window_w/2), (window_h/3));
        }
        gc.DrawCenterString(msg, (window_w/2), (window_h/2));
    }

    void drawFinGame()
    {
        //ゲーム終了時の描画
        gc.DrawCenterString("Result: "+countSec+"."+countMiliSec, (window_w/2), (window_h/3));
        gc.DrawCenterString("Gap: "+resultSec+"."+resultMiliSec, (window_w/2), (window_h/3)+30);
        gc.DrawCenterString("長押しでリトライ!", (window_w/2), (window_h/2)+30);
    }
    ///
    /// end ----------------------------------------------------------
    ///

    public void resetValue()
    {
        // set canvas size
        gc.SetResolution(window_w, window_h);
        gameState = 0;
        
    }
}

