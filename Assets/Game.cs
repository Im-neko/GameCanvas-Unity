
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
    int player_speed = 3;
    int active_box_num = 0;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void InitGame()
    {
        // キャンバスの大きさを設定します
        gc.SetResolution(720, 1280);
        gc.SetResolution(640, 480);
        camera_id = 0;
        camera_name = gc.GetCameraDeviceName(camera_id);
        gc.StartCameraService(camera_id);
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

        active_box_num = 5 + count/600;
        if(active_box_num > BOX_NUM){
              active_box_num = BOX_NUM;
        }
        for(int i = 0;i < active_box_num; i++ ){
          //繰り返す内容（箱を動かす、プレイヤーとの接触判定）

        }
    }

    /// <summary>
    /// 描画の処理
    /// </summary>
    public override void DrawGame()
    {
        gc.ClearScreen();
        gc.DrawScaledRotateCameraImage(100, 100, 25, 25, gc.CurrentCameraRotation);
        gc.DrawScaledRotateCameraImage(player_x,player_y,5,7,gc.CurrentCameraRotation);
        gc.DrawString(camera_name, 15, 15);
    }
}
