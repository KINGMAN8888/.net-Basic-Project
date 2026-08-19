# دليل مشروع ShopApp — شرح تفصيلي سطر بسطر

> دليل تعليمي كامل لمشروع **ASP.NET Core MVC 8** — من أول سطر في `Program.cs` لحد آخر Tag Helper في الـ Views.

---

## إزاي تستخدم الدليل ده

الدليل مقسوم لـ **١٦ فصل**. كل فصل بيشرح جزء من المشروع، وبينتهي بـ **تمارين عملية** تحت عنوان "جرّب بنفسك".

**نصيحة مهمة:** مفتوح عندك الـ VS Code جنب الدليل ده. كل ما تقرا شرح ملف، افتح الملف نفسه وشوف الكلام على أرض الواقع. القراءة لوحدها مش هتثبّت المعلومة — التجربة هي اللي بتثبّتها.

**ترتيب القراءة المقترح للمبتدئ:**

| الترتيب | الفصل | الموضوع |
|---|---|---|
| ١ | الفصل 1 | إيه هو MVC أصلاً؟ — المفهوم |
| ٢ | الفصل 3 | رحلة الـ Request — الصورة الكبيرة |
| ٣ | الفصل 5 | الموديلات — البيانات |
| ٤ | الفصل 7 | العلاقات — الربط |
| ٥ | الفصل 8 | الـ DbContext — قاعدة البيانات |
| ٦ | الفصل 10 | Dependency Injection — أهم مفهوم في ‎.NET‎ الحديث |
| ٧ | الفصل 12 | ‏`Program.cs` — نقطة التجميع |
| ٨ | الفصل 13 | الـ Controllers — المنطق |
| ٩ | الفصل 15 | الـ Views — الواجهة |
| ١٠ | الفصل 16 | Migrations — إدارة الداتابيز |

باقي الفصول مرجعية — ارجعلها وقت الحاجة.

---

## فهرس المحتويات

| # | الفصل | بيجاوب على سؤال |
|---|---|---|
| 1 | إيه هو ASP.NET Core MVC؟ | ليه أصلاً بنقسّم المشروع كده؟ |
| 2 | خريطة المشروع | كل ملف بيعمل إيه؟ |
| 3 | رحلة الـ Request | إيه اللي بيحصل لما أكتب URL؟ |
| 4 | ملف `.csproj` | إزاي المشروع بيعرف الباكدجات؟ |
| 5 | `Models/Category.cs` | إزاي أعرّف بيانات؟ |
| 6 | `Models/Product.cs` | إيه الـ Data Annotations دي؟ |
| 7 | العلاقات (Relations) | إزاي أربط جدولين؟ |
| 8 | `Data/AppDbContext.cs` | إزاي الكود بيكلّم الداتابيز؟ |
| 9 | `appsettings.json` | فين بحط الإعدادات؟ |
| 10 | Dependency Injection | إيه ده وليه مهم؟ |
| 11 | `Services/` | ليه أعمل طبقة زيادة؟ |
| 12 | `Program.cs` | إزاي كل ده بيتجمّع؟ |
| 13 | `CategoriesController.cs` | إزاي أعمل CRUD؟ |
| 14 | `ProductsController.cs` | إيه الفرق بين النمطين؟ |
| 15 | الـ Views و Razor | إزاي أعرض البيانات؟ |
| 16 | Migrations | إزاي أعدّل الداتابيز؟ |
| 17 | الأمان | إيه الحمايات المطبّقة؟ |
| 18 | أخطاء شائعة | لما حاجة تبوظ أعمل إيه؟ |
| 19 | خطواتك الجاية | أتعلم إيه بعد كده؟ |

---

# الفصل 1: إيه هو ASP.NET Core MVC؟

## المشكلة اللي MVC بيحلها

تخيل إنك عايز تعمل صفحة تعرض منتجات. من غير أي تنظيم، هتكتب ملف واحد فيه:

- كود بيتصل بالداتابيز
- كود بيتحقق إن السعر مش سالب
- كود HTML بيرسم الجدول
- كود بيتعامل مع زرار "احذف"

الملف ده هيبقى ٥٠٠ سطر. ولما تيجي تغيّر شكل الجدول، هتلف وسط كود الداتابيز. ولما تيجي تختبر التحقق من السعر، مش هتقدر — لأنه متشابك مع الـ HTML.

**MVC بيقسّم المسؤولية على تلات أجزاء:**

| الجزء | مسؤوليته | في مشروعنا |
|---|---|---|
| **Model** | شكل البيانات وقواعدها | `Category.cs` و `Product.cs` |
| **View** | العرض والشكل فقط | ملفات `.cshtml` |
| **Controller** | استقبال الطلب وتنسيق الرد | `ProductsController.cs` |

## القاعدة الذهبية

> **الـ Controller مش بيرسم HTML، والـ View مش بتكلّم الداتابيز.**

كل واحد في حتته. لما تحب تغيّر الشكل → تفتح الـ View بس. لما تحب تغيّر منطق الحفظ → تفتح الـ Controller أو الـ Service بس.

## فين الـ Model في مشروعنا بالظبط؟

كلمة "Model" في MVC أوسع من مجرد كلاس البيانات. عندنا:

| الملف | دوره في الـ "M" |
|---|---|
| `Models/Product.cs` | الـ Entity — شكل البيانات وقواعد التحقق |
| `Data/AppDbContext.cs` | طبقة الوصول للداتابيز |
| `Services/ProductService.cs` | منطق العمل (Business Logic) |

التلاتة دول مجتمعين بيشكّلوا الـ "M" في MVC.

---

## جرّب بنفسك — الفصل 1

**١.** افتح `Views/Products/Index.cshtml` ودوّر على أي سطر بيتصل بالداتابيز. مش هتلاقي — ده الفصل بين المسؤوليات.

**٢.** افتح `Controllers/ProductsController.cs` ودوّر على أي وسم HTML. مش هتلاقي كمان.

**٣.** سؤال للتفكير: لو عايز تغيّر ألوان جدول المنتجات، هتفتح أنهي ملف؟ ولو عايز تمنع حفظ منتج سعره أقل من ١٠٠ جنيه؟

<details>
<summary>الإجابة</summary>

- تغيير الألوان → `Views/Products/Index.cshtml` (View فقط)
- منع سعر أقل من ١٠٠ → `Models/Product.cs` في الـ `[Range]` (Model فقط)

ولاحظ إن كل تعديل في ملف واحد بس. ده بالظبط اللي MVC اتعمل عشانه.
</details>

---

# الفصل 2: خريطة المشروع

## الشجرة الكاملة

```
ShopApp/
│
├── Models/                      ← طبقة البيانات
│   ├── Category.cs              ← الموديل الأول — التصنيف
│   ├── Product.cs               ← الموديل الثاني — المنتج
│   └── ErrorViewModel.cs        ← موديل صفحة الخطأ — جاي مع القالب
│
├── Data/                        ← طبقة الوصول للداتابيز
│   └── AppDbContext.cs          ← الـ DbContext + العلاقة + البيانات المبدئية
│
├── Services/                    ← طبقة منطق العمل
│   ├── IProductService.cs       ← العقد (Interface)
│   └── ProductService.cs        ← التنفيذ
│
├── Controllers/                 ← طبقة استقبال الطلبات
│   ├── CategoriesController.cs  ← CRUD التصنيفات
│   ├── ProductsController.cs    ← CRUD المنتجات
│   └── HomeController.cs        ← التحويل + صفحة الخطأ
│
├── Views/                       ← طبقة العرض
│   ├── Categories/
│   │   ├── Index.cshtml         ← قائمة التصنيفات
│   │   ├── Create.cshtml        ← فورم الإضافة
│   │   ├── Edit.cshtml          ← فورم التعديل
│   │   ├── Details.cshtml       ← صفحة التفاصيل
│   │   └── Delete.cshtml        ← تأكيد الحذف
│   ├── Products/                ← نفس الخمس صفحات
│   ├── Shared/
│   │   ├── _Layout.cshtml       ← القالب المشترك — الهيدر والفوتر
│   │   ├── Error.cshtml         ← صفحة الخطأ
│   │   └── _ValidationScriptsPartial.cshtml  ← سكربتات التحقق
│   ├── _ViewImports.cshtml      ← الـ using والـ Tag Helpers لكل الـ Views
│   └── _ViewStart.cshtml        ← بيحدد الـ Layout الافتراضي
│
├── Migrations/                  ← تاريخ تغييرات الداتابيز
│   ├── 20260819084454_InitialCreate.cs           ← الأوامر — Up و Down
│   ├── 20260819084454_InitialCreate.Designer.cs  ← لقطة وقت الإنشاء
│   └── AppDbContextModelSnapshot.cs              ← اللقطة الحالية
│
├── wwwroot/                     ← الملفات الثابتة — يوصلها المتصفح مباشرة
│   ├── css/site.css
│   ├── js/site.js
│   └── lib/                     ← Bootstrap و jQuery
│
├── Properties/
│   └── launchSettings.json      ← إعدادات التشغيل المحلي — البورت والبيئة
│
├── Program.cs                   ← نقطة البداية
├── appsettings.json             ← الإعدادات — Connection String
├── appsettings.Development.json ← إعدادات تطوير — بتطغى على اللي فوق
├── ShopApp.csproj               ← تعريف المشروع والباكدجات
└── shopapp.db                   ← ملف قاعدة بيانات SQLite — بيتولد تلقائياً
```

## القواعد اللي ASP.NET بيعتمد عليها (Conventions)

ASP.NET Core بيعتمد على **الاصطلاح بدل الإعداد** — يعني بيفهم نيتك من التسمية من غير ما تكتب إعدادات:

| الاصطلاح | المثال | النتيجة |
|---|---|---|
| اسم الكنترولر لازم ينتهي بـ `Controller` | `ProductsController` | الراوت بيبقى `/Products` |
| الـ View لازم تكون في `Views/<اسم الكنترولر>/` | `Views/Products/` | `return View()` بيلاقيها لوحده |
| اسم ملف الـ View = اسم الـ Action | `Index()` → `Index.cshtml` | ربط تلقائي |
| ملف يبدأ بـ `_` = ملف مساعد | `_Layout.cshtml` | مش صفحة مستقلة |
| اسم `Id` أو `<Class>Id` | `Id` في `Product` | Primary Key تلقائي |
| اسم `<Navigation>Id` | `CategoryId` | Foreign Key تلقائي |

> **ليه ده مهم؟** لأنك لما تخالف الاصطلاح، الحاجات بتقف عن الشغل من غير رسالة خطأ واضحة. لو حطيت `Index.cshtml` في `Views/Product/` (مفرد) بدل `Views/Products/`، هتاخد خطأ "View not found".

---

## جرّب بنفسك — الفصل 2

**١.** غيّر اسم فولدر `Views/Products` لـ `Views/Product` وشغّل المشروع. هتاخد إيه؟ رجّعه تاني.

**٢.** افتح `Views/_ViewStart.cshtml` — سطر واحد بس. اقرا محتواه وفكّر: ده بيعمل إيه؟

<details>
<summary>الإجابة</summary>

```csharp
@{
    Layout = "_Layout";
}
```

بيقول لكل الـ Views في المشروع: "استخدم `_Layout.cshtml` كقالب". عشان كده مش محتاج تكتب السطر ده في كل View لوحدها. لو عايز View معينة من غير Layout، اكتب فيها `@{ Layout = null; }`.
</details>

**٣.** فولدر `wwwroot` — جرّب تفتح `http://localhost:5124/css/site.css` في المتصفح. هيفتح عادي. دلوقتي جرّب `http://localhost:5124/Program.cs`. مش هيفتح — ليه؟

<details>
<summary>الإجابة</summary>

`wwwroot` هو الفولدر الوحيد المكشوف للمتصفح (عن طريق `app.UseStaticFiles()`). أي ملف بره الفولدر ده — كودك، إعداداتك، الداتابيز — محمي ومش قابل للتحميل. ده إجراء أمني أساسي.
</details>

---

# الفصل 3: رحلة الـ Request

ده أهم فصل في الدليل. لو فهمت الرحلة دي، هتفهم ASP.NET كله.

## السيناريو: المستخدم كتب `localhost:5124/Products/Details/2`

### الخطوات التسعة

**١ — المتصفح يبعت الطلب**
```http
GET /Products/Details/2 HTTP/1.1
Host: localhost:5124
```

**٢ — Kestrel يستقبله**
سيرفر الويب المدمج في ‎.NET‎ بيستلم الطلب ويحوّله لكائن `HttpContext`.

**٣ — الطلب يمر على الـ Middleware Pipeline** بالترتيب اللي في `Program.cs`:

| الترتيب | الـ Middleware | بيعمل إيه |
|---|---|---|
| ١ | `UseHttpsRedirection()` | لو الطلب على http يحوّله لـ https |
| ٢ | `UseStaticFiles()` | لو الطلب لملف في `wwwroot` يبعته ويوقف هنا |
| ٣ | `UseRouting()` | يحلّل الـ URL ويحدد الوجهة |
| ٤ | `UseAuthorization()` | يفحص الصلاحيات (مفيش حماية في مشروعنا دلوقتي) |

**٤ — الـ Router يطابق الـ URL** مع القالب `{controller=Products}/{action=Index}/{id?}`:

| الجزء في الـ URL | المتغير | القيمة |
|---|---|---|
| `Products` | controller | `Products` |
| `Details` | action | `Details` |
| `2` | id | `2` |

**٥ — الـ DI Container يبني الكنترولر** بسلسلة اعتماديات:

| المطلوب | يحتاج | النتيجة |
|---|---|---|
| `ProductsController` | `IProductService` | يعمل `ProductService` |
| `ProductService` | `AppDbContext` | يعمل `AppDbContext` |
| `AppDbContext` | `DbContextOptions` | ياخدها من الإعدادات |

وبعدها: `new ProductsController(productService, context)`

**٦ — Model Binding** يحوّل `"2"` النصية لـ `int` ويحطها في المعطى، ثم ينادي `await controller.Details(2)`

