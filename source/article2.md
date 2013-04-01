## JavaScript (4) – 在互動網頁中的應用：以功能表為例 (作者：陳鍾誠)

JavaScript 是唯一被各家瀏覽器所共同支援的程式語言，因此在設計網站的時候，我們如果不採用像 Flash 或 Silverlight 這樣的外掛技術，就必須採用 JavaScript 來設計互動式網頁。

在 node.js 這樣的伺服端 JavaScript 開發平台推出之後，我們就能夠採用 JavaScript 同時設計 Client 端與 Server 端的程式，這樣的模式相當的吸引人，我們會在後續的文章中介紹這樣的網站設計方法。

在本文當中，我們將透過 JavaScript 設計一個互動網頁的功能表，以便展示瀏覽器中的 JavaScript 程式是如何運作的。

### 功能表程式

以下是一個功能表的程式的執行結果，當我們的滑鼠移到功能項上時，就會浮現子功能表，而當我們點下子功能表中的項目時，就會出現一個 alert 視窗，顯示該功能子項被點選的訊息。

![功能表程式的執行畫面](../img/JavaScriptMenuRun.jpg)

以下是這個網頁的原始 HTML 程式碼，其中 `<style>...</style>` 部分是 CSS 原始碼，而 `<script ...</script>` 部分則是 JavaScript 程式碼。

```html
<html>
<head>
<title>範例 -- 功能表實作</title>
<style>
.menu   { background-color:black; color:white; padding:10px; 
          vertical-align:top; width:100px; list-style-type:none; }
.menu a { color:white; text-decoration:none; }
</style>
<script type="text/javascript">
function show(id) {
  document.getElementById(id).style.visibility='visible';
}
 
function hide(id) {
  document.getElementById(id).style.visibility='hidden';
}
</script>    
  </head>
  <body onload="JavaScript:hide('popup1');hide('popup2');">
      <ul onmouseover="show('popup1');"  onmouseout="hide('popup1')" 
          style="position:absolute; left:100px; top:20px">
        <li id="menu1" class="menu">menu1</li>
        <ul id="popup1" class="menu">
          <li><a href="JavaScript:alert('1.1');">menu 1.1</a></li>
          <li><a href="JavaScript:alert('1.2');">menu 1.2</a></li>
        </ul> 
      </ul>
      <ul onmouseover="show('popup2');" onmouseout="hide('popup2')" 
          style="position:absolute; left:220px; top:20px">
        <li id="menu2" class="menu">menu2</li>
        <ul id="popup2" class="menu">
          <li><a href="JavaScript:alert('2.1');">menu 2.1</a></li>
          <li><a href="JavaScript:alert('2.2');">menu 2.2</a></li>
          <li><a href="JavaScript:alert('2.3');">menu 2.3</a></li>
        </ul>
      </ul>
  </body>
</html>
```

雖然以上程式只是一個小小的功能表程式碼，但是要能夠讀懂，而且寫得出來，卻要懂相當多的技術才行，這些技術包含 HTML, CSS , JavaScript 與 Document Object Model (DOM)。


### 程式解析

在 HTML 的一開始，我們用以下語法描述了功能表所需要的 CSS 樣式，當我們套用在像 `<li id="menu2" class="menu">menu2</li>` 這樣的 HTML 項目上時，就會呈現比較好看的功能表排版格式，而這正是 CSS 樣式的功用。

```html
<style>
.menu   { background-color:black; color:white; padding:10px; 
          vertical-align:top; width:100px; list-style-type:none; }
.menu a { color:white; text-decoration:none; }
</style>

```

以上的 CSS 語法中，要求功能表要以黑底白字的方式 (`background-color:black; color:white;`) 顯示，邊緣補上 10 點的空白(`padding:10px;`)，而且是以向上靠攏 (`vertical-align:top;`) 的方式，每個功能表的寬度都是 100 點 (`width:100px;`)，然後不要顯示項目前面的點符號 (`list-style-type:none;`)。

