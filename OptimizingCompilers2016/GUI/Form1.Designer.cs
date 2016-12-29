namespace OptimizingCompilers2016.GUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Console = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Menu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.анализToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defUseИнформацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.глобальнаяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.внутриБлоковToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.анализАктивныхПеременныхToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.анализДоступныхВыраженийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.деревоДоминаторовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.фронтДоминировнияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.итерационныйФронтДоминированияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.графПотоковУправленияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.глубинноеОстовноеДеревоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.классификацияРёберToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обратныеРёбраToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.натуральныеЦиклыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оптимизацииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалениеМёртвогоКодаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.наОсновеАнализаАктивныхПеременныхToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.удалениеМёртвогоКодаToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.оптимизацияОбщихПодвыраженийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.блочныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.межблочныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.свёрткаКонстантToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.протяжкаКонстантToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.внутриБлоковToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.глобальнаяToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.учетАлгебраическихТождествToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SourceCode_TabPage = new System.Windows.Forms.TabPage();
            this.Code = new System.Windows.Forms.RichTextBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.ThreeAddrCode_TabPage = new System.Windows.Forms.TabPage();
            this.ResultCode = new System.Windows.Forms.RichTextBox();
            this.BaseBlock_TabPage = new System.Windows.Forms.TabPage();
            this.BaseBlocks = new System.Windows.Forms.RichTextBox();
            this.Refresh = new System.Windows.Forms.Button();
            this.NewWindow = new System.Windows.Forms.Button();
            this.Result = new System.Windows.Forms.RichTextBox();
            this.NewWindow2 = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.synchronScrolling = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.SourceCode_TabPage.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.ThreeAddrCode_TabPage.SuspendLayout();
            this.BaseBlock_TabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // Console
            // 
            this.Console.BackColor = System.Drawing.Color.White;
            this.Console.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Console.ForeColor = System.Drawing.Color.DarkRed;
            this.Console.Location = new System.Drawing.Point(16, 531);
            this.Console.Name = "Console";
            this.Console.Size = new System.Drawing.Size(943, 104);
            this.Console.TabIndex = 3;
            this.Console.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu,
            this.анализToolStripMenuItem,
            this.оптимизацииToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(962, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Menu
            // 
            this.Menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuOpen,
            this.MenuExit});
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(48, 20);
            this.Menu.Text = "Файл";
            // 
            // MenuOpen
            // 
            this.MenuOpen.Name = "MenuOpen";
            this.MenuOpen.Size = new System.Drawing.Size(121, 22);
            this.MenuOpen.Text = "Открыть";
            this.MenuOpen.Click += new System.EventHandler(this.MenuOpen_Click);
            // 
            // MenuExit
            // 
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(121, 22);
            this.MenuExit.Text = "Выход";
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // анализToolStripMenuItem
            // 
            this.анализToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defUseИнформацияToolStripMenuItem,
            this.анализАктивныхПеременныхToolStripMenuItem,
            this.анализДоступныхВыраженийToolStripMenuItem,
            this.toolStripSeparator1,
            this.деревоДоминаторовToolStripMenuItem,
            this.фронтДоминировнияToolStripMenuItem,
            this.итерационныйФронтДоминированияToolStripMenuItem,
            this.toolStripSeparator2,
            this.графПотоковУправленияToolStripMenuItem,
            this.глубинноеОстовноеДеревоToolStripMenuItem,
            this.классификацияРёберToolStripMenuItem,
            this.обратныеРёбраToolStripMenuItem,
            this.натуральныеЦиклыToolStripMenuItem});
            this.анализToolStripMenuItem.Name = "анализToolStripMenuItem";
            this.анализToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.анализToolStripMenuItem.Text = "Анализ";
            // 
            // defUseИнформацияToolStripMenuItem
            // 
            this.defUseИнформацияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.глобальнаяToolStripMenuItem,
            this.внутриБлоковToolStripMenuItem});
            this.defUseИнформацияToolStripMenuItem.Name = "defUseИнформацияToolStripMenuItem";
            this.defUseИнформацияToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.defUseИнформацияToolStripMenuItem.Text = "Def-Use информация";
            // 
            // глобальнаяToolStripMenuItem
            // 
            this.глобальнаяToolStripMenuItem.Name = "глобальнаяToolStripMenuItem";
            this.глобальнаяToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.глобальнаяToolStripMenuItem.Text = "Глобальная";
            this.глобальнаяToolStripMenuItem.Click += new System.EventHandler(this.глобальнаяToolStripMenuItem_Click);
            // 
            // внутриБлоковToolStripMenuItem
            // 
            this.внутриБлоковToolStripMenuItem.Name = "внутриБлоковToolStripMenuItem";
            this.внутриБлоковToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.внутриБлоковToolStripMenuItem.Text = "Внутри блоков";
            this.внутриБлоковToolStripMenuItem.Click += new System.EventHandler(this.внутриБлоковToolStripMenuItem_Click);
            // 
            // анализАктивныхПеременныхToolStripMenuItem
            // 
            this.анализАктивныхПеременныхToolStripMenuItem.Name = "анализАктивныхПеременныхToolStripMenuItem";
            this.анализАктивныхПеременныхToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.анализАктивныхПеременныхToolStripMenuItem.Text = "Анализ активных переменных";
            this.анализАктивныхПеременныхToolStripMenuItem.Click += new System.EventHandler(this.анализАктивныхПеременныхToolStripMenuItem_Click);
            // 
            // анализДоступныхВыраженийToolStripMenuItem
            // 
            this.анализДоступныхВыраженийToolStripMenuItem.Name = "анализДоступныхВыраженийToolStripMenuItem";
            this.анализДоступныхВыраженийToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.анализДоступныхВыраженийToolStripMenuItem.Text = "Анализ доступных выражений";
            this.анализДоступныхВыраженийToolStripMenuItem.Click += new System.EventHandler(this.анализДоступныхВыраженийToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(285, 6);
            // 
            // деревоДоминаторовToolStripMenuItem
            // 
            this.деревоДоминаторовToolStripMenuItem.Name = "деревоДоминаторовToolStripMenuItem";
            this.деревоДоминаторовToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.деревоДоминаторовToolStripMenuItem.Text = "Дерево доминаторов";
            this.деревоДоминаторовToolStripMenuItem.Click += new System.EventHandler(this.деревоДоминаторовToolStripMenuItem_Click);
            // 
            // фронтДоминировнияToolStripMenuItem
            // 
            this.фронтДоминировнияToolStripMenuItem.Name = "фронтДоминировнияToolStripMenuItem";
            this.фронтДоминировнияToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.фронтДоминировнияToolStripMenuItem.Text = "Фронт доминирования";
            this.фронтДоминировнияToolStripMenuItem.Click += new System.EventHandler(this.фронтДоминировнияToolStripMenuItem_Click);
            // 
            // итерационныйФронтДоминированияToolStripMenuItem
            // 
            this.итерационныйФронтДоминированияToolStripMenuItem.Name = "итерационныйФронтДоминированияToolStripMenuItem";
            this.итерационныйФронтДоминированияToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.итерационныйФронтДоминированияToolStripMenuItem.Text = "Итерационный фронт доминирования";
            this.итерационныйФронтДоминированияToolStripMenuItem.Click += new System.EventHandler(this.итерационныйФронтДоминированияToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(285, 6);
            // 
            // графПотоковУправленияToolStripMenuItem
            // 
            this.графПотоковУправленияToolStripMenuItem.Name = "графПотоковУправленияToolStripMenuItem";
            this.графПотоковУправленияToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.графПотоковУправленияToolStripMenuItem.Text = "Граф потока управления";
            this.графПотоковУправленияToolStripMenuItem.Click += new System.EventHandler(this.графПотоковУправленияToolStripMenuItem_Click);
            // 
            // глубинноеОстовноеДеревоToolStripMenuItem
            // 
            this.глубинноеОстовноеДеревоToolStripMenuItem.Name = "глубинноеОстовноеДеревоToolStripMenuItem";
            this.глубинноеОстовноеДеревоToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.глубинноеОстовноеДеревоToolStripMenuItem.Text = "Глубинное остовное дерево";
            this.глубинноеОстовноеДеревоToolStripMenuItem.Click += new System.EventHandler(this.глубинноеОстовноеДеревоToolStripMenuItem_Click);
            // 
            // классификацияРёберToolStripMenuItem
            // 
            this.классификацияРёберToolStripMenuItem.Name = "классификацияРёберToolStripMenuItem";
            this.классификацияРёберToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.классификацияРёберToolStripMenuItem.Text = "Классификация рёбер";
            this.классификацияРёберToolStripMenuItem.Click += new System.EventHandler(this.классификацияРёберToolStripMenuItem_Click);
            // 
            // обратныеРёбраToolStripMenuItem
            // 
            this.обратныеРёбраToolStripMenuItem.Name = "обратныеРёбраToolStripMenuItem";
            this.обратныеРёбраToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.обратныеРёбраToolStripMenuItem.Text = "Обратные рёбра";
            this.обратныеРёбраToolStripMenuItem.Click += new System.EventHandler(this.обратныеРёбраToolStripMenuItem_Click);
            // 
            // натуральныеЦиклыToolStripMenuItem
            // 
            this.натуральныеЦиклыToolStripMenuItem.Name = "натуральныеЦиклыToolStripMenuItem";
            this.натуральныеЦиклыToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.натуральныеЦиклыToolStripMenuItem.Text = "Натуральные циклы";
            this.натуральныеЦиклыToolStripMenuItem.Click += new System.EventHandler(this.натуральныеЦиклыToolStripMenuItem_Click);
            // 
            // оптимизацииToolStripMenuItem
            // 
            this.оптимизацииToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.удалениеМёртвогоКодаToolStripMenuItem,
            this.оптимизацияОбщихПодвыраженийToolStripMenuItem,
            this.свёрткаКонстантToolStripMenuItem,
            this.протяжкаКонстантToolStripMenuItem,
            this.учетАлгебраическихТождествToolStripMenuItem});
            this.оптимизацииToolStripMenuItem.Name = "оптимизацииToolStripMenuItem";
            this.оптимизацииToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.оптимизацииToolStripMenuItem.Text = "Оптимизации";
            // 
            // удалениеМёртвогоКодаToolStripMenuItem
            // 
            this.удалениеМёртвогоКодаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.наОсновеАнализаАктивныхПеременныхToolStripMenuItem1,
            this.удалениеМёртвогоКодаToolStripMenuItem1});
            this.удалениеМёртвогоКодаToolStripMenuItem.Name = "удалениеМёртвогоКодаToolStripMenuItem";
            this.удалениеМёртвогоКодаToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.удалениеМёртвогоКодаToolStripMenuItem.Text = "Удаление мёртвого кода";
            // 
            // наОсновеАнализаАктивныхПеременныхToolStripMenuItem1
            // 
            this.наОсновеАнализаАктивныхПеременныхToolStripMenuItem1.Name = "наОсновеАнализаАктивныхПеременныхToolStripMenuItem1";
            this.наОсновеАнализаАктивныхПеременныхToolStripMenuItem1.Size = new System.Drawing.Size(304, 22);
            this.наОсновеАнализаАктивныхПеременныхToolStripMenuItem1.Text = "На основе анализа активных переменных";
            this.наОсновеАнализаАктивныхПеременныхToolStripMenuItem1.Click += new System.EventHandler(this.наОсновеАнализаАктивныхПеременныхToolStripMenuItem1_Click);
            // 
            // удалениеМёртвогоКодаToolStripMenuItem1
            // 
            this.удалениеМёртвогоКодаToolStripMenuItem1.Name = "удалениеМёртвогоКодаToolStripMenuItem1";
            this.удалениеМёртвогоКодаToolStripMenuItem1.Size = new System.Drawing.Size(304, 22);
            this.удалениеМёртвогоКодаToolStripMenuItem1.Text = "Удаление мёртвого кода";
            this.удалениеМёртвогоКодаToolStripMenuItem1.Click += new System.EventHandler(this.удалениеМёртвогоКодаToolStripMenuItem1_Click);
            // 
            // оптимизацияОбщихПодвыраженийToolStripMenuItem
            // 
            this.оптимизацияОбщихПодвыраженийToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.блочныеToolStripMenuItem,
            this.межблочныеToolStripMenuItem});
            this.оптимизацияОбщихПодвыраженийToolStripMenuItem.Name = "оптимизацияОбщихПодвыраженийToolStripMenuItem";
            this.оптимизацияОбщихПодвыраженийToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.оптимизацияОбщихПодвыраженийToolStripMenuItem.Text = "Общие подвыражения";
            // 
            // блочныеToolStripMenuItem
            // 
            this.блочныеToolStripMenuItem.Name = "блочныеToolStripMenuItem";
            this.блочныеToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.блочныеToolStripMenuItem.Text = "Блочные";
            this.блочныеToolStripMenuItem.Click += new System.EventHandler(this.блочныеToolStripMenuItem_Click);
            // 
            // межблочныеToolStripMenuItem
            // 
            this.межблочныеToolStripMenuItem.Name = "межблочныеToolStripMenuItem";
            this.межблочныеToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.межблочныеToolStripMenuItem.Text = "Межблочные";
            this.межблочныеToolStripMenuItem.Click += new System.EventHandler(this.межблочныеToolStripMenuItem_Click);
            // 
            // свёрткаКонстантToolStripMenuItem
            // 
            this.свёрткаКонстантToolStripMenuItem.Name = "свёрткаКонстантToolStripMenuItem";
            this.свёрткаКонстантToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.свёрткаКонстантToolStripMenuItem.Text = "Свёртка констант";
            this.свёрткаКонстантToolStripMenuItem.Click += new System.EventHandler(this.свёрткаКонстантToolStripMenuItem_Click);
            // 
            // протяжкаКонстантToolStripMenuItem
            // 
            this.протяжкаКонстантToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.внутриБлоковToolStripMenuItem1,
            this.глобальнаяToolStripMenuItem1});
            this.протяжкаКонстантToolStripMenuItem.Name = "протяжкаКонстантToolStripMenuItem";
            this.протяжкаКонстантToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.протяжкаКонстантToolStripMenuItem.Text = "Распространение констант";
            // 
            // внутриБлоковToolStripMenuItem1
            // 
            this.внутриБлоковToolStripMenuItem1.Name = "внутриБлоковToolStripMenuItem1";
            this.внутриБлоковToolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.внутриБлоковToolStripMenuItem1.Text = "Внутри блоков";
            this.внутриБлоковToolStripMenuItem1.Click += new System.EventHandler(this.внутриБлоковToolStripMenuItem1_Click);
            // 
            // глобальнаяToolStripMenuItem1
            // 
            this.глобальнаяToolStripMenuItem1.Name = "глобальнаяToolStripMenuItem1";
            this.глобальнаяToolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.глобальнаяToolStripMenuItem1.Text = "Глобальное";
            this.глобальнаяToolStripMenuItem1.Click += new System.EventHandler(this.глобальнаяToolStripMenuItem1_Click);
            // 
            // учетАлгебраическихТождествToolStripMenuItem
            // 
            this.учетАлгебраическихТождествToolStripMenuItem.Name = "учетАлгебраическихТождествToolStripMenuItem";
            this.учетАлгебраическихТождествToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.учетАлгебраическихТождествToolStripMenuItem.Text = "Учет алгебраических тождеств";
            this.учетАлгебраическихТождествToolStripMenuItem.Click += new System.EventHandler(this.учетАлгебраическихТождествToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SourceCode_TabPage
            // 
            this.SourceCode_TabPage.Controls.Add(this.Code);
            this.SourceCode_TabPage.Location = new System.Drawing.Point(4, 22);
            this.SourceCode_TabPage.Name = "SourceCode_TabPage";
            this.SourceCode_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.SourceCode_TabPage.Size = new System.Drawing.Size(328, 428);
            this.SourceCode_TabPage.TabIndex = 0;
            this.SourceCode_TabPage.Text = "Исходный код";
            this.SourceCode_TabPage.UseVisualStyleBackColor = true;
            // 
            // Code
            // 
            this.Code.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Code.Location = new System.Drawing.Point(3, 0);
            this.Code.Name = "Code";
            this.Code.Size = new System.Drawing.Size(325, 426);
            this.Code.TabIndex = 0;
            this.Code.Text = "";
            this.Code.TextChanged += new System.EventHandler(this.Code_TextChanged);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.SourceCode_TabPage);
            this.tabControl2.Controls.Add(this.ThreeAddrCode_TabPage);
            this.tabControl2.Controls.Add(this.BaseBlock_TabPage);
            this.tabControl2.Location = new System.Drawing.Point(12, 27);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(336, 454);
            this.tabControl2.TabIndex = 6;
            this.tabControl2.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl2_Selecting);
            // 
            // ThreeAddrCode_TabPage
            // 
            this.ThreeAddrCode_TabPage.Controls.Add(this.ResultCode);
            this.ThreeAddrCode_TabPage.Location = new System.Drawing.Point(4, 22);
            this.ThreeAddrCode_TabPage.Name = "ThreeAddrCode_TabPage";
            this.ThreeAddrCode_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ThreeAddrCode_TabPage.Size = new System.Drawing.Size(328, 428);
            this.ThreeAddrCode_TabPage.TabIndex = 1;
            this.ThreeAddrCode_TabPage.Text = "Трёхадресный код";
            this.ThreeAddrCode_TabPage.UseVisualStyleBackColor = true;
            // 
            // ResultCode
            // 
            this.ResultCode.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ResultCode.Location = new System.Drawing.Point(0, 0);
            this.ResultCode.Name = "ResultCode";
            this.ResultCode.Size = new System.Drawing.Size(328, 425);
            this.ResultCode.TabIndex = 0;
            this.ResultCode.Text = "";
            // 
            // BaseBlock_TabPage
            // 
            this.BaseBlock_TabPage.Controls.Add(this.BaseBlocks);
            this.BaseBlock_TabPage.Location = new System.Drawing.Point(4, 22);
            this.BaseBlock_TabPage.Name = "BaseBlock_TabPage";
            this.BaseBlock_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.BaseBlock_TabPage.Size = new System.Drawing.Size(328, 428);
            this.BaseBlock_TabPage.TabIndex = 2;
            this.BaseBlock_TabPage.Text = "Базовые блоки";
            this.BaseBlock_TabPage.UseVisualStyleBackColor = true;
            // 
            // BaseBlocks
            // 
            this.BaseBlocks.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BaseBlocks.Location = new System.Drawing.Point(0, 0);
            this.BaseBlocks.Name = "BaseBlocks";
            this.BaseBlocks.Size = new System.Drawing.Size(328, 425);
            this.BaseBlocks.TabIndex = 0;
            this.BaseBlocks.Text = "";
            // 
            // Refresh
            // 
            this.Refresh.Location = new System.Drawing.Point(16, 487);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(99, 31);
            this.Refresh.TabIndex = 7;
            this.Refresh.Text = "Обновить";
            this.Refresh.UseVisualStyleBackColor = true;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // NewWindow
            // 
            this.NewWindow.Location = new System.Drawing.Point(249, 487);
            this.NewWindow.Name = "NewWindow";
            this.NewWindow.Size = new System.Drawing.Size(99, 31);
            this.NewWindow.TabIndex = 8;
            this.NewWindow.Text = "В новом окне";
            this.NewWindow.UseVisualStyleBackColor = true;
            this.NewWindow.Click += new System.EventHandler(this.NewWindow_Click);
            // 
            // Result
            // 
            this.Result.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Result.Location = new System.Drawing.Point(354, 49);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(596, 432);
            this.Result.TabIndex = 9;
            this.Result.Text = "";
            this.Result.VScroll += new System.EventHandler(this.Result_VScroll);
            // 
            // NewWindow2
            // 
            this.NewWindow2.Location = new System.Drawing.Point(618, 487);
            this.NewWindow2.Name = "NewWindow2";
            this.NewWindow2.Size = new System.Drawing.Size(99, 31);
            this.NewWindow2.TabIndex = 10;
            this.NewWindow2.Text = "В новом окне";
            this.NewWindow2.UseVisualStyleBackColor = true;
            this.NewWindow2.Click += new System.EventHandler(this.NewWindow2_Click);
            // 
            // Save
            // 
            this.Save.Enabled = false;
            this.Save.Location = new System.Drawing.Point(132, 487);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(99, 31);
            this.Save.TabIndex = 11;
            this.Save.Text = "Запомнить";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // synchronScrolling
            // 
            this.synchronScrolling.AutoSize = true;
            this.synchronScrolling.Location = new System.Drawing.Point(818, 508);
            this.synchronScrolling.Name = "synchronScrolling";
            this.synchronScrolling.Size = new System.Drawing.Size(141, 17);
            this.synchronScrolling.TabIndex = 12;
            this.synchronScrolling.Text = "Синхронная прокрутка";
            this.synchronScrolling.UseVisualStyleBackColor = true;
            this.synchronScrolling.CheckedChanged += new System.EventHandler(this.synchronScrolling_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(962, 665);
            this.Controls.Add(this.synchronScrolling);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.NewWindow2);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.NewWindow);
            this.Controls.Add(this.Refresh);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.Console);
            this.Controls.Add(this.menuStrip1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Оптимизирующий компилятор";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.SourceCode_TabPage.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.ThreeAddrCode_TabPage.ResumeLayout(false);
            this.BaseBlock_TabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox Console;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Menu;
        private System.Windows.Forms.ToolStripMenuItem MenuOpen;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabPage SourceCode_TabPage;
        private System.Windows.Forms.RichTextBox Code;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage ThreeAddrCode_TabPage;
        private System.Windows.Forms.TabPage BaseBlock_TabPage;
        private System.Windows.Forms.RichTextBox BaseBlocks;
        private System.Windows.Forms.ToolStripMenuItem оптимизацииToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem анализToolStripMenuItem;
        private System.Windows.Forms.Button Refresh;
        private System.Windows.Forms.Button NewWindow;
        private System.Windows.Forms.RichTextBox ResultCode;
        private System.Windows.Forms.ToolStripMenuItem удалениеМёртвогоКодаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defUseИнформацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem анализАктивныхПеременныхToolStripMenuItem;
        private System.Windows.Forms.RichTextBox Result;
        private System.Windows.Forms.ToolStripMenuItem оптимизацияОбщихПодвыраженийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem свёрткаКонстантToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem протяжкаКонстантToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem учетАлгебраическихТождествToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem глобальнаяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem внутриБлоковToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem анализДоступныхВыраженийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem деревоДоминаторовToolStripMenuItem;
        private System.Windows.Forms.Button NewWindow2;
        private System.Windows.Forms.ToolStripMenuItem итерационныйФронтДоминированияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem фронтДоминировнияToolStripMenuItem;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.ToolStripMenuItem графПотоковУправленияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem глубинноеОстовноеДеревоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem натуральныеЦиклыToolStripMenuItem;
        private System.Windows.Forms.CheckBox synchronScrolling;
        private System.Windows.Forms.ToolStripMenuItem внутриБлоковToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem глобальнаяToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem наОсновеАнализаАктивныхПеременныхToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem классификацияРёберToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem блочныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem межблочныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалениеМёртвогоКодаToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem обратныеРёбраToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