**٧ — الـ Action ينفّذ** — `await _productService.GetByIdAsync(2)` وEF Core يولّد:
```sql
SELECT p.*, c.* FROM Products p
LEFT JOIN Categories c ON p.CategoryId = c.Id
WHERE p.Id = 2
```
والنتيجة كائن `Product` جوّاه `Category`.

**٨ — `return View(product)`** — Razor يدوّر على `Views/Products/Details.cshtml`، يلفّها في `Views/Shared/_Layout.cshtml`، ويحوّل الكل لـ HTML.

**٩ — الرد يرجع** عكس الـ Pipeline لحد المتصفح. وبعدها الـ `DbContext` يتقفل ويتخلّص منه (لأنه `Scoped`).

## نقطة مهمة: الـ Middleware زي طبقات البصلة

الـ Middleware مش مجرد قايمة بتتنفذ بالترتيب — كل واحد بيلفّ اللي بعده:

```
Request  →  [ HttpsRedirection [ StaticFiles [ Routing [ Endpoint ] ] ] ]  →  Response
```

يعني الطلب بيدخل من برّه لجوّه، والرد بيخرج من جوّه لبرّه.

يعني كل Middleware بيشوف الطلب **وهو داخل** والرد **وهو خارج**. وده السبب إن **الترتيب مهم جداً**.

**مثال على خطأ الترتيب:** لو حطيت `UseAuthorization()` **قبل** `UseRouting()`، نظام الصلاحيات مش هيعرف المستخدم رايح على أنهي صفحة، فمش هيعرف يقرر يسمحله ولا لأ.

---

## جرّب بنفسك — الفصل 3

**١.** في `Program.cs`، انقل سطر `app.UseStaticFiles();` وحطه **بعد** `app.UseRouting();`. شغّل وشوف لو الـ CSS لسه شغال. (هيشتغل، بس بكفاءة أقل — الطلب بيمر على الـ Router الأول من غير داعي.)

**٢.** جرّب تفتح `localhost:5124/Products/Details/999`. إيه اللي بيحصل؟ وليه؟

<details>
<summary>الإجابة</summary>

بترجعلك صفحة 404. لأن في الـ Action:

```csharp
var product = await _productService.GetByIdAsync(id.Value);
if (product is null) return NotFound();
```

الـ `GetByIdAsync` رجّع `null` لأن مفيش منتج بالـ Id ده، فالـ Action رجّع `NotFound()` اللي بتترجم لـ HTTP 404.
</details>

**٣.** جرّب تفتح `localhost:5124/Products` من غير action. ليه اشتغلت ورجّعت القائمة؟

<details>
<summary>الإجابة</summary>

بسبب القيمة الافتراضية في قالب الراوت:

```csharp
pattern: "{controller=Products}/{action=Index}/{id?}"
```

`action=Index` معناها: لو المستخدم مكتبش action، استخدم `Index`. وعلامة `?` في `{id?}` معناها إن الـ id اختياري.
</details>

**٤.** جرّب تفتح `localhost:5124` لوحده. هيوديك على `/Products`. دوّر في `HomeController.cs` على السبب.
# الفصل 4: ملف `ShopApp.csproj`

ده ملف تعريف المشروع. الـ `dotnet` CLI بيقراه عشان يعرف يبني إيه وبأي أدوات.

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.11" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.11">
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
  </ItemGroup>

</Project>
```

## سطر بسطر

**`Sdk="Microsoft.NET.Sdk.Web"`**
بيقول إن ده مشروع ويب. الـ SDK ده بيجيب معاه تلقائياً كل مكتبات ASP.NET Core (Kestrel، Routing، Razor…) من غير ما تضيفها يدوي. لو كان مشروع Console كان هيبقى `Microsoft.NET.Sdk`.

**`<TargetFramework>net8.0</TargetFramework>`**
إصدار .NET المستهدف. لازم يكون عندك SDK الإصدار ده أو أحدث.

**`<Nullable>enable</Nullable>`**
دي ميزة قوية جداً في C# الحديثة اسمها **Nullable Reference Types**. لما تتفعّل:

```csharp
string name;    // ← الكومبايلر بيفترض إنها مش هتبقى null أبداً
string? name;   // ← علامة ؟ معناها "ممكن تبقى null"
```

الكومبايلر بيحذّرك لو حاولت تستخدم متغير ممكن يكون `null` من غير ما تتأكد. ده بيمنع أشهر خطأ في البرمجة: `NullReferenceException`.

في مشروعنا شوف الفرق:

```csharp
public string Name { get; set; } = string.Empty;  // مطلوب، فبنديله قيمة ابتدائية
public string? Description { get; set; }          // اختياري، ممكن يفضل null
public Category? Category { get; set; }           // ممكن ما تتحمّلش من الداتابيز
```

**`<ImplicitUsings>enable</ImplicitUsings>`**
بيضيف `using` تلقائي لأشهر المكتبات (`System`، `System.Linq`، `System.Collections.Generic`، `Microsoft.AspNetCore.Mvc` في مشاريع الويب…). عشان كده مش هتلاقي `using System;` في أول أي ملف — هي موجودة ضمنياً.

**`<PrivateAssets>all</PrivateAssets>`**
موجودة على باكدج `EntityFrameworkCore.Design` تحديداً. معناها: "الباكدج ده أداة وقت التطوير بس، متضمّهوش في النسخة النهائية اللي هترفعها على السيرفر". الباكدج ده بيستخدمه `dotnet ef` عشان يعمل الـ Migrations — التطبيق نفسه وهو شغال مش محتاجه.

---

## جرّب بنفسك — الفصل 4

**١.** شيل `<Nullable>enable</Nullable>` وشغّل `dotnet build`. هتلاحظ إن التحذيرات اختفت — بس كمان الحماية اختفت. رجّعها.

**٢.** جرّب تضيف باكدج جديد وشوف الملف بيتغير إزاي:
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```
افتح الـ `.csproj` بعدها. بعد كده شيله:
```bash
dotnet remove package Microsoft.EntityFrameworkCore.SqlServer
```

---

# الفصل 5: `Models/Category.cs`

## الكود الكامل

```csharp
using System.ComponentModel.DataAnnotations;

namespace ShopApp.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم التصنيف مطلوب")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "الاسم لازم يكون بين 2 و 60 حرف")]
    [Display(Name = "اسم التصنيف")]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
```

## سطر بسطر

### `namespace ShopApp.Models;`

لاحظ الفاصلة المنقوطة في الآخر من غير أقواس `{}`. دي ميزة C# 10 اسمها **File-scoped namespace** — كل الملف جوّه الـ namespace ده. بتوفّر مستوى إزاحة كامل مقارنة بالطريقة القديمة.

### `public int Id { get; set; }`

**المفتاح الأساسي (Primary Key).** EF Core بيتعرف عليه أوتوماتيك بقاعدتين:

1. خاصية اسمها `Id` بالظبط
2. أو خاصية اسمها `<اسم الكلاس>Id` — يعني `CategoryId` هنا برضه كان هيشتغل

ولأن نوعه `int`، EF بيخليه **AUTOINCREMENT** تلقائياً. شوف SQL اللي اتولّد:

```sql
"Id" INTEGER NOT NULL CONSTRAINT "PK_Categories" PRIMARY KEY AUTOINCREMENT
```

لو حبيت تسمّيه اسم تاني خالص (مثلاً `CategoryCode`)، لازم تقوله صراحة:

```csharp
[Key]
public int CategoryCode { get; set; }
```

### `[Required(ErrorMessage = "...")]`

ده **Data Annotation** — سمة (Attribute) بتضيف معلومة إضافية على الخاصية. `[Required]` بتعمل حاجتين في نفس الوقت:

| المكان | التأثير |
|---|---|
| في الداتابيز | العمود بيبقى `NOT NULL` |
| في الفورم | لو المستخدم ساب الحقل فاضي، `ModelState.IsValid` بترجع `false` والرسالة بتظهر |

والرسالة بتظهر في الـ View من خلال:
```html
<span asp-validation-for="Name" class="text-danger"></span>
```

### `[StringLength(60, MinimumLength = 2)]`

بتحدد أقصى وأدنى طول. تأثيرها:
- في SQL Server: العمود بيبقى `NVARCHAR(60)` بدل `NVARCHAR(MAX)`
- في التحقق: نص أقل من حرفين أو أكتر من ٦٠ بيترفض

> **ملحوظة:** في SQLite كل النصوص نوعها `TEXT` من غير حد أقصى، فالقيد بيتطبّق في مستوى التحقق بس مش في الداتابيز. لو حوّلت لـ SQL Server هتلاقي الفرق.

### `[Display(Name = "اسم التصنيف")]`

بتحدد الاسم اللي هيظهر للمستخدم. بتُستخدم في:

```html
<label asp-for="Name"></label>          <!-- هيطبع: اسم التصنيف -->
@Html.DisplayNameFor(m => m.Name)       <!-- نفس النتيجة -->
```

من غيرها كان هيظهر اسم الخاصية بالإنجليزي: `Name`. دي طريقة نظيفة تخلّي الواجهة عربي والكود إنجليزي.

### `public string Name { get; set; } = string.Empty;`

**ليه `= string.Empty`؟** بسبب `<Nullable>enable</Nullable>`. الكومبايلر عايز يتأكد إن أي خاصية `string` (مش `string?`) عندها قيمة من أول لحظة. من غير القيمة الابتدائية دي هتاخد تحذير:

```
Warning CS8618: Non-nullable property 'Name' must contain a non-null value
when exiting constructor.
```

### `public DateTime CreatedAt { get; set; } = DateTime.UtcNow;`

**ليه `UtcNow` مش `Now`؟**

`DateTime.Now` بيرجّع وقت الجهاز اللي شغّال عليه السيرفر. لو السيرفر في أمريكا والمستخدم في مصر، الأرقام هتبقى مختلفة. ولو نقلت السيرفر لمنطقة زمنية تانية، البيانات القديمة هتبقى غلط.

**القاعدة:** خزّن دايماً بـ UTC، واعرض بالتوقيت المحلي وقت العرض:

```csharp
// وقت التخزين
CreatedAt = DateTime.UtcNow;

// وقت العرض
@Model.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm")
```

### `public ICollection<Product> Products { get; set; } = new List<Product>();`

دي **Navigation Property** — أهم سطر في الملف من ناحية العلاقات. الفصل السابع كله عنها.

**ليه `ICollection` مش `List`؟** لأن EF Core محتاج يستبدلها بمجموعة خاصة بيه (Proxy) عشان يتتبّع التغييرات. `ICollection` هي أبسط واجهة بتديله المرونة دي. `List` كانت هتشتغل بس بتقيّده.

**ليه القيمة الابتدائية `= new List<Product>()`؟** عشان تتجنب `NullReferenceException`. لو التصنيف لسه اتعمل ومفيهوش منتجات، `category.Products.Count` هترجع `0` بدل ما ترمي استثناء.

---

## جرّب بنفسك — الفصل 5

**١.** غيّر `[StringLength(60, MinimumLength = 2)]` لـ `MinimumLength = 5`، شغّل، وحاول تضيف تصنيف اسمه "أب". شوف الرسالة بتظهر فين وإزاي.

**٢.** شيل `[Display(Name = "اسم التصنيف")]` وشوف الليبل في صفحة الإضافة بقى إيه.

**٣.** أضف خاصية جديدة:
```csharp
[Display(Name = "مفعّل")]
public bool IsActive { get; set; } = true;
```
شغّل المشروع دلوقتي. هيقع. ليه؟

<details>
<summary>الإجابة</summary>

هتاخد استثناء زي:
```
SQLite Error 1: 'no such column: c.IsActive'
```

لأنك غيّرت الـ **Model** بس منقلتش للـ **Database**. الكود دلوقتي بيدوّر على عمود مش موجود.

الحل: اعمل Migration جديدة تنقل التغيير:
```bash
dotnet ef migrations add AddIsActiveToCategory
dotnet ef database update
```

الدرس المهم: **أي تعديل في الموديلات لازم يتبعه Migration.** الفصل ١٦ بيشرح ده بالتفصيل.
</details>

---

# الفصل 6: `Models/Product.cs`

## الكود الكامل

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApp.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم المنتج مطلوب")]
    [StringLength(100)]
    [Display(Name = "اسم المنتج")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 1_000_000, ErrorMessage = "السعر لازم يكون أكبر من صفر")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "السعر")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "الكمية لا يمكن أن تكون سالبة")]
    [Display(Name = "الكمية بالمخزن")]
    public int Stock { get; set; }

    [Display(Name = "متاح للبيع")]
    public bool IsAvailable { get; set; } = true;

    [Required(ErrorMessage = "لازم تختار تصنيف")]
    [Display(Name = "التصنيف")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}
```

## النقاط الجديدة

### `public decimal Price { get; set; }`

**قاعدة ذهبية: استخدم `decimal` للفلوس، مش `double` ولا `float`.**

ليه؟ لأن `double` بيخزّن الأرقام بنظام ثنائي (Binary Floating Point)، وده بيسبب أخطاء تقريب:

```csharp
double a = 0.1 + 0.2;
Console.WriteLine(a);          // 0.30000000000000004
Console.WriteLine(a == 0.3);   // False  ← كارثة في نظام محاسبي

decimal b = 0.1m + 0.2m;
Console.WriteLine(b);          // 0.3
Console.WriteLine(b == 0.3m);  // True   ← صح
```

`decimal` بيخزّن بنظام عشري، فدقته مضمونة للفلوس. أبطأ شوية لكن الدقة أهم.

### `[Column(TypeName = "decimal(18,2)")]`

بتحدد نوع العمود في الداتابيز بالظبط:
- **18** = إجمالي عدد الأرقام (الدقة)
- **2** = عدد الأرقام بعد العلامة العشرية

يعني بتقدر تخزّن لحد `9,999,999,999,999,999.99`.

**ليه محتاجينها؟** لأن EF من غيرها في SQL Server بيستخدم `decimal(18,2)` كافتراضي بس بيطلع تحذير. وفي حالات تانية ممكن يقرّب الأرقام من غير ما تحس. تحديدها صراحةً بيوضّح النية ويمنع المفاجآت.

### `[Range(0.01, 1_000_000)]`

الشرطة السفلية في `1_000_000` هي **Digit Separator** — ميزة في C# لتسهيل القراءة. الكومبايلر بيتجاهلها تماماً. `1_000_000` و `1000000` نفس الشيء بالظبط.

### `[Range(0, int.MaxValue)]`

بتمنع الكميات السالبة. `int.MaxValue` = 2,147,483,647.

### `public int CategoryId { get; set; }`

**المفتاح الأجنبي (Foreign Key).** EF Core بيتعرف عليه بالاصطلاح:

> اسم الخاصية = اسم Navigation Property + `Id`
> `Category` (navigation) + `Id` = `CategoryId` ✓

وده اللي خلّى EF يولّد:

```sql
CONSTRAINT "FK_Products_Categories_CategoryId"
    FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
