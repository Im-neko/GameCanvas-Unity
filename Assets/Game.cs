
using Sequence = System.Collections.IEnumerator;

/// <summary>
/// ゲームクラス。
/// 学生が編集すべきソースコードです。
/// </summary>
public sealed class Game : GameBase
{
    // 変数の宣言
    int sec = 0;
    const int BALL_NUM = 30;
    int[] ball_x = new int [BALL_NUM];
    int[] ball_y = new int [BALL_NUM];
    int[] ball_col = new int [BALL_NUM];
    int[] ball_speed = new int [BALL_NUM];
    int ball_w = 24;
    int ball_h = 24;
    int player_x = 304;
    int player_y = 448;
    int player_speed = 3;
    int player_w = 32;
    int player_h = 32;
    int player_img = 4;
    int player_col = 4;
    int combo = 0;
    int score = 0;
    int time = 1800;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void InitGame()
    {
        // キャンバスの大きさを設定します
        gc.SetResolution(640, 480);
        for(int i =0 ; i < BALL_NUM ; i ++ )
        {
            resetBall(i);
        }
    }

    /// <summary>
    /// 動きなどの更新処理
    /// </summary>
    public override void UpdateGame()
    {
        // // 起動からの経過時間を取得します
        // sec = (int)gc.TimeSinceStartup;
        time -= 1;
        for(int i =0 ; i < BALL_NUM ; i ++ )
        {
            ball_y[i] = ball_y[i] + ball_speed[i];
            if(gc.CheckHitRect(ball_x[i], ball_y[i], ball_w, ball_h, player_x, player_y, player_w, player_h)){
                if(player_col == ball_col[i]) {
                    combo++;
                    score += combo;
                }
                else {
                    combo = 0;
                }
                if(time > 0){
                    score += ball_col[i];
                }
                player_col = ball_col[i];
                resetBall(i);
            }
            if (ball_y[i] > 480) {
                resetBall(i);
            }
        }

        if(gc.GetPointerFrameCount(0) > 0 ){
            if(gc.GetPointerX(0) > 320) {
                player_x += player_speed;  
                player_img = 4;
            } else {
                player_x -= player_speed;  
                player_img = 5;
            }
        }
    }

    /// <summary>
    /// 描画の処理
    /// </summary>
    public override void DrawGame()
    {
        gc.ClearScreen();
        DrawClipImage(player_img,player_x,player_y,0,64,32,32);
        for(int i =0 ; i < BALL_NUM ; i ++ ){
            gc.DrawImage(ball_col[i],ball_x[i],ball_y[i]);  
        }
        if(time > 0){
            int u = 32+ ((time%60)/30)*32;
            int v = (player_col - 1) *32;
            DrawClipImage(player_img,player_x,player_y, u,v,32,32);
        }
        else {
            DrawClipImage(player_img,player_x,player_y, 96,(player_col - 1) *32,32,32);
        }
        gc.SetColor(0,0,0);
        if(time > 0){
              gc.DrawString("time:"+time,0,0);
        }
        else {
              gc.DrawString("finished!!",0,0);
        }
        gc.DrawString("score:"+score,0,24);
        gc.DrawString("combo:"+combo,0,48);
    }

    void resetBall(int id){
        ball_x[id] = gc.Random(0,616);
        ball_y[id] = -gc.Random(24,480);
        ball_speed[id] = gc.Random(3,6);
        ball_col[id] = gc.Random(1,3);
    }

    void DrawClipImage(int img,int x,int y,int u,int v,int w,int h){
        gc.DrawClipImage(img,x-u/2,y-v/2,u/2,v/2,w,h);
    }
}
