using System;
using System.Drawing;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

namespace Game_Caro
{
    partial class GameCaro : Form
    {
        #region Properties
        GameBoard board;
        SocketManager socket;
        Socket playerSocket;
        Socket opponentSocket;
        string PlayerName;

        public GameCaro(Socket playerSocket, Socket opponentSocket)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            this.playerSocket = playerSocket;
            this.opponentSocket = opponentSocket;

            board = new GameBoard(pn_GameBoard, txt_PlayerName, pb_Avatar);
            board.PlayerClicked += Board_PlayerClicked;
            board.GameOver += Board_GameOver;

            pgb_CountDown.Step = Constance.CountDownStep;
            pgb_CountDown.Maximum = Constance.CountDownTime;

            tm_CountDown.Interval = Constance.CountDownInterval;
            socket = new SocketManager();

            txt_Chat.Text = "";

            NewGame();
            Listen();
        }
        #endregion

        #region Methods

        void NewGame()
        {
            pgb_CountDown.Value = 0;
            tm_CountDown.Stop();

            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = true;

            btn_Undo.Enabled = true;
            btn_Redo.Enabled = true;

            board.DrawGameBoard();
        }

        void EndGame()
        {
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;

            btn_Undo.Enabled = false;
            btn_Redo.Enabled = false;

            tm_CountDown.Stop();
            pn_GameBoard.Enabled = false;
        }

        private void GameCaro_Load(object sender, EventArgs e)
        {
            lbl_About.Text = "Tic Tac Toe project in\nC# WinForms\n-- ♦ ♦ ♦ --\nWritten by: Quân Đặng";
            tm_About.Enabled = true;
        }

        private void GameCaro_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                e.Cancel = true;
            else
            {
                try
                {
                    socket.Send(opponentSocket, new SocketData((int)SocketCommand.QUIT, "", new Point()));
                }
                catch { }
            }
        }

        private void Board_PlayerClicked(object sender, BtnClickEvent e)
        {
            tm_CountDown.Start();
            pgb_CountDown.Value = 0;

            try
            {
                pn_GameBoard.Enabled = false;
                socket.Send(opponentSocket, new SocketData((int)SocketCommand.SEND_POINT, "", e.ClickedPoint));

                undoToolStripMenuItem.Enabled = false;
                redoToolStripMenuItem.Enabled = false;

                btn_Undo.Enabled = false;
                btn_Redo.Enabled = false;

                Listen();
            }
            catch
            {
                EndGame();
                MessageBox.Show("Không có kết nối nào tới máy đối thủ", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Board_GameOver(object sender, EventArgs e)
        {
            EndGame();

            socket.Send(opponentSocket, new SocketData((int)SocketCommand.END_GAME, "", new Point()));
        }

        private void Tm_CountDown_Tick(object sender, EventArgs e)
        {
            pgb_CountDown.PerformStep();

            if (pgb_CountDown.Value >= pgb_CountDown.Maximum)
            {
                EndGame();

                socket.Send(opponentSocket, new SocketData((int)SocketCommand.TIME_OUT, "", new Point()));
            }
        }

        private void Tm_About_Tick(object sender, EventArgs e)
        {
            lbl_About.Location = new Point(lbl_About.Location.X, lbl_About.Location.Y - 2);

            if (lbl_About.Location.Y + lbl_About.Height < 0)
                lbl_About.Location = new Point(lbl_About.Location.X, Grb_About.Height - 10);
        }

        private void Listen()
        {
            Thread ListenThread = new Thread(() =>
            {
                try
                {
                    SocketData data = (SocketData)socket.Receive(playerSocket);
                    ProcessData(data);
                }
                catch { }
            });

            ListenThread.IsBackground = true;
            ListenThread.Start();
        }

        private void ProcessData(SocketData data)
        {
            PlayerName = board.ListPlayers[board.CurrentPlayer == 1 ? 0 : 1].Name;

            switch (data.Command)
            {
                case (int)SocketCommand.SEND_POINT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        board.OtherPlayerClicked(data.Point);
                        pn_GameBoard.Enabled = true;

                        pgb_CountDown.Value = 0;
                        tm_CountDown.Start();

                        undoToolStripMenuItem.Enabled = true;
                        redoToolStripMenuItem.Enabled = true;

                        btn_Undo.Enabled = true;
                        btn_Redo.Enabled = true;
                    }));
                    break;

                case (int)SocketCommand.SEND_MESSAGE:
                    txt_Chat.Text = data.Message;
                    break;

                case (int)SocketCommand.NEW_GAME:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        NewGame();
                        pn_GameBoard.Enabled = false;
                    }));
                    break;

                case (int)SocketCommand.UNDO:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        pgb_CountDown.Value = 0;
                        board.Undo();
                    }));
                    break;

                case (int)SocketCommand.REDO:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        board.Redo();
                    }));
                    break;

                case (int)SocketCommand.END_GAME:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        EndGame();
                        MessageBox.Show(PlayerName + " đã chiến thắng ♥ !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                    break;

                case (int)SocketCommand.TIME_OUT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        EndGame();
                        MessageBox.Show("Hết giờ rồi !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                    break;

                case (int)SocketCommand.QUIT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        tm_CountDown.Stop();
                        EndGame();

                        board.PlayMode = 2;
                        socket.CloseConnect();

                        MessageBox.Show("Đối thủ đã chạy mất dép", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                    break;

                default:
                    break;
            }

            Listen();
        }
        #endregion
    }
}