```

### `public Category? Category { get; set; }`

**Navigation Property** للطرف الواحد. من خلالها بتوصل لبيانات التصنيف مباشرة:

```csharp
product.Category.Name   // "معالجات"
```

**ليه `Category?` بعلامة استفهام؟** لأنها **مش دايماً محمّلة**. لو جبت المنتج كده:

```csharp
var p = await _context.Products.FindAsync(1);
Console.WriteLine(p.Category.Name);   // 💥 NullReferenceException
```

`Category` هتبقى `null` لأنك ما طلبتش من EF يجيبها. لازم تستخدم `Include`:

```csharp
var p = await _context.Products
    .Include(x => x.Category)     // ← دلوقتي هيعمل JOIN ويجيبها
    .FirstOrDefaultAsync(x => x.Id == 1);

Console.WriteLine(p.Category.Name);   // ✓ "معالجات"
```

**ده بالظبط سبب علامة الاستفهام** — الكومبايلر بيفكّرك إنها ممكن تكون فاضية، فتستخدم `?.` بدل `.`:

```csharp
@item.Category?.Name    // آمن — لو null هيطبع فاضي بدل ما يقع
```

---

## جرّب بنفسك — الفصل 6

**١.** في `ProductService.GetAllAsync()`، شيل سطر `.Include(p => p.Category)` وشغّل. افتح `/Products` — إيه اللي اختفى من الجدول؟ وليه الصفحة ما وقعتش؟

<details>
<summary>الإجابة</summary>

عمود التصنيف هيبقى فاضي. الصفحة ما وقعتش لأن الـ View بتستخدم `@item.Category?.Name` بعلامة الاستفهام — لو كانت مكتوبة `@item.Category.Name` من غير `?` كانت هتقع بـ `NullReferenceException`.

ده بالظبط سبب وجود `?` في تعريف الخاصية.
</details>

**٢.** غيّر نوع `Price` من `decimal` لـ `double` واعمل Migration. بعدها ضيف منتج بسعر `0.1` وتاني بـ `0.2`، واعمل مجموعهم في كود. جرّب نفس الحاجة بـ `decimal`. لاحظ الفرق.

**٣.** غيّر `[Range(0.01, 1_000_000)]` لـ `[Range(100, 5000)]` وحاول تضيف منتج بسعر ٥٠. اقرا الرسالة اللي هتظهر — من فين جت؟

---

# الفصل 7: العلاقات (Relations)

## الفكرة الأساسية

في قواعد البيانات العلاقية، الجداول بتترابط بـ **المفاتيح**. الجدول التابع بيحتفظ بمفتاح الجدول الأصلي.

```
Categories                         Products
┌────┬──────────────┐              ┌────┬──────────────────┬────────────┐
│ Id │ Name         │              │ Id │ Name             │ CategoryId │
├────┼──────────────┤              ├────┼──────────────────┼────────────┤
│ 1  │ CPUs         │◄─────────────┤ 1  │ Ryzen 7 7800X3D  │     1      │
│    │              │◄─────────────┤ 2  │ Intel i5-14600K  │     1      │
│ 2  │ GPUs         │◄─────────────┤ 3  │ RTX 4070 Super   │     2      │
└────┴──────────────┘              └────┴──────────────────┴────────────┘
   One                                Many
```

الجدول الشمال (`Categories`) هو الطرف **One**، والجدول اليمين (`Products`) هو الطرف **Many**.

التصنيف رقم ١ ليه منتجين. التصنيف رقم ٢ ليه منتج واحد. دي علاقة **One-to-Many**.

## الأركان الأربعة لأي علاقة في EF Core

| # | الركن | في الكود | في الداتابيز |
|---|---|---|---|
| ١ | Primary Key في الأصلي | `Category.Id` | `PK_Categories` |
| ٢ | Foreign Key في التابع | `Product.CategoryId` | عمود `CategoryId` |
| ٣ | Navigation من التابع للأصلي | `Product.Category` | (مش موجود — مفهوم في الكود بس) |
| ٤ | Navigation من الأصلي للتابع | `Category.Products` | (مش موجود — مفهوم في الكود بس) |

> **نقطة مهمة جداً:** الأركان ٣ و ٤ **مش موجودة في الداتابيز**. مفيش عمود اسمه `Products` في جدول `Categories`. دي مجرد وسيلة راحة في الكود — EF بيترجمها لـ `JOIN` وقت الاستعلام.

## طريقتان لتعريف العلاقة

### الطريقة الأولى: بالاصطلاح (تلقائي)

مجرد ما تسمّي الخاصية `CategoryId`، EF بيفهم العلاقة لوحده. مش محتاج تكتب أي حاجة زيادة.

### الطريقة الثانية: Fluent API (صريح)

في `AppDbContext.OnModelCreating`:

```csharp
modelBuilder.Entity<Product>()
    .HasOne(p => p.Category)
    .WithMany(c => c.Products)
    .HasForeignKey(p => p.CategoryId)
    .OnDelete(DeleteBehavior.Cascade);
```

**اقراها كجملة إنجليزية:**

> الكيان `Product` **له واحد** (`HasOne`) من `Category`، و**اللي له كتير** (`WithMany`) من `Products`، **والمفتاح الأجنبي هو** (`HasForeignKey`) `CategoryId`، **وعند الحذف** (`OnDelete`) اعمل `Cascade`.

**ليه نكتبها لو الاصطلاح كافي؟** لتلات أسباب:

1. **الوضوح** — أي حد يفتح الكود يشوف العلاقة صريحة
2. **التحكم في `OnDelete`** — الاصطلاح بيختار سلوك افتراضي، وإنت عايز تختار بنفسك
3. **الحالات المعقدة** — لو عندك أكتر من علاقة بين نفس الجدولين، الاصطلاح بيتلخبط

## سلوك الحذف (`OnDelete`)

السؤال: **لو حذفت تصنيف عنده ٥ منتجات، يحصل إيه للمنتجات؟**

| السلوك | النتيجة |
|---|---|
| `Cascade` | المنتجات الخمسة بتتحذف معاه (اللي إحنا مختارينه) |
| `Restrict` | الحذف بيترفض ويرمي استثناء طالما فيه منتجات |
| `SetNull` | `CategoryId` بيبقى `null` (محتاج الخاصية تبقى `int?`) |
| `NoAction` | تسيب القرار للداتابيز نفسها |

**إحنا اخترنا `Cascade`.** ده مناسب هنا لأن منتج من غير تصنيف مالوش معنى. بس في أنظمة تانية `Restrict` بيبقى أأمن — تخيل نظام فواتير: مش هينفع تحذف عميل وتضيّع كل فواتيره.

## الفهارس (Indexes)

```csharp
modelBuilder.Entity<Category>()
    .HasIndex(c => c.Name)
    .IsUnique();
```

**الـ Index** زي فهرس الكتاب — بدل ما تقلّب كل الصفحات عشان تلاقي موضوع، بتبص في الفهرس وتروح على الصفحة على طول.

`IsUnique()` بتضيف فايدة تانية: **بتمنع التكرار على مستوى الداتابيز**. حتى لو الكود فيه ثغرة، الداتابيز نفسها هترفض تصنيفين بنفس الاسم.

ولاحظ إن EF عمل index تاني لوحده:

```sql
CREATE INDEX "IX_Products_CategoryId" ON "Products" ("CategoryId");
```

ده تلقائي على كل Foreign Key — عشان الـ `JOIN` يبقى سريع.

## أنواع العلاقات التلاتة

| النوع | المثال | الملاحظة |
|---|---|---|
| **One-to-Many** ← اللي عندنا | تصنيف واحد ← منتجات كتير | الـ FK في الجدول التابع |
| **One-to-One** | مستخدم واحد ← ملف شخصي واحد | الـ FK بيبقى PK كمان |
| **Many-to-Many** | منتج ← وسوم كتير، ووسم ← منتجات كتير | بيحتاج جدول وسيط |

في EF Core 5 وما بعده، الـ Many-to-Many بقى تلقائي:

```csharp
public class Product { public ICollection<Tag> Tags { get; set; } }
public class Tag     { public ICollection<Product> Products { get; set; } }
// EF بيعمل جدول ProductTag لوحده
```

## تحميل البيانات المرتبطة: تلات طرق

### 1. Eager Loading — `Include` (اللي بنستخدمه)

```csharp
var products = await _context.Products.Include(p => p.Category).ToListAsync();
```
استعلام واحد فيه `JOIN`. **الأفضل في معظم الحالات.**

### 2. Explicit Loading

```csharp
var product = await _context.Products.FindAsync(1);
await _context.Entry(product).Reference(p => p.Category).LoadAsync();
```
تحميل يدوي وقت الحاجة. مفيد لما تكون مش متأكد إنك هتحتاج البيانات المرتبطة.

### 3. Lazy Loading — تحميل كسول

يحتاج باكدج إضافي، وبيحمّل البيانات أول ما تلمس الخاصية. **تجنّبه** — بيسبب مشكلة **N+1**:

```csharp
foreach (var p in products)      // استعلام واحد يجيب ١٠٠ منتج
    Console.WriteLine(p.Category.Name);   // ١٠٠ استعلام إضافي! 💥
```

١٠١ استعلام بدل واحد. ده بيقتل الأداء في الجداول الكبيرة.

---

## جرّب بنفسك — الفصل 7

**١.** روح على `/Categories/Delete/1` واحذف تصنيف "معالجات". بعدها افتح `/Products`. المنتجات اللي كانت تحته اختفت — ده الـ Cascade Delete شغّال.

**٢.** غيّر `DeleteBehavior.Cascade` لـ `DeleteBehavior.Restrict`، اعمل Migration جديدة، وجرّب تحذف تاني. إيه اللي بيحصل؟

<details>
<summary>الإجابة</summary>

هتاخد `DbUpdateException` بسبب انتهاك قيد المفتاح الأجنبي. الداتابيز بترفض تحذف صف لسه فيه صفوف تابعة ليه.

في نظام حقيقي، تمسك الاستثناء ده وتعرض رسالة مفهومة:
```csharp
try { await _context.SaveChangesAsync(); }
catch (DbUpdateException)
{
    TempData["Error"] = "مينفعش تحذف تصنيف فيه منتجات. احذف المنتجات الأول.";
    return RedirectToAction(nameof(Index));
}
```
</details>

**٣.** جرّب تضيف تصنيفين بنفس الاسم بالظبط. إيه اللي بيحصل ومين اللي منع ده؟

<details>
<summary>الإجابة</summary>

الداتابيز هترفض بسبب `IX_Categories_Name` اللي معمول `IsUnique()`. لاحظ إن الرفض جه من **الداتابيز** مش من كود التحقق — عشان كده الرسالة هتبقى استثناء وحش.

**تمرين إضافي:** ضيف تحقق في الـ Controller قبل الحفظ عشان الرسالة تبقى مفهومة:
```csharp
if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
{
    ModelState.AddModelError("Name", "الاسم ده مستخدم قبل كده");
    return View(category);
}
```
</details>

**٤.** أضف خاصية `Description` للمنتج، ثم استخدم `Select` عشان تجيب أعمدة محددة بس:
```csharp
var slim = await _context.Products
    .Select(p => new { p.Name, p.Price, CategoryName = p.Category.Name })
    .ToListAsync();
```
ليه ده أسرع من `Include`؟

<details>
<summary>الإجابة</summary>

لأنه بيجيب **الأعمدة اللي محتاجها بس** من الداتابيز، مش كل أعمدة الجدولين. في جدول فيه ٢٠ عمود وإنت محتاج ٣، الفرق في حجم البيانات المنقولة كبير جداً.

ده الأساس اللي بيتبنى عليه مفهوم **DTO / ViewModel** — وهو خطوتك الجاية في التعلم.
</details>
# الفصل 8: `Data/AppDbContext.cs`

الـ DbContext هو **الجسر بين الكود وقاعدة البيانات**. مسؤول عن تلات حاجات:

1. تعريف الجداول (من خلال `DbSet`)
2. تتبّع التغييرات على الكائنات (Change Tracking)
3. ترجمة استعلامات LINQ لـ SQL

## الكود الكامل

```csharp
using Microsoft.EntityFrameworkCore;
using ShopApp.Models;

namespace ShopApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "معالجات", Description = "CPUs", CreatedAt = new DateTime(2025, 1, 1) },
            new Category { Id = 2, Name = "كروت شاشة", Description = "GPUs", CreatedAt = new DateTime(2025, 1, 1) }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Ryzen 7 7800X3D", Price = 1750m, Stock = 12, IsAvailable = true, CategoryId = 1 },
            new Product { Id = 2, Name = "Intel Core i5-14600K", Price = 1250m, Stock = 8, IsAvailable = true, CategoryId = 1 },
            new Product { Id = 3, Name = "RTX 4070 Super", Price = 2600m, Stock = 5, IsAvailable = true, CategoryId = 2 }
        );
    }
}
```

## سطر بسطر

### `public class AppDbContext : DbContext`

بنرث من `DbContext` — الكلاس الأساسي في EF Core اللي فيه كل منطق التتبّع والترجمة.

### الـ Constructor

```csharp
public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
{
}
```

الـ Constructor ده **فاضي عن قصد**. كل شغله إنه يستقبل الإعدادات (`options`) ويمرّرها للكلاس الأب بـ `: base(options)`.

**فين الإعدادات دي بتتحدد؟** في `Program.cs`:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));
```

**ليه الطريقة دي أحسن من كتابة الإعدادات جوّه الـ Context؟**

