# Программирование

+ [Создание приложения](#Создание-приложения)
+ [Стили, иконки, логотипы](#Стили-иконки-логотипы)
+ [Создание модели даных](#Создание-модели-даных)
* [Список услуг/товаров](#Список-услуг/товаров)
    + [Отображение списка услуг в табличном виде с выводом миниатюр (изображений)](#Отображение-списка-услуг-в-табличном-виде)
    + [Режим администратора](#Режим-администратора)
        + модальное окно (создание, возвращаемый результат)
        + обратная связь с окном (INotifyPropertyChanged)
        + условное отображение кнопок в панели и в *DataGrid*-е
    - ~~Макет "плиткой"~~
    - Сортировка (своя)
    - Фильтры
    - Поиск (по нескольким полям)
    - Количество отображаемых/всего записей
    - Удаление (с проверкой продаж)
* Добавление/редактирование 
    - проверка на дубль по названию
* Дополнительные фото (CRUD)
* Запись на услугу / продажа
* Ближайшие записи (окно)
    - выделение цветом по времени
    - автообновление 

## Создание приложения

Тип приложения: **WPF, .NET Framefork**

**Внимание!** По **заданию** название приложения (т.е. проекта) должно совпадать с названием огранизации. Русскими буквами, конечно, писать не нужно. Пишите латиницей либо русскую транскрипцию либо по-английски. Например: DoeduSam, SchoolGreatBritain...

>Нигде в задании не нашел как называется компания, пусть будет *AutoService*

В *WPF* подразумевается использование фрейма (*Frame*) с навигацией по страницам (*Page*) и если вы будете использовать этот механизм, то получите сколько то баллов, но можно не мудрить и прямо на главном окне показывать **DataGrid** с основной таблицей (в нашем задании это услуги). А дополнительные таблицы открывать в новых окнах.

Для начала разметка должна быть примерно такой:

```xml
        ...
        Title="Услуги авто сервиса" 
        MinHeight="300" MinWidth="300"
        Height="450" Width="800">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="150"/>
            <ColumnDefinition Width="1*"/>
        </Grid.ColumnDefinitions>

        <StackPanel 
            Orientation="Vertical"
            Margin="5"
            VerticalAlignment="Bottom">
            <Button 
                x:Name="ExitButton"
                Content="Выход"
                Click="ExitButton_Click"/>
        </StackPanel>
    </Grid>
</Window>
```

Сразу задали название окна, разбили *Grid* на две колонки, задали минимальный размер окна (минимальная ширина больше левой колонки, остальное пока не важно)

В компоненте *StackPanel* я задал границу (*Margin*), чтобы кнопки не прилипали к границам грида.

![](../img/demo59.png)

В конструкторе сразу задем *DataContext*

```cs
this.DataContext = this;
```

И реализуем обработчик для кнопки *Выход*:

```cs
private void ExitButton_Click(object sender, RoutedEventArgs e)
{
    Application.Current.Shutdown();
}
```

## Стили, иконка, логотип

### Фирменный стиль (цвет и шрифт)

В заданиях WorldSkills и в Демо-экзамене в частности есть требование о соблюдении фирменного стиля. 

Для создания стиля нужно открыть разметку приложения (*App.xaml*)

И в тег `<Application.Resources>` добавить стили, например:

```xml
<Application.Resources>
    <Style TargetType="Button">
        <Setter Property="Background" Value="#ff9c1a"/>
    </Style>
</Application.Resources>
```

, где:

* **TargetType** - тип элемента, которому задаем стиль. В нашем случае кнопкам;
* **Property** - свойство, которое меняем
* **Value** - значение

Теперь у всех **Кнопок** в нашем приложении **фон** будет рыжим.

Таким образом мы можем менять любое свойство элементов.

Для того, чтобы какой-то стиль подействовал на все элементы окна, его нужно задать окну. Например, зададим шрифт:

```xml
...
FontFamily="Arial Black"
Title="Услуги авто сервиса" 
MinHeight="100" MinWidth="300"
Height="450" Width="800">
```

Обратите внимание, для кнопки мы задавали только фон, шрифт наследуется от окна:

![](../img/demo60.png)


Но что, если нужно задать стиль не всем кнопкам, а только некоторым?

Для этого стилю нужно задать аттрибут ``x:Key``:

```xml
<Style TargetType="Button" x:Key="BrownButtonStyle">
...
```

и назначить этот стиль нужным элементам:

```xml
<Button 
    Name="ExitButton" 
    Content="Exit" 
    Click="ExitButton_Click"
    Style="{StaticResource BrownButtonStyle}"/>
```

Обратите внимание, мы указываем не просто название стиля, а выражение в фигурных скобках. Фигурные скобки означают, что внутри не фиксированное значение, а вычисляемое. В нашем случае указание получить статичный ресурс с указанным названием.

### Установка иконки

В контекстном меню **приложения** выбираете *свойства*

![свойства проекта](../img/demo14.png)

В первом же пункте "приложение" через "обзор" находите иконку (она есть в "общих ресурсах"). Студия автоматически скопирует файл иконки в корень проекта, руками его копировать не нужно.

![свойства проекта](../img/demo15.png)

### Добавление логотипа

Для добавления ресурсов в проект можно создать в нем каталог (в контекстном меню проекта выбрать *Добавить* - *Создать папку*) и скопировать в него нужный ресурс, в нашем случае логотип `service_logo.png`.

![](../img/task020.png)

>Обратите внимание, копировать нужно именно в IDE, а не средствами ОС в папку, иначе VS будет искать ресурс не в папке проекта, а в текущей папке VS.

И добавим картинку в сетку:

```xml
    ...
    </StackPanel>

    <Image 
        Margin="5"
        Source="./images/service_logo.png" 
        VerticalAlignment="Top"/>
</Grid>
```

Если сейчас попробовать изменить размер окна, то увидим, что во-первых минимальную высоту надо увеличивать и, во-вторых, логотип нужно размещать в вёрстке до кнопок

![](../img/demo61.png)

Новый вариант верстки:

```xml
    ...
    Title="Услуги авто сервиса" 
    MinHeight="300" MinWidth="300"
    Height="450" Width="800">
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="150"/>
        <ColumnDefinition Width="1*"/>
    </Grid.ColumnDefinitions>

    <Image 
        Margin="5"
        Source="./images/service_logo.png" 
        VerticalAlignment="Top"/>

    <StackPanel 
    ...
```

Теперь кнопки на логотип не наезжают (но кнопок еще добавится в процессе разработки, поэтому вариант не окончательный)

![](../img/demo62.png)

## Создание модели даных

В **C#** для работы с БД используется ORM **Entity**. Т.е. нам в коде не нужно писать запросы к базе, а просто оперировать объектами, которые сгенерирует для нас **Entity**

>ORM (Object-Relational Mapping) – технология программирования, которая связывает базы данных с концепциями объектно-ориентированных языков программирования, создавая «виртуальную объектную базу данных»

Сначала в *Management Studio* запоминаем как называется наш сервер:

В контекстном меню **сервера** выбираем *свойства* 

![имя сервера](../img/task057.png)

и запоминаем **имя** сервера (у вас на демо-экзамене будет другой)

![](../img/task058.png)

### Подключаем БД

1. В контекстном меню проекта выбираем пункты *Добавить -> Создать элемент*

    ![](../img/task028.png)

2. Выбираем в разделе *Данные* элемент *Модель ADO.NET EDM*, не забывая отредактировать *имя* модели:

    ![](../img/task029.png)

    ![](../img/task030.png)

    Далее выбираем источником данных *Microsoft SQL Server*

    ![](../img/task059.png)

3. В мастере моделей *создаем соединение*

    ![](../img/task031.png)

    **Имя сервера** вставляем то, которое запомнили в Management Studio 

    **Проверка подлинности** *SQL Server*

    Имя и пароль вам выдавали в начале курса.

    **Проверяем подключение**, если все нормально, то выбираем свою базу данных (должна быть та же что и логин).

    ![](../img/task032.png)

    Ставим галочку "Да, включить конфиденциальные данные в строку подключения".

4. После выбора подключения мастер спросит какие объекты базы данных нам нужны, выбираем все таблицы:

    ![](../img/demo63.png)

5. При формировании модели система может выдать предупреждения, что скрипт может быть потенциально вредоносным - соглашаемся.

После формирования модели отобразится диаграмма и создадутся файлы модели:

![](../img/demo64.png)

![](../img/demo65.png)

>Если в процессе разработки вы что-то измените в структуре БД, то заново модель создавать не нужно, а просто в модели кликнуть правой кнопкой мыши и *Обновить модель из базы данных*


## Список услуг/товаров

Перед выводом данных нужно получить сам объект вывода (в нашем случае это список услуг). Чтобы не создавать каждый раз новое подключение к БД создадим класс **Core**, который будет содержать статический метод получения экземпляра БД. Т.е. реализуем шаблон "Одиночка". Помимо того, что это экономит ресурсы приложения и сервера, это позволяет модели отслеживать изменения отдельных сущностей (таблиц).

Добавим в приложение класс:

Помня, что нужно соблюдать логическую структуру проекта, создадим папку *classes* в проекте и добавим в неё класс (контекстное меню каталога -> Добавить -> Класс)

![](../img/task077.png)

Пусть класс назвается **Core**:

![](../img/task078.png)

и в нем объявим статическую переменную

```cs
class Core
{
    // demoEntities это название подключения, которое вы дали при создании модели
    public static demoEntities DB = new demoEntities();
}
```

В классе объявляем приватное свойство для хранения списка услуг *_ServiceList* и публичное свойство *ServiceList* для доступа к этому списку:

```cs
private List<Service> _ServiceList;
public List<Service> ServiceList {
    get { return _ServiceList;  }
    set { _ServiceList = value; }
}
```

В конструкторе главного окна получаем список услуг (т.к. мы поместили класс в отдельный каталог, то нужно включить его *namespace* в зависимости `using AutoService.classes;`):

```cs
ServiceList = Core.DB.Service.ToList();
```

### Отображение списка услуг в табличном виде

В разметку добавляем компонент **DataGrid** (во вторую колонку)

```xml
<DataGrid 
    Grid.Column="1"
    ItemsSource="{Binding ServiceList}"/>
```

Если все сделано правильно, то при запуске приложения будет выведено содержимое таблицы:

![](../img/demo66.png)

Видим, что информация выводится не в том виде, который нам нужен. 

1. Отключаем автоматическую генерацию колонок и запрещаем пользователю добавлять строки

    ```xml
    <DataGrid 
        Grid.Column="1"
        ItemsSource="{Binding ServiceList}"
        CanUserAddRows="false"
        AutoGenerateColumns="False"/>
    ```

2. Добавляем в **DataGrid** описание для нужных колонок (*У каждой  услуги должны отображаться следующие данные: наименование услуги,  стоимость, продолжительность, миниатюра главного изображения, размер скидки*)

Каталог с картинками должен лежать там же где **EXE**-файл. Скорее всего это подкаталог `bin\Debug`.

```xml
<DataGrid 
    Grid.Column="1"
    CanUserAddRows="false"
    AutoGenerateColumns="False"
    ItemsSource="{Binding ServiceList}">

    <DataGrid.Columns>
        <!-- колонкам я задаю фиксированную ширину, чтобы они не ёрзали при прокрутке -->
        <DataGridTextColumn
            Width="250"
            Header="Наименование услуги"
            Binding="{Binding Title}"/>
        <DataGridTextColumn 
            Width="100"
            Header="Стоимость"
            Binding="{Binding Cost}"/>
        <DataGridTextColumn 
            Width="150"
            Header="Продолжительность"
            Binding="{Binding DurationInSeconds}"/>
        <DataGridTemplateColumn 
            Width="64"
            Header="">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <!-- для отображения изображения использую геттер, который определен в МОДЕЛИ Service -->
                    <Image 
                        Height="64" 
                        Width="64" 
                        Source="{Binding ImageUri}" />
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>

        <DataGridTextColumn 
            Width="60"
            Header="Скидка"
            Binding="{Binding Discount}"/>
    </DataGrid.Columns>
</DataGrid>
```

Геттер для картинки:

```cs
// Service.cs
public Uri ImageUri {
    get { 
        return new Uri(Path.Combine(Environment.CurrentDirectory, MainImagePath));
    }
}
```

**URI (Uniform Resource Identifiers)**. URI нужны, чтобы идентифицировать и запросить новый вид ресурса. Используя URI, можно обращаться не только к Web-страницам, но и к FTP-серверу, Web-сервису и **локальным файлам**.

**Path.Combine** - метод, который склеивает текущий каталог (*Environment.CurrentDirectory*) и путь к картинками

В текущей разметке мне не нравится, что у цены 4 знака после запятой. К тому же читая задание дальше видим, что нужно рядом вывести цену со скидкой. Завернем оба эти параметра в геттеры:

```cs
// Service.cs
public string CostString
{
    get
    {
        // тут должно быть понятно - преобразование в строку с нужной точностью
        return Cost.ToString("#.##");
    }
}

public string CostWithDiscount
{
    get
    {
        // Convert.ToDecimal - преобразует double в decimal
        // Discount ?? 0 - разнуливает "Nullable" переменную
        return (Cost * Convert.ToDecimal(1 - Discount ?? 0)).ToString("#.##");
    }
}

// ну и сразу пишем геттер на наличие скидки
public Boolean HasDiscount
{
    get
    {
        return Discount > 0;
    }
}

// и перечёркивание старой цены
public string CostTextDecoration
{
    get
    {
        return HasDiscount ? "None" : "Strikethrough";
    }
}
```

Для раскраски строк *DataGrid*-а используем стиль с триггером. Т.е. стиль применяется только к тем строкам, в которых выполняется условие `HasDiscount = True`

```xml
...
ItemsSource="{Binding ServiceList}">

<DataGrid.RowStyle>
    <Style TargetType="DataGridRow">
        <Style.Triggers>
            <DataTrigger 
                Binding="{Binding HasDiscount}" 
                Value="True">
                <Setter 
                    Property="Background" 
                    Value="LightGreen"/>
            </DataTrigger>
        </Style.Triggers>
    </Style>
</DataGrid.RowStyle>

<DataGrid.Columns>
...
```

Обычная *DataGridTextColumn* не поддерживает перечеркивания, приходится заворачивать как картинку в шаблон:

```xml
<DataGridTemplateColumn 
    Width="100"
    Header="Стоимость">

    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <!-- TextBlock поддерживает перечеркивание -->
            <TextBlock 
                TextDecorations="{Binding CostTextDecoration}"
                Text="{Binding CostString}"/>
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>
```

Вроде про отображение списка услуг все расказал (админка, фильтрация и сортировка будут дальше). На текущий момент приложение должно выглядеть примерно так:

![](../img/demo67.png)

## Режим администратора

>В данной подсистеме необходимо добавить режим администратора. Для активации данного режима необходимо ввести код (на этапе разработки код всегда будет одинаковый = 0000). Список услуг должен быть виден всем в клиентской зоне (обычный режим), а функции добавления, удаления, редактирования данных об услуге, а также запись клиента на услугу и просмотр ближайших записей должен быть доступен только администратору (режим администратора).

1. Создание окна для ввода пароля *InputBoxWindow*:

    Окно буду делать универсальным - название окна буду передавать в конструкторе.

    * Создаем отдельный каталог для окон и добавляем **namespace** в зависимости:

        ```cs
        using AutoService.windows;
        ```

    * В каталоге проекта *windows* создаем новое окно (*контекстное меню -> добавить -> Окно WPF*) *InputBoxWindow*

    * Разметка окна *InputBoxWindow* примерно такая:

        ```xml
            MinHeight="110" MinWidth="530"
            Title="{Binding WindowCaption}" Height="110" Width="530">
        <Grid>
            <StackPanel 
                Orientation="Vertical">
                <TextBox 
                    x:Name="TextBox" 
                    Margin="10" 
                    Text="{Binding InputText}"/>
                <StackPanel 
                    Orientation="Horizontal"
                    HorizontalAlignment="Center">
                    <Button 
                        x:Name="OkButton"
                        Content="Ok"
                        Margin="5"
                        Click="OkButton_Click"/>
                    <Button
                        x:Name="CancelButton"
                        Content="Cancel"
                        Margin="5"
                        Click="CancelButton_Click"/>
                </StackPanel>
            </StackPanel>
        </Grid>
        ```

        - заголовок окна (*Title*) будет задаваться в конструкторе
        - не забываем про минимальные размеры окна
        - вводимый текст привязываем к свойству (*{Binding InputText}*)

        Остальное должно быть понятно без комментариев.

    * Код окна *InputBoxWindow*:

        ```cs
        // в классе объявляем свойства для заголовка окна и введенного текста
        public string WindowCaption { get; set; }
        public string InputText { get; set; }
        ```

        ```cs
        public InputBoxWindow(string Caption)
        {
            InitializeComponent();
            // заголовок окна берем из параметров конструктора
            WindowCaption = Caption;
            this.DataContext = this;
        }
        ```

        ```cs
        // в обработчиках кнопок "Ok" и "Cancel" задаем результат модального окна
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        ```

2. Изменения в главном окне (*MainWindow*):

    * в левую панель добавим кнопку для входа в режим администратора (я добавил еще и выход, но это вроде как не обязательно)

        ```xml
        <Button
            Content="{Binding AdminModeCaption}"
            Name="AdminButton"
            Click="AdminButton_Click"/>
        ```

        У меня текст кнопки будет меняться в зависимости от текущего режима, поэтому он сделан привязкой к свойству класса окна. 

    * добавляем необходимые свойства в код:

        ```cs
        private Boolean _IsAdminMode = false;
        // публичный геттер, который меняет текущий режим (Админ/не Админ)
        public Boolean IsAdminMode
        {
            get { return _IsAdminMode; }
            set
            {
                _IsAdminMode = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AdminModeCaption"));
                }
            }
        }
        // этот геттер возвращает текст для кнопки в зависимости от текущего режима
        public string AdminModeCaption {
            get {
                if (IsAdminMode) return "Выйти из режима\nАдминистратора";
                return "Войти в режим\nАдминистратора";
            }
        }
        ```   

        Тут появляется новая *фича* C# - обратная связь с окном (*PropertyChanged*). Дело в том, что окно прорисовывается один раз при создании и дальше существует само по себе. Содержимое свойств класса автоматически не отслеживается.

        Чтобы дать знать визуальной части что какое-то свойство изменилось (и, соответсвенно, перерисовать его) используется интерфейс *INotifyPropertyChanged*.

        Для того, чтобы окно реализовывало этот интерфейс нужно добавить его в описание класса:

        ```cs
        public partial class MainWindow : Window, INotifyPropertyChanged
                                                ^^^^^^^^^^^^^^^^^^^^^^^^     
        ```


        Интерфейс *INotifyPropertyChanged* определен в пространстве имен *System.ComponentModel*, если оно еще не подключено, то добавьте в заголовке using System.ComponentModel (можно это сделать выбрав нужное действие в выпадающем списке возможных решений, как это сделано ниже)

        После добавления интерфейса IDE нас предупредит, что для этого интерфеса нет реализации - добавляем:

        ![](../img/task026.gif)

        в классе у нас появится событие *PropertyChanged*, которое мы и вызываем, когда хотим перерисовать какой-то визуальный элемент
    
        ```cs
        public event PropertyChangedEventHandler PropertyChanged;
        ```

        В нашем случае мы сообщаем окну, что изменилось свойство *AdminModeCaption* (проверка `PropertyChanged != null` нужна, т.к. на момент создания окна это свойство еще не создано):

        ```cs
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs("AdminModeCaption"));
        }
        ```

    * и в обработчике клика кнопки *AdminButton* реализуем логику переключения режима:

        ```cs
        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            // если мы уже в режиме Администратора, то выходим из него 
            if (IsAdminMode) IsAdminMode = false;
            else {
                // создаем окно для ввода пароля
                var InputBox = new InputBoxWindow("Введите пароль Администратора");
                // и показываем его как диалог (модально)
                if ((bool)InputBox.ShowDialog())
                {
                    // если нажали кнопку "Ok", то включаем режим, если пароль введен верно
                    IsAdminMode = InputBox.InputText == "0000";
                }
            }
        }
        ```
3. Добавление кнопок для режима Администратора

    * в классе главного окна создаем геттер *AdminVisibility*, чтобы отображать нужные кнопки только в режиме Администратора:

        ```cs
        public string AdminVisibility {
            get {
                if (IsAdminMode) return "Visible";
                return "Collapsed";
            }
        }
        ```

    * в сеттер свойства *IsAdminMode* добавим вызов уведомления:

        ```cs
        PropertyChanged(this, new PropertyChangedEventArgs("AdminVisibility"));
        ```

    * окну даем название `x:Name="Root"` (в разметке, там же где задаем Title)

    * кнопки редактирования/удаления записи добавляем прямо в *DataGrid*:

        ```xml
        <DataGridTemplateColumn
            Header="Действия">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <StackPanel 
                        Visibility="{Binding DataContext.AdminVisibility, ElementName=Root}"
                        Orientation="Horizontal">
                        <Button 
                                Content="Редактировать" 
                                Name="EditButton" 
                                Click="EditButton_Click"/>
                        <Button 
                                Content="Удалить" 
                                Name="DeleteButton" 
                                Click="DeleteButton_Click"/>
                    </StackPanel>
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>
        ```

        Обратите внимание:
            - кнопки "завернуты" в элемент *StackPanel* т.к. у *DataTemplate* может быть только один потомок
            - *StackPanel* виден только если включен режим администратора (запоминать значения свойства "Visibility" не обязательно, их подскажет Intellisence при вводе);
            - привязка (*Binding*) производится не напрямую к свойству *AdminVisibility*, а через свойство *DataContext* окна. Дело в том, что текущим контекстом в DataGridTemplate будет экземпляр класса **Service**.
    
    * и в боковую панель добавляем кнопку "Добавить услугу" (кнопки "Запись на услугу" и "просмотр записей" сделаете позже, если успеете):

        ```xml
        <Button
            Content="Добавить услугу"
            Visibility="{Binding AdminVisibility}"
            Click="Button_Click"/>
        ```
        
Реализация действий для кнопок Добавить/Редактирвать/Удалить будет ниже.