接著是一段 JavaScript 程式碼的語法，定義了 show(id) 與 hide(id) 這兩個函數，我們可以用這兩個函數在適當的時候讓功能表顯示出來或者是隱藏掉，這樣才能做到「浮現」的功能。

```html
<script type="text/javascript">
function show(id) {
  document.getElementById(id).style.visibility='visible';
}
 
function hide(id) {
  document.getElementById(id).style.visibility='hidden';
}
</script>    
```

然後，開始進入 HTML 的 body 區塊，其中定義了兩組功能表，第一組的內容如下：

```html
      <ul onmouseover="show('popup1');"  onmouseout="hide('popup1')" 
          style="position:absolute; left:100px; top:20px">
        <li id="menu1" class="menu">menu1</li>
        <ul id="popup1" class="menu">
          <li><a href="JavaScript:alert('1.1');">menu 1.1</a></li>
          <li><a href="JavaScript:alert('1.2');">menu 1.2</a></li>
        </ul> 
      </ul>
```

上述區塊最外層的 `<ul>...</ul>` 定義了整個功能表的結構，是由功能母項 `<li id="menu1" class="menu">menu1</li>` 與子功能表 `<ul id="popup1" class="menu">...</ul>` 所組合而成的，而 `<ul onmouseover="show('popup1');"  onmouseout="hide('popup1')" style="position:absolute; left:100px; top:20px">` 這一段除了定義了該功能表要顯示在絕對位置 (100,20) 的地方之外，還定義了 onmouseover 與 onmouseout 的事件，這兩個事件讓功能表可以在滑鼠移入時顯示出來，然後在滑鼠移出時隱藏起來，因而做到了浮現式功能表所要求的條件。

由於我們在 `<li>...</li>` 內的超連結 `<a href="JavaScript:alert('2.1');">menu 2.1</a>` 使用了 JavaScript 語法，因此在該超連結被點選時，就會有警告視窗顯示 2.1 的訊息，這個訊息僅僅是讓我們知道該功能項被點選了而已，沒有真實的功能。

同樣的、第二個功能表的程式碼也是非常類似的，請讀者看看是否能夠讀者其內容。

```html
      <ul onmouseover="show('popup2');" onmouseout="hide('popup2')" 
          style="position:absolute; left:220px; top:20px">
        <li id="menu2" class="menu">menu2</li>
        <ul id="popup2" class="menu">
          <li><a href="JavaScript:alert('2.1');">menu 2.1</a></li>
          <li><a href="JavaScript:alert('2.2');">menu 2.2</a></li>
          <li><a href="JavaScript:alert('2.3');">menu 2.3</a></li>
        </ul>
      </ul>
```

看到這裡，讀者應該大致理解了上述功能表網頁的運作原理，但事實上、我們還漏掉了一行重要的程式碼，那就是 `<body onload="JavaScript:hide('popup1');hide('popup2');">` 這一行，這一行讓浮現功能表能在一開始就處於隱藏狀態，才不會一進來就看到兩個功能表都浮現出來的錯誤情況。


### 結語

從上述範例中，您可以看到瀏覽器中的 JavaScript ，通常是透過調整網頁某些項目的 CSS 屬性，以達成互動性的功能，這種互動網頁技術，事實上是結合了 HTML+CSS+JavaScript 等技術才能達成的功能，因此這三項技術在瀏覽器當中幾乎是合為一體、可以說是缺一不可的。

在本期中我們說明了互動式網頁的設計方式，但這樣的設計方式非常冗長，對程式人員而言是很大的負擔，因此在互動網頁興起之後，就逐漸出現了各式各樣的互動性 JavaScript 框架，也就是現成的 JavaScript 函式庫，讓我們可以輕易做出很好的互動性，像是 jQuery, ExtJS, YUI, Prototype, Dojo 等互動性函式庫，以便減輕 JavaScript 程式人員的負擔，增加程式員的生產力，在下一期當中，我們將使用最常被使用的 jQuery 框架，再度說明互動網頁的寫法，我們下期見！