الطريقة القديمة كانت:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder options)
{
    options.UseSqlite("Data Source=shopapp.db");  // ❌ مربوط بالكود
}
```

المشكلة: الـ connection string اتحول لجزء من الكود. مش هتقدر تغيّره بين بيئة التطوير والإنتاج، ومش هتقدر تستبدله بداتابيز وهمية (In-Memory) وقت الاختبار.

بالطريقة اللي إحنا مستخدمينها، تقدر تعمل اختبار كده:

```csharp
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase("TestDb")     // داتابيز في الذاكرة للاختبار
    .Options;

using var context = new AppDbContext(options);
// اختبر براحتك من غير ما تلمس الداتابيز الحقيقية
```

### `public DbSet<Category> Categories => Set<Category>();`

كل `DbSet<T>` = **جدول في الداتابيز**. اسم الخاصية بيبقى اسم الجدول:

| الخاصية | الجدول |
|---|---|
| `Categories` | `Categories` |
| `Products` | `Products` |

**إيه الفرق بين الطريقتين دول؟**

```csharp
public DbSet<Category> Categories { get; set; }      // الطريقة التقليدية
public DbSet<Category> Categories => Set<Category>(); // اللي إحنا مستخدمينه
```

التانية بتستخدم **Expression-bodied property** (`=>`) وبتنادي `Set<T>()` من الكلاس الأب. الفايدة: مفيش تحذير من الـ Nullable، لأن مفيش خاصية ممكن تفضل `null` — القيمة بتتحسب في كل مرة.

### `OnModelCreating`

الميثود دي بتتنفّذ **مرة واحدة** لما EF يبني نموذج الداتابيز في ذاكرته. هنا بتظبط أي حاجة مش مغطاة بالاصطلاح.

`base.OnModelCreating(modelBuilder)` في الأول — عشان تسيب EF يطبّق قواعده الافتراضية قبل ما تضيف قواعدك.

### `HasData` — البيانات المبدئية

```csharp
modelBuilder.Entity<Category>().HasData(
    new Category { Id = 1, Name = "معالجات", ... }
);
```

دي **Seed Data** — بيانات بتتزرع مع الـ Migration. لاحظ تلات شروط مهمة:

**١. لازم تحدد `Id` صراحة.** عادةً `Id` بيتولّد تلقائياً، لكن في الـ Seed لازم تحدده لأن EF بيستخدمه عشان يقارن بين Migration والتانية ويعرف إيه اللي اتغير.

**٢. لازم القيم تكون ثابتة (Deterministic).** ده غلط:

```csharp
CreatedAt = DateTime.Now    // ❌ بتتغير في كل مرة
```

لأن EF بيقارن القيم بين الـ Migrations. لو القيمة بتتغير، EF هيفتكر إن فيه تعديل ويعمل Migration جديدة كل مرة. عشان كده كتبناها ثابتة:

```csharp
CreatedAt = new DateTime(2025, 1, 1)   // ✓ ثابتة
```

**٣. البيانات بتتزرع مع الـ Migration مش وقت التشغيل.** شوف SQL اللي اتولّد في ملف الـ Migration:

```sql
INSERT INTO "Categories" ("Id", "CreatedAt", "Description", "Name")
VALUES (1, '2025-01-01 00:00:00', 'CPUs', 'معالجات');
```

## تتبّع التغييرات (Change Tracking)

ده أذكى جزء في EF Core. جرّب تفهم الكود ده:

```csharp
var product = await _context.Products.FirstAsync(p => p.Id == 1);
product.Price = 1900;              // مجرد تعديل على كائن عادي في الذاكرة
await _context.SaveChangesAsync(); // EF عرف لوحده إن السعر اتغير!
```

**إزاي عرف؟** لما جبت المنتج، EF خزّن **لقطة (Snapshot)** من قيمه الأصلية. وقت `SaveChanges` بيقارن الحالي بالأصلي، ويولّد `UPDATE` للأعمدة المتغيّرة بس:

```sql
UPDATE "Products" SET "Price" = 1900 WHERE "Id" = 1;
-- لاحظ: Name و Stock مش في الأمر لأنهم ما اتغيروش
```

## `AsNoTracking()` — متى ولماذا

```csharp
await _context.Products.AsNoTracking().ToListAsync();
```

بتقول لـ EF: "مش هعدّل على البيانات دي، متتعبش نفسك بالتتبّع".

**الفايدة:** أسرع وبتاكل ذاكرة أقل — ممكن الفرق يوصل ٣٠٪ في القوائم الكبيرة.

**القاعدة:**

| الحالة | استخدم |
|---|---|
| عرض بيانات فقط (Index, Details) | `AsNoTracking()` ✓ |
| هتعدّل وتحفظ (Edit) | تتبّع عادي (بدون `AsNoTracking`) |

شوف في مشروعنا الفرق:

```csharp
// GetAllAsync — للعرض بس
public async Task<IEnumerable<Product>> GetAllAsync() =>
    await _context.Products.Include(p => p.Category)
        .AsNoTracking()      // ← عرض فقط
        .ToListAsync();

// GetByIdAsync — ممكن نعدّل عليه بعدها
public async Task<Product?> GetByIdAsync(int id) =>
    await _context.Products.Include(p => p.Category)
        .FirstOrDefaultAsync(p => p.Id == id);   // ← بدون AsNoTracking
```

---

## جرّب بنفسك — الفصل 8

**١.** ضيف تصنيف جديد في `HasData`:
```csharp
new Category { Id = 3, Name = "رامات", Description = "RAM", CreatedAt = new DateTime(2025, 1, 1) }
```
اعمل Migration واقرا الملف المتولّد — هتلاقي `InsertData` واحدة بس للتصنيف الجديد، مش الكل. EF قارن وعرف الفرق لوحده.

**٢.** غيّر `CreatedAt = new DateTime(2025, 1, 1)` لـ `DateTime.Now` واعمل Migration مرتين ورا بعض. هتلاحظ إيه؟

<details>
<summary>الإجابة</summary>

كل مرة هتلاقي Migration جديدة فيها `UpdateData` للتاريخ — حتى لو ما غيّرتش أي حاجة. لأن `DateTime.Now` بترجع قيمة مختلفة في كل تنفيذ، فEF بيفتكر إن فيه تعديل.

الدرس: **بيانات الـ Seed لازم تكون ثابتة دايماً.**
</details>

**٣.** أضف السطر ده مؤقتاً في `ProductService.GetAllAsync` قبل `ToListAsync`:
```csharp
.Where(p => p.Price > 1500)
```
شغّل وشوف الـ SQL في الترمينال. هتلاقي `WHERE "p"."Price" > 1500` — يعني الفلترة اتنفّذت في **الداتابيز** مش في ذاكرة التطبيق. دي قوة LINQ to Entities.

**٤.** دلوقتي جرّب الفرق ده:
```csharp
// أ - الفلترة في الداتابيز
var a = await _context.Products.Where(p => p.Price > 1500).ToListAsync();

// ب - الفلترة في الذاكرة
var b = (await _context.Products.ToListAsync()).Where(p => p.Price > 1500);
```
الاتنين بيرجّعوا نفس النتيجة. ليه (أ) أفضل بكتير؟

<details>
<summary>الإجابة</summary>

في (أ): الداتابيز بترجّع المنتجات المطابقة بس.

في (ب): الداتابيز بترجّع **كل** المنتجات للتطبيق، وبعدين التطبيق بيفلتر. لو عندك مليون منتج، هتنقل مليون صف على الشبكة عشان تستخدم ١٠٠ منهم.

**السبب التقني:** `ToListAsync()` بينفّذ الاستعلام فوراً. أي `Where` بعد كده بيشتغل على `List` في الذاكرة (LINQ to Objects) مش على الداتابيز (LINQ to Entities).

**القاعدة: حط كل الفلترة والترتيب قبل `ToListAsync()`.**
</details>

---

# الفصل 9: `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=shopapp.db",
    "_SqlServerExample": "Server=(localdb)\\MSSQLLocalDB;Database=ShopAppDb;Trusted_Connection=True;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

## `ConnectionStrings`

بتتقرا في `Program.cs` كده:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
```

الميثود `GetConnectionString("X")` هي اختصار لـ `Configuration["ConnectionStrings:X"]`.

## `Logging.LogLevel`

بتتحكم في كمية الرسائل اللي بتظهر في الترمينال. المستويات من الأقل للأعلى:

```
Trace  <  Debug  <  Information  <  Warning  <  Error  <  Critical
```

لما تحدد مستوى، بتشوف المستوى ده وكل اللي فوقه.

**السطر ده تحديداً هو سبب سيل رسائل SQL اللي شفتها:**

```json
"Microsoft.EntityFrameworkCore.Database.Command": "Information"
```

حطيته عن قصد عشان تشوف الـ SQL اللي EF بيولّده — ده مفيد جداً وإنت بتتعلم. لما تزهق، غيّره لـ `"Warning"`.

## ترتيب أولوية الإعدادات

ASP.NET بيقرا الإعدادات من مصادر متعددة بالترتيب ده، **والمتأخر بيطغى على المتقدم**:

| الأولوية | المصدر | ملاحظة |
|---|---|---|
| ١ (الأدنى) | `appsettings.json` | الأساس |
| ٢ | `appsettings.{Environment}.json` | حسب البيئة — Development / Production |
| ٣ | User Secrets | أسرار محلية، بيئة التطوير فقط |
| ٤ | متغيرات البيئة (Environment Variables) | الشائع في السيرفرات |
| ٥ (الأعلى) | معطيات سطر الأوامر | بتطغى على الكل |

**مثال عملي:** لو `appsettings.json` فيه `"Default": "Information"` و `appsettings.Development.json` فيه `"Default": "Debug"`، فوقت التطوير هيستخدم `Debug`.

## ⚠️ تحذير أمني مهم

**متحطش أي كلمات سر أو مفاتيح API في `appsettings.json`** — الملف ده بيترفع على Git وأي حد يشوف الريبو هيشوفهم.

**للتطوير المحلي** استخدم User Secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Password=SuperSecret123"
```

بيتخزّن بره فولدر المشروع تماماً، فمستحيل يترفع على Git بالغلط.

**للإنتاج** استخدم متغيرات البيئة أو خدمة إدارة أسرار (Azure Key Vault, AWS Secrets Manager).

---

## جرّب بنفسك — الفصل 9

**١.** غيّر `"Microsoft.EntityFrameworkCore.Database.Command"` لـ `"Warning"` وشغّل. الترمينال هيبقى نضيف. رجّعه لما تحتاج تشوف SQL تاني.

**٢.** أضف قسم إعدادات مخصص:
```json
"AppSettings": {
  "SiteName": "متجر PC ELITES",
  "ItemsPerPage": 10
}
```
واقراه في أي كنترولر:
```csharp
public class ProductsController : Controller
{
    private readonly IConfiguration _config;
    public ProductsController(IConfiguration config) => _config = config;

    public IActionResult Index()
    {
        ViewBag.SiteName = _config["AppSettings:SiteName"];
        var perPage = _config.GetValue<int>("AppSettings:ItemsPerPage");
        // ...
    }
}
```
لاحظ إن `IConfiguration` وصلت للكنترولر بنفس طريقة الـ DI.

**٣.** افتح `appsettings.Development.json` وضيف فيه:
```json
"AppSettings": { "SiteName": "متجر — بيئة تطوير" }
```
شغّل. أنهي قيمة ظهرت وليه؟

---

# الفصل 10: Dependency Injection — الفصل الأهم

## المشكلة أولاً

تخيل كنترولر بيعمل احتياجاته بنفسه:

```csharp
public class ProductsController : Controller
{
    private readonly ProductService _service;

    public ProductsController()
    {
        var context = new AppDbContext(/* إعدادات من فين؟ */);
        _service = new ProductService(context);      // ❌
    }
}
```

**أربع مشاكل قاتلة:**

| المشكلة | التفسير |
|---|---|
| **ارتباط وثيق** | الكنترولر مربوط بـ `ProductService` بالاسم. تغييره = تعديل الكنترولر |
| **استحالة الاختبار** | مينفعش تستبدل الـ Service بنسخة وهمية في الاختبار |
| **تكرار الإعدادات** | كل كنترولر لازم يعرف الـ connection string |
| **مفيش إدارة للعمر** | مين هيقفل الـ `DbContext`؟ ومتى؟ |

## الحل: اقلب المسؤولية

**بدل ما الكنترولر يعمل احتياجاته، حد تاني يعملهاله ويديهاله.**

ده اسمه **Inversion of Control (IoC)**، وتطبيقه العملي اسمه **Dependency Injection**.

```csharp
public class ProductsController : Controller
{
    private readonly IProductService _productService;

    // بس أقول "أنا محتاج IProductService" — و ASP.NET بيجيبهالي
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
}
```

**لاحظ حاجتين:**

1. الكنترولر بيطلب `IProductService` (**الواجهة**) مش `ProductService` (**الكلاس**). فهو مايعرفش أصلاً مين هينفّذ.
2. مفيش `new` خالص. الكنترولر بس بيستقبل.

## التسجيل في `Program.cs`

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

اقراها: **"لما حد يطلب `IProductService`، اديله نسخة من `ProductService`"**.

## الأعمار التلاتة — بالتفصيل

### `AddSingleton` — نسخة واحدة للأبد

```csharp
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
```

```
App Start ══════════ [ ONE INSTANCE ] ══════════► App Stop
```

نسخة واحدة بتتعمل أول مرة وبتفضل لآخر التطبيق. كل المستخدمين بيشاركوا نفس النسخة.

**متى؟** الكاش، قراءة الإعدادات، أي حاجة ثابتة وثقيلة الإنشاء.

**⚠️ خطر:** لازم تكون **Thread-Safe** لأن آلاف الطلبات ممكن يستخدموها في نفس اللحظة. ومتحطش فيها بيانات خاصة بمستخدم معيّن.

### `AddScoped` — نسخة لكل طلب

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

```
Request #1   ├── [ instance A ] ──┤
Request #2   ├── [ instance B ] ──┤
Request #3   ├── [ instance C ] ──┤
```

كل طلب ليه نسخته الخاصة، وبتموت بموت الطلب.

