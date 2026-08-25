# Функциональная модель приложения Surface Manager

Блок-схемы структуры и сценариев использования.

---

## 1. Общая структура приложения

```mermaid
flowchart TB
    subgraph App["Приложение"]
        Layout["Layout: шапка, меню, подвал"]
        Layout --> Main["Основной контент"]
    end

    subgraph Pages["Страницы"]
        Main --> Index["Главная\n(Панель управления)"]
        Main --> Surfaces["Поверхности"]
        Main --> Clients["Клиенты"]
        Main --> Downtime["Простои"]
        Main --> Calendar["Календарь"]
    end

    subgraph Actions["Быстрые действия с главной"]
        Index --> AddSurface["Добавить поверхность"]
        Index --> AddClient["Новый клиент"]
        Index --> AddDowntime["Отметить простой"]
    end

    AddSurface --> Surfaces
    AddClient --> Clients
    AddDowntime --> Downtime
```

---

## 2. Навигация и маршруты

```mermaid
flowchart LR
    subgraph Routes["Маршруты"]
        R0["/"]
        R1["/surfaces"]
        R2["/surfaces/add"]
        R3["/clients"]
        R4["/clients/add"]
        R5["/downtime"]
        R6["/downtime/add"]
        R7["/calendar"]
    end

    R0 --> R1
    R0 --> R2
    R0 --> R3
    R0 --> R4
    R0 --> R5
    R0 --> R6
    R0 --> R7

    R1 --> R2
    R3 --> R4
    R5 --> R6
```

---

## 3. Модуль «Поверхности»

```mermaid
flowchart TB
    subgraph SurfacesModule["Модуль: Рекламные поверхности"]
        A[Список поверхностей] --> B{Действия}
        B --> C[Поиск / фильтр по типу и статусу]
        B --> D[Добавить поверхность]
        B --> E[Подробнее]
        B --> F[Сдать в аренду]
        B --> G[Отметить простой]
    end

    D --> D1[Форма: название, адрес, тип, цена, фото]
    D1 --> D2[Сохранить → список]

    E --> E1[Страница поверхности / карточка]
    F --> F1[Форма аренды: клиент, даты]
    G --> G1["/downtime/add?surface=id"]

    subgraph SurfaceStates["Статусы поверхности"]
        S1[Свободна]
        S2[Занята]
        S3[В простое]
    end
```

---

## 4. Модуль «Клиенты»

```mermaid
flowchart TB
    subgraph ClientsModule["Модуль: Клиенты"]
        A[Список клиентов] --> B{Действия}
        B --> C[Поиск по названию и контактам]
        B --> D[Добавить клиента]
        B --> E[Карточка клиента]
        B --> F[Новая аренда]
        B --> G[Редактировать]
    end

    D --> D1[Форма: компания, контакт, телефон, email, ИНН, адрес]
    D1 --> D2[Сохранить → список]

    E --> E1["/clients/:id"]
    F --> F1[Выбор поверхности и дат]
    G --> G1["/clients/:id/edit"]
```

---

## 5. Модуль «Простои»

```mermaid
flowchart TB
    subgraph DowntimeModule["Модуль: Простои"]
        A[Список простоев] --> B{Действия}
        B --> C[Поиск / фильтр: активные / завершённые]
        B --> D[Отметить простой]
        B --> E[Изменить]
        B --> F[Завершить простой]
    end

    D --> D1[Форма: поверхность, причина, даты, комментарий]
    D1 --> D2[Сохранить → список]

    A --> State{Статус}
    State --> Active[Активный]
    State --> Ended[Завершённый]
```

---

## 6. Модуль «Календарь»

```mermaid
flowchart TB
    subgraph CalendarModule["Модуль: Календарь"]
        A[Календарь по месяцам] --> B[Переключение месяца]
        A --> C[События в ячейках дня]
        C --> C1[Начало аренды]
        C --> C2[Окончание аренды]
        C --> C3[Ремонт / простой]
    end
```

---

## 7. Связи сущностей (данные)

```mermaid
flowchart LR
    subgraph Entities["Сущности"]
        Surface[Поверхность\nназвание, адрес, тип, цена, статус]
        Client[Клиент\nназвание, контакт, телефон, email]
        Rental[Аренда\nповерхность, клиент, даты]
        Downtime[Простой\nповерхность, причина, даты]
    end

    Surface --> Rental
    Client --> Rental
    Surface --> Downtime

    Rental --> CalendarData[События в календаре]
    Downtime --> CalendarData
```

---

## 8. Сценарий: добавление поверхности и аренда

```mermaid
sequenceDiagram
    participant U as Пользователь
    participant P as Приложение
    participant S as Поверхности
    participant C as Клиенты

    U->>P: Главная
    U->>P: Добавить поверхность
    P->>S: /surfaces/add
    U->>S: Заполняет форму
    U->>S: Сохранить
    S->>P: Список поверхностей

    U->>P: Поверхности
    U->>S: Сдать в аренду (свободная)
    P->>C: Выбор клиента / данные
    U->>P: Указывает клиента и даты
    P->>S: Поверхность → статус «Занята»
```

---

## 9. Сводная блок-схема функций

```mermaid
flowchart TB
    Start([Вход в приложение]) --> Layout[Layout]
    Layout --> Nav{Меню}

    Nav --> Dashboard[Главная: статистика, события, быстрые действия]
    Nav --> Surfaces[Поверхности: список, фильтры, CRUD]
    Nav --> Clients[Клиенты: список, поиск, CRUD]
    Nav --> Downtime[Простои: список, фильтры, добавить/завершить]
    Nav --> Calendar[Календарь: просмотр аренд и простоев]

    Dashboard --> QuickAdd[Быстрые действия]
    QuickAdd --> Surfaces
    QuickAdd --> Clients
    QuickAdd --> Downtime

    Surfaces --> SurfaceForm[Форма поверхности]
    Clients --> ClientForm[Форма клиента]
    Downtime --> DowntimeForm[Форма простоя]

    SurfaceForm --> Save[Сохранение]
    ClientForm --> Save
    DowntimeForm --> Save
    Save --> List[Обновление списка]
```

---

Файл можно открыть в любом редакторе с поддержкой Mermaid (VS Code с расширением, GitHub, Notion и т.п.) или сгенерировать изображение через [Mermaid Live Editor](https://mermaid.live).
