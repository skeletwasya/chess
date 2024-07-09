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


        //Клетки доски
        private int number_of_moves = 0;//Количество ходов
        private List<Square[,]> savedMoves = new List<Square[,]>(); //Сохранённые позиции (ходы)
        private Square[,] squares = new Square[8, 8]; //Текущая позиция
        private static int squareSize; //Размер клетки
        public static int SquareSize
        {
            get { return squareSize; }
        }
        //Флаги
        private bool isMouseDown = false; //Является ли кнопка мыши нажатой
        private bool boardLoaded = false; //Можно ли игроку взаимодействовать с доской
        private bool isGrabbable = false; //Можно ли перетаскивать фигуру
        private bool whiteToMove = false; //Ход ли белых сейчас
        private bool promptionFieldEnabled = false; //Происходит ли сейчас превращение пешки
        private bool gameEnded = false; //Окончена ли игра
        private bool gameStarted = false; //Начата ли игра

        //Переменные, необходимые для перетаскивания фигуры
        private Piece grabbedPiece = new Empty(); //Перетаскиваемая фигура
        private PieceColor promotingPawnColor; //Цвет пешки, готовой к превращению
        private Point oldGrabbedPieceCoords; //Начальные координаты перетаскиваемой фигуры
        private Point promotingPawn = new Point(0, 0); //Точка, на которой находится пешка, готовая к превращению     
        //Инициализация доски
        public void SetTheBoard()
        {
            number_of_moves = 0;
            savedMoves.Clear();
            gameEnded = false;
            label2.Text = ""; //label2 - текст, в котором находится сообщение о результате игры, когда она завершается
            //Инициализация клеток доски
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    squares[i, j] = new Square(new Point(i * squareSize, j * squareSize), new Empty());
                }
            }
            DrawPromotionField(); //Нарисовать поле, в котором происходит выбор фигуры превращения. По умолчанию невидимо, с ним пока невозможно взаимодействовать
            //Инициализация фигур на своих начальных позициях
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

            //Сделать текст, отражающий количество ходов, кнопки прокрутки ходов назад, вперед, в конец и кнопку возврата хода видимыми
            textBox1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            //Установка количества ходов в текст бокс
            textBox1.Text = number_of_moves.ToString();
            savedMoves.Add(GameLogic.CopyThePosition(squares));

        }
        //Сравнить две позиции

        //Создать копию массива клеток

        //Отрисовать доску
        private void DrawTheBoard()
        {
            g.Clear(pictureBox1.BackColor);
            //Закрасить клетки
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Math.Abs(i - j) % 2 != 0)
                    {
                        //Закрасить чёрные клетки
                        g.FillRectangle(new SolidBrush(Color.FromArgb(115, 92, 68)), i * squareSize, j * squareSize, squareSize, squareSize);
                    }
                    else
                    {
                        //Закрасить белые клетки
                        g.FillRectangle(new SolidBrush(Color.FromArgb(200, 180, 165)), i * squareSize, j * squareSize, squareSize, squareSize);
                    }
                }
            }
            //Нарисовать фигуры
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    squares[i, j].Piece.Draw(squares[i, j].AbsoluteCoords);
                }
            }
            pictureBox1.Image = bmp;
        }
        //Нарисовать поле, в котором происходит выбор фигуры превращения
        private void DrawPromotionField()
        {
            //Инициализация графики для поля, в котором происходит выбор фигуры превращения
            Bitmap bmp1 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g1 = Graphics.FromImage(bmp1);
            Font font = new Font(new FontFamily("Times New Roman"), 16, GraphicsUnit.Pixel);
            SolidBrush textBrush = new SolidBrush(Color.Black);
            int step = pictureBox2.Width / 4;
            //Нарисовать слова "Ферзь", "Ладья", "Конь", "Слон" друг под другом
            g1.DrawString("Ферзь", font, textBrush, 0, 0);
            g1.DrawString("Ладья", font, textBrush, 0, step);
            g1.DrawString("Конь", font, textBrush, 0, step * 2);
            g1.DrawString("Слон", font, textBrush, 0, step * 3);

            pictureBox2.Image = bmp1;
        }
        //При нажатии левой кнопки мыши
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //Если с доской можно взаимодействовать и если игра не окончена
            if (boardLoaded && !gameEnded)
            {
                //Отрисовать доску каждый раз, когда вызывается данный обработчик события
                DrawTheBoard();
                //Нахождение клетки, которая соответствует координатам
                Square tmp = GameLogic.SquareThatFitsCoords(e.X, e.Y, squares);
                //Проверка, выбирает ли игрок свою фигуру
                if (tmp.Piece is Empty ||
                    (tmp.Piece.PieceColor == PieceColor.White && !whiteToMove) ||
                    (tmp.Piece.PieceColor == PieceColor.Black && whiteToMove))
                {
                    isGrabbable = false;
                }
                else
                {
                    //Установить значение переменной isGrabbable = true, что означает, что сейчас происходит перемещение фигуры по доске
                    isGrabbable = true;
                    //Установка перемещаемой фигуры
                    grabbedPiece = tmp.Piece;
                    //Установка начальных координат перемещаемой фигуры
                    oldGrabbedPieceCoords = new Point(tmp.AbsoluteCoords.X, tmp.AbsoluteCoords.Y);
                    //Очистка клетки от перемещаемой фигуры
                    squares[tmp.AbsoluteCoords.X / squareSize, tmp.AbsoluteCoords.Y / squareSize].Piece = new Empty();
                }
                //Произошло нажатие кнопки мыши
                isMouseDown = true;
            }
        }
        //При перемещении курсора
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //Если ранее произошло нажатие кнопки мыши и если происходит перемещение фигуры
            if (isMouseDown && isGrabbable)
            {
                //Отрисовать доску каждый раз, когда вызывается данный обработчик события
                DrawTheBoard();
                //Если курсор вышел за пределы доски, вернуть фигуру на исходную позицию, иначе рисовать фигуру при перемещении курсора
                if (e.X < 0 || e.X > pictureBox1.Width || e.Y < 0 || e.Y > pictureBox1.Width)
                {
                    //Нарисовать фигуру на исходной позиции
                    grabbedPiece.Draw(new Point(oldGrabbedPieceCoords.X, oldGrabbedPieceCoords.Y));
                    //Вернуть фигуру на исходную клетку
                    squares[oldGrabbedPieceCoords.X / squareSize, oldGrabbedPieceCoords.Y / squareSize].Piece = grabbedPiece;
                    isGrabbable = false;
                    grabbedPiece = new Empty();
                    isMouseDown = false;
                }
                else
                {
                    //Каждый раз, когда вызывается обработчик события, отрисовать перетаскиваемую фигуру так, чтобы её центр находился на месте курсора
                    grabbedPiece.Draw(new Point(e.X - squareSize / 2, e.Y - squareSize / 2));
                    //Команда, необходимая для того, чтобы убрать мерцание анимации
                    this.Refresh();
                }
            }
        }
        //После того, как левая кнопка мыши была отпущена
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            //Если с доской можно взаимодействовать
            if (boardLoaded)
            {
                //Нажатие мыши больше не происходит
                isMouseDown = false;
                //Нахождение клетки, соответствующей координатам
                Square tmp = GameLogic.SquareThatFitsCoords(e.X, e.Y, squares);
                //Отрисовать доску каждый раз, когда вызывается данный обработчик события
                DrawTheBoard();
                //Если происходит перемещение фигуры
                if (isGrabbable)
                {
                    //tmp.AbsoluteCoords.X / squareSize : по этой формуле получаем индекс элемента массива клеток
                    int x = tmp.AbsoluteCoords.X / squareSize;
                    int y = tmp.AbsoluteCoords.Y / squareSize;
                    int old_x = oldGrabbedPieceCoords.X / squareSize;
                    int old_y = oldGrabbedPieceCoords.Y / squareSize;
                    //Если выбранная фигура может попасть в текущую клетку
                    if (grabbedPiece.Movable(oldGrabbedPieceCoords, tmp.AbsoluteCoords, squares))
                    {
                        //Создание копии доски, где будем проверять, будет ли король, после сделанного хода, быть под шахом
                        Square[,] n_squares = new Square[8, 8];
                        n_squares = GameLogic.CopyThePosition(squares);
                        //Создание копии фигуры, на место которой перемещается фигура
                        Piece p_tmp = n_squares[x, y].Piece;
                        //Если пешка дошла до последнего ряда и готова превратиться
                        if (grabbedPiece is Pawn && (y == 0 || y == 7))
                        {
                            //Установить в данную клетку пешку
                            n_squares[x, y].Piece = grabbedPiece;
                            //Если после сделанного хода король не под шахом
                            if (!GameLogic.IsSquareHitByEnemy(n_squares, GameLogic.FindKing(n_squares, whiteToMove ? PieceColor.White : PieceColor.Black), !whiteToMove))
                            {
                                //Установить координаты и цвет превращающейся пешки
                                promotingPawn = new Point(x, y);
                                promotingPawnColor = grabbedPiece.PieceColor;
                                //Сделать активным и видимым поле, где происходит выбор фигуры превращения
                                promptionFieldEnabled = true;
                                pictureBox2.Visible = true;
                                //Сделать так, чтобы взаимодействие с доской стало невозможным
                                boardLoaded = false;
                                //Деактивировать дополнительные кнопки
                                button2.Enabled = false;
                                button3.Enabled = false;
                                button4.Enabled = false;
                                button5.Enabled = false;
                                button6.Enabled = false;
                            }
                            else
                            {
                                //Иначе вернуть фигуру, на место которой пыталась переместиться пешка, на исходную позицию
                                n_squares[x, y].Piece = p_tmp;
                            }
                        }
                        //В клетку, куда предполагается перемещение, временно устанавливается перемещаемая фигура
                        n_squares[x, y].Piece = grabbedPiece;

                        //Установка состояния JustAnPassanted и JustCastled
                        
                        n_squares = GameLogic.SetJustCastledAndReturn(n_squares, x, y, grabbedPiece);

                        //Если после сделанного хода наш король не под шахом
                        if (!GameLogic.IsSquareHitByEnemy(n_squares, GameLogic.FindKing(n_squares, whiteToMove ? PieceColor.White : PieceColor.Black), !whiteToMove))
                        {
                            //Установк состояния AnPassantable
                            n_squares = GameLogic.SetAnPassantableAndReturn(n_squares, old_x, old_y, x, y);
                            n_squares = GameLogic.SetJustAnPassantedAndReturn(n_squares, x, y);
                            //Перемещаемая фигура переместилась
                            n_squares[x, y].Piece.Moved = true;
                            //Присваивание копии позиции основной позиции
                            squares = GameLogic.CopyThePosition(n_squares);
                            //Отрисовать обновлённую доску
                            DrawTheBoard();
                            //Передать ход сопернику
                            whiteToMove = !whiteToMove;
                            //Добавить позицию в массив сделанных ходов
                            savedMoves.Add(GameLogic.CopyThePosition(squares));
                            //Количество ходов ++
                            number_of_moves++;
                            //Отрисовка количества ходов на экране
                            textBox1.Text = number_of_moves.ToString();
                            //Переменная, означающая текст окончания партии
                            string endingLine;
                            //Условие победы
                            //Если игрок не может больше сделать хода
                            if (!GameLogic.CanPlayerMove(whiteToMove ? PieceColor.White : PieceColor.Black, squares, whiteToMove))
                            {
                                //Игра окончена
                                gameEnded = true;
                                //Если при этом, король игрока под шахом, то он проиграл
                                if (GameLogic.IsSquareHitByEnemy(n_squares, GameLogic.FindKing(n_squares, whiteToMove ? PieceColor.White : PieceColor.Black), !whiteToMove))
                                {
                                    endingLine = (whiteToMove ? "Чёрные" : "Белые") + " побеждают! Мат!";
                                }
                                else
                                {
                                    endingLine = "Ничья! Пат!";
                                }
                                label2.Text = endingLine;
                            }
                            //Проверка на троекратное повторение ходов
                            if (savedMoves.Count >= 9)
                            {
                                if (GameLogic.CompareTwoPositions(savedMoves[savedMoves.Count - 1], savedMoves[savedMoves.Count - 5]) &&
                                    GameLogic.CompareTwoPositions(savedMoves[savedMoves.Count - 5], savedMoves[savedMoves.Count - 9]) &&
                                    GameLogic.CompareTwoPositions(savedMoves[savedMoves.Count - 9], savedMoves[savedMoves.Count - 1]))
                                {
                                    //Игра окончена
                                    gameEnded = true;
                                    endingLine = "Ничья! Троекратное повторение ходов!";
                                    label2.Text = endingLine;
                                }
                            }
                        }
                        //Если король после сделанного хода находится под шахом
                        else
                        {
                            //Возвращение оригинальной фигуры на клетку, куда направлялася перетаскиваемая фигура
                            n_squares[x, y].Piece = p_tmp;
                            //Отрисовать перетаскиваемую фигуру на своей изначальной клетке
                            grabbedPiece.Draw(new Point(oldGrabbedPieceCoords.X, oldGrabbedPieceCoords.Y));
                            //Вернуть перетаскиваемую фигуру обратно в массив
                            squares[old_x, old_y].Piece = grabbedPiece;
                        }
                    }
                    //Если выбранная фигура не может попасть в выбранную клетку
                    else
                    {
                        //Отрисовать перетаскиваемую фигуру на своей изначальной клетке
                        grabbedPiece.Draw(new Point(oldGrabbedPieceCoords.X, oldGrabbedPieceCoords.Y));
                        //Вернуть перетаскиваемую фигуру обратно в массив
                        squares[old_x, old_y].Piece = grabbedPiece;
                    }
                }
                //Больше не происходит перетаскивание фигуры
                isGrabbable = false;
                //Очистить перетаскиваемую фигуру
                grabbedPiece = new Empty();
            }
        }
        //При нажатии на всплывающее поле, предназначенное для выбора фигуры, в которую превратится пешка
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            //Если с полем можно взаимодействовать
            if (promptionFieldEnabled)
            {
                //Установка координат превращающейся пешки
                int x = promotingPawn.X;
                int y = promotingPawn.Y;
                int step = pictureBox2.Height / 4;
                //Если происходит нажатие на надпись "Ферзь", "Ладья", "Конь" или "Слон", превратить пешку в соответствующую фигуру 
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
                //Возвращаем ранее заблокированную возможность взаимодействовать с доской,
                //блокируем и скрываем поле, предназначенное для выбора фигуры, в которую превращается пешка,
                //а так же вновь активируем дополнительные кнопки
                boardLoaded = true;
                pictureBox2.Visible = false;
                promptionFieldEnabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                //Добавить позицию в массив сделанных ходов
                savedMoves[savedMoves.Count - 1] = GameLogic.CopyThePosition(squares);
                //Нарисовать новую позицию
                DrawTheBoard();
            }
        }
        //Кнопка запуска новой партии
        public void Start()
        {
            //Установить все игровые элементы в начальное положение
            SetTheBoard();
            //Нарисовать доску
            DrawTheBoard();
            //С доской можно взаимодействовать
            boardLoaded = true;
            //Игра началась
            gameStarted = true;
        }

        //Кнопка, которая запускает игру
        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            //Если игра ещё не началась или если она уже закончилась
            if (!gameStarted || gameEnded)
            {
                //Запустить новую партию
                Start();
            }
            //Иначе, открыть диалоговое окно с вопросом о том, следует ли закончить текущию партию и начинать новую 
            else
            {
                Form2 frm2 = new Form2();
                frm2.MainForm = this;
                frm2.Show();
            }

        }
        //Прокрутка ходов назад
        private void button3_Click(object sender, EventArgs e)
        {
            if (number_of_moves > savedMoves.Count) throw new ArgumentOutOfRangeException();
            //Если сделан хотя бы один ход
            if (number_of_moves != 0)
            {
                //Откатить позицию на один ход назад
                number_of_moves--;
                squares = GameLogic.CopyThePosition(savedMoves[number_of_moves]);
                //Пока не отображается последняя позиция, взаимодействие с доской запрещено
                boardLoaded = false;
                //Серым цветом отобразим невозможность нажатия на эту кнопку
                button6.BackColor = Color.LightGray;
                //Отрисовать доску
                DrawTheBoard();
            }
        }
        //Прокрутка ходов вперёд
        private void button4_Click(object sender, EventArgs e)
        {
            if (number_of_moves > savedMoves.Count || savedMoves.Count == 0) throw new ArgumentOutOfRangeException();
            //Если число ходов не равно числу сохранённых позиций - 1
            if (number_of_moves != savedMoves.Count - 1)
            {
                //Откатить позицию на один ход вперёд
                number_of_moves++;
                squares = GameLogic.CopyThePosition(savedMoves[number_of_moves]);
                //Если число ходов стало равно числу сохранённых позиций - 1
                if (number_of_moves == savedMoves.Count - 1)
                {
                    //Вернуть взаимодействие с доской
                    boardLoaded = true;
                    //Белым цветом отобразим возможность нажатия на эту кнопку
                    button6.BackColor = Color.White;
                }
                DrawTheBoard();
            }
        }
        //Прокрутка ходов в конец
        private void button5_Click(object sender, EventArgs e)
        {
            if (number_of_moves > savedMoves.Count || savedMoves.Count == 0) throw new ArgumentOutOfRangeException();
            number_of_moves = savedMoves.Count - 1;
            //Вернуть взаимодействие с доской
            boardLoaded = true;
            button6.BackColor = Color.White;
            //Откатить позицию в конец
            squares = GameLogic.CopyThePosition(savedMoves[number_of_moves]);
            //Отрисовать доску
            DrawTheBoard();
        }

        //Кнопка возврата хода
        private void button6_Click(object sender, EventArgs e)
        {
            if (number_of_moves > savedMoves.Count || savedMoves.Count == 0) throw new ArgumentOutOfRangeException();
            //Если текущая позиция это позиция последнего сделанного хода и при этом, сделан хотя бы один ход
            if (number_of_moves == savedMoves.Count - 1 && savedMoves.Count != 1)
            {
                //Удалить последний элемент массива позиций
                savedMoves.RemoveAt(savedMoves.Count - 1);
                //Игра больше не окончена(в случае если последний ход привёл к окончанию игры)
                gameEnded = false;
                //Удаление текста окончания игры
                label2.Text = "";
                //Откатить позицию на один ход назад
                number_of_moves--;
                squares = GameLogic.CopyThePosition(savedMoves[number_of_moves]);
                //Отрисовка количества сделанных ходов
                textBox1.Text = Convert.ToString(number_of_moves);
                //Передать ход сопернику
                whiteToMove = !whiteToMove;
                //Отрисовать доску
                DrawTheBoard();
            }
        }
        //Прокрутка в начало партии
        private void button2_Click(object sender, EventArgs e)
        {
            if(savedMoves.Count != 1)
            {
                //Пока не отображается последняя позиция, взаимодействие с доской запрещено
                boardLoaded = false;
                //Откатить позицию в начало
                number_of_moves = 0;
                squares = GameLogic.CopyThePosition(savedMoves[number_of_moves]);
                //Серым цветом отобразим невозможность нажатия на эту кнопку
                button6.BackColor = Color.LightGray;
                DrawTheBoard();
            }
        }
    }
}