نسخة جديدة مع كل طلب HTTP، وبتتخلّص لما الطلب يخلص.

**متى؟** `DbContext` والخدمات اللي بتستخدمه. **ده الاختيار الافتراضي في ٩٠٪ من الحالات.**

**نقطة مهمة:** لو طلبت نفس الخدمة مرتين في نفس الطلب، هتاخد **نفس النسخة**. عشان كده في `ProductsController`:

```csharp
public ProductsController(IProductService productService, AppDbContext context)
```

الـ `context` اللي في الكنترولر هو **نفس** الـ `context` اللي جوّه `ProductService`. نفس النسخة، نفس التتبّع، نفس المعاملة (Transaction).

### `AddTransient` — نسخة كل مرة

```csharp
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
```

```
Inside ONE request:
   1st resolve  →  [ instance A ]
   2nd resolve  →  [ instance B ]
   3rd resolve  →  [ instance C ]
```

جوّه الطلب الواحد نفسه، كل مرة بتطلبها بتاخد نسخة جديدة.

**متى؟** خدمات خفيفة ومالهاش حالة (Stateless).

## جدول القرار السريع

| الحالة | العمر |
|---|---|
| `DbContext` أو أي حاجة بتستخدمه | `Scoped` |
| كاش أو إعدادات ثابتة | `Singleton` |
| خدمة خفيفة بلا حالة | `Transient` |
| **مش عارف؟** | `Scoped` |

## ⚠️ الخطأ القاتل: Captive Dependency

**متسجّلش خدمة عمرها قصير جوّه خدمة عمرها طويل.**

```csharp
builder.Services.AddSingleton<IProductService, ProductService>();  // ❌ كارثة
// ProductService بياخد AppDbContext (Scoped)
```

اللي هيحصل: الـ `DbContext` هيتأسر جوّه الـ Singleton ويفضل عايش للأبد. النتيجة:

- تسريب ذاكرة (كل الكيانات المتتبّعة تفضل في الذاكرة)
- بيانات قديمة (الـ Context مش هيشوف تعديلات الطلبات التانية)
- انهيارات عشوائية (`DbContext` مش Thread-Safe)

**القاعدة:** الخدمة ممكن تعتمد على خدمة عمرها **مساوي أو أطول**، مش أقصر.

| الخدمة | ممكن تعتمد على |
|---|---|
| `Singleton` | `Singleton` فقط |
| `Scoped` | `Scoped` و `Singleton` |
| `Transient` | الكل |

الخبر الحلو: ASP.NET Core بيكتشف الخطأ ده وقت التشغيل في بيئة التطوير ويرمي استثناء واضح.

---

## جرّب بنفسك — الفصل 10

**١.** جرّب الخطأ عن قصد — غيّر التسجيل لـ:
```csharp
builder.Services.AddSingleton<IProductService, ProductService>();
```
شغّل واقرا رسالة الخطأ. هتلاقيها بتقول:
```
Cannot consume scoped service 'AppDbContext' from singleton 'IProductService'
```
رسالة واضحة جداً. رجّعها لـ `AddScoped`.

**٢.** أثبت إن `Scoped` فعلاً نسخة واحدة لكل طلب. ضيف في `ProductService`:
```csharp
public ProductService(AppDbContext context)
{
    _context = context;
    Console.WriteLine($"ProductService instance: {GetHashCode()}");
}
```
وفي الكنترولر:
```csharp
public ProductsController(IProductService productService, AppDbContext context)
{
    _productService = productService;
    _context = context;
    Console.WriteLine($"Controller got context: {context.GetHashCode()}");
}
```
افتح `/Products` مرتين. هتلاحظ إن الأرقام بتتغير بين الطلبين، لكن جوّه الطلب الواحد الأرقام ثابتة.

**٣.** غيّرها لـ `AddTransient` وكرّر التجربة. إيه اللي اتغير؟

**٤.** أضف خدمة جديدة بنفسك:
```csharp
// Services/ICategoryService.cs
public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllWithProductCountAsync();
}
```
نفّذها، سجّلها في `Program.cs`، واستخدمها في `CategoriesController` بدل الـ `DbContext` المباشر.

---

# الفصل 11: طبقة `Services`

## ليه طبقة زيادة أصلاً؟

الـ `CategoriesController` بيستخدم `AppDbContext` مباشرة، والـ `ProductsController` بيستخدم `IProductService`. الاتنين شغالين. فليه التعقيد الزيادة؟

**تلات أسباب:**

### 1. عدم تكرار الكود

تخيل إنك محتاج "المنتجات المتاحة والمخزون فيها أكتر من صفر" في:
- صفحة المنتجات
- الـ API
- تقرير شهري
- صفحة البحث

من غير Service، هتكرر نفس الاستعلام ٤ مرات. مع Service، بتكتبه مرة:

```csharp
public async Task<IEnumerable<Product>> GetAvailableAsync() =>
    await _context.Products
        .Where(p => p.IsAvailable && p.Stock > 0)
        .Include(p => p.Category)
        .AsNoTracking()
        .ToListAsync();
```

### 2. قابلية الاختبار

الكنترولر بيعتمد على `IProductService` (واجهة). في الاختبار تقدر تستبدلها بنسخة وهمية:

```csharp
var fake = new Mock<IProductService>();
fake.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Product>
{
    new Product { Id = 1, Name = "منتج تجريبي", Price = 100 }
});

var controller = new ProductsController(fake.Object, null!);
var result = await controller.Index();

// اختبرت الكنترولر من غير داتابيز خالص
```

لو الكنترولر كان بياخد `AppDbContext` مباشرة، ده كان مستحيل.

### 3. تركيز منطق العمل في مكان واحد

قواعد زي "مينفعش تحذف منتج لو عليه طلبات معلّقة" مكانها الـ Service، مش الكنترولر. الكنترولر شغله استقبال الطلب والرد بس.

## `IProductService.cs` — العقد

```csharp
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
```

**الواجهة بتقول "إيه" من غير "إزاي".** أي كلاس ينفّذها بيبقى صالح للاستخدام.

**ليه `Task<...>` في كل حاجة؟** لأن كل العمليات **غير متزامنة (async)**.

## لحظة — إيه هو async أصلاً؟

```csharp
// متزامن — الخيط بيقف مستني
var products = _context.Products.ToList();

// غير متزامن — الخيط بيسيب مكانه لطلبات تانية
var products = await _context.Products.ToListAsync();
```

**الفرق العملي:**

استعلام الداتابيز بياخد مثلاً ٥٠ ملي ثانية. في الطريقة المتزامنة، الخيط (Thread) بيفضل **واقف مستني** الـ ٥٠ ملي ثانية دول من غير ما يعمل حاجة.

في الطريقة غير المتزامنة، الخيط بيرجع لمجمّع الخيوط (Thread Pool) ويخدم طلبات تانية، ولما الداتابيز ترد بيرجع يكمّل.

**النتيجة:** سيرفر بـ ١٠٠ خيط يقدر يخدم آلاف الطلبات المتزامنة بدل ١٠٠ بس.

**القاعدة الذهبية:** `async` **طول السلسلة**. لو الـ Service بترجّع `Task`، الكنترولر لازم يكون `async` كمان:

```csharp
public async Task<IActionResult> Index()
{
    var products = await _productService.GetAllAsync();
    return View(products);
}
```

**⚠️ متعملش `.Result` أو `.Wait()` أبداً:**

```csharp
var products = _productService.GetAllAsync().Result;   // ❌ ممكن يعلّق التطبيق (Deadlock)
```

## `ProductService.cs` — التنفيذ

```csharp
public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;    // ← DI تاني! الـ Service كمان بتستقبل
    }
```

لاحظ إن الـ DI مش بس للكنترولرات — أي كلاس مسجّل في الـ Container بيقدر يستقبل احتياجاته بنفس الطريقة.

```csharp
    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _context.Products
            .Include(p => p.Category)     // JOIN مع التصنيفات
            .OrderBy(p => p.Name)         // ترتيب في الداتابيز
            .AsNoTracking()               // عرض فقط، بلا تتبّع
            .ToListAsync();               // تنفيذ الاستعلام
```

**ترتيب السطور مقصود:** كل السطور اللي قبل `ToListAsync()` بتبني الاستعلام بس من غير ما تنفّذه. `ToListAsync()` هي اللي بتبعت SQL فعلاً.

ده اسمه **Deferred Execution** — التنفيذ المؤجل.

```csharp
    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return;      // مش موجود؟ اخرج بهدوء

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
```

**ليه `FindAsync` مش `FirstOrDefaultAsync`؟**

`FindAsync` بتدوّر في **الذاكرة المحلية الأول**. لو الكيان اتحمّل قبل كده في نفس الطلب، بترجّعه من غير ما تروح الداتابيز خالص. أسرع لما تكون بتدوّر بالـ Primary Key.

---

## جرّب بنفسك — الفصل 11

**١.** ضيف ميثود جديدة للواجهة:
```csharp
Task<IEnumerable<Product>> SearchAsync(string term);
```
نفّذها:
```csharp
public async Task<IEnumerable<Product>> SearchAsync(string term) =>
    await _context.Products
        .Include(p => p.Category)
        .Where(p => p.Name.Contains(term))
        .AsNoTracking()
        .ToListAsync();
```
واستخدمها في action جديد اسمه `Search`. لاحظ إن الكومبايلر أجبرك تنفّذ الميثود في `ProductService` أول ما ضفتها للواجهة — دي قوة العقود.

**٢.** غيّر `GetAllAsync` تستخدم `FirstOrDefault` بدل `Include` وشوف الـ SQL في الترمينال. قارن عدد الاستعلامات.

**٣.** حوّل `CategoriesController` كله يستخدم `ICategoryService` بدل `AppDbContext`. ده تمرين متكامل هيثبّت كل اللي فات.

---

# الفصل 12: `Program.cs` — نقطة التجميع

الملف ده هو **قلب التطبيق**. مقسوم لأربع مراحل بترتيب صارم.

## المرحلة صفر: البداية

```csharp
var builder = WebApplication.CreateBuilder(args);
```

السطر ده بيعمل حاجات كتير خلف الكواليس:

- يقرا `appsettings.json` و `appsettings.{Environment}.json`
- يقرا متغيرات البيئة ومعطيات سطر الأوامر
- يجهّز نظام التسجيل (Logging)
- يجهّز سيرفر Kestrel
- يعمل الـ **DI Container** الفاضي

## المرحلة 1: تسجيل الخدمات

```csharp
builder.Services.AddControllersWithViews();
```

بتسجّل كل ما يلزم لـ MVC: اكتشاف الكنترولرات، محرّك Razor، Model Binding، التحقق، Tag Helpers.

> فيه أخوات ليها: `AddControllers()` لـ Web API بلا Views، و `AddRazorPages()` لنمط Razor Pages.

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
```

**المعامل `??`** اسمه **Null-Coalescing Operator**. معناه: "لو اللي على الشمال `null`، نفّذ اللي على اليمين".

فبدل ما التطبيق يقع بعدين برسالة غامضة، بيقع **دلوقتي** برسالة واضحة. المبدأ ده اسمه **Fail Fast** — اكتشف الخطأ بأسرع ما يمكن.

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));
```

الميثود `AddDbContext` بتعمل تلات حاجات:
1. تسجّل `AppDbContext` كـ **Scoped** تلقائياً
2. تسجّل `DbContextOptions<AppDbContext>` بالإعدادات المحددة
3. تظبط تجميع الاتصالات (Connection Pooling)

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

خدمتنا المخصصة.

```csharp
var app = builder.Build();
```

**نقطة اللا رجعة.** بعد السطر ده، الـ Container اتقفل ومينفعش تسجّل خدمات جديدة.

## المرحلة 2: الـ Middleware Pipeline

```csharp
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}
```

**ليه الاختلاف بين البيئتين؟**

في **التطوير**: `UseDeveloperExceptionPage()` بتعرض تفاصيل الخطأ كاملة — الـ Stack Trace، السطر اللي وقع، القيم. ممتاز للتشخيص.

في **الإنتاج**: نفس الصفحة دي **ثغرة أمنية خطيرة** — بتكشف بنية مشروعك ومسارات ملفاتك وأحياناً بيانات حساسة. عشان كده بنعرض صفحة خطأ عامة بدلها.

`UseHsts()` بتبعت هيدر بيقول للمتصفح: "المرات الجاية استخدم HTTPS من الأول ومتحاولش HTTP خالص".

```csharp
app.UseHttpsRedirection();   // http → https
app.UseStaticFiles();        // ملفات wwwroot
app.UseRouting();            // تحليل الـ URL
app.UseAuthorization();      // فحص الصلاحيات
```

**الترتيب ده مش عشوائي:**

- `UseStaticFiles` قبل `UseRouting` عشان طلبات الـ CSS والصور تخرج بدري من غير ما تدخل نظام الراوتنج (أداء أفضل)
- `UseAuthorization` **لازم** بعد `UseRouting` عشان يعرف المستخدم رايح فين

## المرحلة 3: الراوتنج

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");
```

**تفكيك القالب:**

| الجزء | المعنى |
|---|---|
| `{controller=Products}` | متغير اسمه controller، افتراضيه `Products` |
| `{action=Index}` | متغير اسمه action، افتراضيه `Index` |
| `{id?}` | متغير اسمه id، **اختياري** (علامة `?`) |

**أمثلة تطبيقية:**

| الـ URL | controller | action | id |
|---|---|---|---|
| `/` | Products | Index | — |
| `/Categories` | Categories | Index | — |
| `/Products/Create` | Products | Create | — |
| `/Products/Edit/5` | Products | Edit | 5 |

## المرحلة 4: الـ Migration التلقائي

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
```

**ليه محتاجين `CreateScope()`؟**

`AppDbContext` مسجّل كـ **Scoped** — يعني بيعيش داخل نطاق طلب HTTP. لكن إحنا هنا **قبل** ما أي طلب يوصل. فلازم نعمل نطاق يدوي.

كلمة `using` بتضمن إن النطاق يتقفل والـ Context يتخلّص منه بعد ما نخلص.

`db.Database.Migrate()` بتطبّق أي Migrations لسه ما اتطبقتش. **دي اللي شفت أثرها في الترمينال أول ما شغّلت المشروع.**

