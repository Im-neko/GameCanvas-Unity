using Sequence = System.Collections.IEnumerator;

/// <summary>
/// ゲームクラス。
/// 学生が編集すべきソースコードです。
/// </summary>
public sealed class Game : GameBase
{
    // 変数の宣言
    int sec = 0;
    int camera_id;
    string camera_name;
    int active_box_num = 0;
    int high_score = 0;

    string pname = "t16076yi";
    string url = "";

    int gameState = 0;

    const int BOX_NUM = 10;
    int[] box_x = new int [BOX_NUM];
    int[] box_y = new int [BOX_NUM];
    int[] box_speed = new int [BOX_NUM];
    int box_w = 24;
    int box_h = 24;

    int player_x = 304;
    int player_y = 400;
    int player_dir = 1;
    int player_speed = 3;


    int score = 0;
    int count = 0;
    string str = "";
    string title = "Title";
    string resultMsg = "GameOver";

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void InitGame()
    {
        // キャンバスの大きさを設定します
        resetValue();
    }

    /// <summary>
    /// 動きなどの更新処理
    /// </summary>
    public override void UpdateGame()
    {
        // 起動からの経過時間を取得します
        sec = (int)gc.TimeSinceStartup;
        if (gc.GetPointerFrameCount(0) ==1 ){
            camera_id++;
            if (camera_id >= gc.CameraDeviceCount) camera_id= 0;
            camera_name = gc.GetCameraDeviceName(camera_id);
            gc.StartCameraService(camera_id);
        }

        //calc,drawの構成
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

    /// <summary>
    /// 描画の処理
    /// </summary>
    public override void DrawGame()
    {
        gc.ClearScreen();
        //calc,drawの構成
        gc.SetColor(0, 0, 0);
        if(gameState == 0){
            //タイトル画面の処理
            drawTitle();
        } else if(gameState == 1){
            //ゲーム中の処理
            drawInGame();
        } else if(gameState == 2){
            //ゲームクリアー時の処理
            drawFinGame();
        }
    }

    public void resetValue()
    {
        gc.SetResolution(640, 480);
        camera_id = 0;
        camera_name = gc.GetCameraDeviceName(camera_id);
        gc.StartCameraService(camera_id);

        score = 0;
        count = 0;
        player_x = 304;
        player_y = 400;
        player_dir = 1;
        player_speed = 5;
        for(int i =0 ; i < BOX_NUM ; i ++ ) {
            box_x[i] = gc.Random(0,616);
            box_y[i] = -gc.Random(100,480);
            box_speed[i] = gc.Random(3,6);
        }
        high_score = gc.Load(0);
    }

    // update methods
    void updateTitle()
    {
        //タイトル画面の処理
        if(gc.GetPointerFrameCount(0) == 1 ){
            resetValue();
            url = "http://web.sfc.keio.ac.jp/~wadari/sdp/k07_web/score.cgi?score=-1&name=" + pname;
            gc.GetOnlineTextAsync(url,out str);
            gameState = 1;
        }
    }

    void updateInGame()
    {
        //ゲーム中の処理
        //判定の部分
        count++;
        score = count / 60;
        box_w = 24 + count / 300;
        box_h = 24 + count / 300; 
        if(gc.GetPointerFrameCount(0) == 1 ){
            player_dir = -player_dir;
        }
        player_x += player_dir * player_speed;
        active_box_num = 5 + count/600;
        if(active_box_num > BOX_NUM){
            active_box_num = BOX_NUM;
        }
        for(int i =0 ; i < active_box_num ; i ++ )
        {
            box_y[i] = box_y[i] + box_speed[i];
            if(box_y[i]> 480){
                box_x[i] = gc.Random(0,616);
                box_y[i] = -gc.Random(100,480);
                box_speed[i] = gc.Random(3,6);
            }
            if (gc.CheckHitRect (
                player_x,player_y,32,32,
                box_x[i],box_y[i],box_w,box_h)) {
                    gameState = 2;
            }
        }
        if(player_x < 0 || player_x > 608){
            gameState = 2;
        } 
        if(score > high_score){
              high_score = score;
        }
    }

    void updateFinGame()
    {
        //ゲーム終了時の処理
        url = "http://web.sfc.keio.ac.jp/~wadari/sdp/k07_web/score.cgi?score="
                + score + "&name=" + pname;
        gc.GetOnlineTextAsync(url,out str);
        if(gc.GetPointerFrameCount(0) > 120 ){
            url = "http://web.sfc.keio.ac.jp/~wadari/sdp/k07_web/score.cgi?score=-1&name=" + pname;
            gc.GetOnlineTextAsync(url,out str);
            gameState = 0;
        }
        gc.Save(0,high_score);
    }

    // draw methods
    void drawTitle()
    {
        //タイトル画面の処理
        gc.DrawString(title, 0, 300);
    }

    void drawInGame()
    {
        //ゲーム中の処理
        gc.DrawString("score" + score, 0, 10);
        gc.DrawString("HIGH:"+high_score,0,60);
        gc.DrawScaledRotateCameraImage(player_x,player_y,5,7,gc.CurrentCameraRotation);
        for(int i =0 ; i < BOX_NUM ; i ++ ){
            gc.FillRect(box_x[i],box_y[i],box_w,box_h);  
        }
    }

    void drawFinGame()
    {
        //ゲーム終了時の処理
        gc.DrawString("Game Over", 0, 200);
        gc.DrawString("score" + score, 0, 300);
        gc.DrawString("HIGH:"+high_score,0,60);
        gc.DrawString(""+str, 0, 400);
    }

}

