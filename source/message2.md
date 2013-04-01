## 軟體短訊-Pandoc 格式轉換系統

在 Web 盛行之後，使用簡易文字的方式撰寫網頁的需求就一直存在，於是產生了像 wiki 這樣的格式，
相對的也就需要一些工具將這些格式轉換成網頁。

維基百科就是這樣一個例子，他們透過 Mediawiki 這樣的格式撰寫文章，然後維基百科的網站就會用
程式將這種簡易的格式轉換成網頁，呈現給使用者看。

Wiki 類型的簡易書寫格式，除了 Mediawiki 之外，還有越來越多從 markdown 衍生出來的語法，像是
ReStructureText (RST), Textile 等等。

在上一篇短訊中，我們介紹了 Markdown 這種很容易撰寫與閱讀的格式，當然我們也就需要一種程式，
可以將 markdown 文件轉換成網頁，pandoc 正是這樣的一個工具程式。

Pandoc 不只可以將 Markdown 轉換成網頁，還可以轉換成 epub, latex, doc, odt 等格式的電子書，
也可以在各種簡易書寫格式之間轉換 (例如 markdown 轉 mediawiki)，因此 pandoc 也被稱為文件格式
轉換的瑞士萬用刀。

Pandoc 的使用很容易，以下是一些 pandoc 的使用範例，其參數的用法與 c 語言編譯器 gcc 很像。

```
pandoc input.md -o output.html               # markdown 轉 html

pandoc -s input.md -o output.tex             # markdown 轉 latex

pandoc -s --webtex input.md -o output.html   # 有數學式可加上 --webtex, --mathml 或 --mathjax 等參數

pandoc -s --toc -c ../css/pmag.css -B header.htm -A footer.htm input.md -o output.htm
```

Pandoc 會自動根據輸入輸出檔的附檔名進行辨認，因此不需要特別指定輸入輸出檔的格式 (當然也可以強制指定格式)。

參數 -s 代表 --standalone，也就是要產生獨立可顯示的輸出檔時使用的，像是單獨的網頁 html，或者 epub 等都會
加上這個選項。

Pandoc 也支援一些 markdown 的延伸語法，像是 latex 數學式可用 `$ ... $` 這樣的方式包裹住，
例如 `$\int_0^{\infty} f(x) dx$` 的呈現結果會是 $\int_0^{\infty} f(x) dx$，但是要用數學式必須加上 
--webtex, --mathml 或 --mathjax 等參數，以便選擇數學式要用哪種顯示方式呈現。

在上述範例中的最後一句，--toc 代表要產生目錄 (Table Of Content, TOC)，而  -c ../css/pmag.css 則是要套用
該 CSS 樣式，-B header.htm 代表要在輸出檔案前補上 header.htm 作為檔頭，而 -A footer.htm 則代表
要在輸出檔案尾端補上檔尾 footer.htm。

Pandoc 的官方網站的網址如下：

* <http://johnmacfarlane.net/pandoc/>

您可以從其中的 [README](http://johnmacfarlane.net/pandoc/README.html) 取得詳細的
使用說明，或者直接參考 [demos](http://johnmacfarlane.net/pandoc/demos.html)，
以範例的方式學習 pandoc 的用法。【本文由陳鍾誠撰寫】