> **⚠️ ملاحظة للإنتاج:** الطريقة دي مريحة للتعلم، لكن في السيرفرات الحقيقية بيُفضّل تطبيق الـ Migrations كخطوة منفصلة في خط النشر (CI/CD)، مش وقت تشغيل التطبيق. السبب: لو عندك أكتر من نسخة من التطبيق شغالة، هيحاولوا يطبّقوا الـ Migration في نفس الوقت.

```csharp
app.Run();
```

بيشغّل السيرفر ويستنى الطلبات. **الكود ده بيقف هنا** لحد ما توقف التطبيق بـ `Ctrl+C`.

---

## جرّب بنفسك — الفصل 12

**١.** غيّر الراوت الافتراضي لـ `Categories`:
```csharp
pattern: "{controller=Categories}/{action=Index}/{id?}"
```
شغّل وافتح `localhost:5124`. هتروح فين؟

**٢.** أضف راوت مخصص **قبل** الافتراضي:
```csharp
app.MapControllerRoute(
    name: "productDetails",
    pattern: "منتج/{id}",
    defaults: new { controller = "Products", action = "Details" });
```
جرّب `localhost:5124/منتج/1`. الراوتنج بيدعم العربي عادي.

**٣.** شيل `app.UseStaticFiles();` وشغّل. الصفحة هتظهر من غير تنسيق خالص — لأن `bootstrap.rtl.min.css` بقى مش قابل للوصول.

**٤.** جرّب تسجّل خدمة **بعد** `builder.Build()`:
```csharp
var app = builder.Build();
builder.Services.AddScoped<IProductService, ProductService>();   // ❌
```
شغّل واقرا رسالة الخطأ. ليه ممنوع؟

<details>
<summary>الإجابة</summary>

```
InvalidOperationException: Cannot modify ServiceCollection after
the application has been built.
```

لأن `Build()` بتحوّل قايمة الخدمات لـ Container محسّن وجاهز. لو سمح بالتعديل بعدها، الخدمات اللي اتعملت بالفعل مش هتشوف التغيير — وده هيسبب سلوك غير متوقع صعب تشخيصه. المنع صراحةً أأمن.
</details>

**٥.** جرّب تشيل بلوك الـ Migration التلقائي، امسح `shopapp.db`، وشغّل. إيه اللي هيحصل؟ وإزاي تحل من غير ما ترجّع الكود؟

<details>
<summary>الإجابة</summary>

هتاخد خطأ `no such table: Products` لأن الداتابيز فاضية.

الحل من غير الكود: طبّق الـ Migration يدوي:
```bash
dotnet ef database update
```

ده بيوضّح إن البلوك ده مجرد وسيلة راحة — الـ Migration ممكن يتطبق يدوي برضه.
</details>
# الفصل 13: `CategoriesController.cs`

## هيكل الكنترولر

```csharp
public class CategoriesController : Controller
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }
    // ... الـ Actions
}
```

**`: Controller`** — بنرث من الكلاس الأساسي اللي بيدينا ميثودات جاهزة زي `View()` و `RedirectToAction()` و `NotFound()`، وخصائص زي `ModelState` و `TempData` و `HttpContext`.

**`private readonly`** — كلمة `readonly` بتمنع تغيير المتغير بعد الـ Constructor. ضمان إضافي إن الـ Context مش هيتبدّل بالغلط في نص الكود.

## نمط الـ CRUD السباعي

كل كنترورلر CRUD في MVC بيتكون من **سبع actions** بالنمط ده:

| # | Action | HTTP | الوظيفة |
|---|---|---|---|
| 1 | `Index` | GET | عرض القائمة |
| 2 | `Details/5` | GET | عرض عنصر واحد |
| 3 | `Create` | GET | عرض فورم فاضي |
| 4 | `Create` | POST | استقبال البيانات وحفظها |
| 5 | `Edit/5` | GET | عرض فورم متعبّي |
| 6 | `Edit/5` | POST | استقبال التعديلات وحفظها |
| 7 | `Delete/5` | GET + POST | تأكيد ثم حذف |

**ليه كل عملية لها GET و POST؟**

- **GET** = "وريني الفورم" — عملية آمنة، مش بتغيّر حاجة
- **POST** = "خد البيانات دي واحفظها" — عملية بتغيّر الحالة

الفصل ده مهم لأن المتصفحات والبروكسيات بتفترض إن GET آمن. لو خليت الحذف على GET، أي زاحف (Crawler) أو أداة تسريع بتفتح الروابط مسبقاً ممكن تمسح بياناتك كلها!

## 1. `Index` — القائمة

```csharp
public async Task<IActionResult> Index()
{
    var categories = await _context.Categories
        .Include(c => c.Products)
        .AsNoTracking()
        .ToListAsync();

    return View(categories);
}
```

**`Task<IActionResult>`** — نوع الإرجاع. `IActionResult` واجهة عامة بتسمح بإرجاع أي نتيجة:

```csharp
return View(model);              // ViewResult      → HTML
return RedirectToAction("X");    // RedirectResult  → 302
return NotFound();               // NotFoundResult  → 404
return Json(data);               // JsonResult      → JSON
return File(bytes, "app/pdf");   // FileResult      → تحميل ملف
return Content("نص");            // ContentResult   → نص خام
```

**`Include(c => c.Products)`** — عشان نقدر نعرض عدد المنتجات في كل تصنيف. من غيرها `item.Products.Count` هترجع صفر دايماً.

**`return View(categories)`** — بيدوّر على `Views/Categories/Index.cshtml` (بالاصطلاح: فولدر باسم الكنترولر، ملف باسم الـ Action) ويمرّر القائمة كموديل.

## 2. `Details` — عنصر واحد

```csharp
public async Task<IActionResult> Details(int? id)
{
    if (id is null) return NotFound();

    var category = await _context.Categories
        .Include(c => c.Products)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (category is null) return NotFound();

    return View(category);
}
```

**ليه `int?` بعلامة استفهام؟**

لأن الـ `id` في الراوت اختياري (`{id?}`). لو المستخدم كتب `/Categories/Details` من غير رقم، `id` هتبقى `null` بدل ما يقع التطبيق.

**فحصين منفصلين مقصودين:**

| الفحص | الحالة |
|---|---|
| `id is null` | المستخدم مبعتش رقم أصلاً |
| `category is null` | بعت رقم لكن مش موجود في الداتابيز |

**`FirstOrDefaultAsync`** بترجّع أول نتيجة أو `null`. أخواتها:

| الميثود | السلوك لو مفيش نتيجة | لو أكتر من نتيجة |
|---|---|---|
| `FirstOrDefaultAsync` | `null` | يرجّع الأول |
| `FirstAsync` | استثناء | يرجّع الأول |
| `SingleOrDefaultAsync` | `null` | **استثناء** |
| `SingleAsync` | استثناء | **استثناء** |

استخدم `Single*` لما تكون متأكد إن فيه نتيجة واحدة بالظبط — بتكشف الأخطاء في البيانات.

## 3. `Create` (GET) — الفورم الفاضي

```csharp
public IActionResult Create() => View();
```

أبسط action في المشروع. مفيش `async` لأن مفيش عمليات داتابيز — بس بيعرض فورم فاضي.

**`=> View()`** — Expression-bodied method، اختصار لـ `{ return View(); }`.

## 4. `Create` (POST) — الحفظ

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Name,Description")] Category category)
{
    if (!ModelState.IsValid) return View(category);

    _context.Add(category);
    await _context.SaveChangesAsync();
    TempData["Success"] = "تم إضافة التصنيف بنجاح";
    return RedirectToAction(nameof(Index));
}
```

### `[HttpPost]`

بتحدد إن الـ action دي بترد على POST بس. عشان كده ينفع يكون عندنا `Create()` مرتين بنفس الاسم — واحدة GET وواحدة POST. الفرق في الـ HTTP Verb.

### `[ValidateAntiForgeryToken]` — حماية من CSRF

**إيه هو هجوم CSRF؟**

1. إنت مسجّل دخول في موقعك، والكوكيز محفوظة في المتصفح
2. تفتح موقع خبيث في تاب تاني
3. الموقع الخبيث فيه فورم مخفي بيبعت POST لموقعك
4. المتصفح بيبعت الكوكيز بتاعتك تلقائياً مع الطلب
5. موقعك بينفّذ العملية وهو فاكر إنك إنت اللي طلبتها 💥

**إزاي التوكن بيمنع ده؟**

- وقت عرض الفورم، السيرفر بيولّد توكن عشوائي ويحطه في حقل مخفي **وفي كوكي**
- وقت الإرسال، لازم الاتنين يتطابقوا
- الموقع الخبيث **مش قادر يقرا** الحقل المخفي بسبب سياسة Same-Origin

**فين الحقل المخفي؟** بيتولّد **تلقائياً** مع أي `<form>` فيها `asp-action`. افتح الصفحة في المتصفح واعمل View Source، هتلاقي:

```html
<input name="__RequestVerificationToken" type="hidden" value="CfDJ8Nr..." />
```

### `[Bind("Name,Description")]` — حماية من Over-posting

**إيه هي مشكلة الـ Over-posting؟**

تخيل موديل فيه:

```csharp
public class User
{
    public string Name { get; set; }
    public bool IsAdmin { get; set; }    // ← خطير
}
```

والفورم فيه حقل الاسم بس. مستخدم ذكي يقدر يفتح أدوات المطوّر ويضيف حقل يدوي:

```html
<input name="IsAdmin" value="true" />
```

الـ Model Binder هيملا `IsAdmin = true` وهو مبسوط، والمستخدم بقى أدمن! 💥

**`[Bind]` بتقول: "املا الخصائص دي بس، وتجاهل أي حاجة تانية جاية من الفورم".**

شوف الفرق بين الـ Create والـ Edit في مشروعنا:

```csharp
// Create — مفيش Id لأنه بيتولّد تلقائياً
[Bind("Name,Description")]

// Edit — Id موجود لأننا محتاجينه نعرف نعدّل مين
[Bind("Id,Name,Description,CreatedAt")]
```

> **ملاحظة:** `[Bind]` حل سريع ومناسب للمشاريع الصغيرة. الحل الاحترافي هو **ViewModels** — كلاسات منفصلة فيها الحقول المسموح بيها بس. هنتكلم عنها في الفصل ١٩.

### `if (!ModelState.IsValid) return View(category);`

`ModelState` بيتملي تلقائياً بنتائج التحقق من الـ Data Annotations. لو حصل خطأ:

```csharp
return View(category);   // نرجّع نفس الفورم بالبيانات اللي المستخدم كتبها
```

**نقطة مهمة:** بنمرّر `category` تاني عشان المستخدم ميعيدش كتابة كل حاجة. الأخطاء بتظهر تلقائياً في `<span asp-validation-for="...">`.

### `TempData["Success"]`

**الفرق بين طرق تمرير البيانات للـ View:**

| الطريقة | العمر | الاستخدام |
|---|---|---|
| `ViewData` / `ViewBag` | الطلب الحالي فقط | تمرير بيانات للـ View |
| `TempData` | **يعيش عبر Redirect واحد** | رسائل بعد إعادة توجيه |
| Session | كل الجلسة | بيانات المستخدم |

بنستخدم `TempData` هنا بالظبط لأن بعد الحفظ فيه **Redirect**. لو استخدمنا `ViewBag` الرسالة كانت هتضيع.

والرسالة بتتعرض في `_Layout.cshtml`:

```html
@if (TempData["Success"] is string msg)
{
    <div class="alert alert-success">@msg</div>
}
```

### `return RedirectToAction(nameof(Index));`

**ليه Redirect مش `return View()`؟**

ده نمط معروف اسمه **PRG — Post/Redirect/Get**. المشكلة اللي بيحلها:

**من غير PRG:**

1. المستخدم يبعت POST — السيرفر يحفظ ويعرض HTML مباشرة
2. المستخدم يعمل Refresh — المتصفح يسأل "تعيد الإرسال؟"
3. النتيجة: **حفظ مكرر** 💥

**مع PRG:**

1. المستخدم يبعت POST — السيرفر يحفظ ويرد بـ Redirect
2. المتصفح يعمل GET للصفحة الجديدة
3. المستخدم يعمل Refresh — بيعيد الـ GET بس
4. النتيجة: **مفيش تكرار** ✓

**`nameof(Index)`** بترجّع النص `"Index"` لكن بطريقة آمنة. لو غيّرت اسم الـ action، الكومبايلر هيقولك إن فيه خطأ. لو كتبت `"Index"` كنص عادي، الخطأ مكنش هيظهر غير وقت التشغيل.

## 5 و 6. `Edit`

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedAt")] Category category)
{
    if (id != category.Id) return NotFound();
    if (!ModelState.IsValid) return View(category);

    try
    {
        _context.Update(category);
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!await _context.Categories.AnyAsync(c => c.Id == id)) return NotFound();
        throw;
    }

    TempData["Success"] = "تم تعديل التصنيف";
    return RedirectToAction(nameof(Index));
}
```

### `if (id != category.Id) return NotFound();`

فحص أمني. الـ `id` جاي من **الراوت** (`/Categories/Edit/5`)، و `category.Id` جاي من **الفورم**. لو مختلفين، يبقى فيه محاولة تلاعب.

### `catch (DbUpdateConcurrencyException)`

**سيناريو التزامن:**

1. مستخدم **أ** يفتح صفحة تعديل التصنيف رقم ١
2. مستخدم **ب** يفتح نفس الصفحة
3. مستخدم **ب** يحفظ ✓
4. مستخدم **أ** يحفظ — إيه اللي يحصل؟

الاستثناء ده بيتولد لما الصف اللي بتعدّله اتغيّر أو اتحذف من تحتك.

الكود بيفرّق بين حالتين:
- **اتحذف** → رجّع 404
- **اتغيّر** → `throw` (أعد رمي الاستثناء عشان يظهر)

> **ملاحظة:** الكشف الكامل للتزامن يحتاج عمود `[Timestamp]` في الموديل. الكود ده يمسك حالة الحذف بشكل موثوق، والتغيير المتزامن يحتاج الإعداد الإضافي ده.

