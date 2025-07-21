# ascii-to-image
A simple program that converts ascii text into an image

# Text to Image (WinForms)

A simple program for visual representation of text as an image. 
Turns any text into colorful stripes and back.

## What is it?

The program takes your text and:
1. Converts each character into a color (according to the ASCII code)
2. Draws vertical stripes of these colors
3. Can restore the text back from the picture

## How to use?

1. **Running**: 
 - Open `FilesSystem.sln` in Visual Studio 
 - Press F5 to run 
2. **Main functions**:
 - 📝 Write text directly in the window
 - 📂 Load/save text files
 - 🎨 Save text as an image (.bmp)
 - 🔍 Load text back from an image 
3. **How it works**:
 - Each character → ASCII code → RGB color
 - The first few pixels are service information
 - The main stripes are a sequence of color characters

## Technical details
- C# WinForms
- System.Drawing for working with images


# Текст в изображение (WinForms)

Простая программа для визуального представления текста в виде изображения.  
Превращает любой текст в разноцветные полоски и обратно.

## Что это?

Программа берет ваш текст и:
1. Преобразует каждый символ в цвет (по ASCII коду)
2. Рисует вертикальные полоски этих цветов
3. Может восстановить текст обратно из картинки

## Как пользоваться?

1. **Запуск**:  
   - Откройте `FilesSystem.sln` в Visual Studio  
   - Нажмите F5 для запуска

2. **Основные функции**:
   - 📝 Писать текст прямо в окне
   - 📂 Загружать/сохранять текстовые файлы
   - 🎨 Сохранять текст как изображение (.bmp)
   - 🔍 Загружать текст обратно из картинки

3. **Как это работает**:
   - Каждый символ → ASCII код → RGB цвет
   - Первые несколько пикселей служебная информация
   - Основные полоски - это последовательность цветов-символов

## Технические детали
- C# WinForms
- System.Drawing для работы с изображениями
- Простой алгоритм преобразования текста ↔ изображение


Написанно для удовольствия
