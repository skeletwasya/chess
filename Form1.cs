using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Linq;
using System.Numerics;

namespace chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            squareSize = pictureBox1.Width / 8; //width = 680px
            GameLogic.g = g;
            GameLogic.SquareSize = squareSize;
        }
        private Bitmap bmp;
        private Graphics g;


        //������ �����
        private int number_of_moves = 0;//���������� �����
        private List<Square[,]> savedMoves = new List<Square[,]>(); //���������� ������� (����)
        private Square[,] squares = new Square[8, 8]; //������� �������
        private static int squareSize; //������ ������
        public static int SquareSize
        {
            get { return squareSize; }
        }
        //�����
        private bool isMouseDown = false; //�������� �� ������ ���� �������
        private bool boardLoaded = false; //����� �� ������ ����������������� � ������
        private bool isGrabbable = false; //����� �� ������������� ������
        private bool whiteToMove = false; //��� �� ����� ������
        private bool promptionFieldEnabled = false; //���������� �� ������ ����������� �����
        private bool gameEnded = false; //�������� �� ����
        private bool gameStarted = false; //������ �� ����

        //����������, ����������� ��� �������������� ������
        private Piece grabbedPiece = new Empty(); //��������������� ������
        private PieceColor promotingPawnColor; //���� �����, ������� � �����������
        private Point oldGrabbedPieceCoords; //��������� ���������� ��������������� ������
        private Point promotingPawn = new Point(0, 0); //�����, �� ������� ��������� �����, ������� � �����������     
        //������������� �����
        public void SetTheBoard()
        {
            number_of_moves = 0;
            savedMoves.Clear();
            gameEnded = false;
            label2.Text = ""; //label2 - �����, � ������� ��������� ��������� � ���������� ����, ����� ��� �����������
            //������������� ������ �����
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    squares[i, j] = new Square(new Point(i * squareSize, j * squareSize), new Empty());
                }
            }
            DrawPromotionField(); //���������� ����, � ������� ���������� ����� ������ �����������. �� ��������� ��������, � ��� ���� ���������� �����������������
            //������������� ����� �� ����� ��������� ��������
            squares[0, 6].Piece = new Pawn(g, PieceColor.White);
            squares[1, 6].Piece = new Pawn(g, PieceColor.White);
            squares[2, 6].Piece = new Pawn(g, PieceColor.White);
            squares[3, 6].Piece = new Pawn(g, PieceColor.White);
            squares[4, 6].Piece = new Pawn(g, PieceColor.White);
            squares[5, 6].Piece = new Pawn(g, PieceColor.White);
            squares[6, 6].Piece = new Pawn(g, PieceColor.White);
            squares[7, 6].Piece = new Pawn(g, PieceColor.White);
            squares[0, 7].Piece = new Rook(g, PieceColor.White);
            squares[1, 7].Piece = new Knight(g, PieceColor.White);
            squares[2, 7].Piece = new Bishop(g, PieceColor.White);
            squares[3, 7].Piece = new Queen(g, PieceColor.White);
            squares[4, 7].Piece = new King(g, PieceColor.White);
            squares[5, 7].Piece = new Bishop(g, PieceColor.White);
            squares[6, 7].Piece = new Knight(g, PieceColor.White);
            squares[7, 7].Piece = new Rook(g, PieceColor.White);

            squares[0, 1].Piece = new Pawn(g, PieceColor.Black);
            squares[1, 1].Piece = new Pawn(g, PieceColor.Black);
            squares[2, 1].Piece = new Pawn(g, PieceColor.Black);
            squares[3, 1].Piece = new Pawn(g, PieceColor.Black);
            squares[4, 1].Piece = new Pawn(g, PieceColor.Black);
            squares[5, 1].Piece = new Pawn(g, PieceColor.Black);
            squares[6, 1].Piece = new Pawn(g, PieceColor.Black);
            squares[7, 1].Piece = new Pawn(g, PieceColor.Black);
            squares[0, 0].Piece = new Rook(g, PieceColor.Black);
            squares[1, 0].Piece = new Knight(g, PieceColor.Black);
            squares[2, 0].Piece = new Bishop(g, PieceColor.Black);
            squares[3, 0].Piece = new Queen(g, PieceColor.Black);
            squares[4, 0].Piece = new King(g, PieceColor.Black);
            squares[5, 0].Piece = new Bishop(g, PieceColor.Black);
            squares[6, 0].Piece = new Knight(g, PieceColor.Black);
            squares[7, 0].Piece = new Rook(g, PieceColor.Black);
            whiteToMove = true;

            //������� �����, ���������� ���������� �����, ������ ��������� ����� �����, ������, � ����� � ������ �������� ���� ��������
            textBox1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            //��������� ���������� ����� � ����� ����
            textBox1.Text = number_of_moves.ToString();
            savedMoves.Add(GameLogic.CopyThePosition(squares));

        }
        //�������� ��� �������

        //������� ����� ������� ������

        //���������� �����
        private void DrawTheBoard()
        {
            g.Clear(pictureBox1.BackColor);
            //��������� ������
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Math.Abs(i - j) % 2 != 0)
                    {
                        //��������� ������ ������
                        g.FillRectangle(new SolidBrush(Color.FromArgb(115, 92, 68)), i * squareSize, j * squareSize, squareSize, squareSize);
                    }
                    else
                    {
                        //��������� ����� ������
                        g.FillRectangle(new SolidBrush(Color.FromArgb(200, 180, 165)), i * squareSize, j * squareSize, squareSize, squareSize);
                    }
                }
            }
            //���������� ������
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    squares[i, j].Piece.Draw(squares[i, j].AbsoluteCoords);
                }
            }
            pictureBox1.Image = bmp;
        }
        //���������� ����, � ������� ���������� ����� ������ �����������
        private void DrawPromotionField()
        {
            //������������� ������� ��� ����, � ������� ���������� ����� ������ �����������
            Bitmap bmp1 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g1 = Graphics.FromImage(bmp1);
            Font font = new Font(new FontFamily("Times New Roman"), 16, GraphicsUnit.Pixel);
            SolidBrush textBrush = new SolidBrush(Color.Black);
            int step = pictureBox2.Width / 4;
            //���������� ����� "�����", "�����", "����", "����" ���� ��� ������
            g1.DrawString("�����", font, textBrush, 0, 0);
            g1.DrawString("�����", font, textBrush, 0, step);
            g1.DrawString("����", font, textBrush, 0, step * 2);
            g1.DrawString("����", font, textBrush, 0, step * 3);

            pictureBox2.Image = bmp1;
        }
        //��� ������� ����� ������ ����
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //���� � ������ ����� ����������������� � ���� ���� �� ��������
            if (boardLoaded && !gameEnded)
            {
                //���������� ����� ������ ���, ����� ���������� ������ ���������� �������
                DrawTheBoard();
                //���������� ������, ������� ������������� �����������
                Square tmp = GameLogic.SquareThatFitsCoords(e.X, e.Y, squares);
                //��������, �������� �� ����� ���� ������
                if (tmp.Piece is Empty ||
                    (tmp.Piece.PieceColor == PieceColor.White && !whiteToMove) ||
                    (tmp.Piece.PieceColor == PieceColor.Black && whiteToMove))
                {
                    isGrabbable = false;
                }
                else
                {
                    //���������� �������� ���������� isGrabbable = true, ��� ��������, ��� ������ ���������� ����������� ������ �� �����
                    isGrabbable = true;
                    //��������� ������������ ������
                    grabbedPiece = tmp.Piece;
                    //��������� ��������� ��������� ������������ ������
                    oldGrabbedPieceCoords = new Point(tmp.AbsoluteCoords.X, tmp.AbsoluteCoords.Y);
                    //������� ������ �� ������������ ������
                    squares[tmp.AbsoluteCoords.X / squareSize, tmp.AbsoluteCoords.Y / squareSize].Piece = new Empty();
                }
                //��������� ������� ������ ����
                isMouseDown = true;
            }
        }
        //��� ����������� �������
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //���� ����� ��������� ������� ������ ���� � ���� ���������� ����������� ������
            if (isMouseDown && isGrabbable)
            {
                //���������� ����� ������ ���, ����� ���������� ������ ���������� �������
                DrawTheBoard();
                //���� ������ ����� �� ������� �����, ������� ������ �� �������� �������, ����� �������� ������ ��� ����������� �������
                if (e.X < 0 || e.X > pictureBox1.Width || e.Y < 0 || e.Y > pictureBox1.Width)
                {
                    //���������� ������ �� �������� �������
                    grabbedPiece.Draw(new Point(oldGrabbedPieceCoords.X, oldGrabbedPieceCoords.Y));
                    //������� ������ �� �������� ������
                    squares[oldGrabbedPieceCoords.X / squareSize, oldGrabbedPieceCoords.Y / squareSize].Piece = grabbedPiece;
                    isGrabbable = false;
                    grabbedPiece = new Empty();
                    isMouseDown = false;
                }
                else
                {
                    //������ ���, ����� ���������� ���������� �������, ���������� ��������������� ������ ���, ����� � ����� ��������� �� ����� �������
                    grabbedPiece.Draw(new Point(e.X - squareSize / 2, e.Y - squareSize / 2));
                    //�������, ����������� ��� ����, ����� ������ �������� ��������
                    this.Refresh();
                }
            }
        }
        //����� ����, ��� ����� ������ ���� ���� ��������
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            //���� � ������ ����� �����������������
            if (boardLoaded)
            {
                //������� ���� ������ �� ����������
                isMouseDown = false;
                //���������� ������, ��������������� �����������
                Square tmp = GameLogic.SquareThatFitsCoords(e.X, e.Y, squares);
                //���������� ����� ������ ���, ����� ���������� ������ ���������� �������
                DrawTheBoard();
                //���� ���������� ����������� ������
                if (isGrabbable)
                {
                    //tmp.AbsoluteCoords.X / squareSize : �� ���� ������� �������� ������ �������� ������� ������
                    int x = tmp.AbsoluteCoords.X / squareSize;
                    int y = tmp.AbsoluteCoords.Y / squareSize;
                    int old_x = oldGrabbedPieceCoords.X / squareSize;
                    int old_y = oldGrabbedPieceCoords.Y / squareSize;
                    //���� ��������� ������ ����� ������� � ������� ������
                    if (grabbedPiece.Movable(oldGrabbedPieceCoords, tmp.AbsoluteCoords, squares))
                    {
                        //�������� ����� �����, ��� ����� ���������, ����� �� ������, ����� ���������� ����, ���� ��� �����
                        Square[,] n_squares = new Square[8, 8];
                        n_squares = GameLogic.CopyThePosition(squares);
                        //�������� ����� ������, �� ����� ������� ������������ ������
                        Piece p_tmp = n_squares[x, y].Piece;
                        //���� ����� ����� �� ���������� ���� � ������ ������������
                        if (grabbedPiece is Pawn && (y == 0 || y == 7))
                        {
                            //���������� � ������ ������ �����
                            n_squares[x, y].Piece = grabbedPiece;
                            //���� ����� ���������� ���� ������ �� ��� �����
                            if (!GameLogic.IsSquareHitByEnemy(n_squares, GameLogic.FindKing(n_squares, whiteToMove ? PieceColor.White : PieceColor.Black), !whiteToMove))
                            {
                                //���������� ���������� � ���� �������������� �����
                                promotingPawn = new Point(x, y);
                                promotingPawnColor = grabbedPiece.PieceColor;
                                //������� �������� � ������� ����, ��� ���������� ����� ������ �����������
                                promptionFieldEnabled = true;
                                pictureBox2.Visible = true;
                                //������� ���, ����� �������������� � ������ ����� �����������
                                boardLoaded = false;
                                //�������������� �������������� ������
                                button2.Enabled = false;
                                button3.Enabled = false;
                                button4.Enabled = false;
                                button5.Enabled = false;
                                button6.Enabled = false;
                            }
                            else
                            {
                                //����� ������� ������, �� ����� ������� �������� ������������� �����, �� �������� �������
                                n_squares[x, y].Piece = p_tmp;
                            }
                        }
                        //� ������, ���� �������������� �����������, �������� ��������������� ������������ ������
                        n_squares[x, y].Piece = grabbedPiece;

                        //��������� ��������� JustAnPassanted � JustCastled
                        
                        n_squares = GameLogic.SetJustCastledAndReturn(n_squares, x, y, grabbedPiece);

                        //���� ����� ���������� ���� ��� ������ �� ��� �����
                        if (!GameLogic.IsSquareHitByEnemy(n_squares, GameLogic.FindKing(n_squares, whiteToMove ? PieceColor.White : PieceColor.Black), !whiteToMove))
                        {
                            //�������� ��������� AnPassantable
                            n_squares = GameLogic.SetAnPassantableAndReturn(n_squares, old_x, old_y, x, y);
                            n_squares = GameLogic.SetJustAnPassantedAndReturn(n_squares, x, y);
                            //������������ ������ �������������
                            n_squares[x, y].Piece.Moved = true;
                            //������������ ����� ������� �������� �������
                            squares = GameLogic.CopyThePosition(n_squares);
                            //���������� ���������� �����
                            DrawTheBoard();
                            //�������� ��� ���������
                            whiteToMove = !whiteToMove;
                            //�������� ������� � ������ ��������� �����
                            savedMoves.Add(GameLogic.CopyThePosition(squares));
                            //���������� ����� ++
                            number_of_moves++;
                            //��������� ���������� ����� �� ������
                            textBox1.Text = number_of_moves.ToString();
                            //����������, ���������� ����� ��������� ������
                            string endingLine;
                            //������� ������
                            //���� ����� �� ����� ������ ������� ����
                            if (!GameLogic.CanPlayerMove(whiteToMove ? PieceColor.White : PieceColor.Black, squares, whiteToMove))
                            {
                                //���� ��������
                                gameEnded = true;
                                //���� ��� ����, ������ ������ ��� �����, �� �� ��������
                                if (GameLogic.IsSquareHitByEnemy(n_squares, GameLogic.FindKing(n_squares, whiteToMove ? PieceColor.White : PieceColor.Black), !whiteToMove))
                                {
                                    endingLine = (whiteToMove ? "׸����" : "�����") + " ���������! ���!";
                                }
                                else
                                {
                                    endingLine = "�����! ���!";
                                }
                                label2.Text = endingLine;
                            }
                            //�������� �� ����������� ���������� �����
                            if (savedMoves.Count >= 9)
                            {
                                if (GameLogic.CompareTwoPositions(savedMoves[savedMoves.Count - 1], savedMoves[savedMoves.Count - 5]) &&
                                    GameLogic.CompareTwoPositions(savedMoves[savedMoves.Count - 5], savedMoves[savedMoves.Count - 9]) &&
                                    GameLogic.CompareTwoPositions(savedMoves[savedMoves.Count - 9], savedMoves[savedMoves.Count - 1]))
                                {
                                    //���� ��������
                                    gameEnded = true;
                                    endingLine = "�����! ����������� ���������� �����!";
                                    label2.Text = endingLine;
                                }
                            }
                        }
                        //���� ������ ����� ���������� ���� ��������� ��� �����
                        else
                        {
                            //����������� ������������ ������ �� ������, ���� ������������ ��������������� ������
                            n_squares[x, y].Piece = p_tmp;
                            //���������� ��������������� ������ �� ����� ����������� ������
                            grabbedPiece.Draw(new Point(oldGrabbedPieceCoords.X, oldGrabbedPieceCoords.Y));
                            //������� ��������������� ������ ������� � ������
                            squares[old_x, old_y].Piece = grabbedPiece;
                        }
                    }
                    //���� ��������� ������ �� ����� ������� � ��������� ������
                    else
                    {
                        //���������� ��������������� ������ �� ����� ����������� ������
                        grabbedPiece.Draw(new Point(oldGrabbedPieceCoords.X, oldGrabbedPieceCoords.Y));
                        //������� ��������������� ������ ������� � ������
                        squares[old_x, old_y].Piece = grabbedPiece;
                    }
                }
                //������ �� ���������� �������������� ������
                isGrabbable = false;
                //�������� ��������������� ������
                grabbedPiece = new Empty();
            }
        }
        //��� ������� �� ����������� ����, ��������������� ��� ������ ������, � ������� ����������� �����
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            //���� � ����� ����� �����������������
            if (promptionFieldEnabled)
            {
                //��������� ��������� �������������� �����
                int x = promotingPawn.X;
                int y = promotingPawn.Y;
                int step = pictureBox2.Height / 4;
                //���� ���������� ������� �� ������� "�����", "�����", "����" ��� "����", ���������� ����� � ��������������� ������ 
                if (e.Y >= 0 && e.Y < step)
                {
                    squares[x, y].Piece = new Queen(g, promotingPawnColor);
                }
                else if (e.Y >= step && e.Y < step * 2)
                {
                    squares[x, y].Piece = new Rook(g, promotingPawnColor);
                }
                else if (e.Y >= step * 2 && e.Y < step * 3)
                {
                    squares[x, y].Piece = new Knight(g, promotingPawnColor);
                }
                else if (e.Y >= step * 3 && e.Y < step * 4)
                {
                    squares[x, y].Piece = new Bishop(g, promotingPawnColor);
                }
                //���������� ����� ��������������� ����������� ����������������� � ������,
                //��������� � �������� ����, ��������������� ��� ������ ������, � ������� ������������ �����,
                //� ��� �� ����� ���������� �������������� ������
                boardLoaded = true;
                pictureBox2.Visible = false;
                promptionFieldEnabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                //�������� ������� � ������ ��������� �����
                savedMoves[savedMoves.Count - 1] = GameLogic.CopyThePosition(squares);
                //���������� ����� �������
                DrawTheBoard();
            }
        }
        //������ ������� ����� ������
        public void Start()
        {
            //���������� ��� ������� �������� � ��������� ���������
            SetTheBoard();
            //���������� �����
            DrawTheBoard();
            //� ������ ����� �����������������
            boardLoaded = true;
            //���� ��������
            gameStarted = true;
        }

        //������, ������� ��������� ����
        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            //���� ���� ��� �� �������� ��� ���� ��� ��� �����������
            if (!gameStarted || gameEnded)
            {
                //��������� ����� ������
                Start();
            }
            //�����, ������� ���������� ���� � �������� � ���, ������� �� ��������� ������� ������ � �������� ����� 
            else
            {
                Form2 frm2 = new Form2();
                frm2.MainForm = this;
                frm2.Show();
            }

        }
        //��������� ����� �����
        private void button3_Click(object sender, EventArgs e)
        {
            if (number_of_moves > savedMoves.Count) throw new ArgumentOutOfRangeException();
            //���� ������ ���� �� ���� ���
            if (number_of_moves != 0)
            {
                //�������� ������� �� ���� ��� �����
                number_of_moves--;
                squares = GameLogic.CopyThePosition(savedMoves[number_of_moves]);
                //���� �� ������������ ��������� �������, �������������� � ������ ���������
                boardLoaded = false;
                //����� ������ ��������� ������������� ������� �� ��� ������
                button6.BackColor = Color.LightGray;
                //���������� �����
                DrawTheBoard();
            }
        }
        //��������� ����� �����
        private void button4_Click(object sender, EventArgs e)
        {
            if (number_of_moves > savedMoves.Count || savedMoves.Count == 0) throw new ArgumentOutOfRangeException();
            //���� ����� ����� �� ����� ����� ���������� ������� - 1
            if (number_of_moves != savedMoves.Count - 1)
            {
                //�������� ������� �� ���� ��� �����
                number_of_moves++;
                squares = GameLogic.CopyThePosition(savedMoves[number_of_moves]);
                //���� ����� ����� ����� ����� ����� ���������� ������� - 1
                if (number_of_moves == savedMoves.Count - 1)
                {
                    //������� �������������� � ������
                    boardLoaded = true;
                    //����� ������ ��������� ����������� ������� �� ��� ������
                    button6.BackColor = Color.White;
                }
                DrawTheBoard();
            }
        }
        //��������� ����� � �����
        private void button5_Click(object sender, EventArgs e)
        {
            if (number_of_moves > savedMoves.Count || savedMoves.Count == 0) throw new ArgumentOutOfRangeException();
            number_of_moves = savedMoves.Count - 1;
            //������� �������������� � ������
            boardLoaded = true;
            button6.BackColor = Color.White;
            //�������� ������� � �����
            squares = GameLogic.CopyThePosition(savedMoves[number_of_moves]);
            //���������� �����
            DrawTheBoard();
        }

        //������ �������� ����
        private void button6_Click(object sender, EventArgs e)
        {
            if (number_of_moves > savedMoves.Count || savedMoves.Count == 0) throw new ArgumentOutOfRangeException();
            //���� ������� ������� ��� ������� ���������� ���������� ���� � ��� ����, ������ ���� �� ���� ���
            if (number_of_moves == savedMoves.Count - 1 && savedMoves.Count != 1)
            {
                //������� ��������� ������� ������� �������
                savedMoves.RemoveAt(savedMoves.Count - 1);
                //���� ������ �� ��������(� ������ ���� ��������� ��� ����� � ��������� ����)
                gameEnded = false;
                //�������� ������ ��������� ����
                label2.Text = "";
                //�������� ������� �� ���� ��� �����
                number_of_moves--;
                squares = GameLogic.CopyThePosition(savedMoves[number_of_moves]);
                //��������� ���������� ��������� �����
                textBox1.Text = Convert.ToString(number_of_moves);
                //�������� ��� ���������
                whiteToMove = !whiteToMove;
                //���������� �����
                DrawTheBoard();
            }
        }
        //��������� � ������ ������
        private void button2_Click(object sender, EventArgs e)
        {
            if(savedMoves.Count != 1)
            {
                //���� �� ������������ ��������� �������, �������������� � ������ ���������
                boardLoaded = false;
                //�������� ������� � ������
                number_of_moves = 0;
                squares = GameLogic.CopyThePosition(savedMoves[number_of_moves]);
                //����� ������ ��������� ������������� ������� �� ��� ������
                button6.BackColor = Color.LightGray;
                DrawTheBoard();
            }
        }
    }
}