## 7. `Delete`

```csharp
// GET — صفحة التأكيد
public async Task<IActionResult> Delete(int? id) { ... return View(category); }

// POST — التنفيذ الفعلي
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var category = await _context.Categories.FindAsync(id);
    if (category is not null)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
    TempData["Success"] = "تم حذف التصنيف";
    return RedirectToAction(nameof(Index));
}
```

### `[ActionName("Delete")]` — ليه؟

C# مبيسمحش بميثودين بنفس الاسم ونفس المعطيات:

```csharp
public IActionResult Delete(int? id)   // GET
public IActionResult Delete(int id)    // POST — ❌ الكومبايلر هيرفض
```

الحل: سمّي الميثود اسم مختلف في C#، وقول لـ ASP.NET تعاملها كأنها `Delete`:

```csharp
[HttpPost, ActionName("Delete")]
public async Task<IActionResult> DeleteConfirmed(int id)
```

دلوقتي `/Categories/Delete/5` بـ POST بتوصل للميثود دي، رغم إن اسمها الحقيقي `DeleteConfirmed`.

---

## جرّب بنفسك — الفصل 13

**١.** شيل `[ValidateAntiForgeryToken]` من `Create` وجرّب تبعت POST من خارج الموقع (بـ Postman مثلاً). هينجح. رجّعها وجرّب تاني — هيترفض بـ 400.

**٢.** شيل `[Bind]` من `Edit` وأضف حقل مخفي في الفورم:
```html
<input type="hidden" name="CreatedAt" value="1990-01-01" />
```
هيتحفظ. ده الـ Over-posting بالظبط.

**٣.** غيّر `return RedirectToAction(nameof(Index))` لـ `return View(category)` بعد الحفظ. أضف تصنيف، ثم اعمل Refresh. المتصفح هيسألك تعيد الإرسال — وهيتضاف تاني. ده اللي PRG بيمنعه.

**٤.** أضف رسالة خطأ مخصصة:
```csharp
if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
{
    ModelState.AddModelError(nameof(Category.Name), "الاسم ده مستخدم قبل كده");
    return View(category);
}
```
حطها قبل `_context.Add(category)`. جرّب تضيف تصنيف باسم موجود.

---

# الفصل 14: `ProductsController.cs`

الفرق الأساسي: بيستخدم `IProductService` بدل `AppDbContext`.

```csharp
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly AppDbContext _context;   // للـ dropdown فقط

    public ProductsController(IProductService productService, AppDbContext context)
    {
        _productService = productService;
        _context = context;
    }
```

> **ملاحظة صريحة:** وجود الاتنين مع بعض هو **حل وسط تعليمي** عشان تشوف النمطين جنب بعض. في مشروع حقيقي، كنت هعمل `ICategoryService` وأشيل الـ `AppDbContext` من الكنترولر خالص.

## الميثود المساعدة

```csharp
private async Task PopulateCategoriesAsync(int? selectedId = null)
{
    var categories = await _context.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
    ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
}
```

**`SelectList`** بتحوّل أي قائمة لـ `<option>` عناصر. معطياتها الأربعة:

| المعطى | القيمة | الوظيفة |
|---|---|---|
| المصدر | `categories` | القائمة |
| `dataValueField` | `"Id"` | اللي هيتخزّن في `value` |
| `dataTextField` | `"Name"` | اللي هيظهر للمستخدم |
| `selectedValue` | `selectedId` | اللي هيبقى محدد مسبقاً |

النتيجة في HTML:

```html
<option value="1">معالجات</option>
<option value="2" selected>كروت شاشة</option>
```

**ليه `private`؟** لأنها مش action — مجرد مساعدة داخلية. لو خليتها `public` هتبقى قابلة للوصول كـ `/Products/PopulateCategoriesAsync` وده مش مطلوب.

## النقطة الحرجة: إعادة التعبئة عند الخطأ

```csharp
[HttpPost]
public async Task<IActionResult> Create([Bind(...)] Product product)
{
    if (!ModelState.IsValid)
    {
        await PopulateCategoriesAsync(product.CategoryId);   // ← لازم!
        return View(product);
    }

    await _productService.AddAsync(product);
    return RedirectToAction(nameof(Index));
}
```

**ليه بننادي `PopulateCategoriesAsync` تاني؟**

`ViewBag` عمره **الطلب الواحد بس**. الـ POST ده طلب جديد تماماً — الـ `ViewBag.Categories` اللي عبّيناها في الـ GET راحت خلاص.

**لو نسيت السطر ده:** المستخدم هيدخل بيانات غلط، الصفحة هترجع، و**الـ dropdown هيبقى فاضي**. وده من أشهر الأخطاء اللي المبتدئين بيقعوا فيها.

---

## جرّب بنفسك — الفصل 14

**١.** شيل `await PopulateCategoriesAsync(product.CategoryId);` من داخل `if (!ModelState.IsValid)`. حاول تضيف منتج من غير اسم. الفورم هيرجع والـ dropdown فاضي. ده الخطأ اللي اتكلمنا عنه.

**٢.** أضف action للبحث:
```csharp
public async Task<IActionResult> Search(string q)
{
    var all = await _productService.GetAllAsync();
    var results = string.IsNullOrWhiteSpace(q)
        ? all
        : all.Where(p => p.Name.Contains(q, StringComparison.OrdinalIgnoreCase));

    ViewBag.Query = q;
    return View("Index", results);
}
```
لاحظ `return View("Index", results)` — بنعيد استخدام نفس الـ View باسم صريح.

**سؤال:** الكود ده بيفلتر في الذاكرة. إزاي تخليه يفلتر في الداتابيز؟ (رجّع للفصل ١١، تمرين ١)

**٣.** حوّل الكنترولر يستخدم `ICategoryService` بدل `AppDbContext` وشيل الـ Context خالص.

---

# الفصل 15: الـ Views ومحرّك Razor

## إيه هو Razor؟

Razor هو محرّك قوالب بيخلط HTML مع C#. الرمز `@` هو نقطة التحوّل بين الاتنين.

```html
<h1>@Model.Name</h1>                     <!-- طباعة قيمة -->
@if (Model.IsAvailable) { <span>متاح</span> }   <!-- شرط -->
@foreach (var p in Model) { <li>@p.Name</li> }  <!-- تكرار -->
@{ var total = Model.Price * Model.Stock; }     <!-- بلوك كود -->
```

## ⚠️ الحماية التلقائية من XSS

Razor **بيرمّز أي مخرجات تلقائياً**. لو منتج اسمه:

```
<script>alert('اخترقتك')</script>
```

`@item.Name` هيطبعه كنص عادي مرئي، **مش هينفّذه كسكربت**. الـ HTML المتولّد:

```html
&lt;script&gt;alert('اخترقتك')&lt;/script&gt;
```

دي حماية مجانية من **XSS (Cross-Site Scripting)** — أشهر ثغرة في تطبيقات الويب.

> **الاستثناء الخطير:** `@Html.Raw(value)` بتطبع من غير ترميز. **متستخدمهاش أبداً** مع أي بيانات جاية من المستخدم.

## ملفات الإعداد التلاتة

### `_ViewImports.cshtml`

```csharp
@using ShopApp
@using ShopApp.Models
@using ShopApp.Services
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

بيتطبّق على **كل** الـ Views. عشان كده بنكتب `@model Product` بدل `@model ShopApp.Models.Product`.

السطر الأخير بيفعّل الـ Tag Helpers — من غيره `asp-for` و `asp-action` هيبقوا مجرد نصوص عادية في الـ HTML.

### `_ViewStart.cshtml`

```csharp
@{
    Layout = "_Layout";
}
```

بيحدد القالب الافتراضي لكل الـ Views.

### `_Layout.cshtml` — القالب المشترك

```html
<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head>
    <title>@ViewData["Title"] - ShopApp</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.rtl.min.css" />
</head>
<body>
    <nav>...</nav>

    <div class="container">
        @if (TempData["Success"] is string msg) { <div class="alert alert-success">@msg</div> }
        <main>@RenderBody()</main>
    </div>

    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

**`dir="rtl"` و `bootstrap.rtl.min.css`** — الاتنين مطلوبين للعربي. الأول بيقلب اتجاه النص، والتاني بيقلب تخطيط Bootstrap كله (الأعمدة، الهوامش، المحاذاة).

**`@RenderBody()`** — هنا بيتحط محتوى الـ View الحالية.

**`@await RenderSectionAsync("Scripts", required: false)`** — نقطة إدراج للسكربتات. `required: false` معناها إن الـ View مش ملزمة تحدد القسم ده.

والـ View بتملاه كده:

```csharp
@section Scripts {
    @await Html.PartialAsync("_ValidationScriptsPartial")
}
```

**ليه السكربتات في الآخر؟** عشان الصفحة تظهر بسرعة. المتصفح بيرسم الـ HTML الأول، وبعدين يحمّل الجافاسكربت.

## الـ Tag Helpers — التفصيل

الـ Tag Helper بيخلّي الـ HTML يفضل HTML لكن بقوة السيرفر.

### `asp-for` — الربط بالموديل

```html
<input asp-for="Name" class="form-control" />
```

بيتولّد:

```html
<input class="form-control"
       type="text"
       id="Name"
       name="Name"
       value="القيمة الحالية"
       data-val="true"
       data-val-required="اسم التصنيف مطلوب"
       data-val-length-max="60" />
```

**لاحظ إنه استنتج ٦ حاجات لوحده:**

| المستنتج | من فين |
|---|---|
| `type="text"` | نوع الخاصية `string` |
| `id` و `name` | اسم الخاصية |
| `value` | قيمة الموديل الحالية |
| `data-val-*` | الـ Data Annotations |

ولو الخاصية `bool` هيبقى `type="checkbox"`، ولو `DateTime` هيبقى `type="date"`.

**`data-val-*` دي هي التحقق من جهة العميل** — jQuery Validation بيقراها ويمنع الإرسال قبل ما يوصل السيرفر أصلاً.

### `asp-action` و `asp-controller`

```html
<a asp-controller="Products" asp-action="Edit" asp-route-id="@item.Id">تعديل</a>
```

بيتولّد: `<a href="/Products/Edit/5">تعديل</a>`

**ليه أحسن من كتابة الرابط يدوي؟** لأنه بيستخدم جدول الراوتنج. لو غيّرت قالب الراوت، كل الروابط بتتحدّث تلقائياً. لو كتبت `href="/Products/Edit/5"` يدوي، كنت هتفضل تدوّر عليها كلها.

**`asp-route-<اسم>`** بتضيف أي معطى للراوت. `asp-route-id="5"` → `/Products/Edit/5`.

### `asp-items` — القوائم المنسدلة

```html
<select asp-for="CategoryId" asp-items="@(ViewBag.Categories as SelectList)" class="form-select">
    <option value="">-- اختر التصنيف --</option>
</select>
```

**ليه `as SelectList`؟** لأن `ViewBag` نوعه `dynamic` — الكومبايلر مش عارف نوعه. الـ `as` بتحوّله للنوع المتوقع.

### `asp-validation-for` و `asp-validation-summary`

```html
<div asp-validation-summary="ModelOnly" class="text-danger"></div>   <!-- أخطاء عامة -->
<span asp-validation-for="Name" class="text-danger"></span>          <!-- خطأ حقل معيّن -->
```

قيم `asp-validation-summary`:

| القيمة | بتعرض |
|---|---|
| `ModelOnly` | الأخطاء العامة بس (مش المرتبطة بحقل) |
| `All` | كل الأخطاء |
| `None` | مفيش |

بنستخدم `ModelOnly` عشان أخطاء الحقول بتظهر جنب حقولها أصلاً — استخدام `All` كان هيكررها مرتين.

### `asp-append-version` — كسر الكاش

```html
<link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
```

بيتولّد: `<link href="/css/site.css?v=Kl_dqr9NVtnMdsM2MUg4qthUnWZm5T1fCEimBPWDNgM" />`

القيمة دي **بصمة (Hash) لمحتوى الملف**. أول ما تعدّل الملف، البصمة تتغير، والمتصفح يعتبره ملف جديد ويحمّله. من غيرها ممكن المستخدم يفضل شايف CSS قديم من الكاش لأيام.

## الـ Views الخمسة — الأنماط

### `Index.cshtml` — القائمة

```csharp
@model IEnumerable<Product>
```

**الموديل مجموعة** — عشان كده `@foreach`.

```csharp
@if (!Model.Any())
{
    <div class="alert alert-info">لا توجد منتجات بعد.</div>
}
```

**حالة الفراغ (Empty State)** — دايماً اتعامل معاها. جدول فاضي بأعمدة بس تجربة سيئة.

### `Create.cshtml` و `Edit.cshtml`

الفرق الوحيد المهم:

```html
<input type="hidden" asp-for="Id" />
```

في `Edit` بس. من غيرها السيرفر مش هيعرف بتعدّل مين.

في `Categories/Edit.cshtml` فيه حقل مخفي تاني:

```html
<input type="hidden" asp-for="CreatedAt" />
```

**ليه؟** لأن `CreatedAt` مش في الفورم، فلو مبعتهاش هترجع `default` (سنة 0001) وتتحفظ كده، وتاريخ الإنشاء الأصلي يضيع.

> ده بالظبط أحد الأسباب اللي بتخلي **ViewModels** حل أنضف — بدل ما تشيل حقول مخفية، بتقرا الكيان الأصلي من الداتابيز وتحدّث الحقول المسموحة بس.

### `Delete.cshtml`

```html
@if (Model.Products.Any())
{
    <div class="alert alert-danger">
        تحذير: حذف هذا التصنيف سيحذف معه <strong>@Model.Products.Count</strong> منتج (Cascade Delete).
    </div>
}
```

**تحذير مشروط.** المستخدم لازم يعرف عواقب فعله قبل ما ينفّذه. ده مبدأ أساسي في تصميم الواجهات.

---

## جرّب بنفسك — الفصل 15

**١.** أضف منتج اسمه:
```
<b>غامق</b>
```
افتح `/Products`. هتلاقيه ظهر كنص عادي مش غامق — دي حماية Razor التلقائية.

