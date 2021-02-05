# Проект **Погода**

практика по такой то теме


Цели:
* получить текущую локацию
* по сети получить погоду для текущей локации
* отобразить погоду на форме

## Начало

Создайте новый проект (*Empty Activity*)

## Получение текущей локации

### Сторонние библиотеки

В стандартной реализации геолокации слишком много букв, к счастью есть [библиотека](https://github.com/BirjuVachhani/locus-android), в которой вся рутина скрыта:

1. Добавляем репозиторий в build.graddle (Project) (в ветку *allprojects* -> *repositories*)

    ```
    maven { url 'https://jitpack.io' }
    ```

    ![](../img/as027.png)


2. Добавляем зависимости в build.graddle (Module app)

    ```
    implementation 'com.google.android.gms:play-services-location:17.0.0'
    implementation 'com.github.BirjuVachhani:locus-android:3.0.1'
    ```

    ![](../img/as028.png)


3. В конструктор добавляем запрос геолокации:

    ```kt
    Locus.getCurrentLocation(this) { result ->
        result.location?.let {
            tv.text = "${it.latitude}, ${it.longitude}"
        } ?: run {
            tv.text = result.error?.message
        }
    }
    ```

    >Последние версии Android Studio почему-то перестали нормально находить визуальные элементы:
    >* добавьте id элементу TextView
    >

Полный текст программы:

```kt
package com.example.locator2

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.birjuvachhani.locus.Locus
import kotlinx.android.synthetic.main.activity_main.*

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        Locus.getCurrentLocation(this) { result ->
            result.location?.let {
                tv.text = "${it.latitude}, ${it.longitude}"
            } ?: run {
                tv.text = result.error?.message
            }
        }

    }
}
```

## Http запросы

В Kotlin-е есть встроенные функции работы с http-запросами, но стандартный код для сетевых запросов сложен, излишен и в реальном мире почти не используется. Используются библиотеки.
Самые популярные: [OkHttp](https://square.github.io/okhttp/) и Retrofit.

Рассмотрим работу к **OkHttp**

https://square.github.io/okhttp/recipes/ - примеры синхронных и асинхронных запросов на котлине

### Подключение библиотеки к проекту:
   
![](/img/as018.png)

1. На закладке **Project** в **Gradle Scripts** открываем файл **build.gradle (Module: app)**

2. В файле находим секцию **dependencies** (зависимости)

3. Добавляем нашу библиотеку ``implementation 'com.squareup.okhttp3:okhttp:4.2.1'``. На момент написания методички последняя версия была 4.2.1, вы можете уточнить актуальную версию на сайте.

4. Синхронизируйте измения (Gradle скачает обновившиеся зависимости)
    
5. В манифест добавляем права на доступ в интернет
```
<uses-permission android:name="android.permission.INTERNET" />
```    

6. В функцию определения координат вместо вывода координат на экран вставаляем вызов функции, запрашивающей погоду для этих координат


```kt
Locus.getCurrentLocation(this) { result ->
    result.location?.let {
        //tv.text = "${it.latitude}, ${it.longitude}"

        getWheather(it.longitude, it.latitude)

    } ?: run {
        tv.text = result.error?.message
    }
}
```

## Вывод иконки погоды

Для отображения иконки погоды используем компонент ImageView и библиотеку Glide. Для установки библиотеки:

    * добавить репозиторий mavenCentral() в build.graddle (Project)
    * добавить зависимость ``implementation 'com.github.bumptech.glide:glide:4.10.0'`` в build.graddle (Module)

Функция запроса погоды

```kt
// http клиент
private val client = OkHttpClient()

fun getWheather(lon: Double, lat: Double) {
    val token = "d4c9eea0d00fb43230b479793d6aa78f"
    val url = "https://api.openweathermap.org/data/2.5/weather?lat=${lat}&lon=${lon}&units=metric&appid=${token}"

    val request = Request.Builder().url(url).build()

    client.newCall(request).enqueue(object : Callback {

        override fun onFailure(call: Call, e: IOException) {
            setText( e.toString() )
        }

        override fun onResponse(call: Call, response: Response) {
            response.use {
                if (!response.isSuccessful) throw IOException("Unexpected code $response")

                // так можно достать заголовки http-ответа
                //for ((name, value) in response.headers) {
                //  println("$name: $value")
                //}

                //строку преобразуем в JSON-объект
                var jsonObj = JSONObject(response.body!!.string())


                // обращение к визуальному объекту из потока может вызвать исключение
                // нужно присвоение делать в UI-потоке
                setText( jsonObj )
            }
        }
    })
}

fun setText(t: JSONObject){
    runOnUiThread { 
        // достаем из ответа сервера название иконки погоды
        val wheather = t.getJSONArray("weather")
        val icoName = wheather.getJSONObject(0).getString("icon")
        val icoUrl = "https://openweathermap.org/img/w/${icoName}.png"

        // аналогично достаньте значение температуры и выведите на экран

        // загружаем иконку и выводим ее на icon (ImageView)
        Glide.with(this).load( icoUrl ).into( icon )
    }
}
```