**٢.** غيّر `@item.Name` لـ `@Html.Raw(item.Name)` وشوف الفرق. **رجّعها فوراً** — ده الباب اللي بتدخل منه ثغرات XSS.

**٣.** شيل `<input type="hidden" asp-for="CreatedAt" />` من `Categories/Edit.cshtml`، عدّل تصنيف، وشوف تاريخ الإنشاء في صفحة التفاصيل. هيبقى `0001-01-01`.

**٤.** شيل `@section Scripts` من `Create.cshtml` وحاول تحفظ فورم ناقص. الرسالة هتظهر بس **بعد** ما الصفحة تروح السيرفر وترجع، مش فوراً. ده الفرق بين التحقق من جهة العميل والسيرفر.

**٥.** أضف Partial View جديدة. اعمل `Views/Shared/_ProductCard.cshtml`:
```html
@model Product
<div class="card mb-2">
    <div class="card-body">
        <h5 class="card-title">@Model.Name</h5>
        <p class="card-text">@Model.Price.ToString("N2") — @Model.Category?.Name</p>
    </div>
</div>
```
واستخدمها في `Index.cshtml`:
```csharp
@foreach (var item in Model)
{
    <partial name="_ProductCard" model="item" />
}
```

---

# الفصل 16: Migrations

## المشكلة اللي بتحلها

عندك موديلات في الكود، وجداول في الداتابيز. لما تضيف خاصية جديدة للموديل، الجدول لازم يتغيّر كمان.

**الطرق الخاطئة:**
- تعدّل الجدول يدوي بـ SQL → زملاؤك مش هيعرفوا يعملوا نفس التعديل
- تمسح الداتابيز وتعملها من جديد → في الإنتاج ده يعني ضياع كل البيانات

**Migrations هي الحل:** كل تغيير بيتسجّل كملف C# قابل للتطبيق والتراجع، وبيتحفظ في Git مع الكود.

## محتوى ملف الـ Migration

```csharp
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(maxLength: 60, nullable: false),
                // ...
            },
            constraints: table => { table.PrimaryKey("PK_Categories", x => x.Id); });

        // ... باقي الجداول والبيانات المبدئية
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Products");
        migrationBuilder.DropTable(name: "Categories");
    }
}
```

| الميثود | متى تتنفّذ |
|---|---|
| `Up()` | عند `database update` — بتطبّق التغيير |
| `Down()` | عند التراجع — بتلغي التغيير |

**لاحظ الترتيب في `Down`:** بيحذف `Products` **قبل** `Categories`. لازم تحذف الجدول التابع الأول عشان قيد المفتاح الأجنبي.

## الملفات التلاتة

| الملف | الدور |
|---|---|
| `<التاريخ>_InitialCreate.cs` | أوامر الـ Up والـ Down |
| `<التاريخ>_InitialCreate.Designer.cs` | لقطة كاملة من النموذج وقت إنشاء الـ Migration |
| `AppDbContextModelSnapshot.cs` | **اللقطة الحالية** — بتتحدّث مع كل Migration |

**إزاي EF بيعرف إيه اللي اتغيّر؟** بيقارن الموديلات الحالية بـ `AppDbContextModelSnapshot.cs` — مش بالداتابيز نفسها. الفرق بينهم هو الـ Migration الجديدة.

> **⚠️ متعدّلش `AppDbContextModelSnapshot.cs` يدوي أبداً.** لو اتلخبط، EF هيولّد Migrations خاطئة.

## جدول `__EFMigrationsHistory`

EF بيعمل جدول خاص في الداتابيز:

```sql
CREATE TABLE "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);
```

فيه أسماء الـ Migrations المطبّقة. وقت `database update`، EF:
1. يقرا الجدول ده
2. يقارنه بملفات فولدر `Migrations`
3. يطبّق الناقص بس

**ده اللي شفته في الترمينال أول تشغيل:**

```
SELECT "MigrationId", "ProductVersion" FROM "__EFMigrationsHistory" ORDER BY "MigrationId";
Applying migration '20260819084454_InitialCreate'.
```

## الأوامر الأساسية

```bash
# إنشاء migration جديدة
dotnet ef migrations add AddProductDescription

# تطبيق كل الـ migrations المعلّقة
dotnet ef database update

# التراجع عن آخر migration (لو لسه ما اتطبقتش)
dotnet ef migrations remove

# الرجوع لـ migration معيّنة
dotnet ef database update InitialCreate

# التراجع عن كل حاجة
dotnet ef database update 0

# عرض القائمة
dotnet ef migrations list

# توليد سكربت SQL بدل التطبيق المباشر (للإنتاج)
dotnet ef migrations script

# سكربت آمن يشتغل مهما كانت حالة الداتابيز
dotnet ef migrations script --idempotent
```

## قواعد ذهبية

**١. سمّي الـ Migration باسم وصفي.**
`AddProductDescription` أحسن ألف مرة من `Migration2`. بعد سنة هتبص على التاريخ وتعرف حصل إيه.

**٢. راجع الملف المتولّد قبل ما تطبّقه.**
EF ذكي بس مش معصوم. خصوصاً في تغييرات زي تغيير نوع عمود.

**٣. متعدّلش Migration اتطبقت بالفعل.**
اعمل واحدة جديدة تصحّح. لو عدّلت المطبّقة، مين طبّقها قبلك هيبقى عنده حالة مختلفة عنك.

**٤. في الإنتاج، استخدم السكربتات.**
```bash
dotnet ef migrations script --idempotent -o deploy.sql
```
راجع الـ SQL بعينك قبل ما تشغّله على بيانات حقيقية.

**٥. خد نسخة احتياطية قبل أي Migration في الإنتاج.** دايماً.

---

## جرّب بنفسك — الفصل 16

**١.** أضف خاصية للمنتج:
```csharp
[StringLength(500)]
[Display(Name = "الوصف")]
public string? Description { get; set; }
```
اعمل Migration واقرا الملف:
```bash
dotnet ef migrations add AddProductDescription
```
هتلاقي `AddColumn` واحد بس — مش إعادة إنشاء الجدول.

**٢.** جرّب التراجع:
```bash
dotnet ef migrations remove
```
هتلاقي الملفات اتمسحت والـ Snapshot رجع لحالته. (ده بيشتغل لأنك ما طبقتهاش لسه.)

**٣.** طبّقها، بعدين جرّب تتراجع:
```bash
dotnet ef migrations add AddProductDescription
dotnet ef database update
dotnet ef database update InitialCreate     # رجوع للـ migration السابقة
dotnet ef migrations remove                  # دلوقتي ينفع تشيلها
```

**٤.** ولّد سكربت SQL وافتحه:
```bash
dotnet ef migrations script --idempotent -o migration.sql
```
اقراه. هتلاقي كل أمر مغلّف في فحص `IF NOT EXISTS`. عشان كده اسمه idempotent — ينفع تشغّله مليون مرة والنتيجة واحدة.

**٥.** امسح `shopapp.db` وشغّل `dotnet ef database update`. هتلاقي الداتابيز اتبنت من الصفر بكل الـ Migrations بالترتيب. ده معنى إن الـ Migrations هي "تاريخ الداتابيز".

---

# الفصل 17: ملخّص الأمان

| الحماية | ضد إيه | فين في الكود |
|---|---|---|
| `[ValidateAntiForgeryToken]` | CSRF | كل POST action |
| `[Bind("...")]` | Over-posting | معطيات الـ Create والـ Edit |
| `ModelState.IsValid` | بيانات غير صالحة | بداية كل POST action |
| ترميز Razor التلقائي | XSS | كل `@` في الـ Views |
| استعلامات EF المعاملية | SQL Injection | تلقائي في كل استعلام |
| `wwwroot` فقط مكشوف | كشف الملفات | `app.UseStaticFiles()` |
| صفحة خطأ عامة في الإنتاج | تسريب معلومات | `app.UseExceptionHandler` |
| `UseHttpsRedirection` + `UseHsts` | التنصت | الـ Pipeline |

## نقطة مهمة: SQL Injection

EF Core بيستخدم **معطيات (Parameters)** تلقائياً. حتى لو المستخدم كتب:

```
'; DROP TABLE Products; --
```

الاستعلام المتولّد بيبقى:

```sql
SELECT * FROM Products WHERE Name = @p0
-- @p0 = '''; DROP TABLE Products; --'
```

النص بيتعامل كـ **قيمة** مش كـ **كود**. آمن تماماً.

**⚠️ الاستثناء الخطير:** لو استخدمت SQL خام مع دمج نصوص:

```csharp
// ❌ خطر شديد
_context.Products.FromSqlRaw($"SELECT * FROM Products WHERE Name = '{userInput}'");

// ✓ آمن
_context.Products.FromSqlInterpolated($"SELECT * FROM Products WHERE Name = {userInput}");
```

الفرق إن `FromSqlInterpolated` بتحوّل المتغيرات لمعطيات تلقائياً.

---

# الفصل 18: أخطاء شائعة وحلولها

### `no such column: X` أو `Invalid column name 'X'`

**السبب:** عدّلت الموديل ونسيت الـ Migration.
**الحل:**
```bash
dotnet ef migrations add AddX
dotnet ef database update
```

### `NullReferenceException` عند `product.Category.Name`

**السبب:** نسيت `.Include(p => p.Category)`.
**الحل:** ضيف الـ Include، أو استخدم `?.` في العرض.

### `InvalidOperationException: The view 'Index' was not found`

**السبب:** الـ View مش في المكان الصح.
**الحل:** تأكد إن المسار `Views/<اسم الكنترولر بدون كلمة Controller>/<اسم الـ Action>.cshtml`.

### الـ dropdown بيفضى بعد خطأ في الفورم

**السبب:** نسيت تعيد تعبئة `ViewBag` في مسار الخطأ.
**الحل:** نادِ `PopulateCategoriesAsync()` جوّه `if (!ModelState.IsValid)`.

### `Cannot consume scoped service from singleton`

**السبب:** سجّلت خدمة بتستخدم `DbContext` كـ `Singleton`.
**الحل:** غيّرها لـ `AddScoped`.

### `dotnet ef` مش متعرّف عليه

**الحل:**
```bash
dotnet tool install --global dotnet-ef
```
ولو لسه، تأكد إن `~/.dotnet/tools` في الـ PATH.

### `Unable to create an object of type 'AppDbContext'`

**السبب:** `dotnet ef` مش لاقي إزاي يعمل الـ Context.
**الحل:** تأكد إنك في فولدر المشروع الصح، وإن `Program.cs` فيه `AddDbContext`.

### التعديلات مش بتتحفظ

**السبب:** نسيت `SaveChangesAsync()`، أو الكيان متجاب بـ `AsNoTracking()`.
**الحل:** شيل `AsNoTracking` من الاستعلامات اللي هتعدّل عليها.

### `The instance of entity type 'Product' cannot be tracked`

**السبب:** حاولت تعمل `Update` لكيان بينما نسخة تانية بنفس الـ Id متتبّعة بالفعل.
**الحل:** استخدم `AsNoTracking` وقت القراءة، أو حدّث الكيان المتتبّع نفسه بدل ما تعمل `Update`.

---

# الفصل 19: خطواتك الجاية

## المستوى الأول — كمّل الأساسيات

**1. ViewModels** ← الأهم على الإطلاق

المشكلة الحالية: بنستخدم الـ Entities مباشرة في الـ Views. ده بيخلينا نحتاج `[Bind]` وحقول مخفية.

الحل:
```csharp
public class ProductCreateViewModel
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 1_000_000)]
    public decimal Price { get; set; }

    public int CategoryId { get; set; }
    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
}
```

الفوايد: مفيش over-posting أصلاً، مفيش `ViewBag`، والفورم بيحمل بالظبط اللي محتاجه.

**2. Pagination** — لو عندك ١٠٠٠ منتج، `/Products` هتنهار.

**3. Search & Filter** — بحث بالاسم وفلترة بالتصنيف والسعر.

**4. Sorting** — ترتيب بالضغط على رأس العمود.

## المستوى الثاني — بنية أنضف

**5. Repository + Unit of Work** — طبقة تجريد فوق EF.

**6. AutoMapper** — تحويل تلقائي بين Entity و ViewModel.

**7. FluentValidation** — تحقق أقوى وأنضف من الـ Data Annotations.

**8. Serilog** — تسجيل احترافي في ملفات أو قواعد بيانات.

## المستوى الثالث — مميزات حقيقية

**9. ASP.NET Core Identity** — تسجيل دخول وصلاحيات كاملة.

**10. Web API** — حوّل المشروع لـ API:
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsApiController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> Get() =>
        Ok(await _productService.GetAllAsync());
}
```
وبعدها اربطه بواجهة React أو Flutter.

**11. رفع الصور** — صورة لكل منتج.

**12. Unit Testing** — استخدم xUnit مع Moq. الـ `IProductService` اللي عملناه جاهز للاختبار من دلوقتي.

## مسار مقترح للأسابيع الجاية

| الأسبوع | الموضوع |
|---|---|
| الأول | ViewModels + Pagination + Search |
| الثاني | Identity — تسجيل دخول وصلاحيات |
| الثالث | رفع الصور + AutoMapper |
| الرابع | Web API + Unit Tests |

## مصادر موثوقة

| المصدر | الرابط |
|---|---|
| توثيق Microsoft الرسمي | learn.microsoft.com/aspnet/core |
| توثيق EF Core | learn.microsoft.com/ef/core |
| قناة Nick Chapsas | يوتيوب — محتوى متقدّم وعملي |
| قناة Milan Jovanović | يوتيوب — معمارية وأنماط تصميم |

---

## كلمة أخيرة

المشروع ده صغير عن قصد — عشان تقدر تمسك كل خيط فيه من أوله لآخره. الفكرة مش إنك تحفظ الكود، الفكرة إنك **تكسّره وتصلّحه**.

كل تمرين في الدليل ده كتبته عشان تجرّبه فعلاً، مش تقراه. لما تشيل `Include` وتشوف العمود بيفضى، أو تشيل `[ValidateAntiForgeryToken]` وتشوف الطلب بيعدّي — المعلومة دي بتتثبّت بشكل مختلف تماماً عن مجرد قراءتها.

اكسر المشروع. ده أسرع طريقة تتعلم بيها.
